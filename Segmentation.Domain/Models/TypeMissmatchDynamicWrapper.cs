using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Text;

namespace Segmentation.Domain.Models
{
    public class TypeMissmatchDynamicWrapper(object value, ILogger<TypeMissmatchDynamicWrapper> logger) : DynamicObject
    {
        private static readonly ConcurrentDictionary<string, Func<object, object, object>> _expressionCache = new();

        HashSet<ExpressionType> supportedBoolOperations = new() { 
            ExpressionType.Equal, 
            ExpressionType.NotEqual, 
            ExpressionType.LessThan, 
            ExpressionType.GreaterThan, 
            ExpressionType.GreaterThanOrEqual, 
            ExpressionType.LessThanOrEqual,
            ExpressionType.Or,
            ExpressionType.And
        };
        public override bool TryBinaryOperation(BinaryOperationBinder binder, object arg, out object? result)
        {
            result = null;
            if (arg.GetType() != value.GetType()) 
            {
                if (supportedBoolOperations.Contains(binder.Operation))
                {
                    result = false;
                }
                else 
                {
                    return result == null;
                }
             
                return true;
            }

            try
            {
                var leftType = value.GetType();
                var rightType = arg.GetType();
                var cacheKey = $"{binder.Operation}_{leftType.FullName}_{rightType.FullName}";

                var compiledFunc = _expressionCache.GetOrAdd(cacheKey, key =>
                {
                    var leftParam = Expression.Parameter(typeof(object), "left");
                    var rightParam = Expression.Parameter(typeof(object), "right");

                    var leftConverted = Expression.Convert(leftParam, leftType);
                    var rightConverted = Expression.Convert(rightParam, rightType);

                    var expr = Expression.MakeBinary(binder.Operation, leftConverted, rightConverted);
                    var convertedExpr = Expression.Convert(expr, typeof(object));
                    var lambda = Expression.Lambda<Func<object, object, object>>(convertedExpr, leftParam, rightParam);
                    return lambda.Compile();
                });

                result = compiledFunc(value, arg);
                return true;
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Failed on TryBinaryOperation");
                result = null;
                return false;
            }
        }
    }
}
