using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HT.APPStore.Models
{
    public class ResponseMessage
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public HttpStatusCode Code { get; set; }
    }
}
