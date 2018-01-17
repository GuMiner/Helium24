namespace Helium24.Definitions.Maps
{
    public class Poi
    {
        public int TypeId { get; set; }

        public string PoiId { get; set; }

        public string LatLng { get; set; }
    }

    public class NewPoi : OneWayVerification
    {
        public string Name { get; set; }
    }

    public class NewPoiData : OneWayVerification
    {
        public int TypeId { get; set; }

        public string LatLng { get; set; }
    }

    public class DeleteIDData : OneWayVerification
    {
        public int TypeId { get; set; }

        public string PoiId { get; set; }
    }
}
