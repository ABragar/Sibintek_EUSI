using System.Collections.Generic;
using System.Linq;

namespace Base.Identity
{



    public struct OperationResult
    {
        private static readonly OperationResult _succeed = new OperationResult(true, null);

        private static readonly IReadOnlyCollection<string> EmptyMessages = new string[] { };


        public IReadOnlyCollection<string> Messages { get; }

        public bool Success { get; }

        internal OperationResult(bool success,IReadOnlyCollection<string> messages)
        {
            Success = success;
            Messages = messages;
        }

        public void ThrowIfError()
        {
            if (!Success)
                throw new OperationException(Messages);
        }

        public OperationResult<T> ToSucceed<T>(T result)
        {

            return new OperationResult<T>(result,true,Messages);
        }

        public OperationResult<T> ToFailed<T>()
        {
            return new OperationResult<T>(default(T),false,Messages);
        }

        public static OperationResult Succeed()
        {
            return _succeed;
        }

        public static OperationResult Succeed(params string[] messages)
        {
            return new OperationResult(true,messages);
        }

        public static OperationResult<T> Succeed<T>(T result, params string[] messages)
        {
            return new OperationResult<T>(result,true,messages);
        }

        public static OperationResult Failed(IEnumerable<string> messages)
        {

            return new OperationResult(false,messages?.ToArray() ?? EmptyMessages);
        }

        public static OperationResult Failed(params string[] messages)
        {


            return new OperationResult(false, messages ?? EmptyMessages);
        }

        public static OperationResult<T> Failed<T>(IEnumerable<string> messages)
        {

            return new OperationResult<T>(default(T), false, messages?.ToArray() ?? EmptyMessages);
        }

        public static OperationResult<T> Failed<T>(params string[] messages)
        {

            return new OperationResult<T>(default(T),false,messages ?? EmptyMessages);
        }
    }


    public struct OperationResult<T>
    {
        private readonly T _result;

        public IReadOnlyCollection<string> Messages { get; }

        public bool Success { get; }

        internal OperationResult(T result,bool success, IReadOnlyCollection<string> messages)
        {
            _result = result;
            Success = success;
            Messages = messages;
        }

        public OperationResult<T2> ToSucceed<T2>(T2 result)
        {

            return new OperationResult<T2>(result, true, Messages);
        }

        public OperationResult<T2> ToFailed<T2>()
        {
            return new OperationResult<T2>(default(T2), false, Messages);
        }

        public OperationResult ToSucceed()
        {

            return new OperationResult(true, Messages);
        }

        public OperationResult ToFailed()
        {
            return new OperationResult(false, Messages);
        }

        public T Result
        {
            get
            {
                if (!Success)
                    throw new OperationException(Messages);

                return _result;
            }
        }

    }
}