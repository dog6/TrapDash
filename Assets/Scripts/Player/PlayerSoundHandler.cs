using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSoundHandler : MonoBehaviour
{

    [SerializeField] private List<AudioClip> jumpSFX, hitSoundSFX, coinPickupSFX;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayJumpSFX() => PlayRandomClip(jumpSFX);

    public void PlayCoinPickupSFX() => PlayRandomClip(coinPickupSFX);

    public void PlayHitSoundFX() => PlayRandomClip(hitSoundSFX);

    public void PlayRandomClip(List<AudioClip> clips)
    {
        int randomClipIndex = Random.Range(0, clips.Count);
        audioSource.PlayOneShot(clips[randomClipIndex]);
    }


}
