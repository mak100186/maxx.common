// ***********************************************************************
// Assembly         : maxx.testcommon
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="FormData.cs" company="maxx.testcommon">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace maxx.testcommon.Models
{
	using System.Net.Http;

	/// <summary>
	/// Class FormData.
	/// </summary>
	public class FormData
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FormData"/> class.
		/// </summary>
		/// <param name="boundary">The boundary.</param>
		/// <param name="fileName">Name of the file.</param>
		/// <param name="formContent">Content of the form.</param>
		/// <param name="name">The name.</param>
		public FormData(string boundary, string fileName, ByteArrayContent formContent, string name)
		{
			Boundary = boundary;
			FileName = fileName;
			FormContent = formContent;
			Name = name;
		}

		/// <summary>
		/// Gets or sets the boundary.
		/// </summary>
		/// <value>The boundary.</value>
		public string Boundary { get; set; }

		/// <summary>
		/// Gets or sets the name of the file.
		/// </summary>
		/// <value>The name of the file.</value>
		public string FileName { get; set; }

		/// <summary>
		/// Gets or sets the content of the form.
		/// </summary>
		/// <value>The content of the form.</value>
		public ByteArrayContent FormContent { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }
	}
}
