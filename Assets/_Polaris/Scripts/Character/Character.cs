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
        [SerializeField] private float gravity = 0.5f;

        private InputController _input;
        private CharacterMovement _movement;
        private RaycastController _rc;
        private Vector2 _v;
        private float _g;

        private void Start()
        {
            _input = GetComponent<InputController>();
            _movement = GetComponent<CharacterMovement>();
            _rc = GetComponent<RaycastController>();
            _v = Vector2.zero;
            _g = -gravity;
        }

        private void Update()
        {
            var directionX = _input.MoveDirection.x;
            _v.x = directionX * stats.Speed;

            if (directionX != 0)
            {
                var dir = Mathf.Sign(directionX);
                transform.localScale = new Vector3(dir, 1, 1);
            }

            _v.y += _g * Time.deltaTime;
            _movement.SetVelocity(_v);

            animator.SetBool("Idle", directionX == 0);
            animator.SetBool("Run", directionX != 0);

            if (_rc.CollisionInfo.Above || _rc.CollisionInfo.Below)
            {
                _movement.SetVelocityY(0);
            }
        }
    }
}
