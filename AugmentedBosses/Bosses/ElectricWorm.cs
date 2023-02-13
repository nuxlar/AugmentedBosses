using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AugmentedBosses
{
  public class ElectricWorm
  {
    private static GameObject electricWorm = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ElectricWorm/ElectricWormBody.prefab").WaitForCompletion();

    public ElectricWorm()
    {
      ModifyStats();
    }

    private void ModifyStats()
    {
      WormBodyPositions2 wormPosition = electricWorm.GetComponent<WormBodyPositions2>();
      WormBodyPositionsDriver wormDriver = electricWorm.GetComponent<WormBodyPositionsDriver>();
      wormPosition.followDelay = 0.1f;
      wormPosition.meatballCount = 10;
      wormPosition.speedMultiplier = 80f;
      wormDriver.maxBreachSpeed = 100f;
      wormDriver.maxTurnSpeed = 1000f;
      wormDriver.turnRateCoefficientAboveGround = 0.25f;
    }
  }
}