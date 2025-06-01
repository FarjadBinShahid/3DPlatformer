using UnityEngine;

namespace _Project.Scripts.Runtime.Character.CharacterStates
{
    public class DashState : BaseState
    {
        public DashState(PlayerController playerController, Animator animator) : base(playerController, animator)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("DashState.OnEnter");
            Animator.CrossFade(DashHash, CrossFadeDuration);
        }

        public override void OnExit()
        {
            Debug.Log("DashState.OnExit");
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            PlayerController.HandleMovement();
        }
    }
}