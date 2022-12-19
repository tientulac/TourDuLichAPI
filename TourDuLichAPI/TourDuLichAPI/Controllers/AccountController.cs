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
    public class AccountController : ApiController
    {
        private LinqDataContext db = new LinqDataContext();

        [HttpPost]
        [Route("api/account/login")]
        public ResponseBase<AccountDTO> Login(Account req)
        {
            try
            {
                var check = db.Accounts.Where(x => x.UserName == req.UserName && x.Password == req.Password);
                if (check.Any())
                {
                    var _taikhoan = (from a in db.Accounts.Where(x => x.AccountId == check.FirstOrDefault().AccountId)
                                     select new AccountDTO
                                     {
                                         AccountId = a.AccountId,
                                         UserName = a.UserName,
                                         Password = a.Password,
                                         Email = a.Email,
                                         Phone = a.Phone,
                                         Balance = a.Balance.GetValueOrDefault(),
                                         AccountType = a.AccountType.GetValueOrDefault(),
                                         Admin = a.Admin.GetValueOrDefault(),
                                         Active = a.Active.GetValueOrDefault(),
                                         Token = createToken(a.UserName)
                                     }).FirstOrDefault();
                    return new ResponseBase<AccountDTO>()
                    {
                        data = _taikhoan,
                        message = "Success",
                        status = 200
                    };
                }
                else
                {
                    return new ResponseBase<AccountDTO>()
                    {
                        message = "Account Info is not correct",
                        status = 404
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseBase<AccountDTO>()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }

        [HttpPost]
        [Route("api/account/register")]
        public ResponseBase<AccountDTO> Register(AccountDTO req)
        {
            try
            {
                var _acc = new Account();
                var _customer = new Customer();

                _acc.UserName = req.UserName;
                _acc.Password = req.Password;
                _acc.Email = req.Email;
                _acc.Phone = req.Phone;
                _acc.AccountType = 2;
                _acc.Admin = req.Admin;
                _acc.Active = req.Active;
                _acc.Balance = 0;
                db.Accounts.InsertOnSubmit(_acc);
                db.SubmitChanges();

                _customer.CustomerName = req.CustomerName;
                _customer.DOB = req.DOB;
                _customer.PassPortCode = req.PassPortCode;
                _customer.Gender = req.Gender;
                _customer.Address = req.Address;
                _customer.AccountId = _acc.AccountId;
                db.Customers.InsertOnSubmit(_customer);
                db.SubmitChanges();
                return new ResponseBase<AccountDTO>()
                {
                    data = req,
                    message = "Success",
                    status = 200
                };
            }
            catch (Exception ex)
            {
                return new ResponseBase<AccountDTO>()
                {
                    data = null,
                    message = ex.Message,
                    status = 500
                };
            }

        }

        [HttpGet]
        [Route("api/account/balance/{id}")]
        public ResponseBase<double> Balance(int id = 0)
        {
            try
            {
                return new ResponseBase<double>()
                {
                    data = db.Accounts.Where(x => x.AccountId == id).FirstOrDefault().Balance.GetValueOrDefault(),
                    message = "Success",
                    status = 500
                };
            }
            catch (Exception ex)
            {
                return new ResponseBase<double>()
                {
                    message = ex.Message,
                    status = 500
                };
            }
        }


        public static string createToken(string Username)
        {
            DateTime issuedAt = DateTime.UtcNow;
            DateTime expires = DateTime.UtcNow.AddDays(10);

            var tokenHandler = new JwtSecurityTokenHandler();

            var userIdentity = new ClaimsIdentity("Identity");
            userIdentity.Label = "Identity";
            userIdentity.AddClaim(new Claim(ClaimTypes.Name, Username));
            var claims = new List<Claim>();
            string sec = "088881139703564148785";
            var identity = new ClaimsPrincipal(userIdentity);
            Thread.CurrentPrincipal = identity;
            var now = DateTime.UtcNow;
            var securityKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
            var signingCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(securityKey, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature);

            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: "http://unisoft.edu.vn/", audience: "http://unisoft.edu.vn/",
                        subject: userIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }


    }
}