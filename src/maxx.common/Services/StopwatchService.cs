// ***********************************************************************
// Assembly         : maxx.common
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="StopwatchService.cs" company="maxx.common">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace maxx.common.Services
{
	using System.Diagnostics;
	using maxx.common.Attributes;
	using maxx.common.Contracts;

	/// <summary>
	/// Class StopwatchService.
	/// Implements the <see cref="maxx.common.Contracts.IStopwatchService" />
	/// </summary>
	/// <seealso cref="maxx.common.Contracts.IStopwatchService" />
	[ExcludeFromCodeCoverage]
	public class StopwatchService : IStopwatchService
	{
		/// <summary>
		/// The timer
		/// </summary>
		private readonly Stopwatch _timer = new();

		/// <inheritdoc />
		public void Start()
		{
			_timer.Start();
		}

		/// <inheritdoc />
		public void Stop()
		{
			_timer.Stop();
		}

		/// <inheritdoc />
		public long ElapsedMilliseconds()
		{
			return _timer.ElapsedMilliseconds;
		}
	}
}
