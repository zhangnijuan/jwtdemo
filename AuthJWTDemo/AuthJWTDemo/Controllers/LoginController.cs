using AuthJWTDemo.Models;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;

namespace AuthJWTDemo.Controllers
{
    public class LoginController : Controller
    {
        
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginServer(string uname, string pwd)
        {
            if (uname == "admin"&&pwd=="123")
            {
                DateTime utime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                AuthInfo auth = new AuthInfo()
                {
                    Name = "zhangnijuan",
                    Uid = 1,
                    Exp = (int)Math.Round((DateTime.Now - utime).TotalMilliseconds) + 60
                };
                // var secretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
                var secretKey = Request.UserAgent;
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
                IJsonSerializer serializer = new JsonNetSerializer();
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
                var token = encoder.Encode(auth, secretKey);
                var refresh_token = Guid.NewGuid();
                return Json(new { code = 50000, message = "登录成功", token = token, refresh_token= refresh_token }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(new { code = 50001, message = "用户名或者密码错误" }, JsonRequestBehavior.DenyGet);
            }            
        }

        [HttpPost]
        public ActionResult GetUser()
        {
            var auth = CallContext.GetData("auth") as AuthInfo;

            return View();
        }
    }
}