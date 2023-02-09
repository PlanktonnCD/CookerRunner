using UnityEngine;

namespace Audio
{
    public abstract class AudioPlayer : MonoBehaviour
    {
        public abstract void PlayClip(AudioClip audioClip, bool isLoop);
        public abstract void MuteClip(AudioClip audioClip, bool isMuted);

        public abstract void MuteAll(bool isMuted);
    }
}