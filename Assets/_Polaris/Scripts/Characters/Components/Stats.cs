using UnityEngine;

namespace Polaris.Characters.Components
{
    
    [CreateAssetMenu(fileName = "Character Stats", menuName = "Polaris/Stats/Player Stats", order = -1)]
    public class Stats : ScriptableObject
    {
        [SerializeField] private float speed = 1.2f;
        [SerializeField] private float maxJumpHeight = 2f;
        [SerializeField] private float minJumpHeight = 0.5f;
        [SerializeField] private float timeToJumpApex = 0.25f;
        // todo: is this needed? does mega man fall faster?
        [SerializeField] private float timeToFallApex = 0.20f;
        

        public float Speed => speed;
        public float Gravity { get; private set; }
        public float FallGravity { get; set; }
        public float MaxJumpVelocity { get; private set; }
        public float MinJumpVelocity { get; private set; }

        private void OnEnable() => CalculateJumpPhysics();

        private void OnValidate() => CalculateJumpPhysics();

        private void CalculateJumpPhysics()
        {
            Gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
            FallGravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToFallApex, 2);
            MaxJumpVelocity = Mathf.Abs(Gravity) * timeToJumpApex;
            MinJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(Gravity) * minJumpHeight);
        }
    }
}