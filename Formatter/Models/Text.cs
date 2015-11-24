using System.Collections.Generic;

namespace Formatter.Models
{
	/// <summary>
	/// class that represent text, parsed into revalent models
	/// </summary>
	public class Text
	{
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