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
        [SerializeField] private GameScreenView _gameScreenView;

        private GameService _gameEngine;
        private bool _isAnimating;
        private bool _isPlaying;

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

        #endregion

        public void StartLevel(DifficultyLevel difficulty)
        {
            _isAnimating = false;
            _isPlaying = true;
            List<List<Tile>> board = _gameEngine.StartGame();
            DifficultConfig config = EnvironmentConfigs.Instance.DifficultConfigCollection.GetConfigByDifficulty(difficulty);

            EnvironmentConfigs.Instance.TileAssetCollection.InitializeRandomTiles(config.ColorQuantity);
            _boardView.CreateBoard(board);
        }

        public void ShowWinPopup(Action onNextLevelCallback)
        {
            _isPlaying = false;
            _gameScreenView.ShowWinPopup(onNextLevelCallback);
        }

        public void ShowLostPopup()
        {
            _isPlaying = false;
            _gameScreenView.ShowLosePopup();
        }

        private void AnimateBoard(List<BoardSequence> boardSequences, int index, Action onComplete)
        {
            BoardSequence boardSequence = boardSequences[index];

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_boardView.DestroyTiles(boardSequence.MatchedPositions, boardSequence.TotalMatchScore));
            sequence.Append(_boardView.MoveTiles(boardSequence.MovedTiles));
            sequence.Append(_boardView.CreateTile(boardSequence.AddedTiles));

            index += 1;
            if (index < boardSequences.Count)
            {
                if (_isPlaying) sequence.onComplete += () => AnimateBoard(boardSequences, index, onComplete);
            }
            else
            {
                sequence.onComplete += () => onComplete();
            }
        }

        private void OnTileSwipe(Vector2Int from, Vector2Int to)
        {
            if (_isAnimating || !_isPlaying) return;

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
                        GameManager.Instance.UpdateLeftMoves();
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
