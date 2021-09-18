// ***********************************************************************
// Assembly         : maxx.common
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="DictionaryExtensions.cs" company="maxx.common">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace maxx.common.Extensions
{
	using System.Collections.Generic;

	/// <summary>
	/// Class DictionaryExtensions.
	/// </summary>
	public static class DictionaryExtensions
	{
		/// <summary>
		/// Add a key and value to dictionary if it doesn't exist.
		/// </summary>
		/// <typeparam name="T1">The type of key</typeparam>
		/// <typeparam name="T2">The type of value</typeparam>
		/// <param name="dictionary">The dictionary</param>
		/// <param name="key">The key</param>
		/// <param name="value">The value</param>
		public static void AddIfNotExists<T1, T2>(this Dictionary<T1, T2> dictionary, T1 key, T2 value)
		{
			if (dictionary.ContainsKey(key: key))
			{
				return;
			}

			dictionary.Add(key: key, value: value);
		}

		/// <summary>
		/// Add range if not exists
		/// </summary>
		/// <typeparam name="TKey">key type</typeparam>
		/// <typeparam name="TValue">value type</typeparam>
		/// <param name="dic">dictionary to be added to</param>
		/// <param name="dicToAdd">dictionary to add</param>
		public static void AddRangeIfNotExist<TKey, TValue>(this IDictionary<TKey, TValue> dic, IDictionary<TKey, TValue> dicToAdd)
		{
			foreach (var (key, value) in dicToAdd)
			{
				if (!dic.ContainsKey(key))
				{
					dic.Add(key, value);
				}
			}
		}
	}
}
