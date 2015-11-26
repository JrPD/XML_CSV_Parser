using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using Formatter.Formatter;

namespace TextParser
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();

			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "api/{controller}/{type}",
				defaults: new { type = RouteParameter.Optional }
			);
			//ConfigureApis(config);
		}
		public static void ConfigureApis(HttpConfiguration config)
		{
			config.Formatters.Insert(0, new CSVFormatter());
			config.Formatters.Insert(1, new XMLFormatter());
		}
	}
}
