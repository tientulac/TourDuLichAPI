using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TourDuLichAPI.Models;

namespace TourDuLichAPI.Controllers
{
    public class VoteTourController : ApiController
    {
        private LinqDataContext db = new LinqDataContext();

        [Route("api/votetour/{tour_id}")]
        public ResponseBase<List<VoteTour>> GetListVote(int tour_id)
        {
            var vote = db.VoteTours.Where(x => x.TourId == tour_id).ToList();
            return new ResponseBase<List<VoteTour>>()
            {
                data = vote,
                message = "Success",
                status = 200
            };
        }

        [Route("api/votetour/{account_id}")]
        public ResponseBase<List<VoteTour>> GetByAccount(int account_id)
        {
            var vote = db.VoteTours.Where(x => x.AccountId == account_id).ToList();
            return new ResponseBase<List<VoteTour>>()
            {
                data = vote,
                message = "Success",
                status = 200
            };
        }

        [HttpDelete]
        [Route("api/votetour/{id}")]
        public ResponseBase<bool> Delete(int id)
        {
            try
            {
                var vote = db.VoteTours.Where(x => x.VoteTourId == id).ToList();
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
                    data = false,
                    message = ex.Message,
                    status = 500
                };
            }
        }

        [HttpPost]
        [Route("api/votetour/insert")]
        public ResponseBase<bool> Insert(VoteTour req)
        {
            try
            {
                req.CreatedAt = DateTime.Now;
                db.VoteTours.InsertOnSubmit(req);
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
                    data = false,
                    message = ex.Message,
                    status = 500
                };
            }
        }
    }
}