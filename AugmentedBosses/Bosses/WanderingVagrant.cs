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
    public void ModifyStats()
    {
      CharacterBody vagrantBody = AugmentedBosses.vagrant.GetComponent<CharacterBody>();
      vagrantBody.acceleration = 500;
      vagrantBody.moveSpeed = 14;
    }
  }
}