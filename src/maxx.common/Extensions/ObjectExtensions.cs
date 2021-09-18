// ***********************************************************************
// Assembly         : maxx.web.api
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="ObjectExtensions.cs" company="Maxx Technologies Australia PTY LTD">
//     Copyright by Maxx Technologies Australia PTY LTD. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace maxx.common.Extensions
{
	using System;
	using Newtonsoft.Json;
	using ObjectsComparer;

	/// <summary>
	/// Class ObjectExtensions.
	/// </summary>
	public static class ObjectExtensions
	{
		/// <summary>
		/// Changes the type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns>T.</returns>
		public static T ChangeType<T>(this object value)
		{
			var t = typeof(T);

			if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				if (value == null)
				{
					return default;
				}

				t = Nullable.GetUnderlyingType(t);
			}

			return (T)Convert.ChangeType(value, t);
		}

		/// <summary>
		/// Determines whether [is deep equal to] [the specified expected].
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="actual">The actual.</param>
		/// <param name="expected">The expected.</param>
		/// <exception cref="System.Exception">
		/// Actual and Expected are different. Differences: {string.Join(Environment.NewLine,
		/// differences)}
		/// </exception>
		public static void IsDeepEqualTo<T>(this T actual, T expected)
		{
			var comparer = new Comparer<T>();
			var isEqual = comparer.Compare(actual, expected, out var differences);

			if (!isEqual)
			{
				throw new Exception($"Actual and Expected are different. Differences: {string.Join(Environment.NewLine, differences)}");
			}
		}

		/// <summary>
		/// Returns a Json string equivalent of the object
		/// </summary>
		/// <param name="thisObject">an object</param>
		/// <returns>Json string</returns>
		public static string ToJson(this object thisObject)
		{
			return JsonConvert.SerializeObject(value: thisObject);
		}
	}
}
