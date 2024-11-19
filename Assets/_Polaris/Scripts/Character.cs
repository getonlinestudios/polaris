using Polaris.Input;
using RaycastControllerCore;
using UnityEngine;

namespace Polaris
{
    public class Character : MonoBehaviour
    {
        [SerializeField] private float speed = 2f;
        
        private Controller2D _raycastController;
        private InputController _inputController;

        private Vector2 _move;

        private void Start()
        {
            _raycastController = GetComponent<Controller2D>();
            _inputController = GetComponent<InputController>();
        }

        private void Update()
        {
            _move.x = _inputController.MoveDirection.x * speed;
            _raycastController.Move(_move * Time.deltaTime);
        }
    }
}