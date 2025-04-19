using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
   CharacterManager character;

   [Header("Ground Check & Jumping")]
   [SerializeField] protected float gravityForce = -9.81f;
   [SerializeField] LayerMask groundLayer;
   [SerializeField] float groundCheckSphereRadius = 1;
   [SerializeField] protected Vector3 yVelocity;
   [SerializeField] protected float groundedYVelocity = -20;
   [SerializeField] protected float fallStartYVelocity = -5;
   protected bool fallingVelocityHasBeenSet = false;
   protected float inAirTimer = 0;

   [Header("Flags")]
   public bool isRolling = false;
   public bool canRotate = true;
   public bool canMove = true;
   public bool canRun = true;
   public bool canRoll = true;
   public bool isGrounded = true;

   protected virtual void Awake()
   {
      character = GetComponent<CharacterManager>();
   }

   protected virtual void Update()
   {
      HandleGroundCheck();

      if (character.characterLocomotionManager.isGrounded)
      {
         if (yVelocity.y < 0)
         {
            inAirTimer = 0;
            fallingVelocityHasBeenSet = false;
            yVelocity.y = groundedYVelocity;
         }
      }
      else
      {
         if (!character.characterNetworkManager.isJumping.Value && !fallingVelocityHasBeenSet)
         {
            fallingVelocityHasBeenSet = true;
            yVelocity.y = fallStartYVelocity;
         }

         inAirTimer += Time.deltaTime;
         character.animator.SetFloat("inAirTimer", inAirTimer);

         yVelocity.y += gravityForce * Time.deltaTime;
      }

      character.characterController.Move(yVelocity * Time.deltaTime);

   }

   protected void HandleGroundCheck()
   {
      isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
   }

    public void OnDrawGizmos()
    {
         Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
    }

    public void EnableCanRotate()
   {
      character.characterLocomotionManager.canRotate = true;
   }
   public void DisableCanRotate()
   {
      character.characterLocomotionManager.canRotate = false;
   }
}
