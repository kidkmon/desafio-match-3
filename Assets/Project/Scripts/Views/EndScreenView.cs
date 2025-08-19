using DG.Tweening;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class EndScreenView : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private RectTransform _blackCircle;
        [SerializeField] private float _blackCircleInitialScale = 1f;
        [SerializeField] private float _blackCircleVelocity = 2f;

        [Header("Popup Settings")]
        [SerializeField] private RectTransform _endPopup;
        [SerializeField] private float _popupVelocity;

        private RectTransform _canvasRect;
        private Vector2 _centerScreen;

        void Start()
        {
            _canvasRect = GetComponent<RectTransform>();
            _centerScreen = _canvasRect.rect.center;

            _blackCircle.gameObject.SetActive(false);
            _endPopup.anchoredPosition = new Vector2(_centerScreen.x, _canvasRect.rect.height / 2f - _endPopup.rect.height);
        }

        public void ShowEndPopup()
        {
            Sequence sequence = DOTween.Sequence();

            _endPopup.gameObject.SetActive(true);
            sequence.Append(_endPopup.DOMoveY(_centerScreen.y, _popupVelocity).SetEase(Ease.InCubic));
            sequence.Append(_endPopup.DOShakePosition(1, 10, 10, 90f, false, true).SetEase(Ease.OutElastic));
        }

        public void OnHomeButtonClicked()
        {
            AudioManager.Instance.PlayClickSound();
            Sequence sequence = DOTween.Sequence();

            sequence.Append(_endPopup.DOMoveY(_canvasRect.rect.height / 2f - _endPopup.rect.height, _popupVelocity).SetEase(Ease.InCubic));

            _blackCircle.gameObject.SetActive(true);
            sequence.Append(_blackCircle.DOScale(Vector3.one * _blackCircleInitialScale, 1).SetEase(Ease.OutQuad));
            sequence.AppendCallback(() =>
            {
                _endPopup.gameObject.SetActive(false);
                GameManager.Instance.ResetStatus();
                AudioManager.Instance.StopBackgroundMusic();
                ScreenManager.Instance.ShowStartScreen();
            });

        }
    }
}
