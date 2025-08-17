using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Gameplay/GameConfig")]
    public class GameConfig : ScriptableObject
    {
        [Header("Board Settings")]
        [SerializeField] private int _boardHeight = 10;
        [SerializeField] private int _boardWidth = 10;

        [Header("Score Settings")]
        [SerializeField] private int _baseScorePerPiece = 10;
        [SerializeField] private int _specialPieceScore = 15;
        [Tooltip("Multiplier for matches of 4 tiles")][SerializeField] private float _match4BonusMultiplier = 1.5f;
        [Tooltip("Multiplier for matches of 5 tiles")][SerializeField] private float _match5BonusMultiplier = 2.0f;
        [Tooltip("Multiplier for matches with T and L shapes")][SerializeField] private float _matchTLBonusMultiplier = 2.0f;
        [Tooltip("Multiplier for combo matches")] private float _comboBonusMultiplier = 1.0f;
        [Tooltip("Increment for ComboBonusMultiplier value (e.g. comboBonusMultiplier += comboIncrement)")][SerializeField] private float _comboIncrement = 1.0f;


        public int BoardHeight => _boardHeight;
        public int BoardWidth => _boardWidth;

        public int BaseScorePerPiece => _baseScorePerPiece;
        public int SpecialPieceScore => _specialPieceScore;
        public float Match4BonusMultiplier => _match4BonusMultiplier;
        public float Match5BonusMultiplier => _match5BonusMultiplier;
        public float MatchTLBonusMultiplier => _matchTLBonusMultiplier;
        public float ComboBonusMultiplier => _comboBonusMultiplier;
        public float ComboIncrement => _comboIncrement;
    }
}
