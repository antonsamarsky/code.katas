using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace StringCalculator
{
	public class StringCalculator
	{
		const string DefaultDelimiter = ",";
		const string SingleDelimiterRegexp = @"//(?<delimiter>\W+)\n";
		const string MultipleDelimiterRegexp = @"//(?<delimiter>\W+)\n";

		public void ExecuteAddCyclic()
		{
			var input = Console.ReadLine();
			if (string.IsNullOrEmpty(input))
			{
				return;
			}

			this.ExecuteAdd(input);
			Console.WriteLine(string.Format("another input please"));

			this.ExecuteAddCyclic();
		}

		public void ExecuteAdd(string input)
		{
			var args = input.Trim().ToLowerInvariant().Split(' ');

			var command = args[0];
			if (command != "scalc")
			{
				return;
			}

			var commandArg = args[1].Replace("'", string.Empty);
			var result = this.Add(commandArg).ToString(CultureInfo.InvariantCulture);

			Console.WriteLine(string.Format("The result is {0}", result));
		}

		public int Add(string input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}

			if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
			{
				return 0;
			}

			var delimiter = DefaultDelimiter;

			var delimiterRegExp = new Regex(SingleDelimiterRegexp);
			if (delimiterRegExp.IsMatch(input))
			{
				var multipleDelimiterRegexp = new Regex(MultipleDelimiterRegexp);
				delimiter = multipleDelimiterRegexp.IsMatch(input) ?
					multipleDelimiterRegexp.Match(input).Result("${delimiter}") :
					delimiterRegExp.Match(input).Result("${delimiter}");

				input = delimiterRegExp.Replace(input, string.Empty);
			}

			var invalidDelimiter = string.Format("{0}\n", delimiter);
			if (input.Contains(invalidDelimiter))
			{
				throw new InvalidOperationException("Invalid delimiter.");
			}

			var numbers = input
				.Split(delimiter.ToArray())
				.SelectMany(c => c.Split('\n'))
				.Where(c => !string.IsNullOrEmpty(c))
				.Select(c => c.Trim())
				.Select(int.Parse).ToArray();

			if (numbers.Any(n => n < 0))
			{
				var negativesValues = string.Join(", ", numbers.Where(n => n < 0).Select(n => n.ToString(CultureInfo.InvariantCulture)).ToArray());
				throw new InvalidOperationException(string.Concat("There are negatives values: ", negativesValues));
			}

			return numbers.Where(n => n < 1000).Sum();
		}

		public void AddToConsole(string input)
		{
			Console.WriteLine(this.Add(input));
		}
	}
}