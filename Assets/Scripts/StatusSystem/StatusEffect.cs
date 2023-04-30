using System;

namespace StatusSystem
{
    public abstract class StatusEffect { 
        public string ID { get; protected set; }
        public object Value { get; protected set; }
    }
    [Serializable]
    public class StatusEffect<T> : StatusEffect
    {
        public new T Value { get; protected set; }
    }
}
