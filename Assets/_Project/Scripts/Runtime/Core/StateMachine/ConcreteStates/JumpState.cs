using _Project.Scripts.Runtime.Character;
using UnityEngine;

namespace _Project.Scripts.Runtime.Core.StateMachine.ConcreteStates
{
    public class JumpState : BaseState
    {
        public JumpState(PlayerController playerController, Animator animator) : base(playerController, animator)
        {
        }


        public override void OnEnter()
        {
            Debug.Log("JumpState.OnEnter");
            Animator.CrossFade(JumpHash, CrossFadeDuration);
        }

        public override void OnExit()
        {
            Debug.Log("JumpState.OnExit");
            base.OnExit();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            PlayerController.HandleJump();
            PlayerController.HandleMovement();
        }
    }
}