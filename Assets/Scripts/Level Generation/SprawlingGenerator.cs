using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace LevelGeneration
{
    public class SprawlingGenerator : MonoBehaviour
    {
        public List<Vector2Int> allLocations;
        public List<Vector2Int> locationsToIter;


        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        [Button("Iterate")]
        public void Iterate()
        {
            foreach (var loc in locationsToIter)
            {
                var invalidDirs = new List<Direction>();
                do
                {
                    if (invalidDirs.Count >= 4)
                        break;

                    var dir = RandomDirection();
                    while (invalidDirs.Contains(dir))
                        dir = RandomDirection();

                    var pos = DirectionFunctions.Add(loc, dir);
                    if (allLocations.Contains(pos))
                    {
                        invalidDirs.Add(dir);
                        continue;
                    }
                    else
                    { 
                        
                    }

                } while (true);
            }
        }

        private Direction RandomDirection() => (Direction)Random.Range(0, 3);

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            if (allLocations.Count > 0)
                foreach (var loc in allLocations)
                {
                    Gizmos.DrawSphere(transform.TransformPoint(ToV3(loc)), 0.3f);
                }
        }
        private Vector3 ToV3(Vector2Int v2i) => new Vector3(v2i.x, v2i.y, 0);
        private bool EvaluateChance(float probability) => Random.value <= probability;
    }
}