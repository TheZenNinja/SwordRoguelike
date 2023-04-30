using System.Collections.Generic;
using UnityEngine;

namespace StatusSystem
{
    public class StatusManager : MonoBehaviour
    {
        public List<StatusEffect> effects;

        void Start()
        {

        }

        void Update()
        {

        }

        public bool HasStatusEffect(string statusID) => effects.Exists(x => x.ID == statusID);
        public StatusEffect GetStatusEffect(string statusID)=> effects.Find(x => x.ID == statusID);
        public StatusEffect<T> GetStatusEffect<T>(string statusID)
        {
            var sts = effects.FindAll(x => x.ID == statusID);
            if (sts.Count > 0)
                foreach (var s in sts)
                    if (s.GetType() == typeof(StatusEffect<T>))
                        return s as StatusEffect<T>;
            return null;
        }
    }
}