using Polaris.CharacterComponents;
using UnityEngine;

namespace Polaris.StateMachines
{
    public class Idle : CharacterState
    {
        private static readonly int IdleAnimation = Animator.StringToHash("Idle");

        public Idle(Character character) : base(character)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            AnimatorController.SetBool(IdleAnimation, true);
            Movement.SetVelocityX(0);
        }

        public override void OnExit()
        {
            base.OnExit();
            
            AnimatorController.SetBool(IdleAnimation, false);
        }
    }
}