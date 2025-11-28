using NReco.Linq;
using Segmentation.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Segmentation.Domain.Implementation
{
    public class ExpressionService
    {
        NReco.Linq.LambdaParser _lambdaParser = new NReco.Linq.LambdaParser();

        public Func<int, bool> Parse(Segment segment) 
        {
            var param = Expression.Parameter(typeof(int), "x");
            var expression = _lambdaParser.Parse(segment.Expression);
            return Expression.Lambda<Func<int, bool>>(expression, param).Compile();
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
