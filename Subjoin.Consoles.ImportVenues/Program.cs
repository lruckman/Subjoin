using System;
using System.Data.Entity.Spatial;
using System.IO;
using System.Linq;
using System.Reflection;
using Subjoin.Consoles.ImportVenues.Engine;
using Subjoin.Consoles.ImportVenues.Models;
using Subjoin.DataAccess;
using Subjoin.DataAccess.Models;

namespace Subjoin.Consoles.ImportVenues
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var path = GetPath();

            Process(path);

            Console.ReadLine();
        }

        private static string GetPath()
        {
            while (true)
            {
                Console.WriteLine("Provide path:");
                var path = Console.ReadLine();

                if (File.Exists(path))
                {
                    return path;
                }

                Console.WriteLine("Unable to access path: {0}", path);
            }
        }

        private static void Process(string path)
        {
            var pois = CsvReader.ReadLines(path, s =>
            {
                var segments = s.Split(new[] {'|'});

                if (segments.Length != 5)
                {
                    return null;
                }

                return new OsmPoiModel
                {
                    CategoryId = int.Parse(segments[0]),
                    Latitude = float.Parse(segments[2]),
                    Longitude = float.Parse(segments[3]),
                    Name = segments[4],
                    OsmId = segments[1]
                };
            }).ToArray();

            Console.WriteLine("({0}) Starting...", DateTime.Now.ToString("T"));

            var db = new SubjoinEntities();

            var venuesExist = db.Venues.Any();

            for (var i = 0; i < pois.Length; ++i)
            {
                var poi = pois[i];

                // validations

                if (poi == null)
                {
                    Console.WriteLine("({0}) skipping null poi... {1} remaining",
                        DateTime.Now.ToString("T"), pois.Length - i);
                }

                poi.Name = (poi.Name ?? "")
                    .Replace("''", "'")
                    .Replace("''", "'")
                    .Replace("''", "'");

                if (string.IsNullOrWhiteSpace(poi.Name))
                {
                    Console.WriteLine("({0}) {1} skipping nameless poi... {2} remaining",
                        DateTime.Now.ToString("T"), poi.OsmId, pois.Length - i);

                    continue;
                }

                if (poi.Latitude == 0 || poi.Longitude == 0)
                {
                    Console.WriteLine("({0}) {1} skipping locationless poi... {2} remaining",
                        DateTime.Now.ToString("T"), poi.OsmId, pois.Length - i);

                    continue;
                }

                DbGeography location;

                try
                {
                    location = new LatLng
                    {
                        Latitude = poi.Latitude,
                        Longitude = poi.Longitude
                    }.ToDbGeography();
                }
                catch (TargetInvocationException)
                {
                    Console.WriteLine("({0}) {1} skipping bad lat/lng... {2} remaining",
                        DateTime.Now.ToString("T"), poi.OsmId, pois.Length - i);
                    continue;
                }

                // process

                Console.WriteLine("({0}) {1} processing... {2} remaining",
                    DateTime.Now.ToString("T"), poi.OsmId, pois.Length - i);

                var venue = !venuesExist
                    ? null
                    : db.Venues
                        .SingleOrDefault(x => x.OSMId == poi.OsmId);

                if (venue == null)
                {
                    db.Venues.Add(venue = new Venue
                    {
                        Active = true,
                        CreatedOn = DateTime.UtcNow,
                        OSMId = poi.OsmId
                    });
                }

                venue.Location = location;
                venue.ModifiedOn = DateTime.UtcNow;
                venue.Name = poi.Name;
                venue.VenueCategoryId = poi.CategoryId;

                if (i%1000 != 0)
                {
                    continue;
                }

                Console.WriteLine("({0}) Commiting batch...", DateTime.Now.ToString("T"));

                db.SaveChanges();
                db.Dispose();

                db = new SubjoinEntities();
            }

            Console.WriteLine("({0}) Commiting final batch...", DateTime.Now.ToString("T"));

            db.SaveChanges();
            db.Dispose();

            Console.WriteLine("({0}) Finished!", DateTime.Now.ToString("T"));
        }
    }
}