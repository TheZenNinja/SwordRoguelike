using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelGeneration
{
    
   
    [System.Serializable]
    public struct NodePosition
    {
        public Vector2Int location;
        public Direction facing;

        public NodePosition(Vector2Int location, Direction facing)
        {
            this.location = location;
            this.facing = facing;
        }

        public Vector2Int left => location + facing.RotateLeft().ToPosition();
        public Vector2Int right => location + facing.RotateRight().ToPosition();
        public Vector2Int forward => location + facing.ToPosition();
        public Vector2Int back => location + facing.Reverse().ToPosition();

        public static bool operator ==(NodePosition a, NodePosition b) => a.location == b.location && a.facing == b.facing;
        public static bool operator !=(NodePosition a, NodePosition b) => !(a == b);
    }
    [System.Serializable]
    public class LevelNode
    {
        public NodePosition position;
        public Vector3 location => new Vector3(location.x, location.y, 0);
        public Direction entrance;
        public List<NodePosition> exits;

        public LevelNode(NodePosition position, Direction entrance, IEnumerable<NodePosition> exits = null)
        {
            this.position = position;
            this.entrance = entrance;
            this.exits = new List<NodePosition>(exits);
        }

        public static bool operator ==(LevelNode a, LevelNode b) => a.position == b.position;
        public static bool operator !=(LevelNode a, LevelNode b) => !(a == b);
    }
    public class LevelGenerator : MonoBehaviour
    {
        [Range(4,32)]
        public int minIter, maxIter;
        public int currentIter;

        public List<LevelNode> allLocations = new List<LevelNode>();
        public List<LevelNode> currentLocations = new List<LevelNode>();

        [Space]
        [Header("Probabilities")]
        [Range(0, 1)]
        public float chanceForTwoDoors;
        [Range(0, 1)]
        public float chanceForThreeDoors;

        [Space]
        [Header("Other")]
        [Range(0, 1)]
        public float stepDuration = .2f;

        void Start()
        {
            StartCoroutine(Generate());
        }


        IEnumerator Generate()
        {
            RunInit();
            for (currentIter = 0; currentIter < maxIter; currentIter++)
            {
                RunCycle();
                yield return new WaitForSeconds(stepDuration);
            }
            RunCleanup();
        }

        void RunInit()
        {
            allLocations = new List<LevelNode>();
            currentLocations = new List<LevelNode>();

            //currentLocations.Add(new LevelNode(new NodePosition(Vector2Int.zero, Direction.north)));//GetRndDir()));
        }
        void RunCycle()
        {
            var newLocations = new List<LevelNode>();

            foreach (var loc in currentLocations)
                foreach (var exit in loc.exits)
                {
                    LevelNode newNode;
                    if (EvaluateChance(chanceForThreeDoors))
                    {
                    }
                    else if (EvaluateChance(chanceForTwoDoors))
                    {

                    }
                    else
                    {

                    }

                }

            /*var newLocations = new List<LevelNode>();
            foreach (var loc in currentLocations)
            {
                bool isValidLocation = false;
                List<Direction> failedDirs = new List<Direction>();

                //if (EvaluateChance(chanceToBranch))
                do
                {
                    if (failedDirs.Count >= 4)
                        throw new System.Exception("Ran out of directions");

                    var dir = GetRndDir();
                    var newLoc = loc.location + GetLocationOffset(dir);

                    if (IsLocationOverlapping(newLoc))
                        failedDirs.Add(dir);
                    else
                    {
                        isValidLocation = true;
                        newLocations.Add(new LevelNode(newLoc, dir));
                    }

                } while (!isValidLocation);
            }*/

            allLocations.AddRange(currentLocations);
            currentLocations.Clear();
            currentLocations.AddRange(newLocations);
        }
    void RunCleanup()
        { 
        
        }

        //public bool IsLocationOverlapping(Vector2Int newLocation)
        //{
        //    return allLocations.Exists(x => x.pos == newLocation) ||
        //        currentLocations.Exists(x => x.location == newLocation);
        //}



        private bool EvaluateChance(float probability) => Random.value <= probability;
        private Direction GetRndDir() => (Direction)Random.Range(0, 3);

        private void OnDrawGizmos()
        {
            /*Gizmos.color = Color.blue;
            if (allLocations.Count > 0)
                foreach (var l in allLocations)
                {
                    if (l.forwardExit)
                        Gizmos.DrawLine(transform.TransformPoint(l.GetLocalPosition()), 
                            transform.TransformPoint(ToV3(l.forward)));
                    if (l.leftExit)
                        Gizmos.DrawLine(transform.TransformPoint(l.GetLocalPosition()),
                            transform.TransformPoint(ToV3(l.left)));
                    if (l.rightExit)
                        Gizmos.DrawLine(transform.TransformPoint(l.GetLocalPosition()),
                            transform.TransformPoint(ToV3(l.right)));

                    Gizmos.DrawSphere(transform.TransformPoint(l.GetLocalPosition()), 0.25f);
                }

            Gizmos.color = Color.red;
            if (currentLocations.Count > 0)
                foreach (var l in currentLocations)
                    Gizmos.DrawSphere(transform.TransformPoint(l.GetLocalPosition()), 0.5f);*/
        }
        private static Vector3 ToV3(Vector2Int v) => new Vector3(v.x, v.y, 0);
        
    }
}