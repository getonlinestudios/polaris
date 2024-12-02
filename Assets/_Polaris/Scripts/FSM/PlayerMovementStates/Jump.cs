using Polaris.Characters;
using Polaris.Input;
using Polaris.Utilities;
using UnityEngine;

namespace Polaris.FSM.PlayerMovementStates
{
    public class Jump : CharacterState
    {
        private static readonly int AnimationId = Animator.StringToHash("Airborne");
        private static readonly int AnimationFloatId = Animator.StringToHash("VelocityY");

        private readonly InputController _input;
        private readonly Character _character;

        public Jump(Character character) : base(character)
        {
            _input = character.Input;
            _character = character;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();
            Mover.SetVelocityY(Stats.MaxJumpVelocity);
            
            Animator.SetBool(AnimationId, true);
        }
        
        public override void Execute()
        {
            base.Execute();
            Mover.ApplyGravity(Stats.Gravity);
            
            // Allow horizontal movement & flipping while airborne
            Mover.SetVelocityX(_input.MoveDirection.x * Stats.Speed);
            _character.OrientSprite((int)_input.MoveDirection.x);
            var mappedValue = Utility.Map(
                Mover.CurrentVelocity.y, 
                -Stats.MaxJumpVelocity, 
                Stats.MaxJumpVelocity, 
                0, 
                1, 
                true);
            Animator.SetFloat(AnimationFloatId, mappedValue);
            
            // Character is on the way up, this is the only time we can 'cancel' the jump.
            if (!_input.Jump && Mover.CurrentVelocity.y > Stats.MinJumpVelocity)
            {
               Mover.SetVelocityY(Stats.MinJumpVelocity); 
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            Animator.SetBool(AnimationId, false);
            Animator.SetFloat(AnimationFloatId, 0f);
        }
    }
}