// ***********************************************************************
// Assembly         : maxx.web.api
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="ILoggingService.cs" company="Maxx Technologies Australia PTY LTD">
//     Copyright by Maxx Technologies Australia PTY LTD. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace maxx.common.Contracts
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;
	using System.Threading.Tasks;
	using maxx.common.Enumerations;

	/// <summary>
	/// Interface ILoggingService
	/// </summary>
	public interface ILoggingService
	{
		/// <summary>
		/// Errors the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="severity">The severity.</param>
		/// <param name="caller">The caller.</param>
		/// <param name="sourceFilePath">The source file path.</param>
		/// <param name="sourceLineNumber">The source line number.</param>
		/// <param name="correlationId">The correlation identifier.</param>
		void Error(string message, Severity severity = Severity.Error, [CallerMemberName] string caller = null, [CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = -1, string correlationId = null);

		/// <summary>
		/// Errors the specified exception.
		/// </summary>
		/// <param name="exception">The exception.</param>
		/// <param name="severity">The severity.</param>
		/// <param name="caller">The caller.</param>
		/// <param name="sourceFilePath">The source file path.</param>
		/// <param name="sourceLineNumber">The source line number.</param>
		/// <param name="correlationId">The correlation identifier.</param>
		void Error(Exception exception, Severity severity = Severity.Error, [CallerMemberName] string caller = null, [CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = -1, string correlationId = null);

		/// <summary>
		/// Logs the specified message.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="severity">The severity.</param>
		/// <param name="caller">The caller.</param>
		/// <param name="sourceFilePath">The source file path.</param>
		/// <param name="sourceLineNumber">The source line number.</param>
		/// <param name="correlationId">The correlation identifier.</param>
		void Log(string message, Severity severity = Severity.Information, [CallerMemberName] string caller = null, [CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = -1, string correlationId = null);

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
		Task RunTask(Func<Task> taskToRun, string message, [CallerMemberName] string caller = null, [CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = -1, string correlationId = null);

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
		Task<T> RunTask<T>(Func<Task<T>> taskToRun, string message, [CallerMemberName] string caller = null, [CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = -1, string correlationId = null);

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
		void TrackDependency(
			string dependencyName, string target = null, string dependencyTypeName = null, string data = null, DateTimeOffset? timestamp = null, TimeSpan? duration = null, string resultCode = null, bool? success = null, [CallerMemberName] string caller = null,
			[CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = -1, Dictionary<string, string> extraProperties = null);

		/// <summary>
		/// Tracks the event.
		/// </summary>
		/// <param name="eventName">Name of the event.</param>
		/// <param name="caller">The caller.</param>
		/// <param name="sourceFilePath">The source file path.</param>
		/// <param name="sourceLineNumber">The source line number.</param>
		void TrackEvent(string eventName, [CallerMemberName] string caller = null, [CallerFilePath] string sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = -1);
	}
}
