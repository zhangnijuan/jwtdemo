using JWT;
using JWT.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;

namespace AuthJWTDemo.Models
{
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool flag = false;
            var tonken = httpContext.Request.Headers["auth"];
            if (tonken != null)
            {
                var secretKey = httpContext.Request.UserAgent;
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                try
                {
                    AuthInfo auth = decoder.DecodeToObject<AuthInfo>(tonken, secretKey, verify: true);
                    if (auth != null)
                    {
                        DateTime exp = new DateTime(auth.Exp, DateTimeKind.Utc);
                        if (DateTime.Now <= exp)
                        {
                            CallContext.SetData("auth", auth);
                            flag = true;
                        }
                    }
                }
                catch (Exception)
                {


                }

            }

            return flag;
        }
    }
}