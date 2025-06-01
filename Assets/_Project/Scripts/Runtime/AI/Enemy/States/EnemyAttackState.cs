using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Runtime.AI.Enemy.States
{
    public class EnemyAttackState : EnemyBaseState
    {
        private readonly NavMeshAgent _agent;
        private readonly Transform _target;
        public EnemyAttackState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform target) : base(enemy, animator)
        {
            _target = target;
            _agent = agent;
        }

        public override void OnEnter()
        {
            Debug.Log($"EnemyAttackState.OnEnter");
            Animator.CrossFade(AttackHash, CrossFadeDuration);
        }

        public override void Update()
        {
            _agent.SetDestination(_target.position);
            Enemy.Attack();
        }
    }
}