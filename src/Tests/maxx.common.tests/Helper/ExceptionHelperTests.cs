// ***********************************************************************
// Assembly         : maxx.common.tests
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="ExceptionHelperTests.cs" company="maxx.common.tests">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace maxx.common.tests.Helper
{
	using System;
	using System.Collections.Generic;
	using maxx.common.Helper;
	using NFluent;
	using Xunit;

	/// <summary>
	/// Class ExceptionHelperTests.
	/// </summary>
	public class ExceptionHelperTests
	{
		/// <summary>
		/// Defines the test method NoExceptionWhenNotNull.
		/// </summary>
		/// <param name="objectValue">The object value.</param>
		/// <param name="objectName">Name of the object.</param>
		[Theory(DisplayName = "NoExceptionWhenNotNull")]
		[InlineData(1, "Name")]
		[InlineData("value", "string")]
		[InlineData(23, "Integer")]
		[InlineData(23.45, "Decimal")]
		public void NoExceptionWhenNotNull(object objectValue, string objectName)
		{
			Check.ThatCode(value: () => { ExceptionHelper.ThrowIfNull(objectToBechecked: objectValue, nameOfObject: objectName); }).DoesNotThrow();
		}

		/// <summary>
		/// Defines the test method ArgumentNullExceptionWhenNull.
		/// </summary>
		/// <param name="objectValue">The object value.</param>
		/// <param name="objectName">Name of the object.</param>
		[Theory(DisplayName = "ArgumentNullExceptionWhenNull")]
		[InlineData(null, "Name")]
		[InlineData(null, "string")]
		[InlineData(null, "Integer")]
		[InlineData(null, "Decimal")]
		public void ArgumentNullExceptionWhenNull(object objectValue, string objectName)
		{
			var expectedMessage = $"Value cannot be null. (Parameter '{objectName}')";
			Check.ThatCode(value: () => { ExceptionHelper.ThrowIfNull(objectToBechecked: objectValue, nameOfObject: objectName); }).Throws<ArgumentNullException>().WithMessage(exceptionMessage: expectedMessage);
		}

		/// <summary>
		/// Defines the test method ExceptionWhenStringIsNullOrEmpty.
		/// </summary>
		/// <param name="stringValue">The string value.</param>
		[Theory(DisplayName = "ExceptionWhenStringIsNullOrEmpty")]
		[InlineData("")]
		[InlineData(null)]
		[InlineData("\t")]
		public void ExceptionWhenStringIsNullOrEmpty(string stringValue)
		{
			var expectedMessage = $"Value cannot be null.{Environment.NewLine}Parameter name: {stringValue}";

			Check.ThatCode(value: () => { ExceptionHelper.ThrowIfNullOrWhiteSpace(stringToBechecked: stringValue, nameOfString: nameof(stringValue)); }).Throws<ArgumentNullException>().WithProperty(propertyName: "ParamName", propertyValue: nameof(stringValue));
		}

		/// <summary>
		/// Defines the test method ExceptionWhenStringIsNullOrEmpty_AlternativeMessage.
		/// </summary>
		/// <param name="stringValue">The string value.</param>
		/// <param name="errorMessage">The error message.</param>
		[Theory(DisplayName = "ExceptionWhenStringIsNullOrEmpty_AlternativeMessage")]
		[InlineData("", "This cannot be empty")]
		[InlineData(null, "This cannot be null")]
		[InlineData("", "This cannot be null or empty")]
		[InlineData(null, "This cannot be null or empty")]
		public void ExceptionWhenStringIsNullOrEmpty_AlternativeMessage(string stringValue, string errorMessage)
		{
			var expectedMessage = $"Value cannot be null.{Environment.NewLine}Parameter name: {stringValue}";

			Check.ThatCode(value: () => { ExceptionHelper.ThrowIfNullOrWhiteSpace(stringToBechecked: stringValue, nameOfString: nameof(stringValue), exceptionMessage: errorMessage); }).Throws<Exception>().WithMessage(exceptionMessage: errorMessage);
		}


		/// <summary>
		/// Defines the test method NoExceptionWhenStringHasAValue.
		/// </summary>
		/// <param name="stringValue">The string value.</param>
		[Theory(DisplayName = "NoExceptionWhenStringHasAValue")]
		[InlineData("Jack")]
		[InlineData("Mary")]
		public void NoExceptionWhenStringHasAValue(string stringValue)
		{
			Check.ThatCode(value: () => { ExceptionHelper.ThrowIfNullOrWhiteSpace(stringToBechecked: stringValue, nameOfString: nameof(stringValue)); }).DoesNotThrow();
		}

		/// <summary>
		/// Defines the test method ThrowExceptionIfNullOrEmpty_ThrowsArgumentNullException_WhenListIsNull.
		/// </summary>
		/// <param name="exceptionMessage">The exception message.</param>
		[Theory(DisplayName = "ThrowExceptionIfNullOrEmpty_ThrowsArgumentNullException_WhenListIsNull")]
		[InlineData(data: null)]
		[InlineData("testExceptionMessage")]
		public void ThrowExceptionIfNullOrEmpty_ThrowsArgumentNullException_WhenListIsNull(string exceptionMessage)
		{
			const string nameOfList = "test list";

			Check.ThatCode(value: () => { ExceptionHelper.ThrowIfNullOrEmpty(listToBeChecked: null, nameOfList: nameOfList, exceptionMessage: exceptionMessage); }).Throws<ArgumentNullException>()
					.WithMessage(exceptionMessage: new ArgumentNullException(paramName: exceptionMessage ?? nameOfList).Message);
		}

		/// <summary>
		/// Defines the test method ThrowExceptionIfNullOrEmpty_ThrowsArgumentException_WhenListIsEmpty.
		/// </summary>
		/// <param name="exceptionMessage">The exception message.</param>
		[Theory(DisplayName = "ThrowExceptionIfNullOrEmpty_ThrowsArgumentException_WhenListIsEmpty")]
		[InlineData(data: null)]
		[InlineData("testExceptionMessage")]
		public void ThrowExceptionIfNullOrEmpty_ThrowsArgumentException_WhenListIsEmpty(string exceptionMessage)
		{
			const string nameOfList = "test list";
			var listToBeChecked = new List<int>();

			Check.ThatCode(value: () => { ExceptionHelper.ThrowIfNullOrEmpty(listToBeChecked: listToBeChecked, nameOfList: nameOfList, exceptionMessage: exceptionMessage); }).Throws<ArgumentException>().WithMessage(exceptionMessage: exceptionMessage ?? nameOfList);
		}

		/// <summary>
		/// Defines the test method ArgumentNullExceptionWhenIntIsNull.
		/// </summary>
		[Fact(DisplayName = "ArgumentNullExceptionWhenIntIsNull")]
		public void ArgumentNullExceptionWhenIntIsNull()
		{
			Check.ThatCode(value: () =>
			{
				int? variable = null;
				ExceptionHelper.ThrowIfNull(objectToBechecked: variable, nameOfObject: nameof(variable));
			}).Throws<ArgumentNullException>().WithProperty(propertyName: "ParamName", propertyValue: "variable");
		}

		/// <summary>
		/// Defines the test method ArgumentNullExceptionWhenStringIsNull.
		/// </summary>
		[Fact(DisplayName = "ArgumentNullExceptionWhenStringIsNull")]
		public void ArgumentNullExceptionWhenStringIsNull()
		{
			Check.ThatCode(value: () =>
			{
				string variable = null;
				ExceptionHelper.ThrowIfNull(objectToBechecked: variable, nameOfObject: nameof(variable));
			}).Throws<ArgumentNullException>().WithProperty(propertyName: "ParamName", propertyValue: "variable");
		}

		/// <summary>
		/// Defines the test method ThrowExceptionIfNullOrEmpty_Pass_NoExceptionIsThrown.
		/// </summary>
		[Fact(DisplayName = "ThrowExceptionIfNullOrEmpty_Pass_NoExceptionIsThrown")]
		public void ThrowExceptionIfNullOrEmpty_Pass_NoExceptionIsThrown()
		{
			const string nameOfList = "test list";
			var listToBeChecked = new List<string> { "1", "2", "3" };

			Check.ThatCode(value: () => { ExceptionHelper.ThrowIfNullOrEmpty(listToBeChecked: listToBeChecked, nameOfList: nameOfList); }).DoesNotThrow();
		}
	}
}
