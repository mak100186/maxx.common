// ***********************************************************************
// Assembly         : maxx.common
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="IStopwatchService.cs" company="maxx.common">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace maxx.common.Contracts
{
	/// <summary>
	/// Stopwatch Service
	/// </summary>
	public interface IStopwatchService
	{
		/// <summary>
		/// Get execution time
		/// </summary>
		/// <returns>execution time in Milliseconds</returns>
		long ElapsedMilliseconds();

		/// <summary>
		/// Start Stopwatch
		/// </summary>
		void Start();

		/// <summary>
		/// Stop Stopwatch
		/// </summary>
		void Stop();
	}
}
