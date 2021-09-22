
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;

namespace DigitalRuby.RainMaker
{
    public class BaseRainScript : MonoBehaviour
    {
        [Tooltip("Camera the rain should hover over, defaults to main camera")]
        public Camera Camera;

        [Tooltip("Whether rain should follow the camera. If false, rain must be moved manually and will not follow the camera.")]
        public bool FollowCamera = true;

        [Tooltip("Medium rain looping clip")]
        public AudioClip RainSound;

        [Tooltip("AudoMixer used for the rain sound")]
        public AudioMixerGroup RainSoundAudioMixer;

        [Tooltip("Intensity of rain (0-1)")]
        [Range(0.0f, 1.0f)]
        public float RainIntensity;

        [Tooltip("Rain particle system")]
        public ParticleSystem RainFallParticleSystem;

        [Tooltip("Particles system for when rain hits something")]
        public ParticleSystem RainExplosionParticleSystem;

        protected LoopingAudioSource audioSourceRain;
        protected LoopingAudioSource audioSourceRainCurrent;
        protected Material rainMaterial;
        protected Material rainExplosionMaterial;
        
        private void CheckForRainChange()
        {
            if (RainFallParticleSystem != null)
            {
                ParticleSystem.EmissionModule e = RainFallParticleSystem.emission;
                e.enabled = RainFallParticleSystem.GetComponent<Renderer>().enabled = true;
                if (!RainFallParticleSystem.isPlaying)
                {
                    RainFallParticleSystem.Play();
                }
                ParticleSystem.MinMaxCurve rate = e.rateOverTime;
                rate.mode = ParticleSystemCurveMode.Constant;
                rate.constantMin = rate.constantMax = RainFallEmissionRate();
                e.rateOverTime = rate;
            }
        }

        protected virtual void Start()
        {

#if DEBUG

            if (RainFallParticleSystem == null)
            {
                Debug.LogError("Rain fall particle system must be set to a particle system");
                return;
            }

#endif

            if (Camera == null)
            {
                Camera = Camera.main;
            }

            audioSourceRain = new LoopingAudioSource(this, RainSound, RainSoundAudioMixer);

            if (RainFallParticleSystem != null)
            {
                ParticleSystem.EmissionModule e = RainFallParticleSystem.emission;
                e.enabled = false;
                Renderer rainRenderer = RainFallParticleSystem.GetComponent<Renderer>();
                rainRenderer.enabled = false;
                rainMaterial = new Material(rainRenderer.material);
                rainMaterial.EnableKeyword("SOFTPARTICLES_OFF");
                rainRenderer.material = rainMaterial;
            }
            if (RainExplosionParticleSystem != null)
            {
                ParticleSystem.EmissionModule e = RainExplosionParticleSystem.emission;
                e.enabled = false;
                Renderer rainRenderer = RainExplosionParticleSystem.GetComponent<Renderer>();
                rainExplosionMaterial = new Material(rainRenderer.material);
                rainExplosionMaterial.EnableKeyword("SOFTPARTICLES_OFF");
                rainRenderer.material = rainExplosionMaterial;
            }

            audioSourceRain.Play();


        }

        protected virtual void Update()
        {

#if DEBUG

            if (RainFallParticleSystem == null)
            {
                Debug.LogError("Rain fall particle system must be set to a particle system");
                return;
            }

#endif

            CheckForRainChange();
        }

        protected virtual float RainFallEmissionRate()
        {
            return (RainFallParticleSystem.main.maxParticles / RainFallParticleSystem.main.startLifetime.constant) * RainIntensity;
        }

        protected virtual bool UseRainMistSoftParticles
        {
            get
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Provides an easy wrapper to looping audio sources with nice transitions for volume when starting and stopping
    /// </summary>
    public class LoopingAudioSource
    {
        public AudioSource AudioSource { get; private set; }
        public float TargetVolume { get; private set; }

        public LoopingAudioSource(MonoBehaviour script, AudioClip clip, AudioMixerGroup mixer)
        {
            AudioSource = script.gameObject.AddComponent<AudioSource>();

            if (mixer != null)
            {
                AudioSource.outputAudioMixerGroup = mixer;
            }

            AudioSource.loop = true;
            AudioSource.clip = clip;
            AudioSource.playOnAwake = false;
            AudioSource.volume = 0.0f;
            AudioSource.Stop();
            TargetVolume = 1.0f;
        }

        public void Play()
        {
            AudioSource.volume = 1.0f;
            if (!AudioSource.isPlaying)
            {
                AudioSource.Play();
            }
        }

        public void Stop()
        {
            AudioSource.volume = 0;
            AudioSource.Stop();
            TargetVolume = 0.0f;
        }
    }
}