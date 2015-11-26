using Formatter.Factory;
using Formatter.Formatter;
using Formatter.Models;
using Formatter.Parser;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Web.Http;
using System.Xml.Serialization;
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
		public HttpResponseMessage GetText()
		{
			string filePath = Func.GetFileName();
			string fileContent = File.ReadAllText(filePath);
			Text text = Parser.ParseInputText(fileContent);

			var xml = new XMLFormatter();
			//var xml = GlobalConfiguration.Configuration.Formatters.XmlFormatter;
			//xml.UseXmlSerializer = true;
			//xml.Indent = true;
			//xml.SetSerializer<Text>(new XmlSerializer(typeof(Text)));
		
			var content = new ObjectContent<Text>(
				text,										// What we are serializing
				xml,									// The media formatter
				// display result as xml document
				new MediaTypeHeaderValue("application/xml")	// The media formatter
				);												

			return new HttpResponseMessage()
			{
				StatusCode = HttpStatusCode.OK,
				Content = content,
			};
		}

		/// <summary>
		/// POST, get string from client, and return parsed text(XML or CSV)
		/// </summary>
		/// <param name="type">XML or CSV</param>
		/// <param name="inputText">text to parse</param>
		/// <returns>text, formatted into selected type</returns>
		[HttpPost]
		public HttpResponseMessage GetText(string type, [FromBody] string inputText)
		{
			// if string is empty
			if (string.IsNullOrWhiteSpace(inputText))
			{
				return Request.CreateResponse(HttpStatusCode.NoContent, "Empty string");
			}
			// save to file
			Func.SaveToFile(inputText);

			// parse into revalent models
			Text text = Parser.ParseInputText(inputText);

			// get formatter from type
			var formatter = FormatFactory.GetFormatter(type);

			// create respons content
			var content = new ObjectContent<Text>(
				text,							// What we are serializing
				formatter//,						// The media formatter
				//mediaTypeHeaderValue.MediaType	// The MIME (multimedia internet message exchange )type
				);

			return new HttpResponseMessage()
			{
				StatusCode = HttpStatusCode.OK,
				Content = content
			};
		}
	}
}