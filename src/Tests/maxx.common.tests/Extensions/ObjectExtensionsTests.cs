// ***********************************************************************
// Assembly         : maxx.common.tests
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="ObjectExtensionsTests.cs" company="maxx.common.tests">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace maxx.common.tests.Extensions
{
	using maxx.common.Extensions;
	using Newtonsoft.Json;
	using NFluent;
	using Xunit;

	/// <summary>
	/// Class ObjectExtensionsTests.
	/// </summary>
	public class ObjectExtensionsTests
	{
		/// <summary>
		/// Class TestObject.
		/// </summary>
		private class TestObject
		{
			/// <summary>
			/// Gets or sets the identifier.
			/// </summary>
			/// <value>The identifier.</value>
			public string Id { get; set; }

			/// <summary>
			/// Gets or sets the name.
			/// </summary>
			/// <value>The name.</value>
			public string Name { get; set; }
		}

		/// <summary>
		/// Defines the test method ObjectExtensions_ToJson_ReturnsExpectedResult.
		/// </summary>
		[Fact(DisplayName = "ObjectExtensions_ToJson_ReturnsExpectedResult")]
		public void ObjectExtensions_ToJson_ReturnsExpectedResult()
		{
			// Arrange
			var obj = new TestObject
			{
				Id = "1",
				Name = "name"
			};

			var expected = JsonConvert.SerializeObject(value: obj);

			// Act
			var actual = obj.ToJson();

			// Assert
			Check.That(value: actual).Equals(obj: expected);
		}
	}
}
