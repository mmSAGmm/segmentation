using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Text;

namespace Segmentation.Domain.Models
{
    public class TypeMissmatchDynamicWrapper(object value, ILogger<TypeMissmatchDynamicWrapper> logger) : DynamicObject
    {
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
                var left = Expression.Constant(value);
                var right = Expression.Constant(arg);

                var expr = Expression.MakeBinary(binder.Operation, left, right);
                var lambda = Expression.Lambda<Func<object>>(Expression.Convert(expr, typeof(object)));
                result = lambda.Compile()();
                return true;
            }
            catch(Exception ex)
            {

                logger.LogError(ex, "Failed on TryBinaryOperation");
                result = null;
                return false;
            }

            return base.TryBinaryOperation(binder, arg, out result);
        }
    }
}
