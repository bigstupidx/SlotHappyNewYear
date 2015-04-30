using UnityEngine;

public class SoundManager : MonoBehaviour {

    public AudioSource audiosource;

    public AudioClip[] audioclips;

    float m_volume;

    public void Play(int idx_music, bool loop)
    {

        if(audio != null)
            audio.Stop();

        audio.volume = m_volume;

        this.audio.loop = loop;

        if (this.audio.loop == false)
        {
            audio.PlayOneShot(audioclips[idx_music]);
        }
        else
        {
            audio.clip = audioclips[idx_music];
            audio.Play();
        }
    }

    public void SetVolume(float value)
    {
        m_volume = value;
    }
}
