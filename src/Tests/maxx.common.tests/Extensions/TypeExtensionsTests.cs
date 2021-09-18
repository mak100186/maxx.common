// ***********************************************************************
// Assembly         : maxx.common.tests
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="TypeExtensionsTests.cs" company="maxx.common.tests">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace maxx.common.tests.Extensions
{
	using System;
	using maxx.common.Extensions;
	using NFluent;
	using Xunit;

	/// <summary>
	/// Class TypeExtensionsTests.
	/// </summary>
	public class TypeExtensionsTests
	{
		/// <summary>
		/// Defines the test method FindBaseClassWith_NullType_ReturnsNull.
		/// </summary>
		[Fact]
		public void FindBaseClassWith_NullType_ReturnsNull()
		{
			var stringType = typeof(string);
			Type destinationType = null;

			var baseClass = stringType.FindBaseClassWith(destinationType);

			Check.That(baseClass).IsNull();
		}

		/// <summary>
		/// Defines the test method FindBaseClassWith_NullTypes_ReturnsNull.
		/// </summary>
		[Fact]
		public void FindBaseClassWith_NullTypes_ReturnsNull()
		{
			var stringType = typeof(string);
			Type[] destinationTypes = null;

			var baseClass = stringType.FindBaseClassWith(destinationTypes);

			Check.That(baseClass).IsNull();
		}
	}
}
