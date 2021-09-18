// ***********************************************************************
// Assembly         : maxx.common.tests
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="EnumerableExtensionsTests.cs" company="maxx.common.tests">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace maxx.common.tests.Extensions
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using maxx.common.Extensions;
	using NFluent;
	using Xunit;

	/// <summary>
	/// Class EnumerableExtentionTests.
	/// </summary>
	public class EnumerableExtensionTests
	{
		/// <summary>
		/// Defines the test method ForEachAsync_ExecutesTheFuncOnEachItemInTheList.
		/// </summary>
		[Fact]
		public async void ForEachAsync_ExecutesTheFuncOnEachItemInTheList()
		{
			// arrange
			var originalList = new List<int> { 1, 2, 3, 4, 5 };
			var counter = 0;

			// act
			await originalList.ForEachAsync(func: i => Task.FromResult(result: counter++));

			// asser
			Check.That(value: counter).Equals(obj: originalList.Count);
		}
	}
}
