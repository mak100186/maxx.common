// ***********************************************************************
// Assembly         : maxx.testcommon
// Author           : Muhammed Ali Khan
// Created          : 09-18-2021
//
// Last Modified By : Muhammed Ali Khan
// Last Modified On : 09-18-2021
// ***********************************************************************
// <copyright file="TestUnityContainer.cs" company="maxx.testcommon">
//     Copyright (c) Maxx Technologies. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace maxx.testcommon.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Reflection;
	using Unity;
	using Unity.Injection;
	using Unity.Lifetime;
	using Unity.Resolution;

	/// <summary>
	/// Class TestUnityContainer.
	/// Implements the <see cref="Unity.IUnityContainer" />
	/// </summary>
	/// <seealso cref="Unity.IUnityContainer" />
	public class TestUnityContainer : IUnityContainer
	{
		/// <summary>
		/// The register factories
		/// </summary>
		public readonly List<RegisterFactory> RegisterFactories = new();
		/// <summary>
		/// The register instances
		/// </summary>
		public readonly List<RegisterInstance> RegisterInstances = new();
		/// <summary>
		/// The register types
		/// </summary>
		public readonly List<RegisterType> RegisterTypes = new();

		/// <inheritdoc />
		public void Dispose()
		{
		}

		/// <inheritdoc />
		public object Resolve(Type type, string name, params ResolverOverride[] resolverOverrides)
		{
			return null;
		}

		/// <inheritdoc />
		public object BuildUp(Type type, object existing, string name, params ResolverOverride[] resolverOverrides)
		{
			return null;
		}

		/// <inheritdoc />
		public IUnityContainer CreateChildContainer()
		{
			return this;
		}

		/// <inheritdoc />
		public IUnityContainer RegisterType(
			Type registeredType, Type mappedToType, string name, ITypeLifetimeManager lifetimeManager,
			params InjectionMember[] injectionMembers)
		{
			RegisterTypes.Add(new RegisterType
			{
				RegisteredType = registeredType,
				MappedToType = mappedToType,
				Name = name,
				TypeLifetimeManager = lifetimeManager,
				InjectionMembers = injectionMembers
			});

			return this;
		}

		/// <inheritdoc />
		public IUnityContainer RegisterInstance(Type type, string name, object instance, IInstanceLifetimeManager lifetimeManager)
		{
			RegisterInstances.Add(new RegisterInstance
			{
				Type = type,
				Name = name,
				Instance = instance,
				LifetimeManager = lifetimeManager
			});

			return this;
		}

		/// <inheritdoc />
		public IUnityContainer RegisterFactory(Type type, string name, Func<IUnityContainer, Type, string, object> factory, IFactoryLifetimeManager lifetimeManager)
		{
			RegisterFactories.Add(new RegisterFactory
			{
				Type = type,
				Name = name,
				Factory = factory,
				LifetimeManager = lifetimeManager
			});

			return this;
		}

		/// <inheritdoc />
		public bool IsRegistered(Type type, string name)
		{
			return true;
		}

		/// <inheritdoc />
		public IUnityContainer Parent { get; }

		/// <inheritdoc />
		public IEnumerable<IContainerRegistration> Registrations { get; }
	}

	/// <summary>
	/// Class RegisterType.
	/// </summary>
	public class RegisterType
	{
		/// <summary>
		/// Gets or sets the injection members.
		/// </summary>
		/// <value>The injection members.</value>
		public InjectionMember[] InjectionMembers { get; set; }

		/// <summary>
		/// Gets or sets the type of the mapped to.
		/// </summary>
		/// <value>The type of the mapped to.</value>
		public Type MappedToType { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the type of the registered.
		/// </summary>
		/// <value>The type of the registered.</value>
		public Type RegisteredType { get; set; }

		/// <summary>
		/// Gets or sets the type lifetime manager.
		/// </summary>
		/// <value>The type lifetime manager.</value>
		public ITypeLifetimeManager TypeLifetimeManager { get; set; }
	}

	/// <summary>
	/// Class RegisterInstance.
	/// </summary>
	public class RegisterInstance
	{
		/// <summary>
		/// Gets or sets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public object Instance { get; set; }

		/// <summary>
		/// Gets or sets the lifetime manager.
		/// </summary>
		/// <value>The lifetime manager.</value>
		public IInstanceLifetimeManager LifetimeManager { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		/// <value>The type.</value>
		public Type Type { get; set; }
	}

	/// <summary>
	/// Class RegisterFactory.
	/// </summary>
	public class RegisterFactory
	{
		/// <summary>
		/// Gets or sets the factory.
		/// </summary>
		/// <value>The factory.</value>
		public Func<IUnityContainer, Type, string, object> Factory { get; set; }

		/// <summary>
		/// Gets or sets the lifetime manager.
		/// </summary>
		/// <value>The lifetime manager.</value>
		public IFactoryLifetimeManager LifetimeManager { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		/// <value>The type.</value>
		public Type Type { get; set; }
	}

	/// <summary>
	/// Class TestContainerExtensions.
	/// </summary>
	public static class TestContainerExtensions
	{
		/// <summary>
		/// Verifies the injection for parameter.
		/// </summary>
		/// <param name="container">The container.</param>
		/// <param name="registerType">Type of the register.</param>
		/// <param name="parameter">The parameter.</param>
		/// <exception cref="Exception">$"Parameter with type {parameter.ParameterType.Name} and name {parameter.Name} in dependency {dependencyName} is not found in registered types or instances.</exception>
		public static void VerifyInjectionForParameter(this TestUnityContainer container, RegisterType registerType, ParameterInfo parameter)
		{
			var dependencyAttribute = (DependencyAttribute)parameter.GetCustomAttribute(typeof(DependencyAttribute));

			Expression<Func<RegisterType, bool>> registeredTypesPredicate = t => t.RegisteredType == parameter.ParameterType;
			Expression<Func<RegisterInstance, bool>> registerInstancesPredicate = t => t.Type == parameter.ParameterType;

			if (dependencyAttribute != null)
			{
				registeredTypesPredicate = registeredTypesPredicate.And(t => t.Name == dependencyAttribute.Name);
				registerInstancesPredicate = registerInstancesPredicate.And(t => t.Name == dependencyAttribute.Name);
			}

			var parameterInRegisteredTypes =
				container.RegisterTypes.FirstOrDefault(registeredTypesPredicate.Compile());

			var parameterInRegisteredInstances =
				container.RegisterInstances.FirstOrDefault(registerInstancesPredicate.Compile());

			if (parameterInRegisteredTypes == null && parameterInRegisteredInstances == null)
			{
				foreach (var injectionMember in registerType.InjectionMembers)
				{
					var injectionConstructor = (InjectionConstructor)injectionMember;
					var parameterInInjectionMembers = injectionConstructor.Data.FirstOrDefault(o => o.GetType().GetInterfaces().Any(i => i == parameter.ParameterType));

					if (parameterInInjectionMembers != null)
					{
						return;
					}
				}

				var dependencyName = registerType.RegisteredType != null ? registerType.RegisteredType.Name : registerType.MappedToType.Name;

				throw new Exception($"Parameter with type {parameter.ParameterType.Name} and name {parameter.Name} in dependency {dependencyName} is not found in registered types or instances.");
			}
		}

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
