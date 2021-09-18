// ***********************************************************************
// Assembly         : maxx.web.api
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="IApplicationInsightConfiguration.cs" company="Maxx Technologies Australia PTY LTD">
//     Copyright by Maxx Technologies Australia PTY LTD. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace maxx.common.Contracts.ApplicationInsights
{
	/// <summary>
	/// Interface IApplicationInsightConfiguration
	/// </summary>
	public interface IApplicationInsightConfiguration
	{
		/// <summary>
		/// Gets or sets the name of the application.
		/// </summary>
		/// <value>The name of the application.</value>
		string ApplicationName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether [automatic flush].
		/// </summary>
		/// <value><c>true</c> if [automatic flush]; otherwise, <c>false</c>.</value>
		bool AutoFlush { get; set; }

		/// <summary>
		/// Gets or sets the instrumentation key.
		/// </summary>
		/// <value>The instrumentation key.</value>
		string InstrumentationKey { get; set; }
	}
}
