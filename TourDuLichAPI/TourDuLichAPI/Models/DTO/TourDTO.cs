using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourDuLichAPI.Models.DTO
{
    public class TourDTO : Tour
    {
        public string VehicleName { get; set; }
        public List<TourSchedule> ListSchedule { get; set; }
        public List<TourImage> ListTourImage { get; set; }
    }
}