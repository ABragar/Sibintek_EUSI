using System;
using System.Linq;
using System.Linq.Expressions;

namespace Base.Translations
{


    internal class CompiledExpression
    {
        internal CompiledExpression(LambdaExpression expression)
        {
            Body = expression.Body;
            Parameter = expression.Parameters.Single();
        }

        public Expression Body { get; }
        public ParameterExpression Parameter { get; }
    }


    public sealed class CompiledExpression<T, TResult>
    {
        
        private readonly Func<T, TResult> _function;


        public CompiledExpression(Expression<Func<T, TResult>> expression)
        {


            _function = expression.Compile();
        }

        public TResult Evaluate(T instance)
        {
            return _function(instance);
        }


    }
}