using System.Collections.Generic;

namespace Formatter.Models
{
	public class Sentence
	{
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