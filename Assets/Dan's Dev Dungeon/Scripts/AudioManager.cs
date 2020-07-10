using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject AudioPrefab;

    public List<AudioClip> audioClips = new List<AudioClip>();
    public List<string> clipName;

    public Dictionary<string, AudioClip> AudioTrack = new Dictionary<string, AudioClip>();

    public static AudioManager AudioPlayer;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < clipName.Count; i++)
        {
            AudioTrack.Add(clipName[i], audioClips[i]);
        }

        AudioPlayer = this;
    }

    public void PlayAudio(string name)
    {
        if (AudioTrack.ContainsKey(name))
        {
            GameObject Audio = Instantiate(AudioPrefab);
            AudioSource SRC = AudioPrefab.GetComponent<AudioSource>();
            SRC.clip = AudioTrack[name];
            SRC.Play();

            Destroy(Audio, SRC.clip.length);
        }
    }
}
