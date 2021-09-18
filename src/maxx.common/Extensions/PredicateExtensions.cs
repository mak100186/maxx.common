// ***********************************************************************
// Assembly         : maxx.common
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="PredicateExtensions.cs" company="maxx.common">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace maxx.common.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;

	/// <summary>
	/// Class PredicateExtensions.
	/// </summary>
	public static class PredicateExtensions
	{
		/// <summary>
		/// Ands the specified left.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>System.Linq.Expressions.Expression&lt;System.Func&lt;T, bool&gt;&gt;.</returns>
		public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
		{
			return right == null ? left : left.CombineLambdas(right, ExpressionType.AndAlso);
		}

		/// <summary>
		/// Begins the specified value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value">The value.</param>
		/// <returns>System.Linq.Expressions.Expression&lt;System.Func&lt;T, bool&gt;&gt;.</returns>
		public static Expression<Func<T, bool>> Begin<T>(bool value = false)
		{
			if (value)
			{
				return parameter => true;
			}

			return parameter => false;
		}

		/// <summary>
		/// Ors the specified left.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <returns>System.Linq.Expressions.Expression&lt;System.Func&lt;T, bool&gt;&gt;.</returns>
		public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
		{
			return left.CombineLambdas(right, ExpressionType.OrElse);
		}

		/// <summary>
		/// Combines the lambdas.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="left">The left.</param>
		/// <param name="right">The right.</param>
		/// <param name="expressionType">Type of the expression.</param>
		/// <returns>System.Linq.Expressions.Expression&lt;System.Func&lt;T, bool&gt;&gt;.</returns>
		private static Expression<Func<T, bool>> CombineLambdas<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right, ExpressionType expressionType)
		{
			if (IsExpressionBodyConstant(left))
			{
				return right;
			}

			var parameter = left.Parameters[0];

			return Expression.Lambda<Func<T, bool>>(Expression.MakeBinary(expressionType, left.Body, new SubstituteParameterVisitor
			{
				Sub =
				{
					[right.Parameters[0]] = parameter
				}
			}.Visit(right.Body)), parameter);
		}

		/// <summary>
		/// Determines whether [is expression body constant] [the specified left].
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="left">The left.</param>
		/// <returns>bool.</returns>
		private static bool IsExpressionBodyConstant<T>(Expression<Func<T, bool>> left)
		{
			return left.Body.NodeType == ExpressionType.Constant;
		}

		/// <summary>
		/// Class SubstituteParameterVisitor.
		/// Implements the <see cref="System.Linq.Expressions.ExpressionVisitor" />
		/// </summary>
		/// <seealso cref="System.Linq.Expressions.ExpressionVisitor" />
		internal class SubstituteParameterVisitor : ExpressionVisitor
		{
			/// <summary>
			/// Gets the sub.
			/// </summary>
			/// <value>The sub.</value>
			public Dictionary<Expression, Expression> Sub { get; } = new();

			/// <summary>
			/// Visits the parameter.
			/// </summary>
			/// <param name="node">The node.</param>
			/// <returns>System.Linq.Expressions.Expression.</returns>
			protected override Expression VisitParameter(ParameterExpression node)
			{
				return Sub[node];
			}
		}
	}
}
