using System;
using RaycastControllerCore;
using UnityEngine;

namespace Polaris
{
    // TODO: Leaving note to make this abstract.
    // Implementations: Player, Enemy, NPC
    public class Character : MonoBehaviour
    {
        [SerializeField] private float speed = 4f;

        private Controller2D _controller;
        private Animator _animator;

        private void Start()
        {
            _controller = GetComponent<Controller2D>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            var directionX = Input.GetAxisRaw("Horizontal");
            _controller.Move(new Vector2(directionX * speed, 0) * Time.deltaTime);

            if (directionX != 0)
            {
                var dir = Mathf.Sign(directionX);
                transform.localScale = new Vector3(dir, 1, 1);
            }

            _animator.SetBool("Idle", directionX == 0);
            _animator.SetBool("Run", directionX != 0);
        }
    }
}
