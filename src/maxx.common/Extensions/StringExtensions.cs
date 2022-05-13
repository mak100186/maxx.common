// ***********************************************************************
// Assembly         : maxx.common
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="StringExtensions.cs" company="maxx.common">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace maxx.common.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Globalization;
	using System.Linq;
	using System.Security;
	using System.Security.Cryptography;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading;
	using System.Web;
	using maxx.common.Enumerations;
	using Microsoft.IdentityModel.Tokens;
	using Newtonsoft.Json;
	using Pluralize.NET.Core;

	/// <summary>
	/// Class StringExtensions.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Computes the hash of the string using a specified hash algorithm
		/// </summary>
		/// <param name="input">The string to hash</param>
		/// <param name="hashType">The hash algorithm to use</param>
		/// <returns>The resulting hash or an empty string on error</returns>
		public static string ComputeHash(this string input, EHashType hashType)
		{
			var hash = GetHash(input: input, hash: hashType);
			var ret = new StringBuilder();

			foreach (var t in hash)
			{
				ret.Append(value: t.ToString(format: "x2"));
			}

			return ret.ToString();
		}

		/// <summary>
		/// Determines whether the specified characters contains any of the input characters.
		/// </summary>
		/// <param name="theString">The string.</param>
		/// <param name="characters">The characters.</param>
		/// <returns><c>true</c> if the specified characters contains any; otherwise, <c>false</c>.</returns>
		public static bool ContainsAny(this string theString, char[] characters)
		{
			return characters.Any(character => theString.Contains(character.ToString()));
		}

		/// <summary>
		/// Converts a string from a Base64 encoded string
		/// </summary>
		/// <param name="toBeDecoded">string to be decoded</param>
		/// <returns>a decoded string</returns>
		public static string DecodeFromBase64(this string toBeDecoded)
		{
			return Base64UrlEncoder.Decode(toBeDecoded);
		}

		/// <summary>
		/// Decrypts a string using the supplied key. Decoding is done using RSA encryption.
		/// </summary>
		/// <param name="stringToDecrypt">String that must be decrypted.</param>
		/// <param name="key">Decryption key.</param>
		/// <returns>The decrypted string or null if decryption failed.</returns>
		/// <exception cref="ArgumentException">Occurs when stringToDecrypt or key is null or empty.</exception>
		public static string Decrypt(this string stringToDecrypt, string key)
		{
			string result = null;

			if (string.IsNullOrEmpty(value: stringToDecrypt))
			{
				throw new ArgumentException(message: "An empty string value cannot be encrypted.");
			}

			if (string.IsNullOrEmpty(value: key))
			{
				throw new ArgumentException(message: "Cannot decrypt using an empty key. Please supply a decryption key.");
			}

			var cspParameters = new CspParameters
			{
				KeyContainerName = key
			};

			var rsa = new RSACryptoServiceProvider(parameters: cspParameters)
			{
				PersistKeyInCsp = true
			};

			var decryptArray = stringToDecrypt.Split(separator: new[] { "-" }, options: StringSplitOptions.None);
			var decryptByteArray = Array.ConvertAll(array: decryptArray, converter: s => Convert.ToByte(value: byte.Parse(s: s, style: NumberStyles.HexNumber)));

			var bytes = rsa.Decrypt(rgb: decryptByteArray, fOAEP: true);

			result = Encoding.UTF8.GetString(bytes: bytes);

			return result;
		}

		/// <summary>
		/// Returns an empty string if input string is null.
		/// </summary>
		/// <param name="inputString">the string under test</param>
		/// <returns>string.empty if its null</returns>
		public static string EmptyIfNull(this string inputString)
		{
			return inputString ?? string.Empty;
		}

		/// <summary>
		/// Converts a string to Base64 encoded string
		/// </summary>
		/// <param name="toBeEncoded">string to be encoded</param>
		/// <returns>a Base64 encoded string</returns>
		public static string EncodeToBase64(this string toBeEncoded)
		{
			return Base64UrlEncoder.Encode(toBeEncoded);
		}

		/// <summary>
		/// Encrypts a string using the supplied key. Encoding is done using RSA encryption.
		/// </summary>
		/// <param name="stringToEncrypt">String that must be encrypted.</param>
		/// <param name="key">Encryption key.</param>
		/// <returns>A string representing a byte array separated by a minus sign.</returns>
		/// <exception cref="ArgumentException">Occurs when stringToEncrypt or key is null or empty.</exception>
		public static string Encrypt(this string stringToEncrypt, string key)
		{
			if (string.IsNullOrEmpty(value: stringToEncrypt))
			{
				throw new ArgumentException(message: "An empty string value cannot be encrypted.");
			}

			if (string.IsNullOrEmpty(value: key))
			{
				throw new ArgumentException(message: "Cannot encrypt using an empty key. Please supply an encryption key.");
			}

			var cspParameters = new CspParameters
			{
				KeyContainerName = key
			};

			var rsa = new RSACryptoServiceProvider(parameters: cspParameters)
			{
				PersistKeyInCsp = true
			};

			var bytes = rsa.Encrypt(rgb: Encoding.UTF8.GetBytes(s: stringToEncrypt), fOAEP: true);

			return BitConverter.ToString(value: bytes);
		}

		/// <summary>
		/// Formats the string according to the specified mask
		/// </summary>
		/// <param name="input">The input string.</param>
		/// <param name="mask">The mask for formatting. Like "A##-##-T-###Z"</param>
		/// <returns>The formatted string</returns>
		public static string FormatWithMask(this string input, string mask)
		{
			if (string.IsNullOrEmpty(value: input))
			{
				return input;
			}

			var output = string.Empty;
			var index = 0;

			foreach (var m in mask)
			{
				if (m == '#')
				{
					if (index < input.Length)
					{
						output += input[index: index];
						index++;
					}
				}
				else
				{
					output += m;
				}
			}

			return output;
		}

		/// <summary>
		/// decodes the HTML.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns>System.String.</returns>
		public static string HtmlDecode(this string data)
		{
			return HttpUtility.HtmlDecode(data);
		}


		/// <summary>
		/// encodes the HTML.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns>System.String.</returns>
		public static string HtmlEncode(this string data)
		{
			return HttpUtility.HtmlEncode(data);
		}

		/// <summary>
		/// Checks if string's value is present in array of string values
		/// </summary>
		/// <param name="value">the string under test</param>
		/// <param name="stringValues">Array of string values to compare</param>
		/// <returns>Return true if any string value matches</returns>
		public static bool In(this string value, params string[] stringValues)
		{
			return stringValues.Any(predicate: otherValue => string.CompareOrdinal(strA: value, strB: otherValue) == 0);
		}

		/// <summary>
		/// Determines whether the specified input is date.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns><c>true</c> if the specified input is date; otherwise, <c>false</c>.</returns>
		public static bool IsDate(this string input)
		{
			return !string.IsNullOrEmpty(input) && DateTime.TryParse(input, out _);
		}

		/// <summary>
		/// Determines whether the specified s is a GUID or not
		/// </summary>
		/// <param name="s">The s.</param>
		/// <returns><c>true</c> if the specified s is unique identifier; otherwise, <c>false</c>.</returns>
		/// <exception cref="ArgumentNullException">s</exception>
		public static bool IsGuid(this string s)
		{
			if (string.IsNullOrWhiteSpace(s))
			{
				throw new ArgumentNullException(nameof(s) + " is null, empty or whitespace");
			}

			var format = new Regex("^[A-Fa-f0-9]{32}$|" + "^({|\\()?[A-Fa-f0-9]{8}-([A-Fa-f0-9]{4}-){3}[A-Fa-f0-9]{12}(}|\\))?$|" + "^({)?[0xA-Fa-f0-9]{3,10}(, {0,1}[0xA-Fa-f0-9]{3,6}){2}, {0,1}({)([0xA-Fa-f0-9]{3,4}, {0,1}){7}[0xA-Fa-f0-9]{3,4}(}})$");
			var match = format.Match(s);

			return match.Success;
		}

		/// <summary>
		/// Identifies whether the string is an integer number
		/// </summary>
		/// <param name="theValue">string number</param>
		/// <returns>A value indicating whether the string is a number</returns>
		public static bool IsNumeric(this string theValue)
		{
			return long.TryParse(s: theValue, style: NumberStyles.Integer, provider: NumberFormatInfo.InvariantInfo, result: out _);
		}

		/// <summary>
		/// Determines whether input is a valid email or not
		/// </summary>
		/// <param name="s">The s.</param>
		/// <returns><c>true</c> if [is valid email address] [the specified s]; otherwise, <c>false</c>.</returns>
		public static bool IsValidEmailAddress(this string s)
		{
			var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");

			return regex.IsMatch(s);
		}

		/// <summary>
		/// Determines whether [is valid URL] [the specified text].
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns><c>true</c> if [is valid URL] [the specified text]; otherwise, <c>false</c>.</returns>
		public static bool IsValidUrl(this string text)
		{
			var rx = new Regex(@"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");

			return rx.IsMatch(text);
		}


		/// <summary>
		/// Returns characters left of specified length
		/// </summary>
		/// <param name="value">String value</param>
		/// <param name="length">Max number of characters to return</param>
		/// <returns>Returns string from left</returns>
		public static string Left(this string value, int length)
		{
			return value?.Length > length ? value.Substring(startIndex: 0, length: length) : value;
		}

		/// <summary>
		/// Returns true if invoking string matches with regex pattern
		/// </summary>
		/// <param name="original">the string under test</param>
		/// <param name="regex">the pattern to match</param>
		/// <returns>whether it matches or not</returns>
		public static bool Matches(this string original, string regex)
		{
			return Regex.IsMatch(input: original, pattern: regex);
		}

		/// <summary>
		/// Returns an empty string if input string is null.
		/// </summary>
		/// <param name="inputString">the string under test</param>
		/// <returns>string.empty if its null</returns>
		public static string NullIfEmpty(this string inputString)
		{
			return inputString == string.Empty ? null : inputString;
		}

		/// <summary>
		/// Parses the query string.
		/// </summary>
		/// <param name="query">The query.</param>
		/// <returns>NameValueCollection.</returns>
		public static NameValueCollection ParseQueryString(this string query)
		{
			return HttpUtility.ParseQueryString(query);
		}

		/// <summary>
		/// Converts to plural.
		/// </summary>
		/// <param name="input">the input string</param>
		/// <returns>System.String</returns>
		public static string Pluralize(this string input)
		{
			return new Pluralizer().Pluralize(input);
		}

		/// <summary>
		/// Reverse string
		/// </summary>
		/// <param name="str">String to output</param>
		/// <returns>Return reverse string</returns>
		public static string Reverse(this string str)
		{
			var charArray = str.ToCharArray();
			Array.Reverse(charArray);

			return new string(charArray);
		}

		/// <summary>
		/// Returns characters right of specified length
		/// </summary>
		/// <param name="value">String value</param>
		/// <param name="length">Max number of characters to return</param>
		/// <returns>Returns string from right</returns>
		public static string Right(this string value, int length)
		{
			return value?.Length > length ? value.Substring(startIndex: value.Length - length) : value;
		}

		/// <summary>
		/// Changes form to singular
		/// </summary>
		/// <param name="input">the input string</param>
		/// <returns>System.String</returns>
		public static string Singularize(this string input)
		{
			return new Pluralizer().Singularize(input);
		}

		/// <summary>
		/// Splits a CSV into a list of strings
		/// </summary>
		/// <param name="csvList">a string containing comma separated values</param>
		/// <param name="nullOrWhitespaceInputReturnsNull">
		/// whether to return null instead of an empty list when the input parameter
		/// is null or white space
		/// </param>
		/// <returns>a list of strings</returns>
		public static List<string> SplitCsv(this string csvList, bool nullOrWhitespaceInputReturnsNull = false)
		{
			if (string.IsNullOrWhiteSpace(csvList))
			{
				return nullOrWhitespaceInputReturnsNull ? null : new List<string>();
			}

			return csvList
						.TrimEnd(',')
						.Split(',')
						.AsEnumerable()
						.Select(s => s.Trim())
						.ToList();
		}

		/// <summary>
		/// Strips the HTML. Used when we want to completely remove HTML code and not encode it with XML entities.
		/// </summary>
		/// <param name="input">The input.</param>
		/// <returns>System.String.</returns>
		public static string StripHtml(this string input)
		{
			// Will this simple expression replace all tags???
			var tagsExpression = new Regex(pattern: @"</?.+?>");

			return tagsExpression.Replace(input: input, replacement: " ");
		}

		/// <summary>
		/// Converts to camelcase
		/// </summary>
		/// <param name="input">The string.</param>
		/// <param name="separators">the separators to split the string</param>
		/// <returns>System.String.</returns>
		public static string ToCamelCase(this string input, char[] separators)
		{
			if (input == null || input.Length < 2)
			{
				return input;
			}

			var words = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);

			var result = words[0].ToLower();

			for (var i = 1; i < words.Length; i++)
			{
				result += words[i].Substring(0, 1).ToUpper() + words[i].Substring(1);
			}

			return result;
		}


		/// <summary>
		/// Converts string to enum object
		/// </summary>
		/// <typeparam name="T">Type of enum</typeparam>
		/// <param name="value">String value to convert</param>
		/// <returns>Returns enum object</returns>
		public static T ToEnum<T>(this string value)
			where T : Enum
		{
			return (T)Enum.Parse(enumType: typeof(T), value: value, ignoreCase: true);
		}

		/// <summary>
		/// Converts json to document model
		/// </summary>
		/// <typeparam name="T">type of document model</typeparam>
		/// <param name="json">json</param>
		/// <returns>document model</returns>
		public static T ToObject<T>(this string json)
		{
			return JsonConvert.DeserializeObject<T>(value: json);
		}

		/// <summary>
		/// Converts to proper case.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>System.String.</returns>
		public static string ToProperCase(this string text)
		{
			var textInfo = Thread.CurrentThread.CurrentCulture.TextInfo;

			return textInfo.ToTitleCase(text);
		}

		/// <summary>
		/// Converts a string into a "SecureString"
		/// </summary>
		/// <param name="str">Input String</param>
		/// <returns>returns an object of <see cref="System.Security.SecureString" /></returns>
		public static SecureString ToSecureString(this string str)
		{
			var secureString = new SecureString();

			foreach (var c in str)
			{
				secureString.AppendChar(c: c);
			}

			return secureString;
		}

		/// <summary>
		/// Converts string to time span, string format should be {x}m, {x}h, {x}d
		/// </summary>
		/// <param name="timeSpan">timeSpan</param>
		/// <returns>TimeSpan</returns>
		public static TimeSpan ToTimeSpan(this string timeSpan)
		{
			var minutes = 0;
			var hours = 0;
			var days = 0;

			if (timeSpan.ToLower().EndsWith(value: "m"))
			{
				minutes = int.Parse(s: timeSpan.Substring(startIndex: 0, length: timeSpan.Length - 1));
			}
			else if (timeSpan.ToLower().EndsWith(value: "h"))
			{
				hours = int.Parse(s: timeSpan.Substring(startIndex: 0, length: timeSpan.Length - 1));
			}
			else if (timeSpan.ToLower().EndsWith(value: "d"))
			{
				days = int.Parse(s: timeSpan.Substring(startIndex: 0, length: timeSpan.Length - 1));
			}

			return new TimeSpan(days: days, hours: hours, minutes: minutes, seconds: 0);
		}

		/// <summary>
		/// Truncates the string until the specified maximum length.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="maxLength">The maximum length.</param>
		/// <param name="suffix">the suffixed characters</param>
		/// <returns>System.String.</returns>
		public static string Truncate(this string text, int maxLength, string suffix = "...")
		{
			var truncatedString = text;

			if (maxLength <= 0)
			{
				return truncatedString;
			}

			var strLength = maxLength - suffix.Length;

			if (strLength <= 0)
			{
				return truncatedString;
			}

			if (text == null || text.Length <= maxLength)
			{
				return truncatedString;
			}

			truncatedString = text.Substring(0, strLength);
			truncatedString = truncatedString.TrimEnd();
			truncatedString += suffix;

			return truncatedString;
		}

		/// <summary>
		/// URLs the decode.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns>System.String.</returns>
		public static string UrlDecode(this string url)
		{
			return HttpUtility.UrlDecode(url);
		}

		/// <summary>
		/// URLs the encode.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns>System.String.</returns>
		public static string UrlEncode(this string url)
		{
			return HttpUtility.UrlEncode(url);
		}

		private static IEnumerable<byte> GetHash(string input, EHashType hash)
		{
			var inputBytes = Encoding.ASCII.GetBytes(s: input);

			return hash switch
			{
				EHashType.MD5 => MD5.Create().ComputeHash(buffer: inputBytes),
				EHashType.SHA1 => SHA1.Create().ComputeHash(buffer: inputBytes),
				EHashType.SHA256 => SHA256.Create().ComputeHash(buffer: inputBytes),
				EHashType.SHA512 => SHA512.Create().ComputeHash(buffer: inputBytes),
				_ => inputBytes
			};
		}
	}
}
