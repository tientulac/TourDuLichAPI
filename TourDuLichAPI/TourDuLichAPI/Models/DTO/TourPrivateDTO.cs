using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourDuLichAPI.Models.DTO
{
    public class TourPrivateDTO : TourPrivate
    {
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public string VehicleName { get; set; }
        public string StatusName { get; set; }
        public string HotelName { get; set; }
        public string AccountName { get; set; }
    }
}