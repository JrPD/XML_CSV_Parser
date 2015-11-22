using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Ajax.Utilities;
using TextParser.Models;


// API Controller
// Get, Post requests
namespace TextParser.Controllers
{
	public class TextController : ApiController
	{
		/// <summary>
		/// GET, load xml file from disc
		/// </summary>
		/// <returns>return xml file</returns>
		[HttpGet]
		public XElement GetParsedText()
		{
			string filePath = ParseFunc.GetFileName();

			var xml = XElement.Load(filePath);

			return (xml);
		}

		/// <summary>
		/// POST, get string from client, and return parsed text(XML or CSV)
		/// </summary>
		/// <param name="type">XML or CSV</param>
		/// <param name="parseText">text to parse</param>
		/// <returns></returns>
		[HttpPost]
		public IHttpActionResult PostText(string type, [FromBody]string parseText)
		{
			// if string is empty 
			if (parseText == "")
			{
				return Ok("Empty string");
			}

			// parse to relevant models
			Text text = ParseFunc.ParseInputText(parseText);

			// detect parse type
			ParseType textType = (ParseType)Enum.Parse(typeof(ParseType), type);

			// switch type
			string response = "";
			switch (textType)
			{
				case ParseType.XML:
				{
					response = ParseFunc.ParseToXML(text);
					break;
				}
				case ParseType.CSV:
				{
					response = ParseFunc.ParseToCSV(text);
					break;
				}
				default:
				{
					break;
				}
			}
			return Ok(response);
		}
	}
}
	