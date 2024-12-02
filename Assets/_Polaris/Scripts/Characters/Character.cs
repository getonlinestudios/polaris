using System;
using Polaris.Characters.Components;
using Polaris.FSM;
using Polaris.FSM.Core;
using Polaris.FSM.PlayerMovementStates;
using Polaris.Input;
using Polaris.Physics;
using Polaris.Utilities;
using UnityEngine;

namespace Polaris.Characters
{
    // TODO: Leaving note to make this abstract.
    // Implementations: Player, Enemy, NPC
    [AddComponentMenu("Polaris/Character/Character")]
    public class Character : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Stats stats;
        [SerializeField] private bool debug;

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
            
            At(_idle, _run, new FunctionPredicate(() => Input.MoveDirection.x != 0)); 
            At(_idle, _jump, new FunctionPredicate(() => Input.Jump && CollisionSensor.Below()));
            At(_idle, _fall, new FunctionPredicate(() => !CollisionSensor.Below()));
            At(_idle, _groundDash, new FunctionPredicate(() => Input.DashPressed));
            
            At(_run, _idle, new FunctionPredicate(() => Input.MoveDirection.x == 0)); 
            At(_run, _jump, new FunctionPredicate(() => Input.Jump && CollisionSensor.Below()));
            At(_run, _fall, new FunctionPredicate(() => !CollisionSensor.Below()));
            At(_run, _groundDash, new FunctionPredicate(() => Input.DashPressed));
            
            At(_jump, _idle, new FunctionPredicate(() => CollisionSensor.Below() && Input.MoveDirection.x == 0));
            At(_jump, _run, new FunctionPredicate(() => CollisionSensor.Below() && Input.MoveDirection.x != 0));
            
            At(_fall, _idle, new FunctionPredicate(() => CollisionSensor.Below() && Input.MoveDirection.x == 0));
            At(_fall, _run, new FunctionPredicate(() => CollisionSensor.Below() && Input.MoveDirection.x != 0));
            
            At(_groundDash, _idle, new FunctionPredicate(() => _groundDash.Duration > stats.DashDuration && Input.MoveDirection.x == 0));
            At(_groundDash, _idle, new FunctionPredicate(() => !Input.DashHeld && Input.MoveDirection.x == 0));
            
            At(_groundDash, _run, new FunctionPredicate(() => _groundDash.Duration > stats.DashDuration && Input.MoveDirection.x != 0));
            At(_groundDash, _run,
                new FunctionPredicate(() => !Input.DashHeld && Input.MoveDirection.x != 0));
            At(_groundDash, _run,
                new FunctionPredicate(() => (Input.MoveDirection.x == 1f && Mover.FacingDirection.AsValue() == -1) 
                                            || (Input.MoveDirection.x == -1f && Mover.FacingDirection.AsValue() == 1)));
        }

        private void Start()
        {
            _stateMachine.SetState(_idle);
        }

        private void Update()
        {
            _stateMachine.Update();
            Mover.ComponentUpdate();
            
            if (CollisionSensor.Above() || CollisionSensor.Below())
            {
                Mover.SetVelocityY(0);
            }
        }
    }
}
