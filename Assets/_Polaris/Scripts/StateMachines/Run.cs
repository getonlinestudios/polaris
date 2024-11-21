using Polaris.CharacterComponents;

namespace Polaris.StateMachines
{
    public class Run : CharacterState
    {
        public Run(Character character) : base(character)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            AnimatorController.SetBool("Run", true);
        }

        public override void Execute()
        {
            base.Execute();
            // Movement.SetVelocityX(Movement.Speed * InputController.MoveDirection.x);
            Movement.SetVelocityX(0 * InputController.MoveDirection.x);
        }

        public override void OnExit()
        {
            base.OnExit();
            AnimatorController.SetBool("Run", false);
        }
    }
}