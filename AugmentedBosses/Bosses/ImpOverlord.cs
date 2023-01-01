using RoR2;
using RoR2.Projectile;
using RoR2.CharacterAI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using EntityStates;
using EntityStates.TitanMonster;

namespace AugmentedBosses
{
  public class ImpOverlord
  {
    public void ModifyStats()
    {
      CharacterBody overlordBody = AugmentedBosses.impBoss.GetComponent<CharacterBody>();
      overlordBody.baseAcceleration = 30f;
      overlordBody.baseAttackSpeed = 1.25f;
    }
  }
}