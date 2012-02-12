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
	}
}