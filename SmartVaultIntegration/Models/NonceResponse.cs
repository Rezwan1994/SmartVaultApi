using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASPNET_MVC_HelloWorldSVRest.Models
{
    public class NonceResponse
    {
        public class Error
        {
            public bool success { get; set; }
        }

        public class Message
        {
            public string code { get; set; }
        }

        public class NonceRoot
        {
            public Error error { get; set; }
            public Message message { get; set; }
        }
    }
}