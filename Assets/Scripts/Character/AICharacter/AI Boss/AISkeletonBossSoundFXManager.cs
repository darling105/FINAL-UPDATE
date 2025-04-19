using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISkeletonBossSoundFXManager : CharacterSoundFXManager
{
    [Header("Club Whooshes")]
    public AudioClip[] clubWhooshes;

    [Header("Club Impact")]
    public AudioClip[] clubImpacts;

    public virtual void PlayClubImpactSoundFX()
    {
        if (clubImpacts.Length > 0)
            PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(clubImpacts));
    }

}
