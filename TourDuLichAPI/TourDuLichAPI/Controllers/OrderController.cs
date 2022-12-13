using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using TourDuLichAPI.Models;
using TourDuLichAPI.Models.DTO;

namespace TourDuLichAPI.Controllers
{
    public class OrderController : ApiController
    {
        private LinqDataContext db = new LinqDataContext();

        [HttpGet]
        [Route("api/order/GetOrderByCustomer/{account_id}")]
        public ResponseBase<List<Order>> GetOrderByCustomer(int account_id)
        {
            try
            {
                var customerId = db.Customers.Where(x => x.AccountId == account_id).FirstOrDefault().CustomerId;
                var listTicket = db.Orders.Where(x => x.CustomerId == customerId).ToList() ?? null;
                return new ResponseBase<List<Order>>()
                {
                    data = listTicket,
                    message = "Success",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseBase<List<Order>>()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }

        [HttpGet]
        [Route("api/order/CancelOrder/{id}")]
        public ResponseBase<bool> CancelOrder(int id)
        {
            try
            {
                var _order = db.Orders.Where(x => x.OrderId == id).FirstOrDefault();
                var customerId = _order.CustomerId;
                var acc_id = db.Customers.Where(x => x.CustomerId == customerId).FirstOrDefault().AccountId;
                var tk = db.Accounts.Where(x => x.AccountId == acc_id).FirstOrDefault();
                tk.Balance = tk.Balance > 0 ? tk.Balance : 0;
                _order.Status = 2;
                _order.DeletedAt = DateTime.Now;

                if (_order.CreatedAt.GetValueOrDefault().AddDays(3) > DateTime.Now)
                {
                    tk.Balance += _order.TotalPrice;
                    db.SubmitChanges();
                    return new ResponseBase<bool>()
                    {
                        data = true,
                        message = "Success",
                        status = 200
                    };
                }
                else if (_order.CreatedAt.GetValueOrDefault().AddDays(5) > DateTime.Now)
                {
                    tk.Balance += _order.TotalPrice * (float)(70 / 100);
                    db.SubmitChanges();
                    return new ResponseBase<bool>()
                    {
                        data = true,
                        message = "Success",
                        status = 200
                    };
                }
                else if (_order.CreatedAt.GetValueOrDefault().AddDays(8) > DateTime.Now)
                {
                    tk.Balance += _order.TotalPrice / 2;
                    db.SubmitChanges();
                    return new ResponseBase<bool>()
                    {
                        data = true,
                        message = "Success",
                        status = 200
                    };
                }
                else
                {
                    return new ResponseBase<bool>()
                    {
                        data = false,
                        message = "Bạn chỉ có thể hủy tour trong vòng 3 ngày từ lúc đặt vé.",
                        status = 500
                    };
                }
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
        [Route("api/order/buyTicket")]
        public ResponseBase<bool> BuyTicket(OrderDTO req)
        {
            try
            {
                var customerId = db.Customers.Where(x => x.AccountId == req.AccountId).FirstOrDefault().CustomerId;
                var ticketGrowup = db.Tickets.Where(x => x.TicketType == 2).FirstOrDefault().Price ?? 0;
                var ticketChild = db.Tickets.Where(x => x.TicketType == 1).FirstOrDefault().Price ?? 0;

                req.CreatedAt = DateTime.Now;
                req.QRCode = JsonConvert.SerializeObject(req);
                req.CustomerId = customerId;
                var _tour = db.Tours.Where(x => x.TourId == req.TourId).FirstOrDefault();
                var _order = new Order();
                _order.Status = req.Status;
                _order.CreatedAt = DateTime.Now;
                _order.QRCode = req.QRCode;
                _order.CustomerId = req.CustomerId;
                _order.TotalPrice = ticketGrowup * (req.TicketGrowup ?? 0) + ticketChild * (req.TicketChild ?? 0) + (_tour.Price ?? 0);
                _order.TicketGrowup = req.TicketGrowup ?? 0;
                _order.TicketChild = req.TicketChild ?? 0;
                _order.TourId = req.TourId;

                if (db.Accounts.Where(x => x.AccountId == req.AccountId).FirstOrDefault().Balance - _order.TotalPrice < 0)
                {
                    return new ResponseBase<bool>()
                    {
                        data = false,
                        message = "Số dư không đủ",
                        status = 500
                    };
                }

                db.Orders.InsertOnSubmit(_order);
                db.SubmitChanges();

                var tk = db.Accounts.Where(x => x.AccountId == req.AccountId).FirstOrDefault();
                tk.Balance -= _order.TotalPrice;
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