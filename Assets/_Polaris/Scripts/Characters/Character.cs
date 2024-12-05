using Polaris.Characters.Components;
using Polaris.Input;
using UnityEngine;

namespace Polaris.Characters
{
    [AddComponentMenu("Polaris/Character/Character")]
    public class Character : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Stats stats;

        public Stats Stats => stats;
        public InputController Input { get; private set; }
        public Movement Mover { get; private set; }
        public CollisionSensor Sensor { get; private set; }
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
            Sensor = GetComponent<CollisionSensor>();
        }

        private void Update()
        {
            Mover.ComponentUpdate();
        }
    }
}
