// <copyright file="ExcludeFromCodeCoverageAttribute.cs" company="Maxx Technologies Australia PTY LTD">
// Copyright by Maxx Technologies Australia PTY LTD. All rights reserved.
// </copyright>

namespace maxx.common.Attributes
{
	using System;

	/// <summary>
	/// Initializes a new instance of the <see cref="ExcludeFromCodeCoverageAttribute" /> class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Interface)]
	[ExcludeFromCodeCoverage]
	public class ExcludeFromCodeCoverageAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExcludeFromCodeCoverageAttribute" /> class.
		/// </summary>
		/// <param name="reason">The reason.</param>
		public ExcludeFromCodeCoverageAttribute(string reason = null)
		{
			Reason = reason;
		}

		/// <summary>
		/// Gets or sets the reason.
		/// </summary>
		/// <value>The reason.</value>
		public string Reason { get; set; }
	}
}
