using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource Music;
    public AudioSource Sounds;

    public AudioClip[] musicClips;
    public AudioClip[] soundClips;

    public void PlaySound(int _index, float _volume = 1f)
    {
        Sounds.PlayOneShot(soundClips[_index], _volume);
    }
}