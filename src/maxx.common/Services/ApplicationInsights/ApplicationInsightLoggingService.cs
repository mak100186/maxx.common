// ***********************************************************************
// Assembly         : maxx.web.api
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="ApplicationInsightLoggingService.cs" company="Maxx Technologies Australia PTY LTD">
//     Copyright by Maxx Technologies Australia PTY LTD. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace maxx.common.Services.ApplicationInsights
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;
	using maxx.common.Contracts.ApplicationInsights;
	using maxx.common.Enumerations;
	using Microsoft.ApplicationInsights.DataContracts;

	/// <summary>
	/// Class ApplicationInsightLoggingService.
	/// Implements the <see cref="LoggingService" />
	/// </summary>
	/// <seealso cref="LoggingService" />
	public class ApplicationInsightLoggingService : LoggingService
	{
		/// <summary>
		/// The configuration
		/// </summary>
		private readonly IApplicationInsightConfiguration _configuration;
		/// <summary>
		/// The telemetry client wrapper
		/// </summary>
		private readonly ITelemetryClientWrapper _telemetryClientWrapper;

		/// <summary>
		/// Initializes a new instance of the <see cref="ApplicationInsightLoggingService"/> class.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		/// <param name="telemetryClientWrapper">The telemetry client wrapper.</param>
		public ApplicationInsightLoggingService(IApplicationInsightConfiguration configuration, ITelemetryClientWrapper telemetryClientWrapper)
		{
			ValidateConfiguration(configuration);
			_configuration = configuration;
			_telemetryClientWrapper = telemetryClientWrapper;
		}

		/// <summary>
		/// Traces the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="severity">The severity.</param>
		/// <param name="caller">The caller.</param>
		/// <param name="sourceFilePath">The source file path.</param>
		/// <param name="sourceLineNumber">The source line number.</param>
		/// <param name="correlationId">The correlation identifier.</param>
		public override void Trace(string message, Severity severity = Severity.Verbose, [CallerMemberName] string caller = null, [CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = -1, string correlationId = null)
		{
			var properties = new Dictionary<string, string>
			{
				{
					nameof(caller),
					caller
				},
				{
					nameof(sourceFilePath),
					sourceFilePath
				},
				{
					nameof(sourceLineNumber),
					sourceLineNumber.ToString()
				}
			};

			if (!string.IsNullOrWhiteSpace(correlationId))
			{
				properties.Add(nameof(correlationId), correlationId);
			}

			Trace(message, properties, GetLogSeverityLevel(severity));
		}

		/// <summary>
		/// Traces the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="executionDuration">Duration of the execution.</param>
		/// <param name="severity">The severity.</param>
		/// <param name="caller">The caller.</param>
		/// <param name="sourceFilePath">The source file path.</param>
		/// <param name="sourceLineNumber">The source line number.</param>
		/// <param name="correlationId">The correlation identifier.</param>
		public override void Trace(string message, TimeSpan executionDuration, Severity severity = Severity.Verbose, [CallerMemberName] string caller = null, [CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = -1, string correlationId = null)
		{
			var properties = new Dictionary<string, string>
			{
				{
					nameof(caller),
					caller
				},
				{
					nameof(sourceFilePath),
					sourceFilePath
				},
				{
					nameof(sourceLineNumber),
					sourceLineNumber.ToString()
				},
				{
					"executationDuration",
					executionDuration.ToString()
				}
			};

			if (!string.IsNullOrWhiteSpace(correlationId))
			{
				properties.Add(nameof(correlationId), correlationId);
			}

			Trace(message, properties, GetLogSeverityLevel(severity));
		}

		/// <summary>
		/// Tracks the dependency.
		/// </summary>
		/// <param name="dependencyName">Name of the dependency.</param>
		/// <param name="target">The target.</param>
		/// <param name="dependencyTypeName">Name of the dependency type.</param>
		/// <param name="data">The data.</param>
		/// <param name="timestamp">The timestamp.</param>
		/// <param name="duration">The duration.</param>
		/// <param name="resultCode">The result code.</param>
		/// <param name="success">if set to <c>true</c> [success].</param>
		/// <param name="caller">The caller.</param>
		/// <param name="sourceFilePath">The source file path.</param>
		/// <param name="sourceLineNumber">The source line number.</param>
		/// <param name="extraProperties">The extra properties.</param>
		public override void TrackDependency(
			string dependencyName, string target = null, string dependencyTypeName = null, string data = null, DateTimeOffset? timestamp = null, TimeSpan? duration = null, string resultCode = null, bool? success = null, [CallerMemberName] string caller = null,
			[CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = -1, Dictionary<string, string> extraProperties = null)
		{
			var dictionary = new Dictionary<string, string>
			{
				{ nameof(caller), caller },
				{ nameof(sourceFilePath), sourceFilePath },
				{ nameof(sourceLineNumber), sourceLineNumber.ToString() }
			};

			var dependencyTelemetry1 = new DependencyTelemetry
			{
				Name = dependencyName
			};
			var dependencyTelemetry2 = dependencyTelemetry1;

			if (!string.IsNullOrWhiteSpace(target))
			{
				dependencyTelemetry2.Target = target;
			}

			if (!string.IsNullOrWhiteSpace(dependencyTypeName))
			{
				dependencyTelemetry2.Type = dependencyTypeName;
			}

			if (!string.IsNullOrWhiteSpace(data))
			{
				dependencyTelemetry2.Data = data;
			}

			if (timestamp.HasValue)
			{
				dependencyTelemetry2.Timestamp = timestamp.Value;
			}

			if (duration.HasValue)
			{
				dependencyTelemetry2.Duration = duration.Value;
			}

			if (!string.IsNullOrWhiteSpace(resultCode))
			{
				dependencyTelemetry2.ResultCode = resultCode;
			}

			if (success.HasValue)
			{
				dependencyTelemetry2.Success = success.Value;
			}

			foreach (var keyValuePair in dictionary)
			{
				dependencyTelemetry2.Properties.Add(keyValuePair.Key, keyValuePair.Value);
			}

			if (extraProperties != null)
			{
				foreach (var extraProperty in extraProperties)
				{
					dependencyTelemetry2.Properties.Add(extraProperty);
				}
			}

			if (!string.IsNullOrEmpty(_configuration.ApplicationName))
			{
				dependencyTelemetry2.Properties.Add("applicationName", _configuration.ApplicationName);
			}

			_telemetryClientWrapper.TrackDependency(dependencyTelemetry2);
			Flush();
		}

		/// <summary>
		/// Tracks the event.
		/// </summary>
		/// <param name="eventName">Name of the event.</param>
		/// <param name="caller">The caller.</param>
		/// <param name="sourceFilePath">The source file path.</param>
		/// <param name="sourceLineNumber">The source line number.</param>
		public override void TrackEvent(string eventName, [CallerMemberName] string caller = null, [CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = -1)
		{
			var dictionary = new Dictionary<string, string>
			{
				{ nameof(caller), caller },
				{ nameof(sourceFilePath), sourceFilePath },
				{ nameof(sourceLineNumber), sourceLineNumber.ToString() }
			};
			var eventTelemetry = new EventTelemetry(eventName);

			foreach (var keyValuePair in dictionary)
			{
				eventTelemetry.Properties.Add(keyValuePair.Key, keyValuePair.Value);
			}

			if (!string.IsNullOrEmpty(_configuration.ApplicationName))
			{
				eventTelemetry.Properties.Add("applicationName", _configuration.ApplicationName);
			}

			_telemetryClientWrapper.TrackEvent(eventTelemetry);
			Flush();
		}

		/// <summary>
		/// Flushes this instance.
		/// </summary>
		private void Flush()
		{
			if (!_configuration.AutoFlush)
			{
				return;
			}

			_telemetryClientWrapper.Flush();
		}

		/// <summary>
		/// Gets the log severity level.
		/// </summary>
		/// <param name="severity">The severity.</param>
		/// <returns>SeverityLevel.</returns>
		private SeverityLevel GetLogSeverityLevel(Severity severity)
		{
			switch (severity)
			{
				case Severity.Information:
					return SeverityLevel.Information;
				case Severity.Warning:
					return SeverityLevel.Warning;
				case Severity.Error:
					return SeverityLevel.Error;
				case Severity.Critical:
					return SeverityLevel.Critical;
				default:
					return SeverityLevel.Verbose;
			}
		}

		/// <summary>
		/// Traces the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="properties">The properties.</param>
		/// <param name="severityLevel">The severity level.</param>
		private void Trace(string message, Dictionary<string, string> properties, SeverityLevel severityLevel)
		{
			var traceTelemetry = new TraceTelemetry(message, severityLevel);

			foreach (var property in properties)
			{
				traceTelemetry.Properties.Add(property.Key, property.Value);
			}

			if (!string.IsNullOrEmpty(_configuration.ApplicationName))
			{
				traceTelemetry.Properties.Add("applicationName", _configuration.ApplicationName);
			}

			_telemetryClientWrapper.TrackTrace(traceTelemetry);
			Flush();
		}

		/// <summary>
		/// Validates the configuration.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		/// <exception cref="ArgumentException">configuration</exception>
		/// <exception cref="ArgumentException">InstrumentationKey</exception>
		private void ValidateConfiguration(IApplicationInsightConfiguration configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentException(nameof(configuration));
			}

			if (string.IsNullOrEmpty(configuration.InstrumentationKey))
			{
				throw new ArgumentException("InstrumentationKey");
			}
		}
	}
}
