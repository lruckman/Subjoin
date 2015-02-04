namespace Subjoin.Consoles.ImportVenues.Models
{
    public class OsmPoiModel
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string OsmId { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
    }
}