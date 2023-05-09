using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ContextSteering
{
    public static class Direction
    {
        public static List<Vector2> eightDirections = new List<Vector2>()
        { 
            new Vector2 ( 1,    0).normalized,
            new Vector2 ( 1,    1).normalized,
            new Vector2 ( 0,    1).normalized,
            new Vector2 ( 1,   -1).normalized,
            new Vector2 ( 0,   -1).normalized,
            new Vector2 (-1,   -1).normalized,
            new Vector2 (-1,    0).normalized,
            new Vector2 (-1,    1).normalized,
        };
    }
    [ExecuteAlways]
    public class ContextSteeringV2 : MonoBehaviour
    {
        const int NUM_DIRS = 8;

        [Header("Target Player")]
        public Transform player;

        [Header("Wall Avoidance")]
        public Collider2D[] obstacles;
        public float wallAvoidRadius = 5;
        public float proximityRadius = 1;


        [Header("AI Avoidance")]
        public float aiAvoidRadius = 5;
        public Collider2D[] nearbyAI;


        [Header("Weights")]
        public float[] targetDirWeights = new float[NUM_DIRS];
        public float[] wallAvoidWeights = new float[NUM_DIRS];
        public float[] aiAvoidWeights   = new float[NUM_DIRS];
        public float[] totalWeights     = new float[NUM_DIRS];

        public bool debugTarget;
        public bool debugAvoidWall;
        public bool debugAiAvoid;
        public bool debugTotal;


        public Vector2 finalDirection;

        /*void Update()
        {
            EvaluatePlayerGoal((player.position - transform.position).normalized);
            EvaluateObstacle();
            EvaluateAvoidAI();
            finalDirection = GetFinalDir();
        }*/

        public Vector2 GetGoalDirection(Vector2 goalPosition)
        {
            EvaluatePlayerGoal((goalPosition - transform.position.ToV2()).normalized);
            EvaluateObstacle();
            EvaluateAvoidAI();
            finalDirection = GetFinalDir();
            return finalDirection;
        }


        public Vector2 GetFinalDir()
        {
            totalWeights = new float[NUM_DIRS];

            for (int i = 0; i < NUM_DIRS; i++)
            {
                var inverseWeights = wallAvoidWeights[i] + aiAvoidWeights[i];

                totalWeights[i] += targetDirWeights[i];
                totalWeights[GetMirror(i)] += inverseWeights;
            }

            float maxValue = 0;
            for (int i = 0; i < NUM_DIRS; i++)
                if (totalWeights[i] > maxValue)
                    maxValue = totalWeights[i];


            for (int i = 0; i < NUM_DIRS; i++)
            {
                totalWeights[i] = Mathf.Clamp01(totalWeights[i] / maxValue);
            }

            return GetNormalizedDir(totalWeights);
        }
        /// <summary>
        /// Add up all weights and return the normalized direction as a result
        /// </summary>
        Vector2 GetNormalizedDir(float[] weights)
        {
            var sum = Vector2.zero;
            for (int i = 0; i < NUM_DIRS; i++)
                sum += Direction.eightDirections[i] * weights[i];
            return sum.normalized;
        }

        /// <summary>
        /// Weighs the directions by how close it is to the target position
        /// </summary>
        /// <param name="targetDir"></param>
        public void EvaluatePlayerGoal(Vector2 targetDir)
        {
            targetDirWeights = new float[NUM_DIRS];
            for (int i = 0; i < NUM_DIRS; i++)
                targetDirWeights[i] = Mathf.Clamp01(Vector2.Dot(targetDir, Direction.eightDirections[i]));
        }
        public void EvaluateObstacle()
        {
            wallAvoidWeights = EvaluateList(obstacles.Select(x => x.ClosestPoint(transform.position)).ToArray(), proximityRadius, wallAvoidRadius);
        }
        public void EvaluateAvoidAI()
        {
            aiAvoidWeights = EvaluateList(nearbyAI.Select(x => x.ClosestPoint(transform.position)).ToArray(), proximityRadius, aiAvoidRadius);
        }
        public float[] EvaluateList(Vector2[] points, float minRange, float maxRange)
        {
            var weights = new float[NUM_DIRS];

            if (points.Length == 0)
                return weights;

            foreach (var point in points)
            {
                var dirToPoint = point - transform.position.ToV2();

                float distToPoint = dirToPoint.magnitude;
                dirToPoint.Normalize();

                if (distToPoint > maxRange)
                    continue;

                var distWeight = dirToPoint.magnitude <= minRange ? 1 : (maxRange - distToPoint) / maxRange;

                for (int i = 0; i < NUM_DIRS; i++)
                {
                    var dot = Vector2.Dot(dirToPoint, Direction.eightDirections[i]);

                    var weightedVal = dot * distWeight;

                    if (weightedVal > weights[i])
                        weights[i] = weightedVal;
                }
            }
            return weights;
        }

        private int GetMirror(int i)
        {
            i += 4;
            if (i >= 8)
                i -= 8;
            return i;
        }

        private void OnDrawGizmos()
        {
            if (debugAvoidWall)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, proximityRadius);
                Gizmos.DrawWireSphere(transform.position, wallAvoidRadius);
            }

            if (debugAiAvoid)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(transform.position, proximityRadius);
                Gizmos.DrawWireSphere(transform.position, aiAvoidRadius);
            }

            for (int i = 0; i < NUM_DIRS; i++)
            {
                var dir = Direction.eightDirections[i];
                var p = transform.position;

                Gizmos.color = Color.green;
                if (debugTarget)
                    Gizmos.DrawLine(p, p.ToV2() + dir * targetDirWeights[i]);

                Gizmos.color = Color.red;
                if (debugAvoidWall)
                    Gizmos.DrawLine(p, p.ToV2() + dir * wallAvoidWeights[i]);

                Gizmos.color = Color.cyan;
                if (debugAiAvoid)
                    Gizmos.DrawLine(p, p.ToV2() + dir * aiAvoidWeights[i]);


                Gizmos.color = Color.magenta;
                if (debugTotal)
                    Gizmos.DrawLine(p, p.ToV2() + dir * totalWeights[i]);
            }



            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + finalDirection.ToV3() * 1.25f);
        }
    }
}