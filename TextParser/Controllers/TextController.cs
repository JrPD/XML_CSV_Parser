using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;
using Formatter.Factory;
using Formatter.Models;
using Formatter.Formatter;
using Formatter.Parser;
using TextParser.Classes;


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
		public XElement GetText()
		{
			string filePath = Func.GetFileName();
			
			var xml = XElement.Load(filePath);

			return (xml);
		}

		/// <summary>
		/// POST, get string from client, and return parsed text(XML or CSV)
		/// </summary>
		/// <param name="type">XML or CSV</param>
		/// <param name="inputText">text to parse</param>
		/// <returns>text, formatted into selected type</returns>
		[HttpPost]
		public HttpResponseMessage PostText(string type, [FromBody]string inputText)
		{
			// if string is empty 
			if (string.IsNullOrWhiteSpace(inputText))
			{
				return Request.CreateResponse(HttpStatusCode.NoContent, "Empty string");
			}

			// parse into revalent models
			Text text = Parser.ParseInputText(inputText);

			IFormatter formatter = FormatFactory.GetFormatter(type);
			string response = formatter.Format(text);

			// save to file
			Func.SaveToFile(response);

			return Request.CreateResponse(HttpStatusCode.OK, response);
		}
	}
}
	