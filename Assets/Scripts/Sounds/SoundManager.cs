using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource[] _backgroundSound;
    
    private AudioSource _audioSource;
    
    private UnityAction _onMusicTurn;

    private int _backgroundSoundIndex = 0;

    private bool _isWaitingIntro = false;

    public bool IsWaitingIntro => _isWaitingIntro;
    
    public int CurrentBackgroundSound => _backgroundSoundIndex;
    
    public event UnityAction OnMusicTurn
    {
        add { _onMusicTurn -= value; _onMusicTurn += value; }
        remove => _onMusicTurn -= value;
    }
    
    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        
        SetVolume(GetVolume());
    }
    
    public void PlaySound(SoundType soundType)
    {
        AudioClip audioClip = SoundsBase.Instance.GetAudioClip(soundType);

        if (audioClip != null)
        {
            _audioSource.PlayOneShot(audioClip);
        }
    }
    
    public void PlaySound(SoundType soundType, GameObject parentObject)
    {
        AudioSource audioSource = parentObject.GetComponent<AudioSource>()
            ? parentObject.GetComponent<AudioSource>() 
            : parentObject.AddComponent<AudioSource>();

        AudioClip audioClip = SoundsBase.Instance.GetAudioClip(soundType);

        if (audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }
    public IEnumerator SwitchBackgroundWithIntro(AudioSource introSource, int backgroundIndex)
    {
        PauseBackgroundMusic();
        
        introSource.Play();
   
        _isWaitingIntro = true;
        
        while (introSource.isPlaying)
        {
            yield return null;
        }

        _isWaitingIntro = false;
        
        SwitchBackground(backgroundIndex);
        
        yield return null;
    }

    public void SwitchBackground(int index, float delay = 0.1f)
    {
        PauseBackgroundMusic();
        
        if (index < _backgroundSound.Length)
        {
            StartCoroutine(SwitchDelay(index, delay));
        }
    }

    private IEnumerator SwitchDelay(int index, float delay)
    {
        yield return new WaitForSeconds(delay);
            
        _backgroundSoundIndex = index;
            
        PlayBackgroundMusic(true);
    }

    public void PlayBackgroundMusic(bool reset = false)
    {
        if (!_backgroundSound[_backgroundSoundIndex].isPlaying)
        {
            if (reset)
            {
                _backgroundSound[_backgroundSoundIndex].time = 0;
            }
            
            _backgroundSound[_backgroundSoundIndex].Play();
        }
    }
    
    public void PauseBackgroundMusic()
    {
        _backgroundSound[_backgroundSoundIndex].Pause();
    }

    public void TurnMusic()
    {
        if (IsMusicOff())
        {
            MusicOn();
        }
        else
        {
            MusicOff();
        }
        
        _onMusicTurn?.Invoke();
    }

    public float GetVolume()
    {
        return PlayerPrefs.GetFloat("volume", 1f);
    }
    
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("volume", volume);
    }
    
    public static bool IsMusicOff()
    {
        return AudioListener.volume == 0;
    }

    public static void MusicOn()
    {
        AudioListener.volume = 1;
    }

    public static void MusicOff()
    {
        AudioListener.volume = 0;
    }
}
