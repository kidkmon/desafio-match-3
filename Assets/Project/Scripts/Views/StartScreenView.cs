using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gazeus.DesafioMatch3
{
    public class StartScreenView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private RectTransform _blackCircle;
        [SerializeField] private RectTransform _bombTransform;
        [SerializeField] private RectTransform _startMenuTransform;
        [SerializeField] private float _blackCircleInitialScale = 1f;
        [SerializeField] private float _bombFallVelocity = 2f;
        [SerializeField] private float _explosionTime = 0.5f;
        [SerializeField] private float _shakeMenuForce = 10f;
        [SerializeField] private float _shakeMenuDuration = 0.7f;

        [Header("VFX References")]
        [SerializeField] private ParticleSystem _explosionEffect;
        [SerializeField] private ParticleSystem _debrisEffect;

        [Header("Buttons References")]
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _difficultyButton;
        [SerializeField] private TextMeshProUGUI _difficultyText;

        private RectTransform _canvasRect;
        private Vector2 _centerScreen;

        private DifficultyLevel _currentDifficulty = DifficultyLevel.Easy;

        private void Start()
        {
            DOTween.Init();

            _canvasRect = GetComponent<RectTransform>();
            _centerScreen = _canvasRect.rect.center;

            _startMenuTransform.gameObject.SetActive(false);

            _blackCircle.localScale = Vector3.one * _blackCircleInitialScale;
            _blackCircle.gameObject.SetActive(true);
            _backgroundImage.gameObject.SetActive(true);

            _bombTransform.anchoredPosition = new Vector2(_centerScreen.x, _canvasRect.rect.height / 2f + _bombTransform.rect.height);
            _bombTransform.gameObject.SetActive(true);

            _difficultyText.text = _currentDifficulty.ToString();

            StartMenuAnimation();
        }

        private void StartMenuAnimation()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(_blackCircle.DOScale(0, _bombFallVelocity).SetEase(Ease.OutQuad));
            sequence.Join(_bombTransform.DOMoveY(_centerScreen.y, _bombFallVelocity).SetEase(Ease.InCubic));

            sequence.AppendCallback(() =>
            {
                _bombTransform.gameObject.SetActive(false);
                _explosionEffect.Play();
                _debrisEffect.Play();

            });
            sequence.AppendInterval(_explosionTime);

            // Show start menu with shake effect
            sequence.AppendCallback(() =>
                {
                    _backgroundImage.gameObject.SetActive(true);
                    _startMenuTransform.gameObject.SetActive(true);
                    _bombTransform.gameObject.SetActive(true);
                    _bombTransform.transform.localScale = Vector2.zero;
                });

            sequence.Append(_bombTransform.transform.DOScale(1.0f, _shakeMenuDuration).SetEase(Ease.OutElastic));
            sequence.Join(_startMenuTransform.DOShakePosition(_shakeMenuDuration, _shakeMenuForce, 10, 90f, false, true).SetEase(Ease.OutElastic));
        }

        private void OnEnable()
        {
            _playButton.onClick.AddListener(OnPlayButtonClicked);
            _difficultyButton.onClick.AddListener(OnDifficultyButtonClicked);
        }
        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(OnPlayButtonClicked);
            _difficultyButton.onClick.RemoveListener(OnDifficultyButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            AudioManager.Instance.PlayClickSound();
            AudioManager.Instance.StopBackgroundMusic();
            StartGameAnimation();
        }

        private void OnDifficultyButtonClicked()
        {
            AudioManager.Instance.PlayClickSound();
            _currentDifficulty = (DifficultyLevel)(((int)_currentDifficulty + 1) % System.Enum.GetValues(typeof(DifficultyLevel)).Length);
            _difficultyText.text = _currentDifficulty.ToString();
        }

        private void StartGameAnimation()
        {
            _bombTransform.gameObject.SetActive(false);
            _startMenuTransform.gameObject.SetActive(false);

            _blackCircle.DOScale(Vector3.one * _blackCircleInitialScale, 1).SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    _blackCircle.gameObject.SetActive(false);
                    _backgroundImage.gameObject.SetActive(false);

                    ScreenManager.Instance.ShowGameScreen();
                    GameManager.Instance.StartGame(_currentDifficulty, GameManager.Instance.Level);

                    AudioManager.Instance.PlayBackgroundMusic();
                });

        }
    }
}
