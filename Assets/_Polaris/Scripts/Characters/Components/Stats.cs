using UnityEngine;
using UnityEngine.Serialization;

namespace Polaris.Characters.Components
{
    
    [CreateAssetMenu(fileName = "Character Stats", menuName = "Polaris/Stats/Player Stats", order = -1)]
    public class Stats : ScriptableObject
    {
        [SerializeField] private float speed = 1.2f;
        
        [Header("Dash")]
        [SerializeField] private float dashSpeed = 3.2f;
        [SerializeField] private float groundDashDuration = 1.2f;
        [SerializeField] private float airDashDuration = 1.0f;
        
        [Header("Jumping")]
        [SerializeField] private float maxJumpHeight = 2f;
        [SerializeField] private float minJumpHeight = 0.5f;
        [SerializeField] private float timeToJumpApex = 0.25f;
        [SerializeField] private float timeToFallApex = 0.20f;
        
        [Header("Required Settings")]
        [SerializeField] 
        [Tooltip("This is the required gravity to be considered grounded. Note, this changes a lot of the default jump values.")]
        private float requiredGravity = 3f;


        public float Speed => speed;
        public float DashSpeed => dashSpeed;
        public float Gravity { get; private set; }
        public float FallGravity { get; private set; }
        public float MaxJumpVelocity { get; private set; }
        public float MinJumpVelocity { get; private set; }
        public float DashDuration => groundDashDuration;
        public float RequiredGravityToBeConsideredGrounded => requiredGravity;

        private void OnEnable() => CalculateJumpPhysics();

        private void OnValidate()
        {
            CalculateJumpPhysics();
            Debug.Log($"Gravity={Gravity}");
            Debug.Log($"MaxJump={MaxJumpVelocity}");
            Debug.Log($"MinJump={MinJumpVelocity}");
        }

        private void CalculateJumpPhysics()
        {
            Gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            FallGravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToFallApex, 2);
            MaxJumpVelocity = Mathf.Abs(Gravity) * timeToJumpApex;
            MinJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(Gravity) * minJumpHeight);
        }
    }
}