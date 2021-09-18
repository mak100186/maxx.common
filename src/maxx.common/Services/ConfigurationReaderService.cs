// ***********************************************************************
// Assembly         : maxx.web.api
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="ConfigurationReaderService.cs" company="Maxx Technologies Australia PTY LTD">
//     Copyright by Maxx Technologies Australia PTY LTD. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace maxx.common.Services
{
	using System;
	using System.Diagnostics;
	using maxx.common.Contracts;
	using maxx.common.Extensions;
	using Microsoft.Extensions.Configuration;

	/// <summary>
	/// Class ConfigurationReaderService.
	/// Implements the <see cref="IConfigurationReaderService" />
	/// </summary>
	/// <seealso cref="IConfigurationReaderService" />
	public class ConfigurationReaderService : IConfigurationReaderService
	{
		/// <summary>
		/// The configuration
		/// </summary>
		private readonly IConfiguration _configuration;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationReaderService"/> class.
		/// </summary>
		/// <param name="settingFileName">Name of the setting file.</param>
		public ConfigurationReaderService(string settingFileName)
		{
			_configuration = new ConfigurationBuilder()
											.AddJsonFile(settingFileName, true, true)
											.Build();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationReaderService"/> class.
		/// </summary>
		/// <param name="configuration">The configuration.</param>
		public ConfigurationReaderService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		/// <summary>
		/// Gets the value from path. Use : to be the path separator
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="path">The path.</param>
		/// <returns>T.</returns>
		public T GetValueFromPath<T>(string path)
		{
			try
			{
				var value = _configuration[path];

				return value.ChangeType<T>();
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);

				throw;
			}
		}
	}
}
