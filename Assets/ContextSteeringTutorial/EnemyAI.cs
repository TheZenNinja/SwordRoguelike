using System.Collections;
using UnityEngine;

namespace ContextSteeringTut
{
    public class EnemyAI : MonoBehaviour
    {
        public IDetector[] detectors;

        [SerializeField] AIData aiData;

        private float detectDelay = 0.05f;
        float currentDelay = 0;

        private void Start()
        {
            detectors = GetComponents<IDetector>();
        }

        private void FixedUpdate()
        {
            
            if (currentDelay <= 0 ) {
                currentDelay = detectDelay;
                foreach (var detector in detectors)
                    detector.Detect(aiData);
            }
            currentDelay -= Time.fixedDeltaTime;
        }
    }
}