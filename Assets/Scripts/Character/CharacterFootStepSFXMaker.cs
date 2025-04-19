using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CharacterFootStepSFXMaker : MonoBehaviour
{
    CharacterManager character;

    AudioSource audioSource;
    GameObject steppedOnObject;

    private bool hasTouchedGround = false;
    private bool hasPlayedFootStepSFX = false;
    [SerializeField] float distanceToGround = 0.05f;

    private void Awake()
    {
        character = GetComponent<CharacterManager>();
        audioSource = GetComponentInParent<AudioSource>();
    }

    private void FixedUpdate()
    {
        CheckForFootSteps();
    }

    private void CheckForFootSteps()
    {
        if (character == null)
            return;

        if (!character.characterNetworkManager.isMoving.Value)
            return;

        RaycastHit hit;

        if (Physics.Raycast(transform.position, character.transform.TransformDirection(Vector3.down), out hit, distanceToGround, WorldUtilityManager.instance.GetEnviroLayers()))
        {
            hasTouchedGround = true;

            if (!hasPlayedFootStepSFX)
                steppedOnObject = hit.transform.gameObject;
        }
        else
        {
            hasTouchedGround = false;
            hasPlayedFootStepSFX = false;
            steppedOnObject = null;
        }

        if (hasTouchedGround && !hasPlayedFootStepSFX)
        {
            hasPlayedFootStepSFX = true;
            PlayFootStepSoundFX();
        }
    }

    private void PlayFootStepSoundFX()
    {
        //audioSource.PlayOneShot(WorldSoundFXManager.instance.ChooseRandomFootStepSoundBasedOnGround(steppedOnObject, character));

        character.characterSoundFXManager.PlayFootStepSoundFX();
    }

}
