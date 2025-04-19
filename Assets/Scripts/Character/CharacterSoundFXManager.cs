using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Damage Grunts")]
    [SerializeField] protected AudioClip[] damageGrunts;

    [Header("Attack Grunts")]
    [SerializeField] protected AudioClip[] attackGrunts;

    [Header("Footstep SFX")]
    [SerializeField] protected AudioClip[] footSteps;
    // public AudioClip[] stoneFootSteps;
    // public AudioClip[] dirtFootSteps;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySoundFX(AudioClip soundFX, float volume = 1, bool randomizePitch = true, float pitchRandom = 0.1f)
    {
        audioSource.PlayOneShot(soundFX, volume);

        audioSource.pitch = 1;

        if (randomizePitch)
        {
            audioSource.pitch += Random.Range(-pitchRandom, pitchRandom);
        }
    }

    public void PlayRollSoundFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.rollSFX);
    }

    public virtual void PlayDamageGruntSoundFX()
    {
        if (damageGrunts.Length > 0)
            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(damageGrunts));
    }

    public virtual void PlayAttackGruntSoundFX()
    {
        if (damageGrunts.Length > 0)
            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(attackGrunts));
    }

    public virtual void PlayFootStepSoundFX()
    {
        PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(footSteps));
    }

    public virtual void PlayStanceBreakSoundFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.stanceBreakSFX);
    }

    public virtual void PlayCriticalSoundFX()
    {
        audioSource.PlayOneShot(WorldSoundFXManager.instance.criticalSFX);
    }

    public virtual void PlayBlockSoundFX()
    {
        
    }

}
