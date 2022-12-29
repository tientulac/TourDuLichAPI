using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using TourDuLichAPI.Models;

namespace TourDuLichAPI.Controllers
{
    public class ProfileController: ApiController
    {
        private LinqDataContext db = new LinqDataContext();

        [Route("api/profile/GetProfile/{account_id}")]
        public ResponseBase<Customer> GetProfile(int account_id)
        {
            var ma = db.Customers.Where(x => x.AccountId == account_id).FirstOrDefault().CustomerId;
            var customer = db.Customers.Where(x => x.AccountId == account_id).FirstOrDefault() ?? null;
            return new ResponseBase<Customer>()
            {
                data = customer,
                message = "Success",
                status = 200
            };
        }

        [Route("api/profile/NapTien/{account_id}")]
        public ResponseBase<bool> NapTien(int account_id)
        {
            var tk = db.Accounts.Where(x => x.AccountId == account_id).FirstOrDefault();
            tk.Balance = tk.Balance > 0 ? tk.Balance : 0;
            tk.Balance += 5000000;
            db.SubmitChanges();
            return new ResponseBase<bool>()
            {
                data = true,
                message = "Success",
                status = 200
            };
        }

        [HttpPost]
        [Route("api/profile/updateProfile")]
        public ResponseBase<bool> UpdateProfile(Customer req)
        {
            try
            {
                var cus = db.Customers.Where(x => x.CustomerId == req.CustomerId).FirstOrDefault();
                if (cus.CustomerId > 0)
                {
                    cus.CustomerName = req.CustomerName;
                    cus.DOB = req.DOB;
                    cus.PassPortCode = req.PassPortCode;
                    cus.Gender = req.Gender;
                    cus.Address = req.Address;
                    db.SubmitChanges();
                    return new ResponseBase<bool>()
                    {
                        data = true,
                        message = "Success",
                        status = 200
                    };
                }
                return new ResponseBase<bool>()
                {
                    data = false,
                    message = "Not Found",
                    status = 404
                };
            }
            catch(Exception ex) {
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