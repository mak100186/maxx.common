// ***********************************************************************
// Assembly         : maxx.common
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="TypeExtensions.cs" company="maxx.common">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace maxx.common.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// Class TypeExtensions.
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>
		/// Finds base class with destination types
		/// </summary>
		/// <param name="sourceType">source Type</param>
		/// <param name="destinationTypes">destination Types</param>
		/// <returns>Common Type</returns>
		public static Type FindBaseClassWith(this Type sourceType, Type[] destinationTypes)
		{
			if (destinationTypes == null)
			{
				return null;
			}

			var commonType = sourceType;

			foreach (var destinationType in destinationTypes)
			{
				commonType = commonType.FindBaseClassWith(destinationType);
			}

			return commonType;
		}

		/// <summary>
		/// Finds base class with destination type
		/// </summary>
		/// <param name="sourceType">source Type</param>
		/// <param name="destinationType">destination Type</param>
		/// <returns>Common Type</returns>
		public static Type FindBaseClassWith(this Type sourceType, Type destinationType)
		{
			if (destinationType == null)
			{
				return null;
			}

			return sourceType
						.GetClassHierarchy()
						.Intersect(destinationType.GetClassHierarchy())
						.FirstOrDefault(type => !type.IsInterface);
		}

		/// <summary>
		/// Get class hirearchy
		/// </summary>
		/// <param name="type">type</param>
		/// <returns>list of tyepes</returns>
		public static IEnumerable<Type> GetClassHierarchy(this Type type)
		{
			var typeInHierarchy = type;

			do
			{
				yield return typeInHierarchy;

				typeInHierarchy = typeInHierarchy.BaseType;
			}
			while (typeInHierarchy != null && !typeInHierarchy.IsInterface);
		}
	}
}
