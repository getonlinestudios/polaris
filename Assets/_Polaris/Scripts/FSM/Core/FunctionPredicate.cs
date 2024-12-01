using System;

namespace Polaris.FSM.Core
{
    public class FunctionPredicate : IPredicate
    {
        private readonly Func<bool> _function;

        public FunctionPredicate(Func<bool> func)
        {
            _function = func;
        }
        
        public bool Evaluate() => _function.Invoke();
    }
}