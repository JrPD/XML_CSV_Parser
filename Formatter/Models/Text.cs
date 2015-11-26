using System.Collections.Generic;
using System.Xml.Serialization;

namespace Formatter.Models
{
	/// <summary>
	/// class that represent text, parsed into revalent models
	/// </summary>
	[XmlRoot("Text")]
	public class Text
	{

		[XmlElement("Sentence")]
		public List<Sentence> Sentences { get; private set; }

		public Text()
		{
			Sentences = new List<Sentence>();
		}

		public void Add(Sentence item)
		{
			Sentences.Add(item);
		}
	}
}