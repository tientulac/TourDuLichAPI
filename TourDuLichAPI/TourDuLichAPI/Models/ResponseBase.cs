using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TourDuLichAPI.Models
{
    public class ResponseBase<T>
    {
        public int status { get; set; }
        public string message { get; set; }
        public T data { get; set; }
    }
}