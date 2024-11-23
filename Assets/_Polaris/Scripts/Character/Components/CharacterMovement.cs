using Polaris.Physics;
using UnityEngine;

namespace Polaris.Character.Components
{
    [AddComponentMenu("Polaris/Character/Components/Character Movement")]
    public class CharacterMovement : MonoBehaviour
    {
        public Vector2 CurrentVelocity => _velocity;
        
        private RaycastController _raycastController;
        private Vector2 _velocity;

        private void Awake()
        {
            _raycastController = GetComponent<RaycastController>();
            _velocity = Vector2.zero;
        }

        public void SetVelocityX(float value)
        {
            _velocity.x = value;
        }
        
        public void SetVelocityY(float value)
        {
            _velocity.y = value;
        }
        
        public void SetVelocity(Vector2 value)
        {
            _velocity = value;
        }

        private void Update()
        {
            _raycastController.Move(_velocity * Time.deltaTime);
        }
    }
}