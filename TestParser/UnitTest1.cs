using System;
using System.Net.Http.Formatting;
using Formatter.Factory;
using Formatter.Models;
using Formatter.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestParser
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestParserType()
		{
			var inputText = "XML has been widely adopted as a way to format data in many contexts." +
			                " For example, you can find XML on the Web, in configuration files," +
			                " in Microsoft Office Word files, and in databases." +
			                "LINQ to XML is an up-to-date, redesigned approach to programming with XML." +
			                " It provides the in-memory document modification capabilities of " +
			                "the Document Object Model (DOM), and supports LINQ query expressions. " +
			                "Although these query expressions are syntactically different from XPath," +
			                " they provide similar functionality." +
			                " Assert.AreEqual(expected, actual, 0.001, Account not debited correctly;" +
			                "hi  .   dfd.   .text ,fine";


			var text = Parser.ParseInputText(inputText);
			Assert.IsInstanceOfType(text, typeof (Text), "Not Text format");
			Assert.IsNotNull(text, "Text is empty");

			foreach (var sentence in text.Sentences)
			{
				Assert.IsNotNull(sentence, "Sentense is null");
				Assert.IsTrue(sentence.Words.Count != 0, "Sentence does not conatains words");
			}

			inputText = "";
			text = Parser.ParseInputText(inputText);
			Assert.IsInstanceOfType(text, typeof(Text), "Not Text format");
			Assert.IsNotNull(text, "Text is empty");


			foreach (var sentence in text.Sentences)
			{
				Assert.IsNotNull(sentence, "Sentense is null");
				Assert.IsTrue(sentence.Words.Count != 0);
			}
		}


		[TestMethod]
		public void TestFormattersType()
		{
			var formatter = FormatFactory.GetFormatter("XML");
			Assert.IsInstanceOfType(formatter, typeof (BufferedMediaTypeFormatter), "Not Text format");

			var formatter1 = FormatFactory.GetFormatter("CSV");
			Assert.IsInstanceOfType(formatter1, typeof (BufferedMediaTypeFormatter), "Not Text format");

			var formatter2 = FormatFactory.GetFormatter("text");
			Assert.IsInstanceOfType(formatter2, typeof (BufferedMediaTypeFormatter), "Not Text format");
		}


		[TestMethod]
		public void TestCSVFormatter()
		{
			var formatter = FormatFactory.GetFormatter("XML");



		}
	}
}
