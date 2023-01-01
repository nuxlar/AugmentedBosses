using RoR2;
using RoR2.Projectile;
using RoR2.CharacterAI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AugmentedBosses
{
  public class WanderingVagrant
  {
    // Accel 15
    // Base Move speed 6
    // Level Move Speed 0
    public void ModifyStats()
    {
      CharacterBody vagrantBody = AugmentedBosses.vagrant.GetComponent<CharacterBody>();
      vagrantBody.baseAcceleration = 20f;
      vagrantBody.baseMoveSpeed = 8f;
      vagrantBody.baseAttackSpeed = 1.25f;
      vagrantBody.baseDamage = 5;
    }
  }
}