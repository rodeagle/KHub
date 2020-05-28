using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.Caching;
using KHub.DAL;

namespace KHub
{
    public static class Identity
    {

        public static  HttpContext _context;

        public static void Init(HttpContext context)
        {
            _context = context;
        }

        public static bool IsSignedIn => _context != null &&
            _context.Request.Cookies["Credential"] != null &&
            MemoryCache.Default.Get(_context.Request.Cookies.First(x => x.Key == "Credential").Value) != null;

        public static User User
        {
            get
            {
                if (!IsSignedIn)
                    return null;
                var cookie = _context.Request.Cookies.First(x=> x.Key == "Credential").Value;

                var user = (User)MemoryCache.Default.Get(cookie);

                if (user == null)
                {
                    _context.Response.Cookies.Delete(cookie);
                }

                return user;
            }
        }

    }
}

