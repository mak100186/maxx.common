// ***********************************************************************
// Assembly         : maxx.web.api
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="ITelemetryClientWrapper.cs" company="Maxx Technologies Australia PTY LTD">
//     Copyright by Maxx Technologies Australia PTY LTD. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace maxx.common.Contracts.ApplicationInsights
{
	using Microsoft.ApplicationInsights.DataContracts;

	/// <summary>
	/// Interface ITelemetryClientWrapper
	/// </summary>
	public interface ITelemetryClientWrapper
	{
		/// <summary>
		/// Flushes this instance.
		/// </summary>
		void Flush();

		/// <summary>
		/// Tracks the dependency.
		/// </summary>
		/// <param name="dependencyTelemetry">The dependency telemetry.</param>
		void TrackDependency(DependencyTelemetry dependencyTelemetry);

		/// <summary>
		/// Tracks the event.
		/// </summary>
		/// <param name="eventTelemetry">The event telemetry.</param>
		void TrackEvent(EventTelemetry eventTelemetry);

		/// <summary>
		/// Tracks the trace.
		/// </summary>
		/// <param name="traceTelemetry">The trace telemetry.</param>
		void TrackTrace(TraceTelemetry traceTelemetry);
	}
}
