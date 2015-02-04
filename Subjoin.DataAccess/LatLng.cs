using System.Data.Entity.Spatial;

namespace Subjoin.DataAccess
{
    public class LatLng
    {
        public double? Longitude { get; set; }

        public double? Latitude { get; set; }

        public bool HasValue
        {
            get { return Latitude.GetValueOrDefault()!=0 && Longitude.GetValueOrDefault()!=0; }
        }

        public DbGeography ToDbGeography()
        {
            return DbGeography.PointFromText(string.Format("POINT({1} {0})", Latitude, Longitude), 4326);
        }
    }
}