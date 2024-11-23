using UnityEngine;

namespace Polaris.Character.Components
{
    
    [CreateAssetMenu(fileName = "Character Stats", menuName = "Polaris/Stats/Player Stats", order = -1)]
    public class Stats : ScriptableObject
    {
        [SerializeField] private float speed = 1.2f;

        public float Speed => speed;
    }
}