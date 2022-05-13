// ***********************************************************************
// Assembly         : maxx.common
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="AttributeExtensions.cs" company="maxx.common">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace maxx.common.Extensions
{
	using System;
	using System.Reflection;

	/// <summary>
	/// Attribute Extensions
	/// </summary>
	public static class AttributeExtensions
	{
		/// <summary>
		/// Get Attribute Value
		/// </summary>
		/// <typeparam name="TAttribute">TAttribute</typeparam>
		/// <typeparam name="TValue">TValue</typeparam>
		/// <param name="type">type</param>
		/// <param name="valueSelector">valueSelector</param>
		/// <returns>Attribute value</returns>
		public static TValue GetAttributeValue<TAttribute, TValue>(
			this Type type,
			Func<TAttribute, TValue> valueSelector)
			where TAttribute : Attribute
		{
			type.GetTypeInfo().GetCustomAttribute(attributeType: typeof(TAttribute), inherit: true);

            if (type.GetTypeInfo().GetCustomAttribute(attributeType: typeof(TAttribute), inherit: true) is TAttribute att)
			{
				return valueSelector(arg: att);
			}

			return default;
		}
	}
}
