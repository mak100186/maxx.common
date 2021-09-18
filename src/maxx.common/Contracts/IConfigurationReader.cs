// ***********************************************************************
// Assembly         : maxx.common
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="IConfigurationReader.cs" company="maxx.common">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace maxx.common.Contracts
{
	/// <summary>
	/// Configuration Reader
	/// </summary>
	public interface IConfigurationReader
	{
		/// <summary>
		/// Gets Value
		/// </summary>
		/// <param name="section">section</param>
		/// <param name="key">key</param>
		/// <returns>config value</returns>
		T GetValue<T>(string section, string key);
	}
}
