using BingMapsRESTToolkit;
using MyEnquiry_BussniessLayer.ViewModels.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEnquiry_BussniessLayer.Helper
{
    public class Location
    {
       static string myapikey= "";
        public static async Task<latAndlng> GetByAddress(string address)
        {
            var model = new latAndlng();
            var request = new GeocodeRequest();
            request.BingMapsKey = myapikey;

            request.Query = address;

           

            var result = await request.Execute();
            if (result.StatusCode == 200)
            {
                var toolkitLocation = (result?.ResourceSets?.FirstOrDefault())
                    ?.Resources?.FirstOrDefault() as BingMapsRESTToolkit.Location;
                model.lat =(decimal) toolkitLocation.Point.Coordinates[0];
                model.lng = (decimal)toolkitLocation.Point.Coordinates[1];

            }

            return model;

        }
    }
}
