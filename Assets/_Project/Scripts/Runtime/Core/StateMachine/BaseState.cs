using _Project.Scripts.Runtime.Character;
using UnityEngine;

namespace _Project.Scripts.Runtime.Core.StateMachine
{
    public abstract class BaseState : IState
    {
        protected readonly PlayerController PlayerController;
        protected readonly Animator Animator;
        
        protected static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        protected static readonly int JumpHash = Animator.StringToHash("Jump");


        protected const float CrossFadeDuration = 0.1f;

        protected BaseState(PlayerController playerController, Animator animator)
        {
            PlayerController = playerController;
            Animator =  animator;
        }

        public virtual void OnEnter()
        {
            //noop
        }

        public virtual void OnExit()
        {
            //noop
        }

        public virtual void Update()
        {
            //noop
        }

        public virtual void FixedUpdate()
        {
            //noop
        }
    }
}