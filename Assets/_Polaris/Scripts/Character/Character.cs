using Polaris.Character.Components;
using Polaris.Input;
using Polaris.Physics;
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

        private InputController _input;
        private Movement _mover;
        private RaycastController _rc;

        private void Start()
        {
            _input = GetComponent<InputController>();
            _mover = GetComponent<Movement>();
            _rc = GetComponent<RaycastController>();
        }

        private void Update()
        {
            var directionX = _input.MoveDirection.x;

            if (directionX != 0)
            {
                var dir = Mathf.Sign(directionX);
                transform.localScale = new Vector3(dir, 1, 1);
            }
            
            _mover.SetVelocityX(directionX * stats.Speed);

            if (_input.Jump && _rc.CollisionInfo.Below)
            {
               _mover.SetVelocityY(stats.MaxJumpVelocity);
            }

            if (!_input.Jump && !_rc.CollisionInfo.Below && _mover.CurrentVelocity.y > stats.MinJumpVelocity)
            {
               _mover.SetVelocityY(stats.MinJumpVelocity);
            }

            _mover.AddToVelocityY(stats.Gravity * Time.deltaTime);

            _mover.Run();

            animator.SetBool("Idle", directionX == 0);
            animator.SetBool("Run", directionX != 0);

            if (_rc.CollisionInfo.Above || _rc.CollisionInfo.Below)
            {
                _mover.SetVelocityY(0);
            }
        }
    }
}
