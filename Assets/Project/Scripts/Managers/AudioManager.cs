using System.Collections;
using UnityEngine;

namespace Gazeus.DesafioMatch3
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("Intro Audios")]
        [SerializeField] AudioClip _fallingBombSound;
        [SerializeField] AudioClip _bombExplosionSound;

        [Header("UI Audios")]
        [SerializeField] AudioClip _clickSound;

        [Header("Game Audios")]
        [SerializeField] AudioClip _backgroundMusic;
        [SerializeField] AudioClip _bombDestroyedSound;
        [SerializeField] AudioClip _levelWinSound;
        [SerializeField] AudioClip _levelLostSound;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private IEnumerator StartIntroSounds()
        {
            AudioSource.PlayClipAtPoint(_fallingBombSound, Camera.main.transform.position, 0.5f);
            yield return new WaitForSeconds(_fallingBombSound.length);
            AudioSource.PlayClipAtPoint(_bombExplosionSound, Camera.main.transform.position, 0.25f);
            yield return new WaitForSeconds(_bombExplosionSound.length);
            PlayBackgroundMusic();
        }

        #region Public Methods
        public void PlayIntroSound() => StartCoroutine(StartIntroSounds());
        public void PlayClickSound() => AudioSource.PlayClipAtPoint(_clickSound, Camera.main.transform.position, 0.5f);
        public void PlayBombDestroyedSound() => AudioSource.PlayClipAtPoint(_bombDestroyedSound, Camera.main.transform.position, 0.5f);
        public void PlayLevelWinSound() => AudioSource.PlayClipAtPoint(_levelWinSound, Camera.main.transform.position, 0.5f);
        public void PlayLevelLostSound() => AudioSource.PlayClipAtPoint(_levelLostSound, Camera.main.transform.position, 0.5f);

        public void PlayBackgroundMusic()
        {
            _audioSource.clip = _backgroundMusic;
            _audioSource.loop = true;
            _audioSource.volume = 0.15f;
            _audioSource.Play();
        }

        public void StopBackgroundMusic()
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
        }

        #endregion
    }
}
