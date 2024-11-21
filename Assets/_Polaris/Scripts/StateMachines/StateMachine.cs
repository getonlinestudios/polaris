using Polaris.CharacterComponents;
using Polaris.Input;
using UnityEngine;

namespace Polaris.StateMachines
{
    public class StateMachine
    {
        public CharacterState Current { get; private set; }
        public CharacterState Previous { get; private set; }

        private PlayerInputController _inputController;

        private CharacterState _idle;
        private CharacterState _run;

        public StateMachine(Character character)
        {
            _inputController = character.InputController;
            
            _idle = new Idle(character);
            _run = new Run(character);

            Current = _idle;
        }

        public void InitialOnEnter()
        {
             Current.OnEnter();
        }

        public void Update()
        {
            MovementTransitionChecks();
            Current.Execute();
        }
        
        private void ChangeState(CharacterState next)
        {
            if (next == Current)
            {
                return;
            }
            Current.OnExit();
            Previous = Current;
            Current = next;
            Current.OnEnter();
        }

        private void MovementTransitionChecks()
        {
            // Grounded
            if (_inputController.MoveDirection.x == 0)
            {
                // idle
                ChangeState(_idle);
            }
            
            if (_inputController.MoveDirection.x != 0)
            {
                // run
                ChangeState(_run);
            }
        }
    }
}