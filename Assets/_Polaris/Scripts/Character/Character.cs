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
        [SerializeField] private bool debug;

        private InputController _input;
        private Movement _mover;
        private RaycastController _rc;
        private CollisionSensor _collisionSensor;

        private void Start()
        {
            _input = GetComponent<InputController>();
            _mover = GetComponent<Movement>();
            _collisionSensor = GetComponent<CollisionSensor>();
            _rc = GetComponent<RaycastController>();
        }

        /// <summary>
        /// Takes a value and turns it into a float between 0 and 1.
        /// </summary>
        /// <param name="value">The value to turn to 0 or 1.</param>
        /// <param name="min1">The lowest possible number value can be.</param>
        /// <param name="max1">The highest possible number value can be.</param>
        /// <param name="min2">The desired lowest possible number the output will be.</param>
        /// <param name="max2">The desired highest possible number the output will be.</param>
        /// <param name="clamp">Should the value be clamped between the input.</param>
        /// <returns>The current value as a float.</returns>
        private static float Map(float value, float min1, float max1, float min2, float max2, bool clamp = false)
        {
            var v = min2 + (max2 - min2) * ((value - min1) / (max1 - min1));

            return clamp ? Mathf.Clamp(v, Mathf.Min(min2, max2), Mathf.Max(min2, max2)) : v;
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

            if (_input.Jump && _collisionSensor.Below())
            {
               _mover.SetVelocityY(stats.MaxJumpVelocity);
               animator.SetBool("Jump", true);
            }

            if (!_input.Jump && _collisionSensor.Below())
            {
               animator.SetBool("Jump", false);
               animator.SetFloat("VelocityY", 0f);
            }
            else
            {
                var mappedYVelocity = Map(_mover.CurrentVelocity.y, -stats.MaxJumpVelocity, stats.MaxJumpVelocity, 0, 1, true);
                animator.SetFloat("VelocityY", mappedYVelocity);
            }
            

            if (!_input.Jump && !_collisionSensor.Below() && _mover.CurrentVelocity.y > stats.MinJumpVelocity && !debug)
            {
               _mover.SetVelocityY(stats.MinJumpVelocity);
            }

            _mover.AddToVelocityY(stats.Gravity * Time.deltaTime);

            _mover.Run();

            animator.SetBool("Idle", directionX == 0 && _collisionSensor.Below());
            animator.SetBool("Run", directionX != 0 && _collisionSensor.Below());

            if (_collisionSensor.Above() || _collisionSensor.Below())
            {
                _mover.SetVelocityY(0);
            }
        }
    }
}
