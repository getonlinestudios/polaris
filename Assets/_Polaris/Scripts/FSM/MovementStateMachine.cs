using Polaris.Characters;

namespace Polaris.FSM
{
    public class MovementStateMachine : IState
    {
        private CharacterState _current;
        
        private Idle _idle;
        private Run _run;

        public MovementStateMachine(Character character)
        {
            _idle = new Idle(character);
            _run = new Run(character);
        }
        
        public void OnEnter()
        {
        }

        public void Execute()
        {
        }

        public void OnExit()
        {
        }
    }
}