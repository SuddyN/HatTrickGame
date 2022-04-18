using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager: MonoBehaviour {

    public Sound[] sounds;

    private static AudioManager _instance;
    public static AudioManager Instance {
        get {
            if (_instance == null) {
                Debug.LogError("GameManager is null!");
            }
            return _instance;
        }
    }

    void Awake() {

        if (_instance == null) {
            _instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    private void Start() {
        Play("bgmusic");
    }

    public void Play(string name) {
        Sound playSound = Array.Find(sounds, sound => sound.name.Equals(name));
        if (playSound == null) {
            Debug.LogError("Sound: " + name + " not found in sounds list!");
            return;
        }
        playSound.source.Play();
    }
}
