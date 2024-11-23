using Polaris.Character.Components;
using Polaris.Input;
using RaycastControllerCore;
using UnityEngine;

namespace Polaris.Character
{
    // TODO: Leaving note to make this abstract.
    // Implementations: Player, Enemy, NPC
    [AddComponentMenu("Polaris/Character/Character")]
    public class Character : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Stats stats;

        private Controller2D _controller;
        private InputController _input;

        private void Start()
        {
            _controller = GetComponent<Controller2D>();
            _input = GetComponent<InputController>();
        }

        private void Update()
        {
            var directionX = _input.MoveDirection.x;
            _controller.Move(new Vector2(directionX * stats.Speed, 0) * Time.deltaTime);

            if (directionX != 0)
            {
                var dir = Mathf.Sign(directionX);
                transform.localScale = new Vector3(dir, 1, 1);
            }

            animator.SetBool("Idle", directionX == 0);
            animator.SetBool("Run", directionX != 0);
        }
    }
}
