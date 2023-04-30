using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelGeneration
{
    public class LevelTileLayout
    { }
    [CreateAssetMenu(menuName = "Level Gen/Palette")]
    public class LevelTilePalette : ScriptableObject
    {
        public List<LevelTileLayout> tiles;
        public LevelTileLayout deadEnd;
    }
}