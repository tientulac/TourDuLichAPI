using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TourDuLichAPI.Models;
using TourDuLichAPI.Models.DTO;

namespace TourDuLichAPI.Controllers
{
    public class TourController : ApiController
    {
        private LinqDataContext db = new LinqDataContext();

        public ResponseBase<List<TourDTO>> ListTour(string tourName = "", DateTime? startDate = null, string priceOrder = "", string locationFrom = "", string locationTo = "", double startPrice = 0, double endPrice = 0)
        {
            var lstTour = (from a in db.Tours
                           select new TourDTO
                           {
                               TourId = a.TourId,
                               TourName = a.TourName,
                               StartDate = a.StartDate,
                               EndDate = a.EndDate,
                               TourTime = a.TourTime,
                               LocationFrom = a.LocationFrom,
                               LocationTo = a.LocationTo,
                               Price = a.Price,
                               Poster = a.Poster,
                               Descrip = a.Descrip,
                               VehicleId = a.VehicleId,
                               VehicleName = db.Vehicles.Where(x => x.VehicleId == a.VehicleId).FirstOrDefault().VahicleName ?? "",
                           });
            if (!String.IsNullOrEmpty(tourName))
            {
                lstTour = lstTour.Where(x => x.TourName.ToLower().Contains(tourName.ToLower()));
            }
            if (!String.IsNullOrEmpty(locationFrom))
            {
                lstTour = lstTour.Where(x => x.LocationFrom.ToLower().Contains(locationFrom.ToLower()));
            }
            if (!String.IsNullOrEmpty(locationTo))
            {
                lstTour = lstTour.Where(x => x.LocationTo.ToLower().Contains(locationTo.ToLower()));
            }
            if (!String.IsNullOrEmpty(priceOrder))
            {
                if (priceOrder.Equals("ADSC"))
                {
                    lstTour = lstTour.OrderBy(x => x.Price);

                }
                else
                {
                    lstTour = lstTour.OrderByDescending(x => x.Price);
                }
            }
            if (startDate != null)
            {
                lstTour = lstTour.Where(x => x.StartDate >= startDate);
            }
            if (startPrice > 0)
            {
                lstTour = lstTour.Where(x => x.Price >= startPrice);
            }
            if (endPrice > 0)
            {
                lstTour = lstTour.Where(x => x.Price <= endPrice);
            }
            return new ResponseBase<List<TourDTO>>()
            {
                data = lstTour.ToList(),
                message = "Success",
                status = 200
            };
        }
    }
}