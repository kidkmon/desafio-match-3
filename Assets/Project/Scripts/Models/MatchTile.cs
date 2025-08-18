using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class MatchTile
    {
        public List<Vector2Int> MatchedPositions { get; } = new List<Vector2Int>();
        public HashSet<Vector2Int> UniqueMatchedPositions => new(MatchedPositions);
        public int MatchSize => UniqueMatchedPositions.Count;
        public bool IsTLShapeMatch { get; set; } = false; // Indicates if the match is a T or L shape
        public bool IsClearTileMatch { get; set; } = false; // Indicates if the match is to clear tile row or column
    }
}
