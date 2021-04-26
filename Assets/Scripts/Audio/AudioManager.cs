using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace LD48.Audio
{
    public class AudioManager : PersistentSingleton<AudioManager>
    {
        [SerializeField] private AudioMixerGroup sfxGroup;
        [SerializeField] private List<AudioSource> sfxAudioSources = new List<AudioSource>();

        private int sfxSourceIndex = 0;

        public void Start()
        {
            foreach (var audioSource in sfxAudioSources)
            {
                audioSource.outputAudioMixerGroup = sfxGroup;
                audioSource.loop = false;
            }
        }
        
        private AudioSource GetNextSfxSource()
        {
            sfxSourceIndex += 1;
            if (sfxSourceIndex >= sfxAudioSources.Count)
            {
                sfxSourceIndex = 0;
            }

            return sfxAudioSources[sfxSourceIndex];
        }

        public void PlaySfx(AudioClip clip)
        {
            PlaySfx(clip, 1f);
        }
        
        public void PlaySfx(AudioClip clip, float pitch)
        {
            if (sfxAudioSources.Count == 0) return;
            var source = GetNextSfxSource();
            source.pitch = pitch;

            source.clip = clip;
            source.Play();
        }
    }
}