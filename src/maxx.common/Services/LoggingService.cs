// ***********************************************************************
// Assembly         : maxx.web.api
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="LoggingService.cs" company="Maxx Technologies Australia PTY LTD">
//     Copyright by Maxx Technologies Australia PTY LTD. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace maxx.common.Services
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;
	using System.Threading.Tasks;
	using maxx.common.Contracts;
	using maxx.common.Enumerations;

	/// <summary>
	/// Class LoggingService.
	/// Implements the <see cref="ILoggingService" />
	/// </summary>
	/// <seealso cref="ILoggingService" />
	public abstract class LoggingService : ILoggingService
	{
		/// <summary>
		/// Logs the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="severity">The severity.</param>
		/// <param name="caller">The caller.</param>
		/// <param name="sourceFilePath">The source file path.</param>
		/// <param name="sourceLineNumber">The source line number.</param>
		/// <param name="correlationId">The correlation identifier.</param>
		public void Log(string message, Severity severity = Severity.Information, [CallerMemberName] string caller = null, [CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = -1, string correlationId = null)
		{
			Trace(message, severity, caller, sourceFilePath, sourceLineNumber, correlationId);
		}

		/// <summary>
		/// Errors the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="severity">The severity.</param>
		/// <param name="caller">The caller.</param>
		/// <param name="sourceFilePath">The source file path.</param>
		/// <param name="sourceLineNumber">The source line number.</param>
		/// <param name="correlationId">The correlation identifier.</param>
		public void Error(string message, Severity severity = Severity.Error, [CallerMemberName] string caller = null, [CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = -1, string correlationId = null)
		{
			Trace(message, severity, caller, sourceFilePath, sourceLineNumber, correlationId);
		}

		/// <summary>
		/// Errors the specified exception.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="severity">The severity.</param>
		/// <param name="caller">The caller.</param>
		/// <param name="sourceFilePath">The source file path.</param>
		/// <param name="sourceLineNumber">The source line number.</param>
		/// <param name="correlationId">The correlation identifier.</param>
		public void Error(Exception exception, Severity severity = Severity.Error, [CallerMemberName] string caller = null, [CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = -1, string correlationId = null)
		{
			Trace(exception.ToString(), severity, caller, sourceFilePath, sourceLineNumber, correlationId);
		}

		/// <summary>
		/// Tracks the event.
		/// </summary>
		/// <param name="eventName">Name of the event.</param>
		/// <param name="caller">The caller.</param>
		/// <param name="sourceFilePath">The source file path.</param>
		/// <param name="sourceLineNumber">The source line number.</param>
		public abstract void TrackEvent(string eventName, [CallerMemberName] string caller = null, [CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = -1);

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
		public abstract void TrackDependency(
			string dependencyName, string target = null, string dependencyTypeName = null, string data = null, DateTimeOffset? timestamp = null, TimeSpan? duration = null, string resultCode = null, bool? success = null, [CallerMemberName] string caller = null,
			[CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = -1, Dictionary<string, string> extraProperties = null);

		/// <summary>
		/// Runs the task.
		/// </summary>
		/// <param name="taskToRun">The task to run.</param>
		/// <param name="message">The message.</param>
		/// <param name="caller">The caller.</param>
		/// <param name="sourceFilePath">The source file path.</param>
		/// <param name="sourceLineNumber">The source line number.</param>
		/// <param name="correlationId">The correlation identifier.</param>
		/// <returns>Task.</returns>
		public async Task RunTask(Func<Task> taskToRun, string message, [CallerMemberName] string caller = null, [CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = -1, string correlationId = null)
		{
			var stopWatch = InitializeAndStartStopWatch();
			await Task.Run(taskToRun);
			stopWatch.Stop();
			Trace(message, stopWatch.Elapsed, Severity.Information, caller, sourceFilePath, sourceLineNumber, correlationId);
		}

		/// <summary>
		/// Runs the task.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="taskToRun">The task to run.</param>
		/// <param name="message">The message.</param>
		/// <param name="caller">The caller.</param>
		/// <param name="sourceFilePath">The source file path.</param>
		/// <param name="sourceLineNumber">The source line number.</param>
		/// <param name="correlationId">The correlation identifier.</param>
		/// <returns>Task&lt;T&gt;.</returns>
		public async Task<T> RunTask<T>(Func<Task<T>> taskToRun, string message, [CallerMemberName] string caller = null, [CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = -1, string correlationId = null)
		{
			var stopWatch = InitializeAndStartStopWatch();
			var obj = await Task.Run(taskToRun);
			stopWatch.Stop();
			Trace(message, stopWatch.Elapsed, Severity.Information, caller, sourceFilePath, sourceLineNumber, correlationId);

			return obj;
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
		public abstract void Trace(string message, Severity severity = Severity.Verbose, [CallerMemberName] string caller = null, [CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = -1, string correlationId = null);

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
		public abstract void Trace(
			string message, TimeSpan executionDuration, Severity severity = Severity.Verbose, [CallerMemberName] string caller = null, [CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = -1, string correlationId = null);

		/// <summary>
		/// Initializes the and start stop watch.
		/// </summary>
		/// <returns>Stopwatch.</returns>
		private Stopwatch InitializeAndStartStopWatch()
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			return stopwatch;
		}
	}
}
