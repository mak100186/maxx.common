// ***********************************************************************
// Assembly         : maxx.common.tests
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="DictionaryExtensionsTests.cs" company="maxx.common.tests">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace maxx.common.tests.Extensions
{
	using System.Collections.Generic;
	using maxx.common.Extensions;
	using NFluent;
	using Xunit;

	/// <summary>
	/// Class DictionaryExtensionTests.
	/// </summary>
	public class DictionaryExtensionTests
	{
		/// <summary>
		/// Defines the test method AddIfNotExists_AddsKey_WhenKeyDosntExist.
		/// </summary>
		[Fact]
		public void AddIfNotExists_AddsKey_WhenKeyDosntExist()
		{
			// arrange
			const string key1 = "key1";
			const string key2 = "key2";
			const string value1 = "value1";
			const string value2 = "value2";
			var dictionary = new Dictionary<string, string> { { key1, value1 } };

			// act
			dictionary.AddIfNotExists(key: key2, value: value2);

			// assert
			Check.That(value: dictionary[key: key2]).Equals(obj: value2);
		}

		/// <summary>
		/// Defines the test method AddIfNotExists_DoesntThrow_WhenKeyExists.
		/// </summary>
		[Fact]
		public void AddIfNotExists_DoesntThrow_WhenKeyExists()
		{
			// arrange
			const string key = "key";
			const string value1 = "value1";
			const string value2 = "value2";
			var dictionary = new Dictionary<string, string> { { key, value1 } };

			// act
			dictionary.AddIfNotExists(key: key, value: value2);

			// assert
			Check.That(value: dictionary[key: key]).Equals(obj: value1);
		}
	}
}
