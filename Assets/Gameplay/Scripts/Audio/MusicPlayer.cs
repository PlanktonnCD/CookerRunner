using System;
using DG.Tweening;
using Gameplay.Scripts.DataProfiling;
using UnityEngine;
using Zenject;

namespace Audio
{
    public class MusicPlayer : AudioPlayer
    {
        [SerializeField] private AudioSource _audioSource;
        private DataManager _dataManager;
        private bool IsMuted => !_dataManager.UserProfileData.SettingsInfoModel.IsSettingEnabled(SettingType.Music);

        [Inject]
        private void Construct(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        private void Start()
        {
            MuteClip(null, IsMuted);
        }

        public override void PlayClip(AudioClip audioClip, bool isLoop)
        {
            var seq = DOTween.Sequence();
            seq.Append(_audioSource.DOFade(0, 0.5f));
            seq.AppendCallback(() =>
            {
                _audioSource.clip = audioClip;
                _audioSource.loop = isLoop;
                _audioSource.Play();
            });
            seq.Append(_audioSource.DOFade(1, 0.5f));
        }

        public override void MuteClip(AudioClip audioClip, bool isMuted)
        {
            _audioSource.mute = isMuted;
        }

        public override void MuteAll(bool isMuted)
        {
        }
    }
}