using UnityEngine;
using System.Collections;

public class BGMManager : MonoBehaviour {

    public AudioClip backgroundLoop;

    private static AudioSource source;

    void Awake()
    {
        source = GetComponent<AudioSource>();
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Level 1")
            Invoke("LoopAfterIntro", source.clip.length);
    }

    private void LoopAfterIntro()
    {
        source.clip = backgroundLoop;
        source.Play();
        source.loop = true;
    }
}
