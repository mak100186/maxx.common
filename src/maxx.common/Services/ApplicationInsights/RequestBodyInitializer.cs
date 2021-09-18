// ***********************************************************************
// Assembly         : maxx.common
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="RequestBodyInitializer.cs" company="maxx.common">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace maxx.common.Services.ApplicationInsights
{
	using System;
	using System.Linq;
	using System.Net.Http;
	using maxx.common.Attributes;
	using Microsoft.ApplicationInsights.Channel;
	using Microsoft.ApplicationInsights.DataContracts;
	using Microsoft.ApplicationInsights.Extensibility;
	using Microsoft.AspNetCore.Http;

	/// <summary>
	/// Initializes to add Request body to AppInsights Telemetry
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class RequestBodyInitializer : ITelemetryInitializer
	{
		/// <summary>
		/// The patch method
		/// </summary>
		private const string PatchMethod = "PATCH";
		/// <summary>
		/// The HTTP context accessor
		/// </summary>
		private readonly IHttpContextAccessor _httpContextAccessor;

		/// <summary>
		/// Initializes a new instance of the <see cref="RequestBodyInitializer" /> class.
		/// </summary>
		/// <param name="httpContextAccessor">instance of <see cref="IHttpContextAccessor" /></param>
		public RequestBodyInitializer(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		/// <summary>
		/// Gets the restricted telemetry names.
		/// </summary>
		/// <value>The restricted telemetry names.</value>
		private static string[] RestrictedTelemetryNames => new[] { "POST Authentication/Authenticate", "POST Users/CreateUser", "PUT Users/ChangePassword" };

		/// <inheritdoc />
		public void Initialize(ITelemetry telemetry)
		{
			if (telemetry is RequestTelemetry requestTelemetry)
			{
				if ((requestTelemetry.Name.StartsWith(HttpMethod.Post.ToString(), StringComparison.InvariantCultureIgnoreCase)
						|| requestTelemetry.Name.StartsWith(HttpMethod.Put.ToString(), StringComparison.InvariantCultureIgnoreCase)
						|| requestTelemetry.Name.StartsWith(PatchMethod, StringComparison.InvariantCultureIgnoreCase))
						&& !RestrictedTelemetryNames.Any(x => requestTelemetry.Name.StartsWith(x)))
				{
					var requestBody = _httpContextAccessor.HttpContext.Items[RequestBodyTracker.RequestBodyKey] as string;

					if (!string.IsNullOrWhiteSpace(requestBody) && requestTelemetry.Properties.All(p => p.Key != "body"))
					{
						requestTelemetry.Properties.Add("body", requestBody);
					}
				}

				var requestIpAddress = _httpContextAccessor.HttpContext.Items[RequestBodyTracker.RequestIpAddressKey] as string;

				if (!string.IsNullOrWhiteSpace(requestIpAddress) && requestTelemetry.Properties.All(p => p.Key != "ipAddress"))
				{
					requestTelemetry.Properties.Add("ipAddress", requestIpAddress);
				}
			}
		}
	}
}
