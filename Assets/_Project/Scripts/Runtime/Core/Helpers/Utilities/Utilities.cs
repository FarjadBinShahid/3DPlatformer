using System;

namespace _Project.Scripts.Runtime.Core.Helpers.Utilities
{

    public abstract class Timer
    {
        protected float InitialTime;
        protected float Time { get; set;}
        public bool IsRunning { get; protected set;}
        
        public float Progress => Time / InitialTime;
        
        public Action OnTimerStart = delegate { };
        public Action OnTimerStop = delegate { };


        protected Timer(float initialTime)
        {
            InitialTime = initialTime;
            IsRunning = false;
        }

        public void Start()
        {
            Time = InitialTime;
            
            if (IsRunning) return;
            
            IsRunning = true;
            OnTimerStart.Invoke();
        }
        
        public void Stop()
        {
            if (!IsRunning) return;
            IsRunning = false;
            OnTimerStop.Invoke();
        }
        
        
        public void Pause() => IsRunning = false;
        public void Resume() => IsRunning = true;
        public abstract void Tick(float deltaTime);
        
    }


    public class CountdownTimer : Timer
    {
        public CountdownTimer(float initialTime) : base(initialTime)
        {
        }

        public override void Tick(float deltaTime)
        {
            if (IsRunning && Time > 0)
            {
                Time -= deltaTime;
            }

            if (IsRunning && Time <= 0)
            {
                Stop();
            }
        }

        public bool IsFinished() => Time <= 0;

        public void Reset() => Time = 0;

        public void Reset(float newTime)
        {
            InitialTime = newTime;
            Reset();
        }
    }

    public class StopwatchTimer : Timer
    {
        public StopwatchTimer() : base(0)
        {
        }

        public override void Tick(float deltaTime)
        {
            if (IsRunning)
            {
                Time += deltaTime;
            }
        }

        public void Reset() => Time = 0;

        public float GetTime() => Time;
    }
}
