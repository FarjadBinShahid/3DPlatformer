using _Project.Scripts.Runtime.Core.Helpers.Utilities;
using UnityEngine;

namespace _Project.Scripts.Runtime.AI.Enemy
{
    public interface IDetectionStrategy
    {
        bool Execute(Transform target, Transform detector, CountdownTimer timer);
    }
}