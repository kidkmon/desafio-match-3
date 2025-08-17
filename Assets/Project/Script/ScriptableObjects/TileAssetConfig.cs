using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    [CreateAssetMenu(fileName = "TileAssetConfig", menuName = "Gameplay/Tile/TileAssetConfig")]
    public class TileAssetConfig : ScriptableObject
    {
        [SerializeField] private Color _color;
        [SerializeField] private bool _isJoker;

        public Color Color => _color;
        public bool IsJoker => _isJoker;
    }
}
