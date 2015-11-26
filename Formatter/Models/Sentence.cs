using System.Collections.Generic;
using System.Xml.Serialization;

namespace Formatter.Models
{
	public class Sentence
	{
		[XmlElement("Word")]
		public List<Word> Words { get; private set; }

		public Sentence()
		{
			Words = new List<Word>();
		}

		public void Add(Word item)
		{
			Words.Add(item);
		}
	}
}