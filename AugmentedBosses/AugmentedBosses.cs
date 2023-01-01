using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using EntityStates;
using EntityStates.TitanMonster;
using EntityStates.BeetleQueenMonster;
using EntityStates.VagrantMonster;
using EntityStates.ImpBossMonster;
using RoR2;
using RoR2.UI;
using RoR2.CharacterAI;
using RoR2.Skills;
using RoR2.Projectile;
using EntityStates.NullifierMonster;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AugmentedBosses
{
  [BepInPlugin("com.Nuxlar.AugmentedBosses", "AugmentedBosses", "0.5.0")]

  public class AugmentedBosses : BaseUnityPlugin
  {
    public static GameObject beetleBody = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Beetle/BeetleBody.prefab").WaitForCompletion();
    public static GameObject beetleQueen = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Beetle/BeetleQueen2Body.prefab").WaitForCompletion();
    public static GameObject beetleQueenMaster = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Beetle/BeetleQueenMaster.prefab").WaitForCompletion();
    public static GameObject titan = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Titan/TitanBody.prefab").WaitForCompletion();
    public static GameObject vagrant = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Vagrant/VagrantBody.prefab").WaitForCompletion();
    public static GameObject dunestrider = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ClayBoss/ClayBossBody.prefab").WaitForCompletion();
    public static GameObject impBoss = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ImpBoss/ImpBossBody.prefab").WaitForCompletion();
    public static GameObject magmaWorm = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/MagmaWorm/MagmaWormBody.prefab").WaitForCompletion();
    public static GameObject grovetender = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Gravekeeper/GravekeeperBody.prefab").WaitForCompletion();
    public static GameObject solusControlUnit = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/RoboBallBoss/RoboBallBossBody.prefab").WaitForCompletion();
    public static GameObject xiConstruct = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/MajorAndMinorConstruct/MegaConstructBody.prefab").WaitForCompletion();
    public static GameObject grandparent = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Grandparent/GrandParentBody.prefab").WaitForCompletion();
    public static GameObject overloadingWorm = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ElectricWorm/ElectricWormBody.prefab").WaitForCompletion();
    public static GameObject scavenger = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Scav/ScavBody.prefab").WaitForCompletion();
    public static GameObject devastator = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidMegaCrab/VoidMegaCrabBody.prefab").WaitForCompletion();
    private static GameObject cannonGhost = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Vagrant/VagrantCannonGhost.prefab").WaitForCompletion();
    private static Material cannonBlue = Addressables.LoadAssetAsync<Material>("RoR2/Base/Vagrant/matVagrantCannonBlue.mat").WaitForCompletion();
    private static Material cannonRed = Addressables.LoadAssetAsync<Material>("RoR2/Base/Vagrant/matVagrantCannonRed.mat").WaitForCompletion();

    public void Awake()
    {
      On.RoR2.HealthComponent.TakeDamage += TakeDamage;
      // Beetle Queen
      On.RoR2.CharacterBody.RecalculateStats += RecalculateStats;
      On.EntityStates.BeetleQueenMonster.SummonEggs.SummonEgg += SummonEgg;
      BeetleQueen beetleQueen = new();
      beetleQueen.ModifyAI();
      beetleQueen.ModifyBeetles();
      beetleQueen.ModifyProjectile();
      // Wandering Vagrant
      CharacterMaster.onStartGlobal += MasterChanges;
      On.RoR2.UI.CombatHealthBarViewer.Awake += HealthbarAwake;
      On.RoR2.CharacterMaster.OnBodyStart += OnBodyStart;
      On.EntityStates.VagrantMonster.FireTrackingBomb.FireBomb += VagrantFireBomb;
      WanderingVagrant wanderingVagrant = new();
      wanderingVagrant.ModifyStats();
      cannonGhost.transform.GetChild(1).GetComponent<MeshRenderer>().material = cannonRed;
      vagrant.transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<SkinnedMeshRenderer>().material = cannonRed;
      // Stone Titan
      StoneTitan stoneTitan = new();
      stoneTitan.ModifyStats();
      On.EntityStates.TitanMonster.FireMegaLaser.FireBullet += FireBullet;
      On.EntityStates.TitanMonster.FireFist.PlacePredictedAttack += PlacePredictedAttack;
      // Magma Worm
      MagmaWorm magmaWorm = new();
      magmaWorm.ModifyStats();
      // Imp Overlord
      ImpOverlord impOverlord = new();
      impOverlord.ModifyStats();
      On.EntityStates.ImpBossMonster.FireVoidspikes.FireSpikeFan += FireSpikeFan;
      On.EntityStates.ImpBossMonster.BlinkState.OnEnter += BlinkStateOnEnter;
    }

    // keeps all enemy HP bars up constantly, mainly for vagrant but idrc anymore so its for everyone
    private void HealthbarAwake(On.RoR2.UI.CombatHealthBarViewer.orig_Awake orig, RoR2.UI.CombatHealthBarViewer self)
    {
      orig(self);
      self.healthBarDuration = float.PositiveInfinity;
    }

    private void TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, RoR2.HealthComponent self, DamageInfo damageInfo)
    {
      float threeQuarters = self.fullHealth * 0.75f;
      float half = self.fullHealth / 2;
      float quarter = self.fullHealth * 0.25f;
      float healthAfterDmg = self.health - damageInfo.damage;
      string name = self.body.name;
      if (name == "BeetleQueen2Body(Clone)" || name == "TitanBody(Clone)" || name == "VagrantBody(Clone)" || name == "MagmaWormBody(Clone)" || name == "ClayBossBody(Clone)" || name == "ImpBossBody(Clone)" || name == "GravekeeperBody(Clone)" || name == "GrandparentBody(Clone)" || name == "ElectricWormBody(Clone)" || name == "RoboBallBossBody(Clone)" || name == "MegaConstructBody(Clone)")
      {
        if ((self.health > threeQuarters && healthAfterDmg < threeQuarters) || (self.health > half && self.health < threeQuarters && healthAfterDmg < half) || (self.health > quarter && self.health < half && healthAfterDmg < quarter))
        {
          damageInfo.damage = damageInfo.damage / 2;
          self.body.AddTimedBuff(RoR2Content.Buffs.ArmorBoost, 10);
          self.body.AddTimedBuff(RoR2Content.Buffs.WarCryBuff, 10);
        }
      }
      orig(self, damageInfo);
    }

    private void MasterChanges(CharacterMaster master)
    {
      if (master.name == "VagrantMaster(Clone)")
      {
        master.GetComponent<CharacterMaster>().GetComponents<AISkillDriver>().Where<AISkillDriver>(x => x.customName == "Chase").First<AISkillDriver>().minDistance = 25f;
        master.GetComponent<CharacterMaster>().GetComponents<AISkillDriver>().Where<AISkillDriver>(x => x.customName == "TrackingBomb").First<AISkillDriver>().movementType = AISkillDriver.MovementType.ChaseMoveTarget;
        master.GetComponent<CharacterMaster>().GetComponents<AISkillDriver>().Where<AISkillDriver>(x => x.customName == "Barrage").First<AISkillDriver>().movementType = AISkillDriver.MovementType.ChaseMoveTarget;
      }
    }

    private void OnBodyStart(On.RoR2.CharacterMaster.orig_OnBodyStart orig, RoR2.CharacterMaster self, CharacterBody body)
    {
      orig(self, body);
      if (body.name == "VagrantBody(Clone)")
        body.inventory.GiveItem(RoR2Content.Items.ShockNearby);
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

    private void VagrantFireBomb(On.EntityStates.VagrantMonster.FireTrackingBomb.orig_FireBomb orig, FireTrackingBomb self)
    {
      Ray aimRay = self.GetAimRay();
      Transform modelTransform = self.GetModelTransform();
      if ((bool)modelTransform)
      {
        ChildLocator component = modelTransform.GetComponent<ChildLocator>();
        if ((bool)component)
          aimRay.origin = component.FindChild("TrackingBombMuzzle").transform.position;
      }
      EffectManager.SimpleMuzzleFlash(FireTrackingBomb.muzzleEffectPrefab, self.gameObject, "TrackingBombMuzzle", false);
      if (!self.isAuthority)
        return;
      ProjectileManager.instance.FireProjectile(FireTrackingBomb.projectilePrefab, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), self.gameObject, self.damageStat * FireTrackingBomb.bombDamageCoefficient, FireTrackingBomb.bombForce, Util.CheckRoll(self.critStat, self.characterBody.master), DamageColorIndex.Default, null, 20);
    }

    private void RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
    {
      orig(self);
      self.armor -= 5f * (float)self.GetBuffCount(RoR2Content.Buffs.BeetleJuice);
    }

    private void SummonEgg(On.EntityStates.BeetleQueenMonster.SummonEggs.orig_SummonEgg orig, SummonEggs self)
    {
      orig(self);
      if (!NetworkServer.active || !(bool)self.characterBody || !(bool)self.teamComponent)
        return;
      List<CharacterBody> characterBodyList = new List<CharacterBody>();
      foreach (Component component1 in Physics.OverlapSphere(((Component)((EntityState)self).characterBody).gameObject.transform.position, (float)(120.0 * ((double)((Component)((EntityState)self).characterBody).gameObject.transform.localScale.x / 1.0)), (int)(LayerIndex.entityPrecise.mask)))
      {
        HurtBox component2 = component1.GetComponent<HurtBox>();
        if ((bool)component2 && (bool)component2.healthComponent && (bool)component2.healthComponent.body && (bool)component2.healthComponent.body && (bool)component2.healthComponent.body.teamComponent && component2.healthComponent.body.teamComponent.teamIndex == ((EntityState)self).teamComponent.teamIndex)
        {
          switch (component2.healthComponent.body.baseNameToken)
          {
            case "BEETLEGUARD_BODY_NAME":
            case "BEETLE_BODY_NAME":
              if (!characterBodyList.Contains(component2.healthComponent.body))
              {
                characterBodyList.Add(component2.healthComponent.body);
                break;
              }
              break;
          }
        }
      }
      if (characterBodyList.Count > 0)
      {
        for (int index = 0; index < characterBodyList.Count; ++index)
        {
          if (characterBodyList[index].GetBuffCount(RoR2Content.Buffs.Warbanner) <= 0)
            characterBodyList[index].AddTimedBuff(RoR2Content.Buffs.Warbanner, 15f);
        }
      }
    }
  }
}