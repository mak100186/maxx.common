// ***********************************************************************
// Assembly         : maxx.common
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="EnumerableExtensions.cs" company="maxx.common">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace maxx.common.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;

	/// <summary>
	/// Class EnumerableExtensions.
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// A method for calling the for each sync.
		/// </summary>
		/// <typeparam name="T">The type of the list.</typeparam>
		/// <param name="list">the list</param>
		/// <param name="func">the async func</param>
		/// <returns>the result</returns>
		public static async Task ForEachAsync<T>(this IEnumerable<T> list, Func<T, Task> func)
		{
			foreach (var value in list)
			{
				await func(arg: value);
			}
		}

		/// <summary>
		/// Gets all repeated.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="extList">The ext list.</param>
		/// <param name="groupProps">The group props.</param>
		/// <returns>IEnumerable&lt;T&gt;.</returns>
		public static IEnumerable<T> GetAllRepeated<T>(this IEnumerable<T> extList, Func<T, object> groupProps)
			where T : class
		{
			//Get All the lines that has repeating
			return extList
						.GroupBy(groupProps)
						.Where(z => z.Count() > 1) //Filter only the distinct one
						.SelectMany(z => z); //All in where has to be retuned
		}
	}
}
