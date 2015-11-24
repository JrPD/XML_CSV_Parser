using System.Collections.Generic;
using System.Linq;
using Formatter.Models;

namespace Formatter.Parser
{
	public static class Parser
	{
		// parse text into revalent model classes
		public static Text ParseInputText(string inputText)
		{
			RemoveSpecialCharacters(ref inputText);
			var sentences = SplitTextIntoSentences(inputText);

			// parse to revalent models
			var parsedText = SplitSentencesIntoWords(sentences);
			return parsedText;
		}

		private static Text SplitSentencesIntoWords(string[] sentences)
		{
			Text parsedText = new Text();

			char[] wordsDelimiterChars = { ' ', ',', ':', '\t' };
			foreach (var sent in sentences)
			{
				// split into words
				string[] words = sent.Split(wordsDelimiterChars);
				List<Word> listWords = words.Where(w => w != "")
					.Select(word => new Word() { Item = word }).ToList();

				Sentence sentence = new Sentence();

				foreach (var word in listWords)
				{
					sentence.Add(word);
				}

				// sort words
				sentence.Words.Sort();
				parsedText.Add(sentence);
			}
			return parsedText;
		}

		private static string[] SplitTextIntoSentences(string inputText)
		{
			// split into sentences
			char[] sentenceDelimiterChars = { '.', '!', '?', '\n' };
			string[] sentences = inputText.Split(sentenceDelimiterChars);
			return sentences.Where(w => !string.IsNullOrWhiteSpace(w)).ToArray();
		}

		private static void RemoveSpecialCharacters(ref string inputText)
		{
			char[] specialCharacters =
			{
				'\\', '/', '"', '(', ')', '-', '-', '[',']','{','}','#','$','%','&',';','@',':'
			};

			// replace to white space, becouse "word1@word2"
			inputText = specialCharacters.Aggregate(inputText, (current, character) => current.Replace(character, ' '));

			// remove repeated spaces, enter, tab
			// enter also is sentence delimeter

			//inputText = Regex.Replace(res, @"\s+$\n|\r", "", RegexOptions.Multiline).TrimEnd();//Regex.Replace(inputText, @"\s+", " ");
		}
	}
}
