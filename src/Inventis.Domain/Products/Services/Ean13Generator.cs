using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Inventis.Domain.Products.Services;

public static class Ean13Generator
{
	private static readonly Regex OnlyDigits = new(@"^\d+$", RegexOptions.Compiled);

	/// <summary>
	/// Generates a random valid EAN-13 code.
	/// </summary>
	/// <returns>13-digit EAN-13 as string</returns>
	public static string GenerateRandom()
	{
		var digits = new char[12];
		for (int i = 0; i < 12; i++)
		{
			digits[i] = (char)('0' + RandomNumberGenerator.GetInt32(0, 10));
		}

		var first12 = new string(digits);
		var checksum = ComputeChecksumDigit(first12);
		return first12 + checksum.ToString();
	}

	/// <summary>
	/// Generates an EAN-13 code using the provided numeric prefix (1..12 digits).
	/// If prefix length < 12, the remaining digits are filled with random digits.
	/// If prefix length == 12, simply computes checksum.
	/// </summary>
	/// <param name="numericPrefix">numeric string of length 1..12</param>
	/// <returns>13-digit EAN-13 as string</returns>
	public static string GenerateFromPrefix(string numericPrefix)
	{
		if (numericPrefix is null) throw new ArgumentNullException(nameof(numericPrefix));
		if (!OnlyDigits.IsMatch(numericPrefix)) throw new ArgumentException("Prefix must contain only digits 0-9.", nameof(numericPrefix));
		if (numericPrefix.Length < 1 || numericPrefix.Length > 12) throw new ArgumentException("Prefix length must be between 1 and 12.", nameof(numericPrefix));

		if (numericPrefix.Length == 12)
		{
			var checksum = ComputeChecksumDigit(numericPrefix);
			return numericPrefix + checksum;
		}

		// pad with random digits to 12
		var builder = numericPrefix.ToCharArray().ToList();
		while (builder.Count < 12)
		{
			builder.Add((char)('0' + RandomNumberGenerator.GetInt32(0, 10)));
		}

		var first12 = new string(builder.ToArray());
		var cs = ComputeChecksumDigit(first12);
		return first12 + cs;
	}

	/// <summary>
	/// Computes the EAN-13 checksum digit for the 12-digit input.
	/// Algorithm: sum of digits in odd positions (1,3,..,11) *1 + sum of digits in even positions (2,4,..,12) *3.
	/// checksum = (10 - (sum % 10)) % 10
	/// </summary>
	/// <param name="first12Digits">string of exactly 12 digits</param>
	/// <returns>checksum digit (0..9)</returns>
	public static int ComputeChecksumDigit(string first12Digits)
	{
		if (first12Digits is null) throw new ArgumentNullException(nameof(first12Digits));
		if (first12Digits.Length != 12) throw new ArgumentException("Input must be exactly 12 digits.", nameof(first12Digits));
		if (!OnlyDigits.IsMatch(first12Digits)) throw new ArgumentException("Input must contain only digits 0-9.", nameof(first12Digits));

		int sum = 0;

		for (int i = 0; i < 12; i++)
		{
			int digit = first12Digits[i] - '0';
			int weight = ((i + 1) % 2 == 1) ? 1 : 3;
			sum += digit * weight;
		}

		int mod = sum % 10;
		int checksum = (10 - mod) % 10;
		return checksum;
	}

	/// <summary>
	/// Validates a full 13-digit EAN-13 code.
	/// </summary>
	/// <param name="ean13">13-digit string</param>
	/// <returns>true if valid</returns>
	public static bool Validate(string ean13)
	{
		if (ean13 is null) return false;
		if (ean13.Length != 13) return false;
		if (!OnlyDigits.IsMatch(ean13)) return false;

		var first12 = ean13.Substring(0, 12);
		int expected = ComputeChecksumDigit(first12);
		int actual = ean13[12] - '0';
		return expected == actual;
	}

	/// <summary>
	/// Generates multiple EAN-13 codes (unique is not guaranteed).
	/// </summary>
	/// <param name="count">number to generate</param>
	public static string[] GenerateMany(int count)
	{
		if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
		var arr = new string[count];
		for (int i = 0; i < count; i++)
			arr[i] = GenerateRandom();
		return arr;
	}
}
