using AuthJWTDemo.Common;
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
            if (uname == "admin" && pwd == "123")
            {
                DateTime utime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                AuthInfo auth = new AuthInfo()
                {
                    Name = "zhangnijuan",
                    Uid = 2,
                    Exp = Convert.ToInt64(Math.Round((DateTime.Now - utime).TotalSeconds)) + 60
                };
                // var secretKey = "GQDstcKsx0NHjPOuXOYg5MbeJ1XT0uFiwDVvVBrk";
                string token = GetToken(auth);
                var cacheKey = "refreshToken:" + auth.Uid;
                RedisHelper redis = new RedisHelper(1);
                string refreshToken;
                if (string.IsNullOrEmpty(redis.StringGet(cacheKey)))
                {
                    refreshToken = Guid.NewGuid().ToString();
                    redis.StringSet(cacheKey, refreshToken,new TimeSpan(0,1,0));
                }
                else
                {
                    refreshToken = redis.StringGet(cacheKey);
                }

                return Json(new { code = 50000, message = "登录成功", token = token, refresh_token = refreshToken }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(new { code = 50001, message = "用户名或者密码错误" }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        [ApiAuthorize]
        public ActionResult GetUser()
        {
            var auth = CallContext.GetData("auth") as AuthInfo;

            return Json(new { auth = auth }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RefreshToken(int uid, string refreshToken)
        {
            var cacheKey = "refreshToken:" + uid;
            RedisHelper redis = new RedisHelper(1);
            var cache = redis.StringGet(cacheKey);
            if (cache != null)
            {
                DateTime utime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                AuthInfo auth = new AuthInfo()
                {
                    Name = "zhangnijuan",
                    Uid = 2,
                    Exp = Convert.ToInt64(Math.Round((DateTime.Now - utime).TotalSeconds)) + 60
                };

                string token = GetToken(auth);
                return Json(new { code = 50000, token = token }, JsonRequestBehavior.DenyGet);
            }
            else
            {
                return Json(new { code = 50001, message = "refreshToken已经过期，请从新登陆！" }, JsonRequestBehavior.DenyGet);
            }

        }

        private string GetToken(AuthInfo auth)
        {
            var secretKey = Request.UserAgent;
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var token = encoder.Encode(auth, secretKey);
            return token;
        }
    }
}