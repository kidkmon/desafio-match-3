using System.Collections.Generic;
using System.Numerics;
using Gazeus.DesafioMatch3.Models;
using UnityEngine;

namespace Gazeus.DesafioMatch3.Core
{
    public class GameService
    {
        private List<List<Tile>> _boardTiles;
        private List<int> _tilesTypes;
        private int _tileCount;

        public bool IsValidMovement(int fromX, int fromY, int toX, int toY)
        {
            List<List<Tile>> newBoard = CopyBoard(_boardTiles);

            (newBoard[toY][toX], newBoard[fromY][fromX]) = (newBoard[fromY][fromX], newBoard[toY][toX]);

            for (int y = 0; y < newBoard.Count; y++)
            {
                for (int x = 0; x < newBoard[y].Count; x++)
                {
                    if (x > 1 &&
                        newBoard[y][x].Type == newBoard[y][x - 1].Type &&
                        newBoard[y][x - 1].Type == newBoard[y][x - 2].Type)
                    {
                        return true;
                    }

                    if (y > 1 &&
                        newBoard[y][x].Type == newBoard[y - 1][x].Type &&
                        newBoard[y - 1][x].Type == newBoard[y - 2][x].Type)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public List<List<Tile>> StartGame()
        {
            int boardWidth = EnvironmentConfigs.Instance.GameConfig.BoardWidth;
            int boardHeight = EnvironmentConfigs.Instance.GameConfig.BoardHeight;
            int colorQuantity = EnvironmentConfigs.Instance.DifficultConfigCollection.GetConfigByDifficulty(GameManager.Instance.Level).ColorQuantity;

            _tilesTypes = new List<int>();
            for (int i = 0; i < colorQuantity; i++)
            {
                _tilesTypes.Add(i);
            }

            _boardTiles = CreateBoard(boardWidth, boardHeight, _tilesTypes);

            return _boardTiles;
        }

        public List<BoardSequence> SwapTile(int fromX, int fromY, int toX, int toY)
        {
            List<List<Tile>> newBoard = CopyBoard(_boardTiles);

            (newBoard[toY][toX], newBoard[fromY][fromX]) = (newBoard[fromY][fromX], newBoard[toY][toX]);

            List<BoardSequence> boardSequences = new();
            List<MatchTile> foundMatches = FindMatches(newBoard);

            while (foundMatches.Count > 0)
            {
                int totalMatchScore = CalculateTotalMatchScore(foundMatches);
                List<Vector2Int> matchedPositions = ClearMatchedTiles(newBoard, foundMatches);
                List<MovedTileInfo> movedTiles = DropTiles(newBoard, matchedPositions);
                List<AddedTileInfo> addedTiles = FillBoard(newBoard);

                BoardSequence sequence = new()
                {
                    TotalMatchScore = totalMatchScore,
                    MatchedPositions = matchedPositions,
                    MovedTiles = movedTiles,
                    AddedTiles = addedTiles
                };
                boardSequences.Add(sequence);
                foundMatches = FindMatches(newBoard);
            }

            _boardTiles = newBoard;

            return boardSequences;
        }

        private static List<List<Tile>> CopyBoard(List<List<Tile>> boardToCopy)
        {
            List<List<Tile>> newBoard = new(boardToCopy.Count);
            for (int y = 0; y < boardToCopy.Count; y++)
            {
                newBoard.Add(new List<Tile>(boardToCopy[y].Count));
                for (int x = 0; x < boardToCopy[y].Count; x++)
                {
                    Tile tile = boardToCopy[y][x];
                    newBoard[y].Add(new Tile { Id = tile.Id, Type = tile.Type });
                }
            }

            return newBoard;
        }

        private List<List<Tile>> CreateBoard(int width, int height, List<int> tileTypes)
        {
            List<List<Tile>> board = new(height);
            _tileCount = 0;
            for (int y = 0; y < height; y++)
            {
                board.Add(new List<Tile>(width));
                for (int x = 0; x < width; x++)
                {
                    board[y].Add(new Tile { Id = -1, Type = -1 });
                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    List<int> noMatchTypes = new(tileTypes.Count);
                    for (int i = 0; i < tileTypes.Count; i++)
                    {
                        noMatchTypes.Add(_tilesTypes[i]);
                    }

                    if (x > 1 &&
                        board[y][x - 1].Type == board[y][x - 2].Type)
                    {
                        noMatchTypes.Remove(board[y][x - 1].Type);
                    }

                    if (y > 1 &&
                        board[y - 1][x].Type == board[y - 2][x].Type)
                    {
                        noMatchTypes.Remove(board[y - 1][x].Type);
                    }

                    board[y][x].Id = _tileCount++;
                    board[y][x].Type = noMatchTypes[Random.Range(0, noMatchTypes.Count)];
                }
            }

            return board;
        }

        private static List<MatchTile> FindMatches(List<List<Tile>> board)
        {
            List<MatchTile> matches = new();

            FindHorizontalMatches(board, matches);
            FindVerticalMatches(board, matches);

            return matches;
        }

        private static void FindHorizontalMatches(List<List<Tile>> board, List<MatchTile> matches)
        {
            int width = board[0].Count;
            int height = board.Count;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width - 2; x++)
                {
                    Tile startTile = board[y][x];
                    if (startTile.Type == -1) continue;

                    if (board[y][x + 1].Type == startTile.Type &&
                        board[y][x + 2].Type == startTile.Type)
                    {
                        MatchTile horizontalMatch = new();

                        for (int i = x; i < width; i++)
                        {
                            if (board[y][i].Type == startTile.Type)
                            {
                                horizontalMatch.MatchedPositions.Add(new Vector2Int(i, y));
                            }
                            else
                            {
                                break;
                            }
                        }

                        matches.Add(horizontalMatch);
                        x += horizontalMatch.MatchSize - 1; // Skip matched tiles
                    }
                }
            }
        }

        private static void FindVerticalMatches(List<List<Tile>> board, List<MatchTile> matches)
        {
            int width = board[0].Count;
            int height = board.Count;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height - 2; y++)
                {
                    Tile startTile = board[y][x];
                    if (startTile.Type == -1) continue;

                    if (board[y + 1][x].Type == startTile.Type &&
                        board[y + 2][x].Type == startTile.Type)
                    {
                        MatchTile verticalMatch = new();

                        for (int i = y; i < height; i++)
                        {
                            if (board[i][x].Type == startTile.Type)
                            {
                                verticalMatch.MatchedPositions.Add(new Vector2Int(x, i));
                            }
                            else
                            {
                                break;
                            }
                        }

                        // Check for T or L shape matches
                        bool isTLShapeMatch = false;
                        foreach (Vector2Int pos in verticalMatch.MatchedPositions)
                        {
                            foreach (MatchTile existingMatch in matches)
                            {
                                if (existingMatch.MatchedPositions.Contains(pos))
                                {
                                    existingMatch.IsTLShapeMatch = true;
                                    existingMatch.MatchedPositions.AddRange(verticalMatch.MatchedPositions);
                                    isTLShapeMatch = true;
                                    break;
                                }
                            }
                            if (isTLShapeMatch) break;
                        }

                        if (!isTLShapeMatch)
                        {
                            matches.Add(verticalMatch);
                        }

                        y += verticalMatch.MatchSize - 1; // Skip matched tiles
                    }
                }
            }
        }

        private int CalculateTotalMatchScore(List<MatchTile> matches)
        {
            GameConfig gameConfig = EnvironmentConfigs.Instance.GameConfig;
            float totalScore = 0, matchScore = 0;

            foreach (MatchTile match in matches)
            {
                if (match.IsTLShapeMatch)
                {
                    matchScore = match.MatchSize * gameConfig.BaseScorePerPiece * gameConfig.MatchTLBonusMultiplier;
                }
                else
                {
                    if (match.MatchSize == 3)
                    {
                        matchScore = match.MatchSize * gameConfig.BaseScorePerPiece;
                    }
                    else if (match.MatchSize == 4)
                    {
                        matchScore = match.MatchSize * gameConfig.BaseScorePerPiece * gameConfig.Match4BonusMultiplier;
                    }
                    else if (match.MatchSize >= 5)
                    {
                        match.IsClearTileMatch = true;
                        matchScore = match.MatchSize * gameConfig.BaseScorePerPiece * gameConfig.Match5BonusMultiplier;
                    }
                }

                totalScore += matchScore;
            }

            return Mathf.RoundToInt(totalScore);
        }

        private List<Vector2Int> ClearMatchedTiles(List<List<Tile>> board, List<MatchTile> matches)
        {
            List<Vector2Int> clearedPositions = new();
            foreach (MatchTile match in matches)
            {
                if (match.IsClearTileMatch)
                {
                    // Clear entire row or column
                    Vector2Int firstPos = match.MatchedPositions[0];
                    Vector2Int secondPos = match.MatchedPositions[1];

                    if (firstPos.x != secondPos.x && firstPos.y == secondPos.y) // Row match
                    {
                        for (int x = 0; x < board[0].Count; x++)
                        {
                            clearedPositions.Add(new Vector2Int(x, firstPos.y));
                        }
                        Debug.Log($"Clearing row at y: {firstPos.y}");
                    }
                    else if (firstPos.y != secondPos.y && firstPos.x == firstPos.x) // Column match
                    {
                        for (int y = 0; y < board.Count; y++)
                        {
                            clearedPositions.Add(new Vector2Int(firstPos.x, y));
                        }
                        Debug.Log($"Clearing column at x: {firstPos.x}");
                    }
                }
                else
                {
                    clearedPositions.AddRange(match.MatchedPositions);
                }
            }

            clearedPositions = new List<Vector2Int>(new HashSet<Vector2Int>(clearedPositions)); // Remove duplicates

            foreach (Vector2Int pos in clearedPositions)
            {
                board[pos.y][pos.x] = new Tile { Id = -1, Type = -1 };
            }

            return clearedPositions;

        }

        private List<MovedTileInfo> DropTiles(List<List<Tile>> board, List<Vector2Int> clearedPositions)
        {
            List<MovedTileInfo> movedTilesList = new();
            Dictionary<int, MovedTileInfo> movedTiles = new();

            for (int i = 0; i < clearedPositions.Count; i++)
            {
                int x = clearedPositions[i].x;
                int y = clearedPositions[i].y;

                if (y > 0)
                {
                    for (int j = y; j > 0; j--)
                    {
                        Tile movedTile = board[j - 1][x];
                        board[j][x] = movedTile;
                        if (movedTile.Type > -1)
                        {
                            if (movedTiles.ContainsKey(movedTile.Id))
                            {
                                movedTiles[movedTile.Id].To = new Vector2Int(x, j);
                            }
                            else
                            {
                                MovedTileInfo movedTileInfo = new()
                                {
                                    From = new Vector2Int(x, j - 1),
                                    To = new Vector2Int(x, j)
                                };
                                movedTiles.Add(movedTile.Id, movedTileInfo);
                                movedTilesList.Add(movedTileInfo);
                            }
                        }
                    }

                    board[0][x] = new Tile
                    {
                        Id = -1,
                        Type = -1
                    };
                }
            }

            return movedTilesList;
        }

        private List<AddedTileInfo> FillBoard(List<List<Tile>> board)
        {
            List<AddedTileInfo> addedTiles = new();

            for (int y = 0; y < board.Count; y++)
            {
                for (int x = 0; x < board[y].Count; x++)
                {
                    if (board[y][x].Type == -1)
                    {
                        int tileType = Random.Range(0, _tilesTypes.Count);
                        Tile tile = board[y][x];
                        tile.Id = _tileCount++;
                        tile.Type = _tilesTypes[tileType];

                        addedTiles.Add(new AddedTileInfo
                        {
                            Position = new Vector2Int(x, y),
                            Type = tileType
                        });
                    }
                }
            }

            return addedTiles;
        }

    }
}
