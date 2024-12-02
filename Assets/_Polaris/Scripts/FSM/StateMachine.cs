using System;
using System.Collections.Generic;
using Polaris.FSM.Core;
using Polaris.FSM.PlayerMovementStates;
using UnityEngine;

namespace Polaris.FSM
{
    public class StateMachine
    {
        public IState Current => _current.State;
        private StateNode _current;
        private readonly Dictionary<Type, StateNode> _stateNodes = new();
        private readonly HashSet<ITransition> _anyTransitions = new();


        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
        }

        public void AddAnyTransition(IState to, IPredicate condition)
        {
            _anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition));
        }

        private StateNode GetOrAddNode(IState state)
        {
            var node = _stateNodes.GetValueOrDefault(state.GetType());

            if (node != null) 
            {
                return node;
            }
            
            node = new StateNode(state);
            _stateNodes.Add(state.GetType(), node);

            return node;
        }

        public void Update()
        {
            var transition = GetTransition();
            if (transition != null)
            {
                ChangeState(transition.To);
            }
            
            _current.State.Execute();
        }


        public void SetState(IState state)
        {
            _current = _stateNodes[state.GetType()];
            _current.State.OnEnter();
        }

        private ITransition GetTransition()
        {
            foreach (var anyTransition in _anyTransitions)
            {
                if (anyTransition.Condition.Evaluate())
                    return anyTransition;
            }

            foreach (var currentStateTransition in _current.Transitions)
            {
                if (currentStateTransition.Condition.Evaluate())
                    return currentStateTransition;
            }

            return null;
        }
        
        private void ChangeState(IState state)
        {
            if (state == _current.State)
            {
                return;
            }

            var nextStateNode = _stateNodes[state.GetType()];
            
            var previousState = _current.State;
            var nextState = nextStateNode.State;
            
            previousState.OnExit();
            nextState.OnEnter();
            
            _current = nextStateNode;
        }

        private class StateNode
        {
            public IState State { get; }
            public HashSet<ITransition> Transitions { get; }

            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<ITransition>();
            }

            public void AddTransition(IState to, IPredicate condition)
            {
                Transitions.Add(new Transition(to, condition));
            }
        }
    }
}