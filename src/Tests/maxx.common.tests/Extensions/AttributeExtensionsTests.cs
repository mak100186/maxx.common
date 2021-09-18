// ***********************************************************************
// Assembly         : maxx.common.tests
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="AttributeExtensionsTests.cs" company="maxx.common.tests">
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
	/// Attribute Extensions Tests
	/// </summary>
	public class AttributeExtensionsTests
	{
		/// <summary>
		/// Class MockedClassWithoutAttribute.
		/// </summary>
		public class MockedClassWithoutAttribute
		{
		}

		/// <summary>
		/// Class MockedClassWithAttribute.
		/// </summary>
		[Mocked(Name = "testName")]
		public class MockedClassWithAttribute
		{
		}

		/// <summary>
		/// Class MockedAttribute.
		/// Implements the <see cref="System.Attribute" />
		/// </summary>
		/// <seealso cref="System.Attribute" />
		[AttributeUsage(AttributeTargets.Class)]
		public class MockedAttribute : Attribute
		{
			/// <summary>
			/// Gets or sets the name.
			/// </summary>
			/// <value>The name.</value>
			public string Name { get; set; }
		}

		/// <summary>
		/// Defines the test method GetAttributeValue_WhenClassDoesNotHaveAttribute_ReturnsNull.
		/// </summary>
		[Fact]
		public void GetAttributeValue_WhenClassDoesNotHaveAttribute_ReturnsNull()
		{
			var type = typeof(MockedClassWithoutAttribute);

			var attribute = type.GetAttributeValue(valueSelector: (MockedAttribute attr) => attr);

			Check.That(value: attribute).IsNull();
		}

		/// <summary>
		/// Defines the test method GetAttributeValue_WhenClassHasAttribute_ReturnsAttribute.
		/// </summary>
		[Fact]
		public void GetAttributeValue_WhenClassHasAttribute_ReturnsAttribute()
		{
			var type = typeof(MockedClassWithAttribute);

			var attribute = type.GetAttributeValue(valueSelector: (MockedAttribute attr) => attr);

			Check.That(value: attribute).IsNotNull();
			Check.That(value: attribute.Name).Equals(obj: "testName");
		}
	}
}
