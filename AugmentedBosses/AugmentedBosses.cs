using BepInEx;
using BepInEx.Configuration;
using EntityStates;
using EntityStates.BeetleQueenMonster;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Skills;
using RoR2.Projectile;
using EntityStates.NullifierMonster;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AddressableAssets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AugmentedBosses
{
  [BepInPlugin("com.Nuxlar.AugmentedBosses", "AugmentedBosses", "1.0.0")]

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
    private static Material cannonGreen = Addressables.LoadAssetAsync<Material>("RoR2/Base/Vagrant/matVagrantCannonGreen.mat").WaitForCompletion();
    private static Material cannonRed = Addressables.LoadAssetAsync<Material>("RoR2/Base/Vagrant/matVagrantCannonRed.mat").WaitForCompletion();

    public void Awake()
    {
      // Beetle Queen
      On.RoR2.CharacterBody.RecalculateStats += RecalculateStats;
      On.EntityStates.BeetleQueenMonster.SummonEggs.SummonEgg += SummonEgg;
      BeetleQueen beetleQueen = new();
      beetleQueen.ModifyAI();
      beetleQueen.ModifyBeetles();
      beetleQueen.ModifyProjectile();
      // Wandering Vagrant
      cannonGhost.transform.GetChild(1).GetComponent<MeshRenderer>().material = cannonRed;
    }

    private void RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
    {
      orig.Invoke(self);
      self.armor -= 5f * (float)self.GetBuffCount(RoR2Content.Buffs.BeetleJuice);
    }

    private void SummonEgg(On.EntityStates.BeetleQueenMonster.SummonEggs.orig_SummonEgg orig, SummonEggs self)
    {
      orig.Invoke(self);
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