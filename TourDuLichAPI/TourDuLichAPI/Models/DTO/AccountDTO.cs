using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourDuLichAPI.Models.DTO
{
    public class AccountDTO : Customer
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int AccountType { get; set; }
        public bool Admin { get; set; }
        public bool Active { get; set; }
        public double Balance { get; set; }
        public string Token { get; set; }
    }
}