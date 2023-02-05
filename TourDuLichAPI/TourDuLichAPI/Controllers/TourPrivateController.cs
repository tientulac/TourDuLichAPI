using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TourDuLichAPI.Models;
using TourDuLichAPI.Models.DTO;

namespace TourDuLichAPI.Controllers
{
    public class TourPrivateController : ApiController
    {
        private LinqDataContext db = new LinqDataContext();

        [HttpGet]
        [Route("api/privatetour/GetPrivateTour")]
        public ResponseBase<List<TourPrivateDTO>> GetPrivateTour()
        {
            try
            {
                var listPrivateTour = (from a in db.TourPrivates
                                       select new TourPrivateDTO
                                       {
                                           TourPrivateId = a.TourPrivateId,
                                           LocationFromId = a.LocationFromId,
                                           LocationToId = a.LocationToId,
                                           Slot = a.Slot,
                                           Type = a.Type,
                                           StartDate = a.StartDate,
                                           ToDate = a.ToDate,
                                           HotelId = a.HotelId,
                                           VehicleId = a.VehicleId,
                                           Price = a.Status == 2 ? a.Price : 0,
                                           LocationFrom = db.Locations.Where(x => x.LocationId == a.LocationFromId).FirstOrDefault().LocationName ?? "",
                                           LocationTo = db.Locations.Where(x => x.LocationId == a.LocationToId).FirstOrDefault().LocationName ?? "",
                                           HotelName = db.Hotels.Where(x => x.HotelId == a.HotelId).FirstOrDefault().HotelName ?? "",
                                           VehicleName = db.Vehicles.Where(x => x.VehicleId == a.VehicleId).FirstOrDefault().VahicleName ?? "",
                                           StatusName = a.Status == 1 ? "Đang chờ duyệt" : a.Status == 2 ? "Duyệt thành công" : "Từ chối duyệt",
                                           Status = a.Status,
                                           AccountName = db.Accounts.Where(x => x.AccountId == a.AccountId).FirstOrDefault().UserName ?? ""
                                       }).ToList() ?? null;
                return new ResponseBase<List<TourPrivateDTO>>()
                {
                    data = listPrivateTour,
                    message = "Success",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseBase<List<TourPrivateDTO>>()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }

        [HttpGet]
        [Route("api/privatetour/GetPrivateTourByAccount/{account_id}")]
        public ResponseBase<List<TourPrivateDTO>> GetPrivateTourByAccount(int account_id)
        {
            try
            {
                var listPrivateTour = (from a in db.TourPrivates.Where(x => x.AccountId == account_id)
                                       select new TourPrivateDTO
                                       {
                                           TourPrivateId = a.TourPrivateId,
                                           LocationFromId = a.LocationFromId,
                                           LocationToId = a.LocationToId,
                                           Slot = a.Slot,
                                           Type = a.Type,
                                           StartDate = a.StartDate,
                                           ToDate = a.ToDate,
                                           HotelId = a.HotelId,
                                           VehicleId = a.VehicleId,
                                           Price = a.Status == 2 ? a.Price : 0,
                                           LocationFrom = db.Locations.Where(x => x.LocationId == a.LocationFromId).FirstOrDefault().LocationName ?? "",
                                           LocationTo = db.Locations.Where(x => x.LocationId == a.LocationToId).FirstOrDefault().LocationName ?? "",
                                           HotelName = db.Hotels.Where(x => x.HotelId == a.HotelId).FirstOrDefault().HotelName ?? "",
                                           VehicleName = db.Vehicles.Where(x => x.VehicleId == a.VehicleId).FirstOrDefault().VahicleName ?? "",
                                           StatusName = a.Status == 1 ? "Đang chờ duyệt" : a.Status == 2 ? "Duyệt thành công" : "Từ chối duyệt",
                                           Status = a.Status,
                                           AccountName = db.Accounts.Where(x => x.AccountId == a.AccountId).FirstOrDefault().UserName ?? ""
                                       }).ToList() ?? null;
                return new ResponseBase<List<TourPrivateDTO>>()
                {
                    data = listPrivateTour,
                    message = "Success",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseBase<List<TourPrivateDTO>>()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }

        [HttpPost]
        [Route("api/privatetour/BookingPrivateTour")]
        public ResponseBase<bool> BookingPrivateTour(TourPrivate req)
        {
            try
            {
                req.Status = 1;
                db.TourPrivates.InsertOnSubmit(req);
                db.SubmitChanges();
                return new ResponseBase<bool>()
                {
                    data = true,
                    message = "Success",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseBase<bool>()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }

        [HttpDelete]
        [Route("api/privatetour/DeletePrivateTour/{privatetour_id}")]
        public ResponseBase<bool> DeletePrivateTour(int privatetour_id)
        {
            try
            {
                var _t = db.TourPrivates.Where(M => M.TourPrivateId == privatetour_id).FirstOrDefault();
                db.TourPrivates.DeleteOnSubmit(_t);
                db.SubmitChanges();
                return new ResponseBase<bool>()
                {
                    data = true,
                    message = "Success",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseBase<bool>()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }

        [HttpPost]
        [Route("api/privatetour/UpdateStatus")]
        public ResponseBase<bool> UpdateStatus(TourPrivate req)
        {
            try
            {
                var _t = db.TourPrivates.Where(M => M.TourPrivateId == req.TourPrivateId).FirstOrDefault();
                _t.Status = req.Status;
                if (req.Status == 2)
                {
                    _t.Price = req.Price;
                }
                db.SubmitChanges();
                return new ResponseBase<bool>()
                {
                    data = true,
                    message = "Success",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseBase<bool>()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }
    }
}