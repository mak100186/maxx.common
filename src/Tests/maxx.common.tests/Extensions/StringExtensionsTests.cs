// ***********************************************************************
// Assembly         : maxx.common.tests
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="StringExtensionsTests.cs" company="maxx.common.tests">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace maxx.common.tests.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using maxx.common.Enumerations;
	using maxx.common.Extensions;
	using NFluent;
	using Xunit;
	using Xunit.Abstractions;

	/// <summary>
	/// Class StringExtensionsTests.
	/// </summary>
	public class StringExtensionsTests
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="StringExtensionsTests"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		public StringExtensionsTests(ITestOutputHelper logger)
		{
			_logger = logger;
		}

		/// <summary>
		/// The logger
		/// </summary>
		private readonly ITestOutputHelper _logger;

		/// <summary>
		/// Defines the test method ToTimeSpan_Tests.
		/// </summary>
		/// <param name="stringTimeSpan">The string time span.</param>
		/// <param name="expectedDurationInMinutes">The expected duration in minutes.</param>
		[Theory]
		[InlineData("10m", 10)]
		[InlineData("5h", 300)]
		[InlineData("1d", 1440)]
		public void ToTimeSpan_Tests(string stringTimeSpan, double expectedDurationInMinutes)
		{
			var timeSpan = stringTimeSpan.ToTimeSpan();

			Check.That(timeSpan.TotalMinutes).Equals(expectedDurationInMinutes);
		}

		/// <summary>
		/// Defines the test method ToReverse_ConvertsTextToReverse_WhenCalled.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="expectedResult">The expected result.</param>
		[Theory]
		[InlineData("00001234", "43210000")]
		[InlineData("12345678", "87654321")]
		[InlineData("555444222", "222444555")]
		[InlineData("000000001", "100000000")]
		public void ToReverse_ConvertsTextToReverse_WhenCalled(string text, string expectedResult)
		{
			var actualResult = text.Reverse();

			Check.That(actualResult).Equals(expectedResult);
		}

		/// <summary>
		/// Defines the test method IsNumeric_ReturnsExpectedResult_WhenCalled.
		/// </summary>
		/// <param name="number">The number.</param>
		/// <param name="expectedResult">if set to <c>true</c> [expected result].</param>
		[Theory]
		[InlineData("1", true)]
		[InlineData("1s", false)]
		public void IsNumeric_ReturnsExpectedResult_WhenCalled(string number, bool expectedResult)
		{
			var actualResult = number.IsNumeric();

			Check.That(actualResult).Equals(expectedResult);
		}

		/// <summary>
		/// Defines the test method ComputeHash_ComputesTheHashBasedOnType_WhenCalled.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <param name="expectedHashString">The expected hash string.</param>
		[Theory]
		[InlineData(EHashType.MD5, "c65dbf2791f37c721acd6faa7b735763")]
		[InlineData(EHashType.SHA1, "a31585d95c3b1775627d49e01848dca55913a194")]
		[InlineData(EHashType.SHA256, "6ba35a896522b4c2cc9bde94aea1e2290afb87a6fe99d42effc86e1a5b927c84")]
		[InlineData(EHashType.SHA512, "79a9a1445b04a6c74de55d171c683d59ef7e6e51470a6b5dbaf9e97a27d43d6c0c4e48a6c66a63fd1f873d2bf605871ea7532a7dd8a554cd022360c22a5ea29c")]
		public void ComputeHash_ComputesTheHashBasedOnType_WhenCalled(EHashType type, string expectedHashString)
		{
			//arrange
			const string subject = "the string to be hashed";

			//act
			var actualHashString = subject.ComputeHash(type);
			_logger.WriteLine(type + " " + actualHashString);

			//assert
			Check.That(actualHashString).IsEqualTo(expectedHashString);
		}

		/// <summary>
		/// Defines the test method Left_ReturnsExpectedResult_WhenCalled.
		/// </summary>
		/// <param name="length">The length.</param>
		/// <param name="expectedResult">The expected result.</param>
		[Theory]
		[InlineData(2, "12")]
		[InlineData(5, "12345")]
		public void Left_ReturnsExpectedResult_WhenCalled(int length, string expectedResult)
		{
			const string subject = "1234567890";

			var actualResult = subject.Left(length);

			Check.That(actualResult).Equals(expectedResult);
		}

		/// <summary>
		/// Defines the test method Right_ReturnsExpectedResult_WhenCalled.
		/// </summary>
		/// <param name="length">The length.</param>
		/// <param name="expectedResult">The expected result.</param>
		[Theory]
		[InlineData(2, "90")]
		[InlineData(5, "67890")]
		public void Right_ReturnsExpectedResult_WhenCalled(int length, string expectedResult)
		{
			const string subject = "1234567890";

			var actualResult = subject.Right(length);

			Check.That(actualResult).Equals(expectedResult);
		}

		/// <summary>
		/// Defines the test method Decrypt_Encrypt_DecryptsStringBasedOnKey_WhenCalled.
		/// </summary>
		[Fact]
		public void Decrypt_Encrypt_DecryptsStringBasedOnKey_WhenCalled()
		{
			//arrange
			const string key = "test key";
			const string subject = "test subject";
			var encryptedString = subject.Encrypt(key);

			//act
			var decryptedString = encryptedString.Decrypt(key);

			//assert
			Check.That(decryptedString).IsEqualTo(subject);
		}

		/// <summary>
		/// Defines the test method EmptyIfNull_ReturnsExpectedResult_WhenCalled.
		/// </summary>
		[Fact]
		public void EmptyIfNull_ReturnsExpectedResult_WhenCalled()
		{
			//arrange
			string subject = null;

			//act
			var actualResult = subject.EmptyIfNull();

			//assert
			Check.That(actualResult).Equals(string.Empty);
		}

		/// <summary>
		/// Defines the test method DecodeFromBase64_DecodeBase64String.
		/// </summary>
		[Fact]
		public void DecodeFromBase64_DecodeBase64String()
		{
			// arrange
			var expected = "to be encoded";
			var base64String = Convert.ToBase64String(inArray: Encoding.ASCII.GetBytes(s: expected));

			// act
			var actual = base64String.DecodeFromBase64();

			// assert
			Check.That(value: actual).Equals(obj: expected);
		}

		/// <summary>
		/// Defines the test method In_ReturnsFalse_WhenNotFound.
		/// </summary>
		[Fact]
		public void In_ReturnsFalse_WhenNotFound()
		{
			//arrange
			const string subject = "subject";

			//act
			var actualResult = subject.In("test value 1", "test value 2", "test value 3");

			//assert
			Check.That(actualResult).IsFalse();
		}

		/// <summary>
		/// Defines the test method In_ReturnsTrue_WhenFound.
		/// </summary>
		[Fact]
		public void In_ReturnsTrue_WhenFound()
		{
			//arrange
			const string subject = "subject";

			//act
			var actualResult = subject.In("test value 1", "test value 2", subject);

			//assert
			Check.That(actualResult).IsTrue();
		}

		/// <summary>
		/// Defines the test method NullIfEmpty_ReturnsExpectedResult_WhenCalled.
		/// </summary>
		[Fact]
		public void NullIfEmpty_ReturnsExpectedResult_WhenCalled()
		{
			//arrange
			var subject = string.Empty;

			//act
			var actualResult = subject.NullIfEmpty();

			//assert
			Check.That(actualResult).IsNull();
		}

		/// <summary>
		/// Defines the test method SplitCsv_Success.
		/// </summary>
		[Fact]
		public void SplitCsv_Success()
		{
			const string input = "a,b,cde,ef";
			var expected = new List<string> { "a", "b", "cde", "ef" };
			var actual = input.SplitCsv();

			Check.That(actual).ContainsExactly(expected);
		}

		/// <summary>
		/// Defines the test method SplitCsv_WhiteSpaceReturnsEmptyList_Success.
		/// </summary>
		[Fact]
		public void SplitCsv_WhiteSpaceReturnsEmptyList_Success()
		{
			const string input = "";
			var expected = new List<string>();
			var actual = input.SplitCsv();

			Check.That(actual).IsEmpty();
		}

		/// <summary>
		/// Defines the test method SplitCsv_WhiteSpaceReturnsNull_Success.
		/// </summary>
		[Fact]
		public void SplitCsv_WhiteSpaceReturnsNull_Success()
		{
			const string input = "";

			var actual = input.SplitCsv(true);

			Check.That(actual).IsNull();
		}

		/// <summary>
		/// Defines the test method ToBase64Encoded_ReturnsExpectedValue_WhenValueIsPassedFromFromBase64Decoded.
		/// </summary>
		[Fact]
		public void ToBase64Encoded_ReturnsExpectedValue_WhenValueIsPassedFromFromBase64Decoded()
		{
			const string testString = "testString";

			var encodedString = testString.EncodeToBase64();
			var decodedString = encodedString.DecodeFromBase64();

			Check.That(testString).IsEqualTo(decodedString);
		}
	}
}
