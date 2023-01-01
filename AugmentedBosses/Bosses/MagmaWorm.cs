using RoR2;
using RoR2.Projectile;
using RoR2.CharacterAI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using EntityStates;
using EntityStates.TitanMonster;

namespace AugmentedBosses
{
  public class MagmaWorm
  {
    public void ModifyStats()
    {
      WormBodyPositions2 wormPosition = AugmentedBosses.magmaWorm.GetComponent<WormBodyPositions2>();
      WormBodyPositionsDriver wormDriver = AugmentedBosses.magmaWorm.GetComponent<WormBodyPositionsDriver>();
      wormPosition.followDelay = 0.1f;
      wormPosition.meatballCount = 10;
      wormPosition.speedMultiplier = 80f;
      wormDriver.maxBreachSpeed = 100f;
      wormDriver.maxTurnSpeed = 1000f;
      wormDriver.turnRateCoefficientAboveGround = 0.25f;
    }
  }
}