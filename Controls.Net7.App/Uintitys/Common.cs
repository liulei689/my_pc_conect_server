using Controls.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Controls.Net7.App.Uintitys
{
    public static class Common
    {
        public static string GetUrl(this string api) 
        {
         return  DefalutConfig.BaseUrl + api;
        }
    }
}
