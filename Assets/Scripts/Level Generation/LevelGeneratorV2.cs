using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
namespace Assets.Scripts.Level_Generation
{
    public class LevelGeneratorV2 : MonoBehaviour
    {
        public LevelTile start;
        public LevelTile deadEnd;
        public List<LevelTile> tilesToChooseFrom;
        [Space]
        [Space]
        public List<LevelTile> completedRooms;
        public List<LevelTile> currentRooms;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        [Button("Run Iter")]
        public void RunIteration()
        {
            foreach (var tile in tilesToChooseFrom)
            {
                foreach (var exit in tile.exits)
                {

                }
            }
        }
        [Button("Clear")]
        public void Clear()
        {
            tilesToChooseFrom.Clear();
            tilesToChooseFrom.Add(start);
        }
    }
}