using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AuthJWTDemo.Models
{
    public class AuthInfo
    {
        public int Uid { get; set; }
        public string Name { get; set; }
        public int Exp { get; set; }
    }
}