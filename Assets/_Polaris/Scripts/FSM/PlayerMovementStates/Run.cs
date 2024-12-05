using Polaris.Characters;
using Polaris.Input;
using UnityEngine;

namespace Polaris.FSM.PlayerMovementStates
{
    public class Run : CharacterState
    {
        private static readonly int AnimationId = Animator.StringToHash("Run");

        private readonly InputController _input;
        private readonly Character _character;

        public Run(Character character) : base(character)
        {
            _input = character.Input;
            _character = character;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            Animator.SetBool(AnimationId, true);
        }
        
        public override void Execute()
        {
            base.Execute();
            Mover.SetVelocityX(_input.HorizontalInput * Stats.Speed);
            _character.OrientSprite(_input.HorizontalInput);
            Mover.ApplyGravity(-Stats.RequiredGravityToBeConsideredGrounded);
        }

        public override void OnExit()
        {
            base.OnExit();
            Animator.SetBool(AnimationId, false);
        }
    }
}