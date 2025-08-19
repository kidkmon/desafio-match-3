using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class ScreenManager : Singleton<ScreenManager>
    {
        [SerializeField] private StartScreenView _startScreen;
        [SerializeField] private GameScreenView _gameScreen;
        [SerializeField] private EndScreenView _endScreen;

        private void Start()
        {
            ShowStartScreen();   
        }

        public void ShowStartScreen()
        {
            _startScreen.gameObject.SetActive(true);
            _gameScreen.gameObject.SetActive(false);
            _endScreen.gameObject.SetActive(false);
            _startScreen.PlayMenuAnimation();
        }

        public void ShowGameScreen()
        {
            _startScreen.gameObject.SetActive(false);
            _gameScreen.gameObject.SetActive(true);
            _endScreen.gameObject.SetActive(false);
        }

        public void ShowEndScreen()
        {
            _startScreen.gameObject.SetActive(false);
            _gameScreen.gameObject.SetActive(true);
            _endScreen.gameObject.SetActive(true);
            _endScreen.ShowEndPopup();
        }
    }
}
