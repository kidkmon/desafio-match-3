using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class LogSystem : Singleton<LogSystem>
    {
        string _logFilePath;

        void Awake()
        {
            _logFilePath = Path.Combine(Application.persistentDataPath, "BombMatch_logs.txt");
            InitializeLogFile();
        }

        void InitializeLogFile()
        {
            File.AppendAllText(_logFilePath, $"Log started at: {DateTime.Now}\n\n");

            Debug.Log($"Local Log file saved in: {Application.persistentDataPath}");
        }

        void OnDestroy()
        {
            File.AppendAllText(_logFilePath, $"\n\nLog ended at: {DateTime.Now}\n------------------------------------------------------------------------\n\n");
        }

        // Basic logging method
        void Log(string message)
        {
            string timestamp = DateTime.Now.ToString("s");
            string formattedMessage = $"[{timestamp}]: {message}";

            // Write to file
            File.AppendAllText(_logFilePath, formattedMessage + "\n");

            Debug.Log(formattedMessage);
        }

        #region Event Logs Methods

        public void LogStartLevel(int level)
        {
            Log($"Start Level: {level}");
        }

        public void LogEndLevel(int level)
        {
            Log($"End Level: {level}");
        }

        public void LogMatchTile(HashSet<Vector2Int> positions, int scoreGained, bool isTLShapeMatch, bool isClearTileMatch)
        {
            string posStr = string.Join(", ", positions);
            string matchType = isTLShapeMatch ? "T/L Shape" : isClearTileMatch ? "Clear" : "Regular";
            Log($"MatchType {matchType} | Score Gained: {scoreGained} | Match {positions.Count} tiles at positions: {posStr}");
        }

        #endregion
    }
}
