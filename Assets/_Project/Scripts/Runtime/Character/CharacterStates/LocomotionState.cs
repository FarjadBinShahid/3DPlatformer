using UnityEngine;

namespace _Project.Scripts.Runtime.Character.CharacterStates
{
    public class LocomotionState : BaseState
    {
        public LocomotionState(PlayerController playerController, Animator animator) : base(playerController, animator)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("LocomotionState.OnEnter");
            Animator.CrossFade(LocomotionHash, CrossFadeDuration);
        }

        public override void OnExit()
        {
            Debug.Log("LocomotionState.OnExit");
            base.OnExit();
        }

        public override void Update()
        {
            base.Update();
        }

        public override void FixedUpdate()
        {
            PlayerController.HandleMovement();
        }
    }
}