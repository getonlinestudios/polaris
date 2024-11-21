using Polaris.Input;
using Polaris.StateMachines;
using UnityEngine;

namespace Polaris.CharacterComponents
{
    public abstract class CharacterComponent : MonoBehaviour
    {
    }

    public class Character : MonoBehaviour
    {
       public CharacterMovement CharacterMovement { get; private set; }
       public PlayerInputController InputController { get; private set; }
       public Animator AnimatorController { get; private set; }
       

       private StateMachine _stateMachine;

       private void Awake()
       {
           InputController = GetComponent<PlayerInputController>();
           CharacterMovement = GetComponent<CharacterMovement>();
           AnimatorController = GetComponent<Animator>();

           _stateMachine = new StateMachine(this);
       }

       private void Start()
       {
           _stateMachine.InitialOnEnter();
       }

       private void Update()
        {
            _stateMachine.Update();
        }
    }
}