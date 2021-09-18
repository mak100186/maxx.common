// ***********************************************************************
// Assembly         : maxx.common
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="RequestBodyTracker.cs" company="maxx.common">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace maxx.common.Services.ApplicationInsights
{
	using System;
	using System.IO;
	using System.Text;
	using System.Threading.Tasks;
	using maxx.common.Attributes;
	using maxx.common.Extensions;
	using Microsoft.AspNetCore.Http;

	/// <summary>
	/// Middleware Tracker for Request Body
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class RequestBodyTracker
	{
		/// <summary>
		/// Key for RequestBody
		/// </summary>
		public const string RequestBodyKey = "RequestBody";

		/// <summary>
		/// Key for RequestIpAddress
		/// </summary>
		public const string RequestIpAddressKey = "RequestIpAddress";

		/// <summary>
		/// The request delegate
		/// </summary>
		private readonly RequestDelegate _requestDelegate;

		/// <summary>
		/// Initializes a new instance of the <see cref="RequestBodyTracker" /> class.
		/// </summary>
		/// <param name="requestDelegate">instance of <see cref="RequestDelegate" /></param>
		public RequestBodyTracker(RequestDelegate requestDelegate)
		{
			_requestDelegate = requestDelegate;
		}

		/// <summary>
		/// Invoke
		/// </summary>
		/// <param name="context">HttpContext</param>
		/// <returns>A <see cref="Task" /> representing the asynchronous operation.</returns>
		public async Task Invoke(HttpContext context)
		{
			try
			{
				context.Request.EnableBuffering();
				await _requestDelegate.Invoke(context);
				RegisterRequestIpAddress(context);
				await RegisterRequestBody(context);
			}
			catch (Exception)
			{
				await RegisterRequestBody(context);
			}
		}

		/// <summary>
		/// Copies the stream to string.
		/// </summary>
		/// <param name="stream">The stream.</param>
		/// <returns>System.String.</returns>
		private static async Task<string> CopyStreamToString(Stream stream)
		{
			var originalPosition = stream.Position;
			RewindStream(stream);
			string requestBody;

			using (var reader = new StreamReader(stream, Encoding.UTF8, true))
			{
				requestBody = await reader.ReadToEndAsync();
				stream.Position = originalPosition;
			}

			return requestBody;
		}

		/// <summary>
		/// Registers the request body.
		/// </summary>
		/// <param name="context">The context.</param>
		private static async Task RegisterRequestBody(HttpContext context)
		{
			if (context.Request.Body?.CanSeek == false || context.Request.Method != HttpMethods.Post && context.Request.Method != HttpMethods.Put && context.Request.Method != HttpMethods.Patch)
			{
				return;
			}

			var body = await CopyStreamToString(context.Request.Body);
			context.Items[RequestBodyKey] = body;
		}

		/// <summary>
		/// Registers the request ip address.
		/// </summary>
		/// <param name="context">The context.</param>
		private static void RegisterRequestIpAddress(HttpContext context)
		{
			context.Items[RequestIpAddressKey] = context?.GetRequestIpAddress();
		}

		/// <summary>
		/// Rewinds the stream.
		/// </summary>
		/// <param name="stream">The stream.</param>
		private static void RewindStream(Stream stream)
		{
			if (stream != null)
			{
				stream.Position = 0L;
			}
		}
	}
}
