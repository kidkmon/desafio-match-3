using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class ScreenManager : Singleton<ScreenManager>
    {
        [SerializeField] private GameObject _startScreen;
        [SerializeField] private GameObject _gameScreen;
        [SerializeField] private GameObject _endScreen;

        private void Start()
        {
            ShowStartScreen();   
        }

        public void ShowStartScreen()
        {
            _startScreen.SetActive(true);
            _gameScreen.SetActive(false);
            _endScreen.SetActive(false);
        }

        public void ShowGameScreen()
        {
            _startScreen.SetActive(false);
            _gameScreen.SetActive(true);
            _endScreen.SetActive(false);
        }

        public void ShowEndScreen()
        {
            _startScreen.SetActive(false);
            _gameScreen.SetActive(true);
            _endScreen.SetActive(true);
        }
    }
}
