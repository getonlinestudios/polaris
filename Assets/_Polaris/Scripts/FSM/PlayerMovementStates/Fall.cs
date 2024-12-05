using Polaris.Characters;
using Polaris.Input;
using Polaris.Utilities;
using UnityEngine;

namespace Polaris.FSM.PlayerMovementStates
{
    public class Fall : CharacterState
    {
        private static readonly int AnimationId = Animator.StringToHash("Airborne");
        private static readonly int AnimationFloatId = Animator.StringToHash("VelocityY");

        private readonly InputController _input;
        private readonly Character _character;

        public Fall(Character character) : base(character)
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
            Mover.ApplyGravity(Stats.Gravity);
            
            var mappedValue = Utility.Map(
                Mover.CurrentVelocity.y, 
                -Stats.MaxJumpVelocity, 
                Stats.MaxJumpVelocity, 
                0, 
                1, 
                true);
            Animator.SetFloat(AnimationFloatId, mappedValue);
            
            // Allow horizontal movement & flipping while airborne
            Mover.SetVelocityX(_input.HorizontalInput * Stats.Speed);
            _character.OrientSprite(_input.HorizontalInput);
        }

        public override void OnExit()
        {
            base.OnExit();
            Animator.SetBool(AnimationId, false);
            Animator.SetFloat(AnimationFloatId, 0f);
        }
    }
}