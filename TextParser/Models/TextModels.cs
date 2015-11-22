using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;

namespace TextParser.Models
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

		//public override string ToString()
		//{
		//	return base.ToString();
		//}
	}

	public class Word:IComparable
	{
		public string item { get; set; }

		public int CompareTo(object obj)
		{
			return String.Compare(item, ((Word)obj).item, StringComparison.CurrentCulture);
		}
	}

	enum ParseType
	{
		XML,
		CSV
	}
	enum TextItems
	{
		text,
		sentence,
		word
	}
}