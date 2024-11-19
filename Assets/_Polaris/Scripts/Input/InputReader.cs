using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace Polaris.Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Polaris/Input/Input Reader")]
    public class InputReader : ScriptableObject, PolarisInputControls.IGameplayActions
    {
        public event UnityAction<Vector2> MoveEvent = delegate { };
        public event UnityAction JumpEvent = delegate { };
        public event UnityAction JumpCanceledEvent = delegate { };

        private PolarisInputControls _inputControls;

        private void OnEnable()
        {
            if (_inputControls != null)
            {
                return;
            }

            _inputControls = new PolarisInputControls();

            _inputControls.Gameplay.SetCallbacks(this);
            _inputControls.Gameplay.Enable();
        }

        private void OnDisable()
        {
            _inputControls.Gameplay.Disable();
        }

        public void OnMovement(InputAction.CallbackContext context)
        {
            Debug.Log(context.ReadValue<Vector2>());
            MoveEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                Debug.Log("Jump started!");
                JumpEvent.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                Debug.Log("Jump canceled!");
                JumpCanceledEvent.Invoke();
            }
        }
    }
}