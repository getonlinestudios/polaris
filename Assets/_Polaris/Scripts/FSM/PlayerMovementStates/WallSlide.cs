using Polaris.Characters;
using Polaris.Input;
using UnityEngine;

namespace Polaris.FSM.PlayerMovementStates
{
    public class WallSlide : CharacterState
    {
        private static readonly int AnimationId = Animator.StringToHash("WallSlide");
        private readonly InputController _input;

        public WallSlide(Character character) : base(character)
        {
            _input = character.Input;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Animator.SetBool(AnimationId, true);
        }

        public override void Execute()
        {
            base.Execute();
            Mover.ApplyGravity(-Stats.WallSlideSpeed);
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