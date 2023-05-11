using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ContextSteering
{
    [System.Serializable]
    public class DetectorContainer
    {
        private const int NUM_DIR = 8;
        public static Vector2[] directions => GenDirs();
        private static Vector2[] GenDirs()
        {
            var dirs = new Vector2[NUM_DIR];

            for (int i = 0; i < NUM_DIR; i++)
            {
                var a = 360f * i / 8 * Mathf.Deg2Rad;
                dirs[i] = new Vector2(Mathf.Cos(a), Mathf.Sin(a)).normalized;
            }
            return dirs;
        }

        public bool inverted = false;
        public float[] weights = new float[NUM_DIR];


        public void Evaluate(IEnumerable<Vector2> dirs)
        {
            //calculating
            var allWeights = new float[dirs.Count(), NUM_DIR];
            for (int d = 0; d < dirs.Count(); d++)
                for (int i = 0; i < NUM_DIR; i++)
                {
                    allWeights[d, i] = Mathf.Clamp01(Vector2.Dot(dirs.ElementAt(d), directions[i]));
                }

            //getting sum for normalizing
            float heighestW = 0;
            for (int i = 0; i < NUM_DIR; i++)
            {
                weights[i] = 0;
                for (int d = 0; d < dirs.Count(); d++)
                {
                    weights[i] += allWeights[d, i];
                }
                if (weights[i] > heighestW)
                    heighestW = weights[i];
            }

            //normalizing
            for (int i = 0; i < NUM_DIR; i++)
                weights[i] /= heighestW;

            CheckIfInverted();
        }
        void CheckIfInverted()
        {
            if (!inverted)
                return;

            for (int i = 0; i < NUM_DIR; i++)
                weights[i] = 1 - weights[i];
        }

        public void Evaluate(Vector2 targetDir)
        {
            for (int i = 0; i < NUM_DIR; i++)
            {
                var d = Vector2.Dot(targetDir, directions[i]);
                weights[i] = Mathf.Clamp01(d);
            }
            CheckIfInverted();
        }

        public Vector2[] GetWeightedDirs()
        {
            var wd = new Vector2[NUM_DIR];
            for (int i = 0; i < NUM_DIR; i++)
                wd[i] = directions[i] * weights[i];
            return wd;
        }
        public Vector2 GetNormalizeDir()
        { 
            var t = new Vector2();

            foreach (var dir in GetWeightedDirs())
                t += dir;

            return t.normalized;
        }
        
    }
}
