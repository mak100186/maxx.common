// ***********************************************************************
// Assembly         : maxx.common
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="IHttpContextWrapper.cs" company="maxx.common">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace maxx.common.Contracts
{
	using Microsoft.AspNetCore.Http;

	/// <summary>
	/// Http context wrapper
	/// </summary>
	public interface IHttpContextWrapper
	{
		/// <summary>
		/// Gets base url
		/// </summary>
		/// <returns>base url</returns>
		string GetBaseUrl();

		/// <summary>
		/// Gets the type of the content.
		/// </summary>
		/// <returns>System.String.</returns>
		string GetContentType();

		/// <summary>
		/// Gets the query string.
		/// </summary>
		/// <returns>IQueryCollection.</returns>
		IQueryCollection GetQueryString();

		/// <summary>
		/// Gets url
		/// </summary>
		/// <returns>base url</returns>
		string GetUrl();

		/// <summary>
		/// Determines whether this instance is authenticated.
		/// </summary>
		/// <returns><c>true</c> if this instance is authenticated; otherwise, <c>false</c>.</returns>
		bool IsAuthenticated();
	}
}
