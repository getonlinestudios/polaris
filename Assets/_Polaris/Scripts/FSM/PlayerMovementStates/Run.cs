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
            Mover.SetVelocityX(_input.MoveDirection.x * Stats.Speed);
            _character.OrientSprite((int)_input.MoveDirection.x);
            Mover.ApplyGravity(-0.001f);
        }

        public override void OnExit()
        {
            base.OnExit();
            Animator.SetBool(AnimationId, false);
        }
    }
}