using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Formatter.Models;
using Formatter.Parser;

namespace Formatter.Formatter
{
	public class XMLFormatter : MediaTypeFormatter
	{
		public XMLFormatter()
		{
			// display result as text. No need parse doc in JS code
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));

			SupportedEncodings.Add(Encoding.UTF8);
			this.MediaTypeMappings.Add(new QueryStringMapping("type", "text",
				new MediaTypeHeaderValue("text/palin")));
		}
		public override bool CanReadType(Type type)
		{
			return false;
		}

		public override bool CanWriteType(Type type)
		{
			if (type == typeof (Text))
			{
				return true;
			}
			return false;
		}

		public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
			TransportContext transportContext)
		{
			var taskSource = new TaskCompletionSource<object>();
			try
			{
				var text = value as Text ?? new Text();
				Encoding effectiveEncoding = SelectCharacterEncoding(content.Headers);

				using (var writer = new StreamWriter(writeStream, effectiveEncoding))
				{
					// define xml writer with options
					//todo accept encoding from headers
					var xmlWriter = new XmlTextWriter(writer)
					{
						Formatting = Formatting.Indented,
						IndentChar = '\t',
						Indentation = 1,
						QuoteChar = '\''
					};

					WriteDoc(xmlWriter, text);
					taskSource.SetResult(null);
				}
			}
			catch (Exception e)
			{
				taskSource.SetException(e);
			}
			return taskSource.Task;
		}

		private void WriteDoc(XmlWriter writer, Text text)
		{
			// write xml document
			writer.WriteStartDocument(true);		//true						// <?xml version="1.0"?>
			writer.WriteStartElement(TextItems.text.ToString());			// <Text>

			// write sentences
			foreach (var sentence in text.Sentences)
			{
				writer.WriteStartElement(TextItems.sentence.ToString());	// <Sentence>

				// write words
				foreach (var word in sentence.Words)
				{
					writer.WriteStartElement(TextItems.word.ToString());	// </Word>
					writer.WriteString(word.Item);
					writer.WriteEndElement();								// </Word>
				}
				writer.WriteEndElement();									// </Sentence>
			}
			writer.WriteEndElement();										// </Text>
			writer.Close();
		}
	}
}
