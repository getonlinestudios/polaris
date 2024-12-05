using Polaris.Characters;
using Polaris.Characters.Components;
using Polaris.Input;
using UnityEngine;

namespace Polaris.FSM.PlayerMovementStates
{
    public class WallJump : CharacterState
    {
        private static readonly int AnimationId = Animator.StringToHash("WallJump");
        private readonly InputController _input;

        private bool _horizontalMovementAllowed;
        private readonly CollisionSensor _sensor;
        private int _awayFromWallDirection;

        public WallJump(Character character) : base(character)
        {
            _input = character.Input;
            _sensor = character.CollisionSensor;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _horizontalMovementAllowed = false;
            Animator.SetBool(AnimationId, true);
            _awayFromWallDirection = _sensor.Right() ? 1 : -1;
            _awayFromWallDirection *= -1; // We want to go in the opposite direction
            
            var wallJumpVector = new Vector2(_awayFromWallDirection * Stats.WallJumpHorizontalSpeed, Stats.MaxJumpVelocity);
            Mover.SetVelocity(wallJumpVector);
        }

        public override void Execute()
        {
            base.Execute();
            
            Mover.ApplyGravity(Stats.Gravity);
            if (Duration > Stats.WallJumpNoHorizontalControlDuration)
            {
                _horizontalMovementAllowed = true;
                // Set the velocity to zero. If there is no input the player will keep moving horizontally.
                Mover.SetHorizontalVelocityToZero(); 
            }

            // todo: doesn't work as expected.
            // if (_input.JumpCanceled && Mover.CurrentVelocity.y > Stats.MinJumpVelocity)
            // {
            //     Mover.SetVelocityY(Stats.MinJumpVelocity);
            // }

            if (!_horizontalMovementAllowed)
            {
                return;
            }
            
            Mover.SetVelocityX(_input.HorizontalInput * Stats.Speed);
        }

        public override void OnExit()
        {
            base.OnExit();
            Animator.SetBool(AnimationId, false);
        }
    }
}