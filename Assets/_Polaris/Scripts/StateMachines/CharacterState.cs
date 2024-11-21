using Polaris.CharacterComponents;
using Polaris.Input;
using UnityEngine;

namespace Polaris.StateMachines
{
    public abstract class CharacterState : IState
    {
        public float Duration => Time.time - StartTime;
        protected float StartTime { get; private set; }
        protected CharacterMovement Movement { get; }
        protected PlayerInputController InputController { get; }
        protected Animator AnimatorController { get; }

        protected CharacterState(Character character)
        {
            Movement = character.GetComponent<CharacterMovement>();
            InputController = character.GetComponent<PlayerInputController>();
            AnimatorController = character.GetComponent<Animator>();
        }

        public virtual void OnEnter()
        {
            StartTime = Time.time;
            Debug.Log($"Entered state: {GetType().Name}");
        }

        public virtual void Execute()
        {
        }

        public virtual void OnExit()
        {
        }
    }
}