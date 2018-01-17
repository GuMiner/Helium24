using Nancy;
using System;
using System.Configuration;
using Nancy.ModelBinding;
using Helium24.Definitions.Maps;

namespace Helium24.Modules
{
    /// <summary>
    /// Module for communicating with our PostgreSQL RPI database for map POI information
    /// </summary>
    public class MapsModule : NancyModule
    {
        public MapsModule()
            : base("/Maps")
        {
            // Returns all POI types found in {'idx', 'name'} format
            Get["/POI"] = parameters =>
            {
                return this.Response.AsJson(Global.MapsStore.GetPoiTypes());
            };

            // Adds a new POI type given the accessCode and name, returning the new idx and name
            Post["/POI"] = parameters =>
            {
                NewPoi newPoi = this.Bind<NewPoi>();
                if (!IsAuthorized(newPoi.AccessCode))
                {
                    return this.Response.AsJson(new string[0], HttpStatusCode.BlockedByWindowsParentalControls);
                }

                return this.Response.AsJson(Global.MapsStore.AddPoiType(newPoi.Name));
            };

            // Returns all POI found in latLng, typeId, poiId format.
            Get["/Data/{typeId:int}"] = parameters =>
            {
                int typeId = parameters.typeId;
                return this.Response.AsJson(Global.MapsStore.GetPoi(typeId));
            };

            // Returns all POI lines found in latLngs, typeId, poiId format.
            Get["/DataLines/{typeId:int}"] = parameters =>
            {
                int typeId = parameters.typeId;
                return this.Response.AsJson(Global.MapsStore.GetPoiLines(typeId));
            };

            // Adds a new POI given the accessCode, typeId, and latLng, returning the typeId and poiId
            Post["/DataLines"] = parameters =>
            {
                NewPoiLine newPoiData = this.Bind<NewPoiLine>();

                if (!IsAuthorized(newPoiData.AccessCode))
                {
                    return this.Response.AsJson(new string[0], HttpStatusCode.BlockedByWindowsParentalControls);
                }

                return this.Response.AsJson(Global.MapsStore.AddPoiLine(newPoiData.TypeId, Tuple.Create(newPoiData.LatOne, newPoiData.LngOne), Tuple.Create(newPoiData.LatTwo, newPoiData.LngTwo)));
            };

            // Removes a POI given the accessCode, typeId, and poiId
            Delete["/DataLines"] = parameters =>
            {
                DeleteIDData newPoiData = this.Bind<DeleteIDData>();
                if (!IsAuthorized(newPoiData.AccessCode))
                {
                    return this.Response.AsJson(new string[0], HttpStatusCode.BlockedByWindowsParentalControls);
                }

                Global.MapsStore.DeletePoiLine(newPoiData.TypeId, newPoiData.PoiId);
                return this.Response.AsJson(string.Empty);
            };

            // Adds a new POI given the accessCode, typeId, and latLng, returning the typeId and poiId
            Post["/Data"] = parameters =>
            {
                NewPoiData newPoiData = this.Bind<NewPoiData>();

                if (!IsAuthorized(newPoiData.AccessCode))
                {
                    return this.Response.AsJson(new string[0], HttpStatusCode.BlockedByWindowsParentalControls);
                }

                return this.Response.AsJson(Global.MapsStore.AddPoi(newPoiData.TypeId, newPoiData.LatLng));
            };

            // Removes a POI given the accessCode, typeId, and poiId
            Delete["/Data"] = parameters =>
            {
                DeleteIDData newPoiData = this.Bind<DeleteIDData>();
                if (!IsAuthorized(newPoiData.AccessCode))
                {
                    return this.Response.AsJson(new string[0], HttpStatusCode.BlockedByWindowsParentalControls);
                }

                Global.MapsStore.DeletePoi(newPoiData.TypeId, newPoiData.PoiId);
                return this.Response.AsJson(string.Empty);
            };
        }

        private bool IsAuthorized(string accessCode)
            => accessCode != null && accessCode.Equals(ConfigurationManager.AppSettings["MapAccessCode"]);
    }
}