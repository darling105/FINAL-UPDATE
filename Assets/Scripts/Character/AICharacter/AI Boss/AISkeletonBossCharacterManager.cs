using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISkeletonBossCharacterManager : AIBossCharacterManager
{
   public AISkeletonBossSoundFXManager skeletonBossSoundFXManager;

   protected override void Awake()
   {
      base.Awake();
      skeletonBossSoundFXManager = GetComponent<AISkeletonBossSoundFXManager>();
   }
}
