using System.Collections;
using System.Linq;
using UnityEngine;

namespace ContextSteering
{
    [ExecuteAlways]
    public class ContextSteeringController : MonoBehaviour
    {
        private const int NUM_DIRS = 8;
        public static Vector2[] directions => GenDirs();
        //TODO: convert to literals for performance
        private static Vector2[] GenDirs()
        {
            var dirs = new Vector2[NUM_DIRS];

            for (int i = 0; i < NUM_DIRS; i++)
            {
                var a = 360f * i / 8 * Mathf.Deg2Rad;
                dirs[i] = new Vector2(Mathf.Cos(a), Mathf.Sin(a)).normalized;
            }
            return dirs;
        }

        [Header("Target Player")]
        public Transform player;

        [Header("Wall Avoidance")]
        public Collider2D[] obstacles;


        [Header("AI Avoidance")]
        public float wallAvoidRadius = 5;
        public float proximityRadius = 1;

        [Header("Weights")]
        public Vector2[] targetDirWeights = new Vector2[NUM_DIRS];
        public Vector2[] wallAvoidWeights = new Vector2[NUM_DIRS];
        public Vector2[] aiAvoidWeights = new Vector2[NUM_DIRS];

        public Vector2 finalDir;

        void Start()
        {
            var rb = GetComponent<Rigidbody2D>();

        }

        // Update is called once per frame
        void Update()
        {
            EvaluatePlayerGoal((player.position - transform.position).normalized);
            EvaluateObstacle();
            EvaluateAvoidAI();

            finalDir = (GetNormalizeDir(targetDirWeights) 
                        - GetNormalizeDir(wallAvoidWeights) 
                        - GetNormalizeDir(aiAvoidWeights)).normalized;
        }
        public void EvaluateAvoidAI() { }
        public void EvaluateObstacle()
        {
            for (int i = 0; i < NUM_DIRS; i++)
                wallAvoidWeights[i] = Vector2.zero;

            if (obstacles.Length == 0)
                return;

            foreach (var obs in obstacles)
            {
                var dirToObs = obs.ClosestPoint(transform.position) - transform.position.ToV2();

                float distToObs = dirToObs.magnitude;
                dirToObs.Normalize();

                if (distToObs > wallAvoidRadius)
                    continue;

                var distWeight = dirToObs.magnitude <= proximityRadius ? 1 : (wallAvoidRadius - distToObs) / wallAvoidRadius;

                for (int i = 0; i < NUM_DIRS; i++)
                {
                    var dot = Vector2.Dot(dirToObs, directions[i]);

                    var weightedVal = dot * distWeight;

                    if (weightedVal > wallAvoidWeights[i].magnitude)
                        wallAvoidWeights[i] = directions[i] * weightedVal;
                }
            }
        }
        public void EvaluatePlayerGoal(Vector2 targetDir)
        {
            for (int i = 0; i < NUM_DIRS; i++)
            {
                var axis = directions[i];
                var d = Vector2.Dot(targetDir, axis);
                targetDirWeights[i] = axis * Mathf.Clamp01(d);
            }
        }
        public Vector2 GetNormalizeDir(Vector2[] weights)
        {
            Vector2 sum = Vector2.zero;
            foreach (var w in weights)
                sum += w;
            return sum.normalized;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            foreach (var w in targetDirWeights)
                Gizmos.DrawLine(transform.position, transform.position + w.ToV3());

            Gizmos.color = Color.red;
            foreach (var w in wallAvoidWeights)
                Gizmos.DrawLine(transform.position, transform.position + w.ToV3());

            Gizmos.DrawWireSphere(transform.position, proximityRadius);
            Gizmos.DrawWireSphere(transform.position, wallAvoidRadius);

            Gizmos.color = Color.yellow;
            foreach (var w in aiAvoidWeights)
                Gizmos.DrawLine(transform.position, transform.position + w.ToV3());


            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + finalDir.ToV3() * 1.25f);

        }
    }
}