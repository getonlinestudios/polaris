using Polaris.Characters;
using UnityEngine;

namespace Polaris.FSM.PlayerMovementStates
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

        public override void Execute()
        {
            base.Execute();
            Mover.ApplyGravity(-Stats.RequiredGravityToBeConsideredGrounded);
        }

        public override void OnExit()
        {
            base.OnExit();
            Animator.SetBool(AnimationId, false);
        }
    }
}