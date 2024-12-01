using Polaris.Characters;
using Polaris.Characters.Components;
using Polaris.FSM.Core;
using UnityEngine;

namespace Polaris.FSM
{
    public abstract class CharacterState : IState
    {
        public float Duration => Time.time - _startTime;
        
        protected Stats Stats { get; }
        protected Movement Mover { get; }
        protected Animator Animator { get; }

        private float _startTime;
        
        protected CharacterState(Character character)
        {
            // todo: this will do a get component call for EVERY state.
            Stats = character.Stats;
            Mover = character.Mover;
            Animator = character.Animator;
        }
        
        public virtual void OnEnter()
        {
            _startTime = Time.time;
        }

        public virtual void Execute()
        {
        }

        public virtual void OnExit()
        {
        }
    }
}