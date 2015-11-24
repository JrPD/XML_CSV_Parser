using System;
using System.Text;
using Formatter.Models;

namespace Formatter.Formatter
{
	class CSVFormatter : IFormatter
	{
		public CSVFormatter()
		{
			
		}

		/// <summary>
		/// parse model to CSV format. Save file to disk
		/// </summary>
		/// <param name="text">input text</param>
		/// <returns>XML formatted text</returns>
		string IFormatter.Format(Text text)
		{
			try
			{
				// write respons to string
				StringBuilder sb = new StringBuilder();
				char separator = ',';

				for (int i = 0; i < text.Sentences.Count; i++)
				{
					sb.AppendFormat("Sentence {0}", i + 1);
					var count = text.Sentences[i].Words.Count;

					// write each word from sentence
					for (int j = 0; j < count; j++)
					{
						var word = text.Sentences[i].Words[j].Item;
						sb.AppendFormat("{0} {1}", separator, word);
					}
					sb.Append("\n");
				}
				// save parsed xml to disk. access by link "/api/text"
				// api controller returns data in XML format
				
				//todo inclide parsetoXML?
				//ParseToXML(text);
				return sb.ToString();
			}
			catch (Exception)
			{
				return "Error occured";
			}
		}
	}
}
