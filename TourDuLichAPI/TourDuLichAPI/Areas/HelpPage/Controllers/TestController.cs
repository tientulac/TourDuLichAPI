using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace TourDuLichAPI.Areas.HelpPage.Controllers
{
    public class TestController: ApiController
    {
        public string Test1()
        {
            return "123";
        }

        public string Test2()
        {
            return "234";
        }
    }
}