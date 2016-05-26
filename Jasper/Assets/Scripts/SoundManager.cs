using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public AudioClip playerMeleeAttack;
    public AudioClip playerRangeAttack;
    public AudioClip playerDamage;
    public AudioClip playerChargeJump;
    public AudioClip playerDash;

    private static AudioSource source;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayClip(AudioClip clip)
    {
        float vol = Random.Range(volLowRange, volHighRange);
        source.PlayOneShot(clip, vol);
    }

    public void Stop()
    {
        source.Stop();
    }
}
