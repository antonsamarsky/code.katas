using System;
using FluentAssertions;
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
		public int AddTest(string number)
		{
			var stringCalculator = new StringCalculator();
			int result = stringCalculator.Add(number);

			return result;
		}
	}
}
