using UnityEngine;

namespace Polaris.Input
{
    [AddComponentMenu("Polaris/Input/Input Controller")]
    public class InputController : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private float inputHoldTime = 0.2f;

        public Vector2 MoveDirection { get; private set; }
        public bool Jump { get; private set; }
        public bool JumpCanceled { get; set; }
        public bool DashPressed { get; set; }
        public bool DashHeld { get; set; }

        private float _jumpInputStartTime;
        private float _dashInputStartTime;

        private void ClearJumpInput()
        {
            if (Time.time >= _jumpInputStartTime + inputHoldTime)
            {
                Jump = false;
            }
        }
        
        private void ClearDashInput()
        {
            if (Time.time >= _dashInputStartTime + inputHoldTime)
            {
                DashPressed = false;
            }
        }

        private void OnDashPressed()
        {
            DashPressed = true;
            _dashInputStartTime = Time.time;
        }

        private void OnDashHeld(bool input)
        {
            DashHeld = input;
        }
        
        private void OnJumpCanceled()
        {
            Jump = false;
            JumpCanceled = true;
        }

        private void OnJump()
        {
            Jump = true;
            JumpCanceled = false;
            _jumpInputStartTime = Time.time;
        }

        private void OnMove(Vector2 inputDirection)
        {
            inputDirection.x = Mathf.RoundToInt(inputDirection.x);
            inputDirection.y = Mathf.RoundToInt(inputDirection.y);
            
            MoveDirection = inputDirection;
        }
        
        private void OnEnable()
        {
            if (inputReader == null)
            {
                inputReader = Instantiate(ScriptableObject.CreateInstance<InputReader>());
                Debug.LogError($"{name} does not have an {nameof(InputReader)} set. " +
                               $"Please ensure one is set for proper behavior. A new one will be temporarily created.");
                return;
            }

            inputReader.MoveEvent += OnMove;
            inputReader.JumpEvent += OnJump;
            inputReader.JumpCanceledEvent += OnJumpCanceled;
            inputReader.DashPressedEvent += OnDashPressed;
            inputReader.DashHeldEvent += OnDashHeld;
        }

        private void OnDisable()
        {
            if (inputReader == null)
            {
                Debug.LogWarning($"There was no reader on {name}, nothing to disable.");
                return;
            }
            
            inputReader.MoveEvent -= OnMove;
            inputReader.JumpEvent -= OnJump;
            inputReader.JumpCanceledEvent -= OnJumpCanceled;
            inputReader.DashPressedEvent -= OnDashPressed;
            inputReader.DashHeldEvent -= OnDashHeld;
        }

        private void Update()
        {
            ClearJumpInput();
            ClearDashInput();
        }
    }
}