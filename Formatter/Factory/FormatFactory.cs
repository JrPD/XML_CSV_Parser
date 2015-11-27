using System;
using System.Net.Http.Formatting;
using Formatter.Formatter;
using Formatter.Parser;

namespace Formatter.Factory
{
	public class FormatFactory
	{
		public static MediaTypeFormatter GetFormatter(string type)
		{
			ParseType formatType = GetType(type);

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
	
		private static ParseType GetType(string type)
		{
			// detect parse type
			ParseType formatType;
			try
			{
				formatType = (ParseType) Enum.Parse(typeof (ParseType), type);
			}
			catch (Exception)
			{
				formatType = ParseType.XML;
			}
			return formatType;
		}

		enum ParseType
		{
			XML,
			CSV
		}
	}
}
