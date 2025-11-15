using System;
using UnityEngine;

public class soundScript : MonoBehaviour
{
    [SerializeField] private AudioSource audioPrefab;
    [SerializeField] private AudioSource bgMusic;
    public bool sfx;
    public bool msc;
    void Start()
    {
        if (!msc)
        {
            bgMusic.Stop();
        }
    }

    public void playClip(AudioClip clip, Boolean randPitch = false)
    {
        if (!sfx)
        {
            return;
        }
        float pitch = 1f;
        if (randPitch)
        {
            pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        }
        AudioSource source = Instantiate(audioPrefab, transform.position, Quaternion.identity);
        source.clip = clip;
        source.pitch = pitch;
        source.Play();
        Destroy(source.gameObject, source.clip.length);
    }

    public void stopMusic()
    {
        bgMusic.Pause();
    }
    public void startMusic()
    {
        bgMusic.Play();
    }
}
