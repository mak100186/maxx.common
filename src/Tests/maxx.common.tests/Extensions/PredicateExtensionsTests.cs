// ***********************************************************************
// Assembly         : maxx.common.tests
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="PredicateExtensionsTests.cs" company="maxx.common.tests">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace maxx.common.tests.Extensions
{
	using System;
	using System.Linq.Expressions;
	using maxx.common.Extensions;
	using NFluent;
	using Xunit;

	/// <summary>
	/// Predicate Extensions Tests
	/// </summary>
	public class PredicateExtensionsTests
	{
		/// <summary>
		/// The equals a
		/// </summary>
		private readonly Expression<Func<string, bool>> equalsA = str => str == "A";
		/// <summary>
		/// The equals c
		/// </summary>
		private readonly Expression<Func<string, bool>> equalsC = str => str == "C";
		/// <summary>
		/// The contains b
		/// </summary>
		private readonly Expression<Func<string, bool>> containsB = str => str.Contains("B");
		/// <summary>
		/// The contains b1
		/// </summary>
		private readonly Expression<Func<string, bool>> containsB1 = str1 => str1 == "B1";

		/// <summary>
		/// Defines the test method Can_Begin_New_Expression.
		/// </summary>
		[Fact]
		public void Can_Begin_New_Expression()
		{
			//arrange
			var predicate = PredicateExtensions.Begin<string>(value: true);
			Expression<Func<string, bool>> expectedOrExpression = str => str == "A" || str.Contains("B");
			var expectedExpression = expectedOrExpression.ToString();

			//Act
			var orExpression = predicate.Or(right: equalsA.Or(right: containsB));
			var resultExpression = orExpression.ToString();

			//Assert
			Check.That(value: resultExpression).Equals(obj: expectedExpression);
		}

		/// <summary>
		/// Defines the test method Can_Begin_New_Expression_False.
		/// </summary>
		[Fact]
		public void Can_Begin_New_Expression_False()
		{
			//arrange
			var predicate = PredicateExtensions.Begin<string>();
			Expression<Func<string, bool>> expectedOrExpression = str => str == "A" || str.Contains("B");
			var expectedExpression = expectedOrExpression.ToString();

			//Act
			var orExpression = predicate.Or(right: equalsA.Or(right: containsB));
			var resultExpression = orExpression.ToString();

			//Assert
			Check.That(value: resultExpression).Equals(obj: expectedExpression);
		}

		/// <summary>
		/// Defines the test method Can_Reduce_Grouped_Predicates.
		/// </summary>
		[Fact]
		public void Can_Reduce_Grouped_Predicates()
		{
			//arrange
			Expression<Func<string, bool>> expectedGroupedPredicate =
				str => (str == "A" || str.Contains("B")) && str == "C";

			var expectedExpression = expectedGroupedPredicate.ToString();

			//act
			var groupedPredicate =
				equalsA.Or(right: containsB)
							.And(right: equalsC);

			var resultExpression = groupedPredicate.ToString();

			//assert
			Check.That(value: resultExpression).Equals(obj: expectedExpression);
		}

		/// <summary>
		/// Defines the test method Can_Reduce_Predicates_With_PredicateExtensions_And_Method.
		/// </summary>
		[Fact]
		public void Can_Reduce_Predicates_With_PredicateExtensions_And_Method()
		{
			//arrange
			Expression<Func<string, bool>> expectedAndExpression = str => str == "A" && str.Contains("B");
			var expectedExpression = expectedAndExpression.ToString();

			//Act
			var andExpression = equalsA.And(right: containsB);
			var resultExpression = andExpression.ToString();

			//Assert
			Check.That(value: resultExpression).Equals(obj: expectedExpression);
		}

		/// <summary>
		/// Defines the test method Can_Reduce_Predicates_With_PredicateExtensions_And_Method_No_Right_Expression.
		/// </summary>
		[Fact]
		public void Can_Reduce_Predicates_With_PredicateExtensions_And_Method_No_Right_Expression()
		{
			//arrange
			Expression<Func<string, bool>> expectedAndExpression = str => str == "A";
			var expectedExpression = expectedAndExpression.ToString();

			//Act
			var andExpression = equalsA.And(right: null);

			//Assert
			var resultExpression = andExpression.ToString();
			Check.That(value: resultExpression).Equals(obj: expectedExpression);
		}

		/// <summary>
		/// Defines the test method Can_Reduce_Predicates_With_PredicateExtensions_And_Method1.
		/// </summary>
		[Fact]
		public void Can_Reduce_Predicates_With_PredicateExtensions_And_Method1()
		{
			//arrange
			Expression<Func<string, bool>> expectedAndExpression = str => str == "A" && str == "B1";
			var expectedExpression = expectedAndExpression.ToString();

			//Act
			var andExpression = equalsA.And(right: containsB1);
			var resultExpression = andExpression.ToString();

			//Assert
			Check.That(value: resultExpression).Equals(obj: expectedExpression);
		}

		/// <summary>
		/// Defines the test method Can_Reduce_Predicates_With_PredicateExtensions_Or_Method.
		/// </summary>
		[Fact]
		public void Can_Reduce_Predicates_With_PredicateExtensions_Or_Method()
		{
			//arrange
			Expression<Func<string, bool>> expectedOrExpression = str => str == "A" || str.Contains("B");
			var expectedExpression = expectedOrExpression.ToString();

			//Act
			var orExpression = equalsA.Or(right: containsB);
			var resultExpression = orExpression.ToString();

			//Assert
			Check.That(value: resultExpression).Equals(obj: expectedExpression);
		}
	}
}
