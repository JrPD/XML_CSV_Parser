using System;

namespace Formatter.Models
{
	public class Word:IComparable
	{
		public string Item { get; set; }

		public int CompareTo(object obj)
		{
			return String.Compare(Item, ((Word)obj).Item, StringComparison.CurrentCulture);
		}
	}
}