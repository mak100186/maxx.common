// ***********************************************************************
// Assembly         : maxx.web.api
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="TelemetryClientWrapper.cs" company="Maxx Technologies Australia PTY LTD">
//     Copyright by Maxx Technologies Australia PTY LTD. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace maxx.common.Services.ApplicationInsights
{
	using maxx.common.Attributes;
	using maxx.common.Contracts.ApplicationInsights;
	using Microsoft.ApplicationInsights;
	using Microsoft.ApplicationInsights.DataContracts;
	using Microsoft.ApplicationInsights.Extensibility;

	/// <summary>
	/// Class TelemetryClientWrapper.
	/// Implements the <see cref="ITelemetryClientWrapper" />
	/// </summary>
	/// <seealso cref="ITelemetryClientWrapper" />
	[ExcludeFromCodeCoverage]
	public class TelemetryClientWrapper : ITelemetryClientWrapper
	{
		/// <summary>
		/// The telemetry client
		/// </summary>
		private readonly TelemetryClient _telemetryClient;

		/// <summary>
		/// Initializes a new instance of the <see cref="TelemetryClientWrapper" /> class.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		public TelemetryClientWrapper(IApplicationInsightConfiguration configuration)
		{
			_telemetryClient = new TelemetryClient(new TelemetryConfiguration
			{
				InstrumentationKey = configuration.InstrumentationKey
			});
		}

		/// <summary>
		/// Tracks the event.
		/// </summary>
		/// <param name="eventTelemetry">The event telemetry.</param>
		public void TrackEvent(EventTelemetry eventTelemetry)
		{
			_telemetryClient.TrackEvent(eventTelemetry);
		}

		/// <summary>
		/// Tracks the dependency.
		/// </summary>
		/// <param name="dependencyTelemetry">The dependency telemetry.</param>
		public void TrackDependency(DependencyTelemetry dependencyTelemetry)
		{
			_telemetryClient.TrackDependency(dependencyTelemetry);
		}

		/// <summary>
		/// Tracks the trace.
		/// </summary>
		/// <param name="traceTelemetry">The trace telemetry.</param>
		public void TrackTrace(TraceTelemetry traceTelemetry)
		{
			_telemetryClient.TrackTrace(traceTelemetry);
		}

		/// <summary>
		/// Flushes this instance.
		/// </summary>
		public void Flush()
		{
			_telemetryClient.Flush();
		}
	}
}
