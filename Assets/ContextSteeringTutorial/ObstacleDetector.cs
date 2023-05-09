using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace ContextSteeringTut
{
    public class ObstacleDetector : MonoBehaviour, IDetector
    {
        [SerializeField] float detectionRadius = 2;

        [SerializeField] LayerMask layer;

        Collider2D[] obstacles;

        public void Detect(AIData aiData)
        {
            obstacles = Physics2D.OverlapCircleAll(transform.position, detectionRadius, layer);
            aiData.obsticles = obstacles;
        }

        private void OnDrawGizmos()
        {
            if (obstacles != null) 
            {
                Gizmos.color = Color.red;
                foreach (var obs in obstacles)
                    Gizmos.DrawSphere(obs.transform.position, .2f);
            }   
        }
    }
}