using System;

namespace Gameplay.Scripts.Animation
{
    public interface IValueType
    {
    }

    public struct Trigger<T> : IValueType where T : Enum
    {
        public T AnimationEnum { get; set; }
    }

    public struct Float<T> : IValueType where T : Enum
    {
        public T AnimationEnum { get; set; }
        public float Value;
    }
    
    public struct Bool<T> : IValueType where T : Enum
    {
        public T AnimationEnum { get; set; }
        public bool Value;
    }
}