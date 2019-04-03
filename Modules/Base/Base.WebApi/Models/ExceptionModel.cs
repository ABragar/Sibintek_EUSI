using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Base.WebApi.Models
{
    public class ExceptionModel
    {
        private readonly Lazy<object> _inner;
        private readonly Exception _exception;
        private readonly Lazy<ICollection<string>> _stack;

        public ExceptionModel(Exception exception)
        {
            _exception = exception;
            Name = _exception.GetType().Name;

            _stack = new Lazy<ICollection<string>>(() =>
                new StackTrace(exception)
                .GetFrames()?
                .Select(GetString).ToList());

            _inner = new Lazy<object>(() =>
            {
                var aggr = _exception as AggregateException;

                if (aggr != null)
                {

                    return aggr.InnerExceptions.Select(x => new ExceptionModel(x));

                }

                return _exception.InnerException == null ? null : new ExceptionModel(_exception.InnerException);


            });

        }


        private static string GetString(StackFrame frame)
        {

            var sb = new StringBuilder();

            var method = frame.GetMethod();
            var type = method.DeclaringType;

            if (type != null)
            {
                WriteNameString(sb, type);

                sb.Append('.');
            }
            sb.Append(method.Name);

            if (method.IsGenericMethod)
            {
                WriteGenericArguments(sb, method.GetGenericArguments());
            }

            sb.Append('(');

            var first = true;
            foreach (var parameter in method.GetParameters())
            {

                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(',');
                }
                WriteNameString(sb, parameter.ParameterType);

                sb.Append(' ');
                sb.Append(parameter.Name);
            }




            sb.Append(')');

            return sb.ToString();
        }



        private static void WriteGenericArguments(StringBuilder sb, IEnumerable<Type> types)
        {
            sb.Append('<');

            var first = true;
            foreach (var arg in types)
            {

                if (first)
                {
                    first = false;
                }
                else
                {
                    sb.Append(',');
                }
                WriteNameString(sb, arg);
            }
            sb.Append('>');
        }

        private static void WriteNameString(StringBuilder sb,Type type)
        {
            if (type.Namespace != null)
            {
                sb.Append(type.Namespace);

                sb.Append('.');
            }

            sb.Append(type.Name);

            if (type.IsGenericType)
            {
                WriteGenericArguments(sb, type.GetGenericArguments());
            }
            
        }

        public string Name { get; }

        public string Message => _exception.Message;

        public ICollection<string> Stack => _stack.Value;

        public object Inner => _inner.Value;


    }
}