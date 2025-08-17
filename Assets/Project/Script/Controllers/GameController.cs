using System;
using System.Collections.Generic;
using DG.Tweening;
using Gazeus.DesafioMatch3.Core;
using Gazeus.DesafioMatch3.Models;
using Gazeus.DesafioMatch3.Views;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Controllers
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private BoardView _boardView;

        private GameService _gameEngine;
        private bool _isAnimating;

        #region Unity
        private void Awake()
        {
            _gameEngine = new GameService();
            _boardView.TileSwiped += OnTileSwipe;
        }

        private void OnDestroy()
        {
            _boardView.TileSwiped -= OnTileSwipe;
        }

        private void Start()
        {
            List<List<Tile>> board = _gameEngine.StartGame();
            DifficultConfig config = EnvironmentConfigs.Instance.DifficultConfigCollection.GetConfigByDifficulty(EnvironmentConfigs.Instance.Level);

            EnvironmentConfigs.Instance.TileAssetCollection.InitializeRandomTiles(config.ColorQuantity);
            _boardView.CreateBoard(board);
        }
        #endregion

        private void AnimateBoard(List<BoardSequence> boardSequences, int index, Action onComplete)
        {
            BoardSequence boardSequence = boardSequences[index];

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_boardView.DestroyTiles(boardSequence.MatchedPosition));
            sequence.Append(_boardView.MoveTiles(boardSequence.MovedTiles));
            sequence.Append(_boardView.CreateTile(boardSequence.AddedTiles));

            index += 1;
            if (index < boardSequences.Count)
            {
                sequence.onComplete += () => AnimateBoard(boardSequences, index, onComplete);
            }
            else
            {
                sequence.onComplete += () => onComplete();
            }
        }

        private void OnTileSwipe(Vector2Int from, Vector2Int to)
        {
            if (_isAnimating) return;

            if (from.x > -1 && from.y > -1)
            {
                _isAnimating = true;
                _boardView.SwapTiles(from.x, from.y, to.x, to.y).onComplete += () =>
                {
                    bool isValid = _gameEngine.IsValidMovement(from.x, from.y, to.x, to.y);
                    if (isValid)
                    {
                        List<BoardSequence> swapResult = _gameEngine.SwapTile(from.x, from.y, to.x, to.y);
                        AnimateBoard(swapResult, 0, () => _isAnimating = false);
                    }
                    else
                    {
                        _boardView.SwapTiles(to.x, to.y, from.x, from.y).onComplete += () => _isAnimating = false;
                    }
                };
            }
        }
    }
}
