// ***********************************************************************
// Assembly         : maxx.common
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="ExceptionHelper.cs" company="maxx.common">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace maxx.common.Helper
{
	using System;
	using System.Collections;

	/// <summary>
	/// Static class containing helper methods related to Exceptions
	/// </summary>
	public static class ExceptionHelper
	{
		/// <summary>
		/// Throws an <see cref="ArgumentNullException" /> if the object passed in is null
		/// </summary>
		/// <param name="objectToBechecked">the object that needs to be checked for null</param>
		/// <param name="nameOfObject">name of the object</param>
		/// <exception cref="System.ArgumentNullException"></exception>
		public static void ThrowIfNull(object objectToBechecked, string nameOfObject)
		{
			if (objectToBechecked == null)
			{
				throw new ArgumentNullException(paramName: nameOfObject);
			}
		}

		/// <summary>
		/// Throws an <see cref="ArgumentNullException" /> or <see cref="ArgumentException" /> if the list passed in is null or
		/// empty
		/// </summary>
		/// <param name="listToBeChecked">the list that needs to be checked</param>
		/// <param name="nameOfList">the name of the list that needs to be checked</param>
		/// <param name="exceptionMessage">Optional. Alternative error message to be injected into the Exception</param>
		/// <exception cref="System.ArgumentNullException"></exception>
		/// <exception cref="System.ArgumentException"></exception>
		public static void ThrowIfNullOrEmpty(IList listToBeChecked, string nameOfList, string exceptionMessage = null)
		{
			if (listToBeChecked == null)
			{
				throw new ArgumentNullException(paramName: exceptionMessage ?? nameOfList);
			}

			if (listToBeChecked.Count == 0)
			{
				throw new ArgumentException(message: exceptionMessage ?? nameOfList);
			}
		}

		/// <summary>
		/// Throws an <see cref="ArgumentNullException" /> if the string passed in is null or empty or white spaces
		/// </summary>
		/// <param name="stringToBechecked">the string that needs to be checked</param>
		/// <param name="nameOfString">the name of the string that needs to be checked</param>
		/// <param name="exceptionMessage">Optional. Alternative error message to be injected into the Exception</param>
		/// <exception cref="System.ArgumentNullException"></exception>
		/// <exception cref="System.ArgumentException"></exception>
		public static void ThrowIfNullOrWhiteSpace(string stringToBechecked, string nameOfString, string exceptionMessage = null)
		{
			if (string.IsNullOrWhiteSpace(value: stringToBechecked))
			{
				if (exceptionMessage == null)
				{
					throw new ArgumentNullException(paramName: nameOfString);
				}

				throw new ArgumentException(message: exceptionMessage);
			}
		}
	}
}
