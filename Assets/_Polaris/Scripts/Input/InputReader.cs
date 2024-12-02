using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Polaris.Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Polaris/Input/Input Reader", order = -1)]
    public class InputReader : ScriptableObject, PolarisInputActions.IGameplayActions
    {
        public event UnityAction<Vector2> MoveEvent = delegate { };
        public event UnityAction JumpEvent = delegate { };
        public event UnityAction JumpCanceledEvent = delegate { };
        public event UnityAction DashPressedEvent = delegate { };
        public event UnityAction<bool> DashHeldEvent = delegate { };
        
        private PolarisInputActions _inputControls;

        private void OnEnable()
        {
            if (_inputControls != null)
            {
                return;
            }

            _inputControls = new PolarisInputActions();

            _inputControls.Gameplay.SetCallbacks(this);
            _inputControls.Gameplay.Enable();
        }

        private void OnDisable()
        {
            _inputControls.Gameplay.Disable();
        }
        
        public void OnMovement(InputAction.CallbackContext context)
        {
            MoveEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                JumpEvent.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                JumpCanceledEvent.Invoke();
            }
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Started)
            {
                DashPressedEvent.Invoke();
            }
            
            if (context.phase == InputActionPhase.Performed)
            {
                DashHeldEvent.Invoke(true);
            }
            
            if (context.phase == InputActionPhase.Canceled)
            {
                DashHeldEvent.Invoke(false);
            }
        }
    }
}
