using System.Collections.Generic;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class MatchTile
    {
        public List<Vector2Int> MatchedPositions { get; } = new List<Vector2Int>();
        public int MatchSize => new HashSet<Vector2Int>(MatchedPositions).Count;
        public bool IsTLShapeMatch { get; set; } = false; // Indicates if the match is a T or L shape
    }
}
