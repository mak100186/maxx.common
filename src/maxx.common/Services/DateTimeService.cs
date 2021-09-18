// ***********************************************************************
// Assembly         : maxx.common
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="DateTimeService.cs" company="maxx.common">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace maxx.common.Services
{
	using System;
	using maxx.common.Attributes;
	using maxx.common.Contracts;

	/// <summary>
	/// Class DateTimeService.
	/// Implements the <see cref="maxx.common.Contracts.IDateTimeService" />
	/// </summary>
	/// <seealso cref="maxx.common.Contracts.IDateTimeService" />
	[ExcludeFromCodeCoverage]
	public class DateTimeService : IDateTimeService
	{
		/// <summary>
		/// Nows this instance.
		/// </summary>
		/// <returns>DateTime.</returns>
		public DateTime Now()
		{
			return DateTime.Now;
		}

		/// <summary>
		/// UTCs the now.
		/// </summary>
		/// <returns>DateTime.</returns>
		public DateTime UtcNow()
		{
			return DateTime.UtcNow;
		}

		/// <summary>
		/// UTCs the now as unix time seconds.
		/// </summary>
		/// <returns>System.Int64.</returns>
		public long UtcNowAsUnixTimeSeconds()
		{
			return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		}
	}
}
