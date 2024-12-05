using System.Linq;
using Polaris.Characters.Components;
using Polaris.FSM;
using Polaris.FSM.Core;
using Polaris.FSM.PlayerMovementStates;
using Polaris.Input;
using UnityEngine;

namespace Polaris.Characters
{
    [AddComponentMenu("Polaris/Character/Character")]
    public class Character : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Stats stats;

        public Stats Stats => stats;
        public InputController Input { get; private set; }
        public Movement Mover { get; private set; }
        public CollisionSensor CollisionSensor { get; private set; }
        public Animator Animator => animator;

        private StateMachine _stateMachine;
        private Idle _idle;
        private Run _run;
        private Jump _jump;
        private Fall _fall;
        private GroundDash _groundDash;
        private GroundDashEnd _groundDashEnd;
        private WallSlide _wallSlide;
        private WallJump _wallJump;

        public void OrientSprite(int direction)
        {
            if (direction == 0) return;
            var dir = Mathf.Sign(direction);
            transform.localScale = new Vector3(dir, 1, 1);
        }

        private void At(IState from, IState to, IPredicate condition) =>
            _stateMachine.AddTransition(from, to, condition);

        private void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);
        
        private void Awake()
        {
            Input = GetComponent<InputController>();
            Mover = GetComponent<Movement>();
            CollisionSensor = GetComponent<CollisionSensor>();
            
            _stateMachine = new StateMachine();
            
            _idle = new Idle(this);
            _run = new Run(this);
            _jump = new Jump(this);
            _fall = new Fall(this);
            _groundDash = new GroundDash(this);
            _groundDashEnd = new GroundDashEnd(this);
            _wallSlide = new WallSlide(this);
            _wallJump = new WallJump(this);
            
            At(_idle, _run, new FunctionPredicate(() => Input.HorizontalInput != 0)); 
            At(_idle, _jump, new FunctionPredicate(() => Input.Jump && CollisionSensor.Below()));
            At(_idle, _fall, new FunctionPredicate(() => !CollisionSensor.Below()));
            At(_idle, _groundDash, new FunctionPredicate(() => Input.DashPressed));
            
            At(_run, _idle, new FunctionPredicate(() => Input.HorizontalInput == 0)); 
            At(_run, _jump, new FunctionPredicate(() => Input.Jump && CollisionSensor.Below()));
            At(_run, _fall, new FunctionPredicate(() => !CollisionSensor.Below()));
            At(_run, _groundDash, new FunctionPredicate(() => Input.DashPressed));
            
            At(_jump, _idle, new FunctionPredicate(() => CollisionSensor.Below() && Input.HorizontalInput == Direction.Neutral));
            At(_jump, _run, new FunctionPredicate(() => CollisionSensor.Below() && Input.HorizontalInput != Direction.Neutral));
            At(_jump, _wallSlide, new FunctionPredicate(() => (CollisionSensor.Right() && Input.HorizontalInput == Direction.Right) 
                                                              || (CollisionSensor.Left() && Input.HorizontalInput == Direction.Left)));
            
            At(_fall, _idle, new FunctionPredicate(() => CollisionSensor.Below() && Input.HorizontalInput == Direction.Neutral));
            At(_fall, _run, new FunctionPredicate(() => CollisionSensor.Below() && Input.HorizontalInput != Direction.Neutral));
            At(_fall, _wallSlide, new FunctionPredicate(() => (CollisionSensor.Right() && Input.HorizontalInput == Direction.Right) 
                                                              || (CollisionSensor.Left() && Input.HorizontalInput == Direction.Left)));
            
            
            At(_groundDash, _run, new FunctionPredicate(() => _groundDash.Duration > stats.DashDuration && Input.HorizontalInput != Direction.Neutral));
            At(_groundDash, _run,
                new FunctionPredicate(() => !Input.DashHeld && Input.HorizontalInput != Direction.Neutral));
            At(_groundDash, _run,
                new FunctionPredicate(() => (Input.HorizontalInput == 1 && Mover.FacingDirection == Direction.Left) 
                                            || (Input.HorizontalInput == -1 && Mover.FacingDirection == Direction.Right)));
            At(_groundDash, _jump, new FunctionPredicate(() => Input.Jump && CollisionSensor.Below()));
            At(_groundDash, _fall, new FunctionPredicate(() => !CollisionSensor.Below()));
            At(_groundDash, _groundDashEnd, new FunctionPredicate(() => _groundDash.Duration > stats.DashDuration && Input.HorizontalInput == Direction.Neutral));
            At(_groundDash, _groundDashEnd, new FunctionPredicate(() => !Input.DashHeld && Input.HorizontalInput == Direction.Neutral));

            var groundDashEndClip =
                Animator.runtimeAnimatorController
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
            At(_groundDashEnd, _run, new FunctionPredicate(() => Input.HorizontalInput != Direction.Neutral));
            At(_groundDashEnd, _jump, new FunctionPredicate(() => Input.Jump && CollisionSensor.Below()));
            At(_groundDashEnd, _jump, new FunctionPredicate(() => !CollisionSensor.Below()));
            
            At(_wallSlide, _idle, new FunctionPredicate(() => CollisionSensor.Below()));

            // todo: should be a wall slide coyote time
            At(_wallSlide, _fall, new FunctionPredicate(() => Input.HorizontalInput == Direction.Neutral));
            At(_wallSlide, _fall, new FunctionPredicate(() => Input.HorizontalInput == Direction.Left && CollisionSensor.Right()));
            At(_wallSlide, _fall, new FunctionPredicate(() => Input.HorizontalInput == Direction.Right && CollisionSensor.Left()));
            // Wall is no longer detected.
            At(_wallSlide, _fall, new FunctionPredicate(() => Input.HorizontalInput == Direction.Left && !CollisionSensor.Left()));
            At(_wallSlide, _fall, new FunctionPredicate(() => Input.HorizontalInput == Direction.Right && !CollisionSensor.Right()));
            At(_wallSlide, _wallJump, new FunctionPredicate(() => Input.Jump));

            At(_wallJump, _idle, new FunctionPredicate(() => CollisionSensor.Below()));
            At(_wallJump, _wallSlide, new FunctionPredicate(() => (CollisionSensor.Right() && Input.HorizontalInput == Direction.Right) 
                                                              || (CollisionSensor.Left() && Input.HorizontalInput == Direction.Left)));
        }

        private void Start()
        {
            // todo: move to Mover
            _stateMachine.SetState(_idle);
        }

        private void Update()
        {
            // todo: move to Mover
            _stateMachine.Update();
            Mover.ComponentUpdate();

            if (CollisionSensor.Above() || CollisionSensor.Below())
            {
                Mover.SetVelocityY(0);
            }
        }
    }
}
