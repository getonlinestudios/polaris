using Polaris.Physics;
using UnityEngine;

namespace Polaris.Characters.Components
{
    [AddComponentMenu("Polaris/Character/Components/Character Movement")]
    public class Movement : MonoBehaviour
    {
        public Vector2 CurrentVelocity => _velocity;
        
        private RaycastController _raycastController;
        private Vector2 _velocity;
        
        public void ComponentUpdate()
        {
            _raycastController.Move(_velocity * Time.deltaTime);
        }

        public void SetVelocityX(float value) => _velocity.x = value;

        public void SetVelocityY(float value) => _velocity.y = value;

        public void AddToVelocityY(float value) => _velocity.y += value;
        public void SetHorizontalVelocityToZero() => _velocity.x = 0;

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