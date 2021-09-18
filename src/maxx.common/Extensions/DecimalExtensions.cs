// ***********************************************************************
// Assembly         : maxx.common
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="DecimalExtensions.cs" company="maxx.common">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace maxx.common.Extensions
{
	using System;

	/// <summary>
	/// Class DecimalExtensions.
	/// </summary>
	public static class DecimalExtensions
	{
		/// <summary>
		/// Rounds to nearest cent.
		/// </summary>
		/// <param name="originalPrice">The original price.</param>
		/// <returns>System.Decimal.</returns>
		public static decimal RoundToNearestCent(this decimal originalPrice)
		{
			return originalPrice.RoundToNearestPlace(2);
		}

		/// <summary>
		/// Rounds to nearest cent to two decimal places. For example 3.2865 rounds to 3.29
		/// </summary>
		/// <param name="value">The original price.</param>
		/// <param name="place">The place.</param>
		/// <returns>System.Decimal.</returns>
		public static decimal RoundToNearestPlace(this decimal value, int place)
		{
			var roundedPrice = Math.Round(value, place);

			return roundedPrice;
		}
	}
}
