using Community.WebApi.App_Start;
using Community.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Community.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //跨域配置
            //config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

       

            // Web API 路由
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            GlobalConfiguration.Configuration.MessageHandlers.Add(new CustomDelegatingHandler());
            config.Filters.Add(new WebApiExceptionFilterAttribute()); 
        }
    }
}
