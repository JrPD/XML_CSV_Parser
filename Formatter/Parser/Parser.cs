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

			char[] delimiterChars = { ' ', ',', ':', '\t' };
			foreach (var sent in sentences)
			{
				// split into words
				string[] words = sent.Split(delimiterChars);
				List<Word> listWords = words.Where(w => w != "")
					.Select(word => new Word() { Item = word }).ToList();

				Sentence sentence = AddWordsToSentence(listWords);

				// sort words
				sentence.Words.Sort();
				parsedText.Add(sentence);
			}
			return parsedText;
		}

		private static Sentence AddWordsToSentence(List<Word> listWords)
		{
			Sentence sentence = new Sentence();
			foreach (var word in listWords)
			{
				sentence.Add(word);
			}
			return sentence;
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
			//inputText = Regex.Replace(res, @"\s+$\n|\r", "", RegexOptions.Multiline).TrimEnd();//Regex.Replace(inputText, @"\s+", " ");
		}
	}
}
