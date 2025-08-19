using System;
using UnityEngine;

[Serializable]
public class LevelGoal
{
    public int TileId;
    public int Quantity;
}


namespace Gazeus.DesafioMatch3
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Gameplay/Level/LevelConfig")]
    public class LevelConfig : ScriptableObject
    {
        [SerializeField] private int _id;
        [SerializeField] private int _maxMoves;
        [SerializeField] private LevelGoal[] _goals;

        public int Id => _id;
        public int MaxMoves => _maxMoves;
        public LevelGoal[] Goals => _goals;
    }
}
