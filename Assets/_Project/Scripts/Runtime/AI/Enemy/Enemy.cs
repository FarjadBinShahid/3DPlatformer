using System;
using _Project.Scripts.Runtime.AI.Enemy.States;
using _Project.Scripts.Runtime.Core.Helpers.Utilities;
using _Project.Scripts.Runtime.Core.Predicates;
using _Project.Scripts.Runtime.Core.StateMachine;
using _Project.Scripts.Runtime.Core.UpdatePublisher;
using _Project.Scripts.Runtime.Environment.Collectibles;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Runtime.AI.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(PlayerDetector))]
    public class Enemy : Entity, IUpdateObserver, IFixedUpdateObserver
    {
        [SerializeField, Self] private NavMeshAgent agent;
        [SerializeField, Self] private PlayerDetector detector;
        [SerializeField, Child] private Animator animator;

        [Header("Wander Settings")]
        [SerializeField] private float wanderRadius = 10f;


        [Header("Attack Settings")]
        [SerializeField] private float attackRange = 1f;
        [SerializeField] private float timeBetweenAttacks = 1f;

        private StateMachine _stateMachine;


        private CountdownTimer _attackTimer;

        private void Start()
        {
            _attackTimer = new CountdownTimer(timeBetweenAttacks);
            SetupStateMachine();
        }

        private void OnEnable()
        {
            UpdatePublisher.RegisterUpdateObserver(this);
            UpdatePublisher.RegisterFixedUpdateObserver(this);
        }

        private void OnDisable()
        {
            UpdatePublisher.UnregisterUpdateObserver(this);
            UpdatePublisher.UnregisterFixedUpdateObserver(this);
        }

        public void ObservedUpdate()
        {
            _stateMachine.Update();
            _attackTimer.Tick(Time.deltaTime);
        }

        public void ObservedFixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }


        #region State Machine

        private void SetupStateMachine()
        {
            _stateMachine = new StateMachine();
            DeclareStates();
        }

        private void DeclareStates()
        {
            var wanderState = new EnemyWanderState(this, animator, agent, wanderRadius);
            var chaseState = new EnemyChaseState(this, animator, agent, detector.Player);
            var attackState = new EnemyAttackState(this, animator, agent, detector.Player);


            AddTransition(wanderState, chaseState, new FuncPredicate(() => detector.CanDetectPlayer()));
            AddTransition(chaseState, wanderState, new FuncPredicate(() => !detector.CanDetectPlayer()));
            AddTransition(chaseState, attackState, new FuncPredicate(() => detector.CanAttackTarget()));
            AddTransition(attackState, chaseState, new FuncPredicate(() => !detector.CanAttackTarget()));


            _stateMachine.SetState(wanderState);
        }

        private void AddTransition(IState from, IState to, IPredicate condition) =>
            _stateMachine.AddTransition(from, to, condition);

        private void AddAnyTransition(IState to, IPredicate condition) =>
            _stateMachine.AddAnyTransition(to, condition);

        #endregion

        public void Attack()
        {
            if (_attackTimer.IsRunning) return;

            _attackTimer.Start();
            Debug.Log("Attacking");
        }

    }
}