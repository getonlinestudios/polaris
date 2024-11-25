using Polaris.Characters;
using Polaris.Input;
using UnityEngine;

namespace Polaris.FSM
{
    public class Idle : CharacterState
    {
        private static readonly int AnimationId = Animator.StringToHash("Idle");

        public Idle(Character character) : base(character)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Mover.SetHorizontalVelocityToZero();
            Animator.SetBool(AnimationId, true);
        }

        public override void OnExit()
        {
            base.OnExit();
            Animator.SetBool(AnimationId, false);
        }
    }
    
    public class Run : CharacterState
    {
        private static readonly int AnimationId = Animator.StringToHash("Run");

        private readonly InputController _input;
        
        public Run(Character character) : base(character)
        {
            _input = character.Input;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            Mover.SetVelocityX(_input.MoveDirection.x * Stats.Speed);
            
            Animator.SetBool(AnimationId, true);
        }

        public override void OnExit()
        {
            base.OnExit();
            Animator.SetBool(AnimationId, false);
        }
    }
}