using System.Linq;
using Polaris.FSM;
using Polaris.FSM.Core;
using Polaris.FSM.PlayerMovementStates;
using Polaris.Input;
using Polaris.Physics;
using UnityEngine;

namespace Polaris.Characters.Components
{
    [AddComponentMenu("Polaris/Character/Components/Character Movement")]
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Stats stats;
        public Vector2 CurrentVelocity => _velocity;
        
        private RaycastController _raycastController;
        private InputController _input;
        private CollisionSensor _sensor;
        private Animator _animator;
        
        private Vector2 _velocity;
        
        private StateMachine _stateMachine;
        private Idle _idle;
        private Run _run;
        private Jump _jump;
        private Fall _fall;
        private GroundDash _groundDash;
        private GroundDashEnd _groundDashEnd;
        private WallSlide _wallSlide;
        private WallJump _wallJump;

        public void ComponentUpdate()
        {
            _stateMachine.Update();
            
            // Make sure that Y velocity is never higher than a certain amount.
                // TODO: I think this should be Gravity / Fall Gravity
            _velocity.y = Mathf.Clamp(_velocity.y, -stats.MaxJumpVelocity, stats.MaxJumpVelocity);
            _raycastController.Move(_velocity * Time.deltaTime);
            
            if (_sensor.Above() || _sensor.Below())
            {
                SetVelocityY(0);
            }
        }

        public int FacingDirection => _raycastController.CollisionInfo.FacingDirection;
        public void SetVelocityX(float value) => _velocity.x = value;
        public void SetVelocityY(float value) => _velocity.y = value;

        /// <summary>
        /// Applies gravity downwards on the Y axis. Please note that
        /// the value will be multiplied by deltaTime to spread the value over time.
        /// </summary>
        /// TODO: Might be a good idea to just get the Stat component and apply that here.
        /// TODO: We need a way to clamp this value so it does not EXCEED a value.
        /// <param name="gravityValue">Value of gravity.</param>
        public void ApplyGravity(float gravityValue) => _velocity.y += gravityValue * Time.deltaTime;
        public void SetHorizontalVelocityToZero() => _velocity.x = 0;
        public void SetVerticalVelocityToZero() => _velocity.y = 0;
        public void SetVelocity(Vector2 value) => _velocity = value;

        private void At(IState from, IState to, IPredicate condition) =>
            _stateMachine.AddTransition(from, to, condition);
        private void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);
        
        private void SetupStateMachine(Character character)
        {
            // todo: maybe this belongs in the state machine (for movement)
            
            _idle = new Idle(character);
            _run = new Run(character);
            _jump = new Jump(character);
            _fall = new Fall(character);
            _groundDash = new GroundDash(character);
            _groundDashEnd = new GroundDashEnd(character);
            _wallSlide = new WallSlide(character);
            _wallJump = new WallJump(character);

            At(_idle, _run, new FunctionPredicate(() => _input.HorizontalInput != 0));
            At(_idle, _jump, new FunctionPredicate(() => _input.Jump && _sensor.Below()));
            At(_idle, _fall, new FunctionPredicate(() => !_sensor.Below()));
            At(_idle, _groundDash, new FunctionPredicate(() => _input.DashPressed));

            At(_run, _idle, new FunctionPredicate(() => _input.HorizontalInput == 0));
            At(_run, _jump, new FunctionPredicate(() => _input.Jump && _sensor.Below()));
            At(_run, _fall, new FunctionPredicate(() => !_sensor.Below()));
            At(_run, _groundDash, new FunctionPredicate(() => _input.DashPressed));

            At(_jump, _idle, new FunctionPredicate(() => _sensor.Below() && _input.HorizontalInput == Direction.Neutral));
            At(_jump, _run, new FunctionPredicate(() => _sensor.Below() && _input.HorizontalInput != Direction.Neutral));
            At(_jump, _wallSlide, new FunctionPredicate(() => (_sensor.Right() && _input.HorizontalInput == Direction.Right)
                                                              || (_sensor.Left() && _input.HorizontalInput == Direction.Left)));

            At(_fall, _idle, new FunctionPredicate(() => _sensor.Below() && _input.HorizontalInput == Direction.Neutral));
            At(_fall, _run, new FunctionPredicate(() => _sensor.Below() && _input.HorizontalInput != Direction.Neutral));
            At(_fall, _wallSlide, new FunctionPredicate(() => (_sensor.Right() && _input.HorizontalInput == Direction.Right)
                                                              || (_sensor.Left() && _input.HorizontalInput == Direction.Left)));


            At(_groundDash, _run,
                new FunctionPredicate(() =>
                    _groundDash.Duration > stats.DashDuration && _input.HorizontalInput != Direction.Neutral));
            At(_groundDash, _run,
                new FunctionPredicate(() => !_input.DashHeld && _input.HorizontalInput != Direction.Neutral));
            At(_groundDash, _run,
                new FunctionPredicate(() => (_input.HorizontalInput == 1 && FacingDirection == Direction.Left)
                                            || (_input.HorizontalInput == -1 && FacingDirection == Direction.Right)));
            At(_groundDash, _jump, new FunctionPredicate(() => _input.Jump && _sensor.Below()));
            At(_groundDash, _fall, new FunctionPredicate(() => !_sensor.Below()));
            At(_groundDash, _groundDashEnd,
                new FunctionPredicate(() =>
                    _groundDash.Duration > stats.DashDuration && _input.HorizontalInput == Direction.Neutral));
            At(_groundDash, _groundDashEnd,
                new FunctionPredicate(() => !_input.DashHeld && _input.HorizontalInput == Direction.Neutral));

            var groundDashEndClip =
                _animator.runtimeAnimatorController
                    .animationClips
                    .FirstOrDefault(c =>
                    {
                        const string groundDashEnd = "Ground Dash End";
                        return groundDashEnd.Equals(c.name);
                    });
            var minDuration = 0.25f;
            if (groundDashEndClip != null)
            {
                minDuration = groundDashEndClip.length;
            }

            At(_groundDashEnd, _idle, new FunctionPredicate(() => _groundDashEnd.Duration > minDuration));
            At(_groundDashEnd, _run, new FunctionPredicate(() => _input.HorizontalInput != Direction.Neutral));
            At(_groundDashEnd, _jump, new FunctionPredicate(() => _input.Jump && _sensor.Below()));
            At(_groundDashEnd, _jump, new FunctionPredicate(() => !_sensor.Below()));

            At(_wallSlide, _idle, new FunctionPredicate(() => _sensor.Below()));

            // todo: should be a wall slide coyote time
            At(_wallSlide, _fall, new FunctionPredicate(() => _input.HorizontalInput == Direction.Neutral));
            At(_wallSlide, _fall, new FunctionPredicate(() => _input.HorizontalInput == Direction.Left && _sensor.Right()));
            At(_wallSlide, _fall, new FunctionPredicate(() => _input.HorizontalInput == Direction.Right && _sensor.Left()));
            // Wall is no longer detected.
            At(_wallSlide, _fall, new FunctionPredicate(() => _input.HorizontalInput == Direction.Left && !_sensor.Left()));
            At(_wallSlide, _fall, new FunctionPredicate(() => _input.HorizontalInput == Direction.Right && !_sensor.Right()));
            At(_wallSlide, _wallJump, new FunctionPredicate(() => _input.Jump));

            At(_wallJump, _idle, new FunctionPredicate(() => _sensor.Below()));
            At(_wallJump, _wallSlide, new FunctionPredicate(() => (_sensor.Right() && _input.HorizontalInput == Direction.Right)
                                                                  || (_sensor.Left() &&
                                                                      _input.HorizontalInput == Direction.Left)));
        }
        
        private void Awake()
        {
            _raycastController = GetComponent<RaycastController>();
            _input = GetComponent<InputController>();
            _sensor = GetComponent<CollisionSensor>();
            _velocity = Vector2.zero;
            _stateMachine = new StateMachine();
        }

        private void Start()
        {
            var character = GetComponent<Character>();
            _animator = character.Animator;
            
            SetupStateMachine(character);

            _stateMachine.SetState(_idle);
        }

    }
}