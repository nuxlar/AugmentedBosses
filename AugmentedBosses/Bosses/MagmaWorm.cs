using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AugmentedBosses
{
  public class MagmaWorm
  {
    private static GameObject magmaWorm = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/MagmaWorm/MagmaWormBody.prefab").WaitForCompletion();

    public MagmaWorm()
    {
      ModifyStats();
    }

    private void ModifyStats()
    {
      WormBodyPositions2 wormPosition = magmaWorm.GetComponent<WormBodyPositions2>();
      WormBodyPositionsDriver wormDriver = magmaWorm.GetComponent<WormBodyPositionsDriver>();
      wormPosition.followDelay = 0.1f;
      wormPosition.meatballCount = 10;
      wormPosition.speedMultiplier = 80f;
      wormDriver.maxBreachSpeed = 100f;
      wormDriver.maxTurnSpeed = 1000f;
      wormDriver.turnRateCoefficientAboveGround = 0.25f;
    }
  }
}