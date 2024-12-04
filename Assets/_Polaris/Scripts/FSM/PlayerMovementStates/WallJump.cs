using Polaris.Characters;
using Polaris.Characters.Components;
using Polaris.Input;
using UnityEngine;

namespace Polaris.FSM.PlayerMovementStates
{
    public class WallJump : CharacterState
    {
        private static readonly int AnimationId = Animator.StringToHash("WallSlide");
        private readonly InputController _input;

        private bool _horizontalMovementAllowed;
        private readonly CollisionSensor _sensor;
        private int _awayFromWallDirection;

        public WallJump(Character character) : base(character)
        {
            _input = character.Input;
            _sensor = character.CollisionSensor;
            _horizontalMovementAllowed = false;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Animator.SetBool(AnimationId, true);
            _awayFromWallDirection = _sensor.Right() ? 1 : -1;
            _awayFromWallDirection *= -1; // We want to go in the opposite direction
        }

        public override void Execute()
        {
            base.Execute();
            if (Duration > 0.15f)
            {
                _horizontalMovementAllowed = true;
            }

            if (!_horizontalMovementAllowed)
            {
                // for a brief time, Inputs in the horizontal direction are not allowed until we are done moving
                // todo: you can also "cut" early. If the button is not pressed.
                // so meaning if Input.Jump is not held for the full time, use the minJumpVelocity
                var dir = new Vector2(_awayFromWallDirection * Stats.WallJumpHorizontalSpeed, Stats.MaxJumpVelocity);
                Mover.SetVelocity(dir);
                return;
            }
            
            
            Mover.SetVelocityX(_input.MoveDirection.x * Stats.Speed);
        }

        public override void OnExit()
        {
            base.OnExit();
            Animator.SetBool(AnimationId, false);
            // if we've canceled, apply a "get off" force away from the wall
            // 2 options
            // Someone marks this as being canceled, then we know to do something (out of state)
            // We figure out if we've hit that criteria ourselves (in the state)
        }
    }
}