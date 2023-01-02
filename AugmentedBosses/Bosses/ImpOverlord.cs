using RoR2;
using UnityEngine;
using EntityStates.ImpBossMonster;

namespace AugmentedBosses
{
  public class ImpOverlord
  {

    public ImpOverlord()
    {
      ModifyStats();
      On.EntityStates.ImpBossMonster.FireVoidspikes.FireSpikeFan += FireSpikeFan;
      On.EntityStates.ImpBossMonster.BlinkState.OnEnter += BlinkStateOnEnter;
    }

    private void ModifyStats()
    {
      CharacterBody overlordBody = AugmentedBosses.impBoss.GetComponent<CharacterBody>();
      overlordBody.baseAcceleration = 30f;
      overlordBody.baseAttackSpeed = 1.25f;
    }

    private void FireSpikeFan(On.EntityStates.ImpBossMonster.FireVoidspikes.orig_FireSpikeFan orig, EntityStates.ImpBossMonster.FireVoidspikes self, Ray aimRay, string muzzleName, string hitBoxGroupName)
    {
      orig(self, aimRay, muzzleName, hitBoxGroupName);
      for (int index = 0; index < FireVoidspikes.projectileCount; ++index)
        self.FireSpikeAuthority(aimRay, ((float)FireVoidspikes.projectileCount / 2f - (float)index) * (-10), ((float)FireVoidspikes.projectileCount / 2f - (float)index) * FireVoidspikes.projectileYawSpread, FireVoidspikes.projectileSpeed + FireVoidspikes.projectileSpeedPerProjectile * (float)index);
      for (int index = 0; index < FireVoidspikes.projectileCount; ++index)
        self.FireSpikeAuthority(aimRay, ((float)FireVoidspikes.projectileCount / 2f - (float)index) * FireVoidspikes.projectileYawSpread, ((float)FireVoidspikes.projectileCount / 2f - (float)index) * FireVoidspikes.projectileYawSpread, FireVoidspikes.projectileSpeed + FireVoidspikes.projectileSpeedPerProjectile * (float)index);
    }

    private void BlinkStateOnEnter(On.EntityStates.ImpBossMonster.BlinkState.orig_OnEnter orig, EntityStates.ImpBossMonster.BlinkState self)
    {
      self.duration = 2f;
      self.exitDuration = 1f;
      self.destinationAlertDuration = 1.5f;
      orig(self);
    }
  }
}