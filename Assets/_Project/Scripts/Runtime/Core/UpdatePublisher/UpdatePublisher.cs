using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Runtime.Core.UpdatePublisher
{
    public class UpdatePublisher : MonoBehaviour
    {
        private static List<IUpdateObserver> _updateObservers = new ();
        private static List<IUpdateObserver> _pendingUpdateObservers = new ();
        private static int _currentUpdateIndex;
        private static int _currentFixedUpdateIndex;
        private static int _currentLateUpdateIndex;
    
        private static List<IFixedUpdateObserver> _fixedUpdateObservers = new ();
        private static List<IFixedUpdateObserver> _pendingFixedUpdateObservers = new ();
        private static List<ILateUpdateObserver> _lateUpdateObservers = new ();
        private static List<ILateUpdateObserver> _pendingLateUpdateObservers = new ();

        
        /*[RuntimeInitializeOnLoadMethod] 
        private static void RuntimeInitializeOnLoad()
        {
            _currentUpdateIndex = 0;
            _currentFixedUpdateIndex = 0;
            _currentLateUpdateIndex = 0;
            _updateObservers = new List<IUpdateObserver>();
            _fixedUpdateObservers = new List<IFixedUpdateObserver>();
            _lateUpdateObservers = new List<ILateUpdateObserver>();
            _pendingUpdateObservers = new List<IUpdateObserver>();
            _pendingFixedUpdateObservers = new List<IFixedUpdateObserver>();
            _pendingLateUpdateObservers = new List<ILateUpdateObserver>();

        }*/
    
        private void Update()
        {
            for (_currentUpdateIndex = _updateObservers.Count - 1; _currentUpdateIndex >= 0; _currentUpdateIndex--)
            {
                _updateObservers[_currentUpdateIndex].ObservedUpdate();
            }

            if (_pendingUpdateObservers.Count <= 0) return;
            _updateObservers.AddRange(_pendingUpdateObservers);
            _pendingUpdateObservers.Clear();
        }
    
        private void FixedUpdate()
        {
            for (_currentFixedUpdateIndex = _fixedUpdateObservers.Count - 1; _currentFixedUpdateIndex >= 0; _currentFixedUpdateIndex--)
            {
                _fixedUpdateObservers[_currentFixedUpdateIndex].ObservedFixedUpdate();
            }

            if (_pendingFixedUpdateObservers.Count <= 0) return;
            _fixedUpdateObservers.AddRange(_pendingFixedUpdateObservers);
            _pendingFixedUpdateObservers.Clear();
        }
    
        private void LateUpdate()
        {
            for (_currentLateUpdateIndex = _lateUpdateObservers.Count - 1; _currentLateUpdateIndex >= 0; _currentLateUpdateIndex--)
            {
                _lateUpdateObservers[_currentLateUpdateIndex].ObservedLateUpdate();
            }

            if (_pendingLateUpdateObservers.Count <= 0) return;
            _lateUpdateObservers.AddRange(_pendingLateUpdateObservers);
            _pendingLateUpdateObservers.Clear();
        }

        public static void RegisterUpdateObserver(IUpdateObserver observer)
        {
            _pendingUpdateObservers.Add(observer);
        }

        public static void UnregisterUpdateObserver(IUpdateObserver observer)
        {
            if (!_updateObservers.Contains(observer)) return;
            _updateObservers.Remove(observer);
            _currentUpdateIndex--;
        }
    
        public static void RegisterFixedUpdateObserver(IFixedUpdateObserver observer)
        {
            _pendingFixedUpdateObservers.Add(observer);
        }
    
        public static void UnregisterFixedUpdateObserver(IFixedUpdateObserver observer)
        {
            if (!_fixedUpdateObservers.Contains(observer)) return;
            _fixedUpdateObservers.Remove(observer);
            _currentFixedUpdateIndex--;
        }
    
        public static void RegisterLateUpdateObserver(ILateUpdateObserver observer)
        {
            _pendingLateUpdateObservers.Add(observer);
        }
    
        public static void UnregisterLateUpdateObserver(ILateUpdateObserver observer)
        {
            if (!_lateUpdateObservers.Contains(observer)) return;
            _lateUpdateObservers.Remove(observer);
            _currentLateUpdateIndex--;
        }
    }
}