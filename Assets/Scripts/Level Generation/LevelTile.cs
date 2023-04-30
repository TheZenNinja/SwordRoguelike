using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Level_Generation
{
    public class LevelTile : MonoBehaviour
    {
        public Vector2Int gridPos;
        public List<Transform> exits;
    }
}