using System.Collections.Generic;
using UnityEngine;

public class SoundsBase: Singleton<SoundsBase>
{
    [SerializeField] private List<SoundAudioClip> _soundsBase;
    
    public AudioClip GetAudioClip(SoundType soundType)
    {
        var soundAudioClip = _soundsBase.Find(item => item.SoundType == soundType);

        return soundAudioClip?.AudioClip;
    }
}