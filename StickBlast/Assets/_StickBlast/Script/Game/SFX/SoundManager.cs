using System;
using System.Collections.Generic;
using _StickBlast.Script.Game.Enums;
using _StickBlast.Script.Game.Management;
using UnityEngine;

namespace _StickBlast.Script.Game.SFX
{
    public class SoundManager : MonoBehaviour, IEventListener
    {
        public enum SoundType
        {
            Game,
            Point,
            Drag,
            Drop,
            Combo,
            ReturnPos
        }
        
        [Serializable]
        public class SoundList
        {
            public SoundType soundType;
            public AudioSource audioSource;
        }

        public List<SoundList> soundList;
        
        private void OnEnable()
        {
            EventManager.Instance.RegisterListener(this);
        }

        private void OnDisable()
        {
            EventManager.Instance.UnregisterListener(this);
        }

        public void PlaySound(SoundType soundType)
        {
            foreach (var sound in soundList)
            {
                if (sound.soundType == soundType)
                {
                    sound.audioSource.Play();
                    break;
                }                
            }
        }


    }
}