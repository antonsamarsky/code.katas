using System;
using System.IO;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace StringCalculator
{
	[TestFixture]
	public class StringCalculatorTest
	{
		[Test]
		public void CtorTest()
		{
			var stringCalculator = new StringCalculator();
			stringCalculator.Should().NotBeNull();
		}

		[TestCase(null, ExpectedException = typeof(ArgumentNullException))]
		[TestCase("", Result = 0)]
		[TestCase(" ", Result = 0)]
		[TestCase("1", Result = 1)]
		[TestCase("999", Result = 999)]
		[TestCase("1,2", Result = 3)]
		[TestCase("1\n2,3", Result = 6)]
		[TestCase("1,\n", ExpectedException = typeof(InvalidOperationException))]
		[TestCase("//;\n1;2", Result = 3)]
		[TestCase("//;\n1;-2;5;-6", ExpectedException = typeof(InvalidOperationException), ExpectedMessage = "There are negatives values: -2, -6")]
		[TestCase("//+\n2+1001", Result = 2)]
		[TestCase("//[***]\n1***2***3", Result = 6)]
		[TestCase("//[*][%]\n1*2%3", Result = 6)]
		[TestCase("//[***][%]\n1***2%3", Result = 6)]
		[TestCase("//[***][%]\n1***2%3***5", Result = 11)]
		[TestCase("//[***][%][,]\n1***2%3,5", Result = 11)]
		public int AddTest(string input)
		{
			var stringCalculator = new StringCalculator();
			int result = stringCalculator.Add(input);

			return result;
		}

		[Test]
		[Description("Everytime you call Add(string) it also outputs the number result of the calculation in a new line to the terminal or console.")]
		public void AddToConsoleTest()
		{
			var input = "1\n2,3";

			var textWriter = Substitute.For<TextWriter>();
			Console.SetOut(textWriter);

			var stringCalculator = new StringCalculator();
			stringCalculator.AddToConsole(input);

			textWriter.Received().WriteLine(6);
		}

		[TestCase("scalc '1,2,3'", Result = "The result is 6")]
		[TestCase("scalc '1\n2'", Result = "The result is 3")]
		[Description("Create a program (test first)that uses string calculator, which the user can invoke through the terminal/console by calling “scalc ‘1,2,3’” and will output the following line before exiting: “The result is 6”")]
		public string ExecuteAddTest(string command)
		{
			var textReader = Substitute.For<TextReader>();
			Console.SetIn(textReader);
			var textWriter = Substitute.For<TextWriter>();
			Console.SetOut(textWriter);

			textReader.ReadLine().Returns(command);

			string result = null;
			textWriter
				.When(w => w.WriteLine(Arg.Any<string>()))
				.Do(c => result = c.Arg<string>());

			var stringCalculator = new StringCalculator();
			stringCalculator.ExecuteAdd(Console.ReadLine());

			return result;
		}

		[Test]
		[Description("Instead of exiting after the first result, the program will ask the user for “another input please” and print the result of the new user input out as well, until the user gives no input and just presses enter. in that case it will exit.")]
		public void ExecuteCommandCyclicTes()
		{
			var textReader = Substitute.For<TextReader>();
			Console.SetIn(textReader);
			var textWriter = Substitute.For<TextWriter>();
			Console.SetOut(textWriter);

			textReader.ReadLine().Returns("scalc '1,2,3");

			textWriter
				.When(t => t.WriteLine("another input please"))
				.Do(c => textReader.ReadLine().Returns(string.Empty));

			var stringCalculator = new StringCalculator();
			stringCalculator.ExecuteAddCyclic();

			textWriter.Received().WriteLine("The result is 6");
			textWriter.Received().WriteLine("another input please");
		}
	}
}
