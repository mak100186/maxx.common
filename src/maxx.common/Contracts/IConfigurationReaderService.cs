// ***********************************************************************
// Assembly         : maxx.web.api
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="IConfigurationReaderService.cs" company="Maxx Technologies Australia PTY LTD">
//     Copyright by Maxx Technologies Australia PTY LTD. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace maxx.common.Contracts
{
	/// <summary>
	/// Interface IConfigurationReaderService
	/// </summary>
	public interface IConfigurationReaderService
	{
		/// <summary>
		/// Gets the value from path.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="path">The path.</param>
		/// <returns>T.</returns>
		T GetValueFromPath<T>(string path);
	}
}
