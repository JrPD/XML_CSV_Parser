using System;
using System.IO;
using System.Text;
using System.Xml;
using Formatter.Models;
using Formatter.Parser;

namespace Formatter.Formatter
{
	class XMLFormatter : IFormatter
	{
		/// <summary>
		/// parse model to XML format.
		/// </summary>
		/// <param name="text">text, parset into revalent model classes</param>
		/// <returns></returns>
		public string Format(Text text)
		{
			// write to memory stream. It good way convert to string then.
			using (var ms = new MemoryStream())
			{
				try
				{
					// define xml writer with options
					//todo accept encoding from headers
					var xmlWriter = new XmlTextWriter(ms, Encoding.GetEncoding(65001))
					{
						Formatting = Formatting.Indented,
						IndentChar = '\t',
						Indentation = 1,
						QuoteChar = '\''
					};

					WriteDoc(ref xmlWriter, text);

					// convert to string.
					var res = Encoding.UTF8.GetString(ms.ToArray());

					// save to file.
					//SaveToFile(res);
					return res;

				}
				catch (Exception)
				{
					return "Error occured";
				}
			}
		}

		private void WriteDoc(ref XmlTextWriter xmlWriter, Text text)
		{
			// write xml document
			xmlWriter.WriteStartDocument(true);								// <?xml version="1.0"?>
			xmlWriter.WriteStartElement(TextItems.text.ToString());			// <Text>

			// write sentences
			foreach (var sentence in text.Sentences)
			{
				xmlWriter.WriteStartElement(TextItems.sentence.ToString()); // <Sentence>

				// write words
				foreach (var word in sentence.Words)
				{
					xmlWriter.WriteStartElement(TextItems.word.ToString()); // </Word>
					xmlWriter.WriteString(word.Item);
					xmlWriter.WriteEndElement();							// </Word>
				}
				xmlWriter.WriteEndElement();								// </Sentence>
			}
			xmlWriter.WriteEndElement();									// </Text>
			xmlWriter.Close();
		}
	}
}
