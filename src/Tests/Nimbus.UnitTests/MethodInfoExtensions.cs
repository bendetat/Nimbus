﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Nimbus.Extensions;

namespace Nimbus.UnitTests
{
    public static class MethodInfoExtensions
    {
        public static bool IsExtensionMethodFor<T>(this MethodInfo methodInfo)
        {
            if (!methodInfo.IsStatic) return false;
            if (!methodInfo.IsPublic) return false;
            if (!methodInfo.IsDefined(typeof (ExtensionAttribute), true)) return false;

            var args = methodInfo.GetParameters();
            if (args.None()) return false;

            var firstArg = args.First();
            return typeof (T).IsAssignableFrom(firstArg.ParameterType);
        }

        public static string MethodName<TType, TResult>(this TType obj, Expression<Func<TType, TResult>> methodExpr)
        {
            return ((MethodCallExpression)methodExpr.Body).Method.Name;
        }

        public static string MethodName<TType, TResult>(Expression<Func<TType, TResult>> methodExpr)
        {
            return ((MethodCallExpression)methodExpr.Body).Method.Name;
        }

        public static string MethodName<T>(this T obj, Expression<Action<T>> methodExpr)
        {
            return ((MethodCallExpression)methodExpr.Body).Method.Name;
        }

        public static string MethodName<T>(Expression<Action<T>> methodExpr)
        {
            return ((MethodCallExpression)methodExpr.Body).Method.Name;
        }
    }
}