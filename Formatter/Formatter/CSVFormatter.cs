using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Formatter.Models;

namespace Formatter.Formatter
{
	public class CSVFormatter : MediaTypeFormatter
	{
		public CSVFormatter()
		{
			SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/csv"));

			// New code:
			SupportedEncodings.Add(Encoding.UTF8);
			//SupportedEncodings.Add(Encoding.GetEncoding("iso-8859-1"));
			this.MediaTypeMappings.Add(new QueryStringMapping("type", "csv",

			new MediaTypeHeaderValue("text/csv")));
		}

		public override bool CanReadType(Type type)
		{
			return false;
		}

		public override bool CanWriteType(Type type)
		{
			if (type == typeof(Text))
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
				Encoding effectiveEncoding = SelectCharacterEncoding(content.Headers);

				using (var writer = new StreamWriter(writeStream, effectiveEncoding))
				{
					var text = value as Text;
					if (text == null)
					{
						text = new Text();
					}
					else
					{
						WriteItem(text, writer);
					}
					taskSource.SetResult(null);

				}
			}
			catch (Exception e)
			{
				taskSource.SetException(e);
			}
			return taskSource.Task;
		}

		private void WriteItem(Text text, StreamWriter writer)
		{
			char separator = ',';

			for (int i = 0; i < text.Sentences.Count; i++)
			{
				writer.Write("Sentence {0}", i + 1);
				var count = text.Sentences[i].Words.Count;

				// write each word from sentence
				for (int j = 0; j < count; j++)
				{
					var word = text.Sentences[i].Words[j].Item;
					writer.Write("{0} {1}", separator, word);
				}
				writer.Write("\n");
			}
		}
	}
}
