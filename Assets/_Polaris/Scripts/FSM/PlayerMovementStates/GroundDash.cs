using Polaris.Characters;
using Polaris.Characters.Components;
using Polaris.Input;
using Polaris.Utilities;
using UnityEngine;

namespace Polaris.FSM.PlayerMovementStates
{
    public class GroundDash : CharacterState
    {
        private static readonly int AnimationId = Animator.StringToHash("Dash");

        private readonly InputController _input;
        private readonly Character _character;

        public GroundDash(Character character) : base(character)
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
            Mover.SetVelocityX(Mover.FacingDirection.AsValue() * Stats.DashSpeed);
            _character.OrientSprite((int)_input.MoveDirection.x);
            Mover.ApplyGravity(-Stats.RequiredGravityToBeConsideredGrounded);
        }

        public override void OnExit()
        {
            base.OnExit();
            Animator.SetBool(AnimationId, false);
        }
    }
}