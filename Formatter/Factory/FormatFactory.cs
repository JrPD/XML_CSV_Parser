using System;
using System.Net.Http.Formatting;
using Formatter.Formatter;
using Formatter.Parser;

namespace Formatter.Factory
{
	public class FormatFactory
	{
		public static BufferedMediaTypeFormatter GetFormatter(string type)
		{
			// detect parse type
			ParseType formatType;
			try
			{
				formatType = (ParseType)Enum.Parse(typeof(ParseType), type);
			}
			catch (Exception)
			{
				formatType = ParseType.XML;
			}

			// select instance of which formatter must be returned
			switch (formatType)
			{
				case ParseType.XML:
				{
					return new XMLFormatter();
				}
				case ParseType.CSV:
				{
					return new CSVFormatter();
				}
				default:
				{
					return new XMLFormatter();
				}
			}
		}
	}
}
