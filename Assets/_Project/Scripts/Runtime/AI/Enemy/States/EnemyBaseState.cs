
using _Project.Scripts.Runtime.Core.StateMachine;
using UnityEngine;

namespace _Project.Scripts.Runtime.AI.Enemy.States
{
    public abstract class EnemyBaseState : IState
    {
        protected readonly Enemy Enemy;
        protected readonly Animator Animator;

        
        protected static readonly int IdleHash = Animator.StringToHash("IdleNormal");
        protected static readonly int WalkHash = Animator.StringToHash("WalkFWD");
        protected static readonly int RunHash = Animator.StringToHash("RunFWD");
        protected static readonly int AttackHash = Animator.StringToHash("Attack01");
        protected static readonly int DieHash = Animator.StringToHash("Die");
        
        protected const float CrossFadeDuration = 0.1f;
        protected EnemyBaseState(Enemy enemy, Animator animator)
        {
            Enemy = enemy;
            Animator = animator;
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