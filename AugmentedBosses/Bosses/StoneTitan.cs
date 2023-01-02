using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using EntityStates;
using EntityStates.TitanMonster;

namespace AugmentedBosses
{
  public class StoneTitan
  {
    // Accel 20
    // Base Move speed 5
    // Level Move Speed 0
    private static GameObject titan = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Titan/TitanBody.prefab").WaitForCompletion();

    public StoneTitan()
    {
      ModifyStats();
      On.EntityStates.TitanMonster.FireMegaLaser.FireBullet += FireBullet;
      On.EntityStates.TitanMonster.FireFist.PlacePredictedAttack += PlacePredictedAttack;
    }
    private void ModifyStats()
    {
      titan.transform.localScale = new Vector3(1, 1, 1);
      CharacterBody titanBody = titan.GetComponent<CharacterBody>();
      SkillLocator titanSkillLocator = titan.GetComponent<SkillLocator>();
      titanSkillLocator.special.skillFamily.variants[0].skillDef.activationState = new SerializableEntityStateType(typeof(ChargeGoldMegaLaser));
      titanBody.baseAttackSpeed = 1.25f;
      titanBody.baseAcceleration = 30f;
      titanBody.baseMoveSpeed = 7f;
    }

    private void PlacePredictedAttack(On.EntityStates.TitanMonster.FireFist.orig_PlacePredictedAttack orig, EntityStates.TitanMonster.FireFist self)
    {
      float num3 = UnityEngine.Random.Range(0.0f, 360f);
      for (int index3 = 0; index3 < 4; ++index3)
      {
        int num4 = 0;
        for (int index4 = 0; index4 < 2; ++index4)
        {
          Vector3 vector3 = Quaternion.Euler(0.0f, num3 + 45f * (float)index3, 0.0f) * Vector3.forward;
          Vector3 position = self.predictedTargetPosition + vector3 * FireGoldFist.distanceBetweenFists * (float)index4;
          float maxDistance = 60f;
          RaycastHit hitInfo;
          if (Physics.Raycast(new Ray(position + Vector3.up * (maxDistance / 2f), Vector3.down), out hitInfo, maxDistance, (int)LayerIndex.world.mask, QueryTriggerInteraction.Ignore))
            position = hitInfo.point;
          self.PlaceSingleDelayBlast(position, FireGoldFist.delayBetweenFists * (float)num4);
          ++num4;
        }
      }
    }

    private void FireBullet(
      On.EntityStates.TitanMonster.FireMegaLaser.orig_FireBullet orig,
      EntityStates.TitanMonster.FireMegaLaser self,
      Transform modelTransform,
      Ray aimRay,
      string targetMuzzle,
      float maxDistance
  )
    {
      if ((bool)(UnityEngine.Object)self.effectPrefab)
        EffectManager.SimpleMuzzleFlash(self.effectPrefab, self.gameObject, targetMuzzle, false);
      if (!self.isAuthority)
        return;
      new BulletAttack()
      {
        owner = self.gameObject,
        weapon = self.gameObject,
        origin = aimRay.origin,
        aimVector = aimRay.direction,
        minSpread = FireMegaLaser.minSpread,
        maxSpread = FireMegaLaser.maxSpread,
        bulletCount = 1U,
        damage = ((FireMegaLaser.damageCoefficient * self.damageStat / FireMegaLaser.fireFrequency)) * 0.75f,
        force = FireMegaLaser.force,
        damageType = DamageType.SlowOnHit,
        muzzleName = targetMuzzle,
        hitEffectPrefab = self.hitEffectPrefab,
        isCrit = Util.CheckRoll(self.critStat, self.characterBody.master),
        procCoefficient = FireMegaLaser.procCoefficientPerTick,
        HitEffectNormal = false,
        radius = 0.0f,
        maxDistance = maxDistance
      }.Fire();
    }

  }
}