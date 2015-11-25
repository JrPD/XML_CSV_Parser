using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using Formatter.Models;
using Formatter.Parser;

namespace Formatter.Formatter
{
	public class XMLFormatter : BufferedMediaTypeFormatter
	{
		public XMLFormatter()
		{
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml"));

			// New code:
			SupportedEncodings.Add(Encoding.UTF8);
			//SupportedEncodings.Add(Encoding.GetEncoding("iso-8859-1"));
			this.MediaTypeMappings.Add(new QueryStringMapping("type", "xml",

			new MediaTypeHeaderValue("application/xml")));
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

		public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
		{
			var text = value as Text;
			if (text == null)
			{
				throw new InvalidOperationException("Cannot serialize type");
			}

			Encoding effectiveEncoding = SelectCharacterEncoding(content.Headers);

			using (var writer = new StreamWriter(writeStream, effectiveEncoding))
			{
				try
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
				}
				catch (Exception)
				{
					throw new InvalidOperationException("Cannot serialize type");
				}
			}
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
