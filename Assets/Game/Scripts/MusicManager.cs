using UnityEngine;

public class MusicManager : MonoBehaviour {

    public AudioClip[] audioclips;

    float m_volume = 1.0f;
    
    public void Play(int idx)
    {
        audio.clip = audioclips[idx];
        audio.volume = m_volume;
        audio.Play();
    }

    public void SetVolume(float value)
    {
        m_volume = value;

        if (audio != null)
        {
            if (m_volume > 0.0f)
                this.Play(0);
            else
                audio.Stop();
        }
    }
}
