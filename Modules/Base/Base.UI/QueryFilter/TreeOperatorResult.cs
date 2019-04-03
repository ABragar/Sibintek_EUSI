using System;

namespace Base.UI.QueryFilter
{
    public class TreeOperatorResult<TValue>
    {
        private readonly TValue _value;
        private readonly Func<IQueryTreeBuilderContext, TValue> _getResult;

        private TreeOperatorResult(TValue value, Func<IQueryTreeBuilderContext, TValue> getResult)
        {
            _value = value;
            _getResult = getResult;
        }

        public bool Mutable => _getResult != null;

        public TValue GetValue()
        {
            if (_getResult != null)
                throw new InvalidOperationException();

            return _value;
        }

        public TValue GetResult(IQueryTreeBuilderContext context)
        {
            return _getResult == null ? _value : _getResult(context);
        }

        public static implicit operator TreeOperatorResult<TValue>(TValue value)
        {
            return new TreeOperatorResult<TValue>(value, null);
        }

        public static implicit operator TreeOperatorResult<TValue>(Func<IQueryTreeBuilderContext, TValue> valueFunc)
        {
            return new TreeOperatorResult<TValue>(default(TValue), valueFunc);
        }

        public TreeOperatorResult<TResult> Modify<TResult>(Func<TValue, TResult> modifyFunc)
        {
            if (_getResult == null)
                return modifyFunc(_value);

            return new TreeOperatorResult<TResult>(default(TResult), context => modifyFunc(_getResult(context)));
        }

        public TreeOperatorResult<TResult> Combine<TRight, TResult>(TreeOperatorResult<TRight> right, Func<TValue, TRight, TResult> combineFunc)
        {
            if (_getResult == null)
                return right.Modify(x => combineFunc(_value, x));

            if (right._getResult == null)
                return Modify(x => combineFunc(x, right._value));

            return new TreeOperatorResult<TResult>(default(TResult), context => combineFunc(GetResult(context), right.GetResult(context)));
        }

    }
}