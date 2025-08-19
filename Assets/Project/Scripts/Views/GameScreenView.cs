using System;
using DG.Tweening;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class GameScreenView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private RectTransform _blackCircle;
        [SerializeField] private float _blackCircleInitialScale = 1f;
        [SerializeField] private float _blackCircleVelocity = 2f;

        [Header("Popup Settings")]
        [SerializeField] private RectTransform _winPopup;
        [SerializeField] private RectTransform _losePopup;
        [SerializeField] private GameObject _retryButton;
        [SerializeField] private float _popupVelocity;

        private RectTransform _canvasRect;
        private Vector2 _centerScreen;

        private Action _onNextLevelCallback;

        private void Start()
        {
            _canvasRect = GetComponent<RectTransform>();
            _centerScreen = _canvasRect.rect.center;

            _blackCircle.gameObject.SetActive(false);

            _winPopup.anchoredPosition = new Vector2(_centerScreen.x, _canvasRect.rect.height / 2f - _winPopup.rect.height);
            _losePopup.anchoredPosition = new Vector2(_centerScreen.x, _canvasRect.rect.height / 2f - _losePopup.rect.height);
            _winPopup.gameObject.SetActive(false);
            _losePopup.gameObject.SetActive(false);
        }

        private void ShowPopup(RectTransform popup)
        {
            Sequence sequence = DOTween.Sequence();

            popup.gameObject.SetActive(true);
            sequence.Append(popup.DOMoveY(_centerScreen.y, _popupVelocity).SetEase(Ease.InCubic));
            sequence.Append(popup.DOShakePosition(1, 10, 10, 90f, false, true).SetEase(Ease.OutElastic));
        }

        private void HidePopup(RectTransform popup)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(popup.DOMoveY(_canvasRect.rect.height / 2f - popup.rect.height, _popupVelocity).SetEase(Ease.InCubic));
            sequence.AppendCallback(() => popup.gameObject.SetActive(false));
        }

        public void ShowWinPopup(Action onNextLevelCallback)
        {
            ShowPopup(_winPopup);
            _onNextLevelCallback = onNextLevelCallback;
        }

        public void ShowLosePopup()
        {
            ShowPopup(_losePopup);
            _retryButton.SetActive(GameManager.Instance.CanDecreaseLife());
        }

        public void HomeButtonClick()
        {
            HidePopup(_winPopup);
            HidePopup(_losePopup);
            StartTransitionAnimation(() =>
            {
                AudioManager.Instance.StopBackgroundMusic();
                ScreenManager.Instance.ShowStartScreen();
            }, true);
        }

        public void NextLevelClick()
        {
            HidePopup(_winPopup);
            StartTransitionAnimation(() => _onNextLevelCallback?.Invoke(), false);
        }

        public void ContinueLevelClick()
        {
            HidePopup(_losePopup);
            GameManager.Instance.DecreaseLife();
        }

        public void StartTransitionAnimation(Action onAnimationComplete, bool fastEnd)
        {
            Sequence sequence = DOTween.Sequence();

            _blackCircle.gameObject.SetActive(true);
            sequence.Append(_blackCircle.DOScale(Vector3.one * _blackCircleInitialScale, 1).SetEase(Ease.OutQuad));
            sequence.AppendCallback(() =>
                {
                    _winPopup.gameObject.SetActive(false);
                    _losePopup.gameObject.SetActive(false);
                    _blackCircle.gameObject.SetActive(!fastEnd);
                });

            if (!fastEnd)
            {
                sequence.AppendCallback(() =>
                {
                    onAnimationComplete?.Invoke();
                });
                sequence.Join(_blackCircle.DOScale(0, _blackCircleVelocity).SetEase(Ease.OutQuad));
            }
        }

    }
}
