using UnityEngine;

namespace Polaris.Input
{
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;
        [SerializeField] private float inputHoldTime = 0.2f;

        public Vector2 MoveDirection { get; private set; }
        public bool Jump { get; private set; }
        
        private float _jumpInputStartTime;
        private Vector2 _inputVector;

        private void ClearJumpInput()
        {
            if (Time.time >= _jumpInputStartTime + inputHoldTime)
            {
                Jump = false;
            }
        }
        
        private void OnJumpCanceled()
        {
            Jump = false;
        }

        private void OnJump()
        {
            Jump = true;
            _jumpInputStartTime = Time.time;
        }

        private void OnMove(Vector2 inputDirection)
        {
            var directionX = (int)(inputDirection * Vector2.right).normalized.x;
            var directionY = (int)(inputDirection * Vector2.up).normalized.y;

            _inputVector.x = directionX;
            _inputVector.y = directionY;
            
            MoveDirection = _inputVector;
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
        }

        private void Update()
        {
            ClearJumpInput();
        }
    }
}