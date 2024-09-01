using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Constants;

namespace PaymentsApi.Models
{
    public class PublicRoutes
    {
        public string publicRoutes { get; set; }

        public static bool IsPublicRoute(string route)
        {
            return route == PublicRoutesConst.autenticate  ||
                   route == PublicRoutesConst.swaggerData  ||
                   route == PublicRoutesConst.swaggerIndex ||
                   route == PublicRoutesConst.swagger;
        }
    }
}