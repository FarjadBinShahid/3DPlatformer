using System;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.Runtime.Environment.MovingPlatforms
{
    public class PlatformMover : MonoBehaviour
    {
        [SerializeField] private Vector3 moveTo = Vector3.zero;
        [SerializeField] private float moveTime = 1.0f;
        [SerializeField] private Ease ease = Ease.InOutQuad;


        private Vector3 _startPosition;


        private void Start()
        {
            _startPosition = transform.position;
            Move();
        }

        private void Move()
        {
            transform.DOMove(_startPosition + moveTo, moveTime)
                .SetEase(ease)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}