using System;
using RaycastControllerCore;
using UnityEngine;

namespace Polaris.CharacterComponents
{
    [RequireComponent(typeof(Controller2D))]
    public class CharacterMovement : CharacterComponent
    {
        [SerializeField] private float speed = 4f;
        public float Speed => speed;
       
        private Controller2D _controller;
        private Vector2 _velocity;

        private void Awake()
        {
            _controller = GetComponent<Controller2D>();
        }

        public void SetVelocityX(float value) => _velocity.x = value;

        private void Update()
        {
            _controller.Move(_velocity * Time.deltaTime);
        }
    }
}