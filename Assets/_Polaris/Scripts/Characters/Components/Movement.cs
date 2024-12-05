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
        private Vector2 _velocity;
        
        public void ComponentUpdate()
        {
            // Make sure that Y velocity is never higher than a certain amount.
                // TODO: I think this should be Gravity / Fall Gravity
            _velocity.y = Mathf.Clamp(_velocity.y, -stats.MaxJumpVelocity, stats.MaxJumpVelocity);
            _raycastController.Move(_velocity * Time.deltaTime);
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

        public void SetVelocity(Vector2 value)
        {
            _velocity = value;
        }
        
        private void Awake()
        {
            _raycastController = GetComponent<RaycastController>();
            _velocity = Vector2.zero;
        }
    }
}