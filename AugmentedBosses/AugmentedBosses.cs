using BepInEx;
using RoR2;
using RoR2.CharacterAI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Linq;

namespace AugmentedBosses
{
  [BepInPlugin("com.Nuxlar.AugmentedBosses", "AugmentedBosses", "0.5.1")]

  public class AugmentedBosses : BaseUnityPlugin
  {
    public static GameObject dunestrider = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ClayBoss/ClayBossBody.prefab").WaitForCompletion();
    public static GameObject impBoss = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ImpBoss/ImpBossBody.prefab").WaitForCompletion();
    public static GameObject grovetender = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Gravekeeper/GravekeeperBody.prefab").WaitForCompletion();

    public static GameObject xiConstruct = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/MajorAndMinorConstruct/MegaConstructBody.prefab").WaitForCompletion();
    public static GameObject grandparent = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Grandparent/GrandParentBody.prefab").WaitForCompletion();
    public static GameObject overloadingWorm = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/ElectricWorm/ElectricWormBody.prefab").WaitForCompletion();
    public static GameObject scavenger = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Scav/ScavBody.prefab").WaitForCompletion();
    public static GameObject devastator = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/VoidMegaCrab/VoidMegaCrabBody.prefab").WaitForCompletion();

    public void Awake()
    {
      // Misc Hooks
      On.RoR2.HealthComponent.TakeDamage += TakeDamage;
      On.RoR2.CharacterBody.RecalculateStats += RecalculateStats;
      CharacterMaster.onStartGlobal += MasterChanges;
      On.RoR2.UI.CombatHealthBarViewer.HandleDamage += HandleDamage;
      On.RoR2.CharacterMaster.OnBodyStart += OnBodyStart;
      // Boss Tweaks
      new BeetleQueen();
      new WanderingVagrant();
      new StoneTitan();
      new MagmaWorm();
      new ImpOverlord();
      new SolusControlUnit();
    }

    private void HandleDamage(On.RoR2.UI.CombatHealthBarViewer.orig_HandleDamage orig, RoR2.UI.CombatHealthBarViewer self, HealthComponent victimHealthComponent, TeamIndex victimTeam)
    {
      if (victimHealthComponent.name.StartsWith("Vagrant"))
        self.GetHealthBarInfo(victimHealthComponent).endTime = float.PositiveInfinity;
      else orig(self, victimHealthComponent, victimTeam);
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
          damageInfo.damage = damageInfo.damage / 2;
        if (self.health > half && self.health < threeQuarters && healthAfterDmg < half)
        {
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

    private void RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
    {
      orig(self);
      self.armor -= 5f * (float)self.GetBuffCount(RoR2Content.Buffs.BeetleJuice);
    }

  }
}