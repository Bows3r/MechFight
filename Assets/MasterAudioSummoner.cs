using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MasterAudioSummoner : MonoBehaviour {
    public static MasterAudioSummoner instance;

    public GameObject AudioObject;

    public List<AudioClip> mechAudioFiles;
    public List<AudioClip> weaponAudioFiles;
    public List<AudioClip> explosionAudioFiles;
    public List<AudioClip> bettySounds;
    public List<AudioClip> deathScreams;
    public AudioClip criticalHit;

    bool playingClip;

    // Use this for initialization
    void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    AudioClip FindFile(string fileName, List<AudioClip> clipBank)
    {
        if (fileName == "" || fileName == null) return null;
        if (clipBank.Count == 0)
        {
            print("Invalid clip bank - count is 0!");
            return null;
        }
        for (int i = 0; i < clipBank.Count; i++)
        {
            if (fileName == clipBank[i].name)
            {
                return clipBank[i];
            }
        }
        print("Could not find audio by name; " + fileName);
        return null;
    }

    public void PlayAudio(List<AudioClip> clipBank, string audioClipName, float volumeLevel, float dopplerRange, Vector3 position)
    {
        AudioClip clip = FindFile(audioClipName, clipBank);
        if (clip == null)
        {
            print("Clip is null - unable to play file (attempted to play;" + audioClipName + " ).");
            return;
        }
        GameObject go = Instantiate(AudioObject, position, Quaternion.identity) as GameObject;
        AudioSource source = go.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.volume = volumeLevel;
        source.clip = clip;
        DestroyAfterTime script = go.AddComponent<DestroyAfterTime>();
        script.randomDopplerRange = dopplerRange;
        source.Play();
    }

    public void PlayCriticalHit(string audioClipName, float volumeLevel, float dopplerRange, Vector3 position)
    {
        if (playingClip) return;
        playingClip = true;
        GameObject go = Instantiate(AudioObject, position, Quaternion.identity) as GameObject;
        AudioSource source = go.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.volume = volumeLevel;
        source.clip = criticalHit;
        DestroyAfterTime script = go.AddComponent<DestroyAfterTime>();
        script.randomDopplerRange = dopplerRange;
        source.Play();

        float length = source.clip.length;

        StartCoroutine(PlayClip(length, audioClipName, volumeLevel, dopplerRange, position));
    }

    IEnumerator PlayClip(float delay, string audioClipName, float volumeLevel, float dopplerRange, Vector3 position)
    {
        yield return new WaitForSeconds(delay);
        AudioClip clip = FindFile(audioClipName, bettySounds);
        if (clip == null)
        {
            print("Clip is null - unable to play file (attempted to play;" + audioClipName + " ).");
            yield break;
        }
        GameObject go = Instantiate(AudioObject, position, Quaternion.identity) as GameObject;
        AudioSource source = go.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.volume = volumeLevel;
        source.clip = clip;
        DestroyAfterTime script = go.AddComponent<DestroyAfterTime>();
        script.randomDopplerRange = dopplerRange;
        source.Play();
        playingClip = false;
    }
}
