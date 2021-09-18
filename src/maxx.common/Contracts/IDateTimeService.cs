// ***********************************************************************
// Assembly         : maxx.common
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="IDateTimeService.cs" company="maxx.common">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace maxx.common.Contracts
{
	using System;

	/// <summary>
	/// Interface IDateTimeService
	/// </summary>
	public interface IDateTimeService
	{
		/// <summary>
		/// Nows this instance.
		/// </summary>
		/// <returns>DateTime.</returns>
		DateTime Now();

		/// <summary>
		/// UTCs the now.
		/// </summary>
		/// <returns>DateTime.</returns>
		DateTime UtcNow();

		/// <summary>
		/// UTCs the now as unix time seconds.
		/// </summary>
		/// <returns>System.Int64.</returns>
		long UtcNowAsUnixTimeSeconds();
	}
}
