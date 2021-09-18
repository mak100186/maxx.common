// ***********************************************************************
// Assembly         : maxx.web.api
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="UnityContainerExtensions.cs" company="Maxx Technologies Australia PTY LTD">
//     Copyright by Maxx Technologies Australia PTY LTD. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace maxx.common.Extensions
{
	using System;
	using maxx.common.Contracts;
	using maxx.common.Contracts.ApplicationInsights;
	using maxx.common.Services;
	using maxx.common.Services.ApplicationInsights;
	using Unity;
	using Unity.Lifetime;

	/// <summary>
	/// Class UnityContainerExtensions.
	/// </summary>
	public static class UnityContainerExtensions
	{
		/// <summary>
		/// Registers logging types in the specified <see cref="IUnityContainer" />.
		/// </summary>
		/// <param name="container">The <see cref="IUnityContainer" /> to add services to.</param>
		/// <param name="applicationInsightsConfiguration">instance of <see cref="IApplicationInsightConfiguration" /></param>
		/// <exception cref="ArgumentNullException">container</exception>
		public static void RegisterLogging(this IUnityContainer container, IApplicationInsightConfiguration applicationInsightsConfiguration)
		{
			if (container == null)
			{
				throw new ArgumentNullException(nameof(container));
			}

			container.RegisterInstance(typeof(IApplicationInsightConfiguration), applicationInsightsConfiguration);
			container.RegisterType<ITelemetryClientWrapper, TelemetryClientWrapper>(new HierarchicalLifetimeManager());
			container.RegisterType<ILoggingService, ApplicationInsightLoggingService>(new ContainerControlledLifetimeManager());
		}
	}
}
