namespace H24.Definitions.Maps
{
    public class PoiLine
    {
        public string PoiId { get; set; }

        public int TypeId { get; set; }

        public float LatOne { get; set; }

        public float LngOne { get; set; }

        public float LatTwo { get; set; }

        public float LngTwo { get; set; }
    }

    public class NewPoiLine : OneWayVerification
    {
        public int TypeId { get; set; }

        public float LatOne { get; set; }

        public float LngOne { get; set; }

        public float LatTwo { get; set; }

        public float LngTwo { get; set; }
    }
}
