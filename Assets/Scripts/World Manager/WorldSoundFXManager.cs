using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldSoundFXManager : MonoBehaviour
{
    public static WorldSoundFXManager instance;

    [Header("Boss Track")]
    [SerializeField] AudioSource bossIntroPlayer;
    [SerializeField] AudioSource bossLoopPlayer;

    [Header("Damage Sounds")]
    public AudioClip[] physicalDamageSFX;

    [Header("Action Sounds")]
    public AudioClip pickUpItemSFX;
    public AudioClip rollSFX;
    public AudioClip stanceBreakSFX;
    public AudioClip criticalSFX;
    public AudioClip healingFlaskSFX;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void PlayBossTrack(AudioClip introTrack, AudioClip loopTrack)
    {
        bossIntroPlayer.volume = 1;
        bossIntroPlayer.clip = introTrack;
        bossLoopPlayer.loop = false;
        bossIntroPlayer.Play();

        bossLoopPlayer.volume = 1;
        bossLoopPlayer.clip = loopTrack;
        bossLoopPlayer.loop = true;
        bossLoopPlayer.PlayDelayed(bossIntroPlayer.clip.length);
    }

    public void StopBossMusic()
    {
        StartCoroutine(FadeOutBossMusicThenStop());
    }

    public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
    {
        int index = Random.Range(0, array.Length);
        return array[index];
    }

    // public AudioClip ChooseRandomFootStepSoundBasedOnGround(GameObject steppedObject, CharacterManager character)
    // {
    //     if (steppedObject.tag == "Dirt")
    //     {
    //         return ChooseRandomSFXFromArray(character.characterSoundFXManager.dirtFootSteps);
    //     }
    //     else if (steppedObject.tag == "Stone")
    //     {
    //         return ChooseRandomSFXFromArray(character.characterSoundFXManager.stoneFootSteps);
    //     }
    //     return null;
    // }

    private IEnumerator FadeOutBossMusicThenStop()
    {

        while (bossLoopPlayer.volume > 0)
        {
            bossLoopPlayer.volume -= Time.deltaTime;
            bossIntroPlayer.volume -= Time.deltaTime;
            yield return null;
        }

        bossIntroPlayer.Stop();
        bossLoopPlayer.Stop();
    }
}
