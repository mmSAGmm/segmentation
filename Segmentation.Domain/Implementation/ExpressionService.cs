using NReco.Linq;
using Segmentation.Domain.Abstractions;
using Segmentation.DomainModels;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Text;

namespace Segmentation.Domain.Implementation
{
    public class ExpressionService : IExpressionService
    {
        NReco.Linq.LambdaParser _lambdaParser = new NReco.Linq.LambdaParser();

        public Func<object, bool> Parse(Segment segment) 
        {
            var param = Expression.Parameter(typeof(object), "x");
            var expression = _lambdaParser.Parse(segment.Expression);
            return Expression.Lambda<Func<object, bool>>(expression, param).Compile();
        }
    }

//    using NReco.Linq;
//using System.Linq.Expressions;

//var parser = new LambdaParser();

//    // создаём параметр "x"
//    var param = Expression.Parameter(typeof(int), "x");

//    // парсим строку в Expression
//    var expr = parser.Parse("x*2+5", new Dictionary<string, object>(), param);

//    // компилируем в Func<int,int>
//    var lambda = Expression.Lambda<Func<int, int>>(expr, param).Compile();

//    Console.WriteLine(lambda(10)); // 


}
