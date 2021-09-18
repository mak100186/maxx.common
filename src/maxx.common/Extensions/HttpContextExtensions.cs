// ***********************************************************************
// Assembly         : maxx.common
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="HttpContextExtensions.cs" company="maxx.common">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace maxx.common.Extensions
{
	using System;
	using System.Linq;
	using maxx.common.Attributes;
	using Microsoft.AspNetCore.Http;

	/// <summary>
	/// Http context extensions
	/// </summary>
	[ExcludeFromCodeCoverage]
	public static class HttpContextExtensions
	{
		/// <summary>
		/// Returns header value based on type T
		/// </summary>
		/// <typeparam name="T">Type T</typeparam>
		/// <param name="httpContext">http context</param>
		/// <param name="headerName">header name</param>
		/// <returns>Type T of the header value</returns>
		public static T GetHeaderValueAs<T>(this HttpContext httpContext, string headerName)
		{
			if (httpContext.Request?.Headers?.TryGetValue(headerName, out var values) ?? false)
			{
				var rawValues = values.ToString(); // writes out as Csv when there are multiple.

				if (!string.IsNullOrEmpty(rawValues))
				{
					return (T)Convert.ChangeType(values.ToString(), typeof(T));
				}
			}

			return default;
		}

		/// <summary>
		/// Returns requests Ip address
		/// </summary>
		/// <param name="httpContext">http Context</param>
		/// <param name="tryUseXForwardHeader">try UseXForwardHeader</param>
		/// <returns>Request's Ip Address</returns>
		public static string GetRequestIpAddress(this HttpContext httpContext, bool tryUseXForwardHeader = true)
		{
			string ip = null;

			//// todo support new "Forwarded" header (2014) https://en.wikipedia.org/wiki/X-Forwarded-For

			//// X-Forwarded-For (csv list):  Using the First entry in the list seems to work
			//// for 99% of cases however it has been suggested that a better (although tedious)
			//// approach might be to read each IP from right to left and use the first public IP.
			//// http://stackoverflow.com/a/43554000/538763
			////

			if (tryUseXForwardHeader)
			{
				ip = httpContext.GetHeaderValueAs<string>("X-Forwarded-For").SplitCsv().FirstOrDefault();
			}

			// RemoteIpAddress is always null in DNX RC1 Update1 (bug).
			if (string.IsNullOrWhiteSpace(ip) && httpContext.Connection?.RemoteIpAddress != null)
			{
				ip = httpContext.Connection.RemoteIpAddress.ToString();
			}

			if (string.IsNullOrWhiteSpace(ip))
			{
				ip = httpContext.GetHeaderValueAs<string>("REMOTE_ADDR");
			}

			return ip;
		}
	}
}
