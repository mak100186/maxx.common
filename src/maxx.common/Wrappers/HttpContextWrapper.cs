// ***********************************************************************
// Assembly         : maxx.common
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="HttpContextWrapper.cs" company="maxx.common">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace maxx.common.Wrappers
{
	using maxx.common.Attributes;
	using maxx.common.Contracts;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Http.Extensions;

	/// <summary>
	/// Http context wrapper
	/// </summary>
	[ExcludeFromCodeCoverage]
	public class HttpContextWrapper : IHttpContextWrapper
	{
		/// <summary>
		/// The HTTP context accessor
		/// </summary>
		private readonly IHttpContextAccessor _httpContextAccessor;

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpContextWrapper" /> class.
		/// </summary>
		/// <param name="httpContextAccessor">http Context Accessor</param>
		public HttpContextWrapper(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		/// <inheritdoc />
		public string GetBaseUrl()
		{
			var baseUrl = _httpContextAccessor.HttpContext.Request.GetDisplayUrl();
			var queryStrings = _httpContextAccessor.HttpContext.Request.QueryString.HasValue ? _httpContextAccessor.HttpContext.Request.QueryString.Value : null;

			if (!string.IsNullOrWhiteSpace(queryStrings))
			{
				baseUrl = baseUrl.Replace(queryStrings, string.Empty);
			}

			return baseUrl;
		}

		/// <inheritdoc />
		public string GetUrl()
		{
			return _httpContextAccessor.HttpContext.Request.GetDisplayUrl();
		}

		/// <inheritdoc />
		public bool IsAuthenticated()
		{
			return _httpContextAccessor.HttpContext.User?.Identity?.IsAuthenticated == true;
		}

		/// <inheritdoc />
		public IQueryCollection GetQueryString()
		{
			return _httpContextAccessor.HttpContext.Request.Query;
		}

		/// <inheritdoc />
		public string GetContentType()
		{
			return _httpContextAccessor.HttpContext.Request.ContentType;
		}
	}
}
