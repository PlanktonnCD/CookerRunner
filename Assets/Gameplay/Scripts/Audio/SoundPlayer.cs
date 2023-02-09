using System;
using Gameplay.Scripts.DataProfiling;
using Pool;
using UniRx;
using UnityEngine;
using Zenject;

namespace Audio
{
    public class SoundPlayer : AudioPlayer
    {
        [SerializeField] private AudioSourcePool _audioSourcePool;
        private DataManager _dataManager;
        private bool _isLoop;
        private bool IsMuted => !_dataManager.UserProfileData.SettingsInfoModel.IsSettingEnabled(SettingType.Sound);

        [Inject]
        private void Construct(DataManager dataManager)
        {
            _dataManager = dataManager;
            _audioSourcePool.CreateAudioSources(10);
        }

        public override void PlayClip(AudioClip audioClip, bool isLoop)
        {
            if (IsMuted) return;

            var audioSource = _audioSourcePool.GetObject();
            audioSource.mute = false;
            audioSource.clip = audioClip;
            audioSource.loop = isLoop;
            audioSource.Play();

            if (audioSource.loop == false)
            {
                int timeWait = (int)Math.Ceiling(audioClip.length);
                Observable.Timer(TimeSpan.FromSeconds(timeWait)).Subscribe(_ => _audioSourcePool.ReturnObject(audioSource));
            }
        }

        public override void MuteClip(AudioClip audioClip, bool isMuted)
        {
            var audioSource = _audioSourcePool.FindObjectByAudioClip(audioClip);
            if (audioSource == null)
            {
                return;
            }

            audioSource.mute = isMuted;
            _audioSourcePool.ReturnObject(audioSource);
        }

        public override void MuteAll(bool isMuted)
        {
            foreach (var audioSource in _audioSourcePool.GetPoolList())
            {
                audioSource.mute = isMuted;
            }
        }
    }
}