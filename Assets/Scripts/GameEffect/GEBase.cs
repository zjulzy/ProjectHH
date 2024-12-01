using System;
using System.Collections.Generic;

namespace ProjectHH.GameEffect
{
    public enum GeType : uint
    {
        InstantDamage = 0,
    }

    [Serializable]
    public struct GeConfig
    {
        public GeType Type;
        public float Duration;
        public float Frequency;
        public float Strength;
    }

    public abstract class GEBase
    {
        private static Dictionary<GeType, Type> GeTypeMap = new Dictionary<GeType, Type>
        {
            { GeType.InstantDamage, typeof(GEInstantDamage) }
        };
        
        public static GEBase Create(GeConfig config)
        {
            if (!GeTypeMap.ContainsKey(config.Type))
            {
                throw new Exception($"Unknown GeType: {config.Type}");
            }

            var type = GeTypeMap[config.Type];
            var instance = Activator.CreateInstance(type) as GEBase;
            if (instance != null)
            {
                instance.RemainingTime = config.Duration;
                instance.Frequency = config.Frequency;
                instance.Strength = config.Strength;
            }
            return instance;
        }

        
        protected float RemainingTime;
        protected float Frequency;
        protected float Strength;
        protected bool HasDuration;

        protected abstract void OnStart();

        protected abstract void OnTick();
    }
}