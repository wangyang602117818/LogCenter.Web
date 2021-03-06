﻿using SSO.Util.Client;
using System.Web;
using System.Web.Mvc;

namespace MessageCenter.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new MyHandleErrorAttribute());
            filters.Add(new ValidateModelStateAttribute());
        }
    }
}
