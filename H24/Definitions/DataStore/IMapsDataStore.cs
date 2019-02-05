using H24.Definitions.Maps;
using System;
using System.Collections.Generic;

namespace H24.Definitions.DataStore
{
    public interface IMapsDataStore
    {
        PoiType AddPoiType(string name);
        List<PoiType> GetPoiTypes();

        Poi AddPoi(int typeId, string latLng);
        List<Poi> GetPoi(int typeId);
        void DeletePoi(int typeId, string poiId);

        List<PoiLine> GetPoiLines(int typeId);
        PoiLine AddPoiLine(int typeId, Tuple<float, float> latLngOne, Tuple<float, float> latLngTwo);
        void DeletePoiLine(int typeId, string poiId);
    }
}
