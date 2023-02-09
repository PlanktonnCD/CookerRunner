using System;
using System.Collections.Generic;
using Audio;
using Configs;
using Gameplay.Scripts.DataProfiling;
using Gameplay.Scripts.DataProfiling.Models;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{

    [SerializeField] private MusicPlayer _musicPlayer;
    [SerializeField] private SoundPlayer _soundPlayer;
    private AudioConfig _audioConfig;
    private DataManager _dataManager;
    private SignalBus _signalBus;

    private SettingsInfoModel Settings => _dataManager.UserProfileData.SettingsInfoModel;
    private bool IsMusicEnabled => Settings.IsSettingEnabled(SettingType.Music);
    private bool IsSoundEnabled => Settings.IsSettingEnabled(SettingType.Sound);

    [Inject]
    private void Construct(AudioConfig audioConfig, DataManager dataManager, SignalBus signalBus)
    {
        _signalBus = signalBus;
        _dataManager = dataManager;
        _audioConfig = audioConfig;
    }

    public void PlayMusic(TrackName trackName, bool isLoop = true)
    {
        PlayTrack(trackName, _musicPlayer, isLoop);
    }

    public void PlaySound(TrackName trackName, bool isLoop = false)
    {
        PlayTrack(trackName, _soundPlayer, isLoop);
    }

    private void PlayTrack(TrackName trackName, AudioPlayer audioPlayer, bool isLoop)
    {
        if (audioPlayer == null)
        {
            return;
        }
        var audioClip = _audioConfig.GetAudioConfig(trackName);
        if (audioClip == null)
        {
            return;
        }
        audioPlayer.PlayClip(audioClip, isLoop);
    }

    public void MuteMusicPlayer(bool isMuted)
    {
        _musicPlayer.MuteClip(null, isMuted);
    }

    public void MuteSoundPlayer(bool isMuted)
    {
        _soundPlayer.MuteAll(isMuted);
    }

    public void TurnOffSound(TrackName trackName, bool isMuted)
    {
        var audioClip = _audioConfig.GetAudioConfig(trackName);
        if (audioClip == null)
        {
            return;
        }
        _soundPlayer.MuteClip(audioClip, isMuted);
    }
}

[Serializable]
public struct EnemyDeathSounds
{
    public int ID;
    public List<TrackName> TrackNames;
}