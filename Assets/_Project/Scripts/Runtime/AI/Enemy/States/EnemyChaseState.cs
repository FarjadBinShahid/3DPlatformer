using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Runtime.AI.Enemy.States
{
    public class EnemyChaseState : EnemyBaseState
    {
        private readonly NavMeshAgent _agent;
        private readonly Transform _target;
        public EnemyChaseState(Enemy enemy, Animator animator, NavMeshAgent agent, Transform target) : base(enemy, animator)
        {
            _agent = agent;
            _target = target;
        }


        public override void OnEnter()
        {
            Debug.Log($"EnemyChaseState.OnEnter");
            Animator.CrossFade(RunHash, CrossFadeDuration);
        }

        public override void OnExit()
        {
            Debug.Log($"EnemyChaseState.OnExit");
        }


        public override void Update()
        {
            _agent.SetDestination(_target.position);
        }
    }
}