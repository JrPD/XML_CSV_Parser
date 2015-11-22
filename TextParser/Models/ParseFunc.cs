using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using Microsoft.Ajax.Utilities;

namespace TextParser.Models
{

	public static class ParseFunc
	{
		// parse text into revalent model classes
		public static Text ParseInputText(string inputText)
		{
			RemoveSpecialCharacters(ref inputText);
			var sentences = SplitTextIntoSentences(inputText);

			// parse to revalent models
			Text parsedText = SplitSentencesIntoWords(sentences);
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
					.Select(word => new Word() { item = word }).ToList();

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
			return sentences.Where(w => w != "").ToArray();
		}

		private static void RemoveSpecialCharacters(ref string inputText)
		{
			char[] specialCharacters =
				{
					'\\', '/', '"', '(', ')', '-', '-', '[',']'
				};

			var res = specialCharacters.Aggregate(inputText, (current, character) => current.Replace(character, ' '));
			inputText = Regex.Replace(res, @"\s+$\n|\r", "", RegexOptions.Multiline).TrimEnd();//Regex.Replace(inputText, @"\s+", " ");
		}

		/// <summary>
		/// parse model to XML format. Save file to disk
		/// </summary>
		/// <param name="text">input text</param>
		/// <returns>XML formatted text</returns>
		public static string ParseToCSV(Text text)
		{
			try
			{
				// write respons to string
				StringBuilder sb = new StringBuilder();
				char separator = ',';

				for (int i = 0; i < text.Sentences.Count; i++)
				{
					sb.AppendFormat("Sentence {0}: ", i + 1);
					var count = text.Sentences[i].Words.Count;

					// write each word in sentence
					for (int j = 0; j < count; j++)
					{
						var word = text.Sentences[i].Words[j].item;
						// do not show separator, if last word
						if (j == count - 1)
						{
							sb.AppendFormat("{0}", word );
							continue;
						}
						sb.AppendFormat("{0}{1} ", word, separator);
					}
					sb.Append("\n");
				}
				// save parsed xml to disk. access by link "/api/text"
				// controller should return data in XML format
				ParseToXML(text);
				return sb.ToString();
			}
			catch (Exception)
			{
				return "Error occured";
			}
			
		}
		//todo override tostring()
		/// <summary>
		/// parse model to CSV format. Save file to disk
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string ParseToXML(Text text)
		{
			// write to memory stream. It good way convert to string then.
			using (var ms = new MemoryStream())
			{
				try
				{
					// define xml writer with options
					var xmlWriter = new XmlTextWriter(ms, Encoding.GetEncoding(65001))
					{
						Formatting = Formatting.Indented,
						IndentChar = '\t',
						Indentation = 1,
						QuoteChar = '\''
					};

					// write xml document
					xmlWriter.WriteStartDocument(true); // <?xml version="1.0"?>
					xmlWriter.WriteStartElement(TextItems.text.ToString()); // <Text>

					// write sentences
					foreach (var sentence in text.Sentences)
					{
						xmlWriter.WriteStartElement(TextItems.sentence.ToString()); // <Sentence>
						
						// write words
						foreach (var word in sentence.Words)
						{
							xmlWriter.WriteStartElement(TextItems.word.ToString()); // </Word>
							xmlWriter.WriteString(word.item);
							xmlWriter.WriteEndElement(); // </Word>
						}
						xmlWriter.WriteEndElement(); // </Sentence>
					}
					xmlWriter.WriteEndElement(); // </Text>
					xmlWriter.Close();

					// convert to string.
					var res = Encoding.UTF8.GetString(ms.ToArray());

					// save to file.
					SaveToFile(res);
					return res;

				}
				catch (Exception)
				{
					return "Error occured";
				}
			}
		}

		/// <summary>
		/// save string to disk
		/// </summary>
		/// <param name="res">text string</param>
		private static void SaveToFile(string res)
		{
			var path = GetFileName();
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			using (StreamWriter file = new System.IO.StreamWriter(GetFileName()))
			{
				file.Write(res);
			}
		}

		/// <summary>
		/// return output.xml location
		/// </summary>
		/// <returns>path to output file</returns>
		public static string GetFileName()
		{
			string path = HttpRuntime.AppDomainAppPath;

			// get file name from web.config
			string filename = ConfigurationManager.AppSettings["filePath"];
			if (filename.IsNullOrWhiteSpace())
			{
				filename = "App_Data\\output.xml";
			}
			return  path + filename;
		}
	}
}