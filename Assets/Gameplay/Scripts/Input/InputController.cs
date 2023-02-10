using System;

namespace Gameplay.Scripts.Input
{
    public class InputController : IDisposable
    {
        private float _sensitivity = 0.02f;
        public delegate void DeltaInputPosition(float position);
        public event DeltaInputPosition DeltaInputPositionEvent;

        public InputController()
        {
            Lean.Touch.LeanTouch.OnFingerUpdate += Touching;
        }

        private void Touching(Lean.Touch.LeanFinger finger)
        {
            if (finger.Index != -42)
            {
                DeltaInputPositionEvent?.Invoke(((finger.ScreenPosition - finger.StartScreenPosition) * _sensitivity).x);
            }
        }

        public void Dispose()
        {
            Lean.Touch.LeanTouch.OnFingerUpdate -= Touching;
        }
    }
}