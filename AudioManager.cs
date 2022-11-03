using UnityEngine;

public class AudioManager : MonoBehaviour   // Singleton
{
    static AudioManager instance;

    public AudioSource Music;
    public AudioSource Sounds;

    public AudioClip[] musicClips;
    public AudioClip[] soundClips;

    public static AudioManager Instance
    { 
        get 
        { 
            return instance;
        } 
    }

    private void Awake()
    {
        instance = this;
    }

    public void PlaySound(int _index, float _volume = 1f)
    {
        Sounds.PlayOneShot(soundClips[_index], _volume);
    }
    public void PlayMusic(int _index)
    {
        Music.clip = musicClips[_index];
        Music.Play();
    }

    public void PauseMusic(bool _pause)
    {
        if (_pause)
            Music.Pause();
        else
            Music.Play();
    }
}