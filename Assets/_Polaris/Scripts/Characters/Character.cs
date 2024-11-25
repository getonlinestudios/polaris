using System;
using Polaris.Characters.Components;
using Polaris.Input;
using Polaris.Physics;
using UnityEngine;

namespace Polaris.Characters
{
    // TODO: Leaving note to make this abstract.
    // Implementations: Player, Enemy, NPC
    [AddComponentMenu("Polaris/Character/Character")]
    public class Character : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Stats stats;
        [SerializeField] private bool debug;

        public Stats Stats => stats;
        public InputController Input { get; private set; }
        public Movement Mover { get; private set; }
        public CollisionSensor CollisionSensor { get; private set; }
        public Animator Animator => animator;

        public void OrientSprite(int direction)
        {
            if (direction == 0) return;
            var dir = Mathf.Sign(direction);
            transform.localScale = new Vector3(dir, 1, 1);
        }
        
        private void Awake()
        {
            Input = GetComponent<InputController>();
            Mover = GetComponent<Movement>();
            CollisionSensor = GetComponent<CollisionSensor>();
        }

        private void Update()
        {
            var directionX = Input.MoveDirection.x;

            if (directionX != 0)
            {
                var dir = Mathf.Sign(directionX);
                transform.localScale = new Vector3(dir, 1, 1);
            }
            
            Mover.SetVelocityX(directionX * stats.Speed);

            if (Input.Jump && CollisionSensor.Below())
            {
               Mover.SetVelocityY(stats.MaxJumpVelocity);
               animator.SetBool("Jump", true);
            }

            if (!Input.Jump && CollisionSensor.Below())
            {
               animator.SetBool("Jump", false);
               animator.SetFloat("VelocityY", 0f);
            }
            else
            {
                var mappedYVelocity = Utility.Map(Mover.CurrentVelocity.y, -stats.MaxJumpVelocity, stats.MaxJumpVelocity, 0, 1, true);
                animator.SetFloat("VelocityY", mappedYVelocity);
            }
            

            if (!Input.Jump && !CollisionSensor.Below() && Mover.CurrentVelocity.y > stats.MinJumpVelocity && !debug)
            {
               Mover.SetVelocityY(stats.MinJumpVelocity);
            }

            Mover.AddToVelocityY(stats.Gravity * Time.deltaTime);

            Mover.ComponentUpdate();

            animator.SetBool("Idle", directionX == 0 && CollisionSensor.Below());
            animator.SetBool("Run", directionX != 0 && CollisionSensor.Below());

            if (CollisionSensor.Above() || CollisionSensor.Below())
            {
                Mover.SetVelocityY(0);
            }
        }
    }
}
