using Polaris.Physics;
using UnityEngine;

namespace Polaris.Characters.Components
{
    [AddComponentMenu("Polaris/Character/Collision Sensor")]
    public class CollisionSensor : MonoBehaviour
    {
        private RaycastController _raycastController;

        public bool Below() => _raycastController.CollisionInfo.Below;
        public bool Above() => _raycastController.CollisionInfo.Above;
        
        private void Awake()
        {
            _raycastController = GetComponent<RaycastController>();
        }
    }
}