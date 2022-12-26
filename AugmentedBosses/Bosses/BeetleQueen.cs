using RoR2;
using RoR2.Projectile;
using RoR2.CharacterAI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AugmentedBosses
{
  public class BeetleQueen
  {
    public void ModifyAI()
    {
      GameObject gameObject = AugmentedBosses.beetleQueenMaster;
      foreach (Object component in (Component[])gameObject.GetComponents<AISkillDriver>())
        Object.Destroy(component);
      AISkillDriver aiSkillDriver1 = gameObject.AddComponent<AISkillDriver>();
      aiSkillDriver1.skillSlot = (SkillSlot)0;
      aiSkillDriver1.requireSkillReady = true;
      aiSkillDriver1.requireEquipmentReady = false;
      aiSkillDriver1.moveTargetType = (AISkillDriver.TargetType)0;
      aiSkillDriver1.minDistance = 0.0f;
      aiSkillDriver1.maxDistance = 130f;
      aiSkillDriver1.selectionRequiresTargetLoS = true;
      aiSkillDriver1.activationRequiresTargetLoS = true;
      aiSkillDriver1.activationRequiresAimConfirmation = true;
      aiSkillDriver1.movementType = (AISkillDriver.MovementType)1;
      aiSkillDriver1.aimType = (AISkillDriver.AimType)1;
      aiSkillDriver1.ignoreNodeGraph = false;
      aiSkillDriver1.driverUpdateTimerOverride = -1f;
      aiSkillDriver1.noRepeat = false;
      aiSkillDriver1.shouldSprint = false;
      aiSkillDriver1.shouldFireEquipment = false;
      aiSkillDriver1.buttonPressType = (AISkillDriver.ButtonPressType)0;
      AISkillDriver aiSkillDriver2 = gameObject.AddComponent<AISkillDriver>();
      aiSkillDriver2.skillSlot = (SkillSlot)3;
      aiSkillDriver2.requireSkillReady = true;
      aiSkillDriver2.requireEquipmentReady = false;
      aiSkillDriver2.moveTargetType = (AISkillDriver.TargetType)0;
      aiSkillDriver2.minDistance = 0.0f;
      aiSkillDriver2.maxDistance = 130f;
      aiSkillDriver2.selectionRequiresTargetLoS = false;
      aiSkillDriver2.activationRequiresTargetLoS = false;
      aiSkillDriver2.activationRequiresAimConfirmation = false;
      aiSkillDriver2.movementType = (AISkillDriver.MovementType)1;
      aiSkillDriver2.aimType = (AISkillDriver.AimType)1;
      aiSkillDriver2.ignoreNodeGraph = false;
      aiSkillDriver2.driverUpdateTimerOverride = -1f;
      aiSkillDriver2.noRepeat = true;
      aiSkillDriver2.shouldSprint = false;
      aiSkillDriver2.shouldFireEquipment = false;
      aiSkillDriver2.buttonPressType = (AISkillDriver.ButtonPressType)0;
      AISkillDriver aiSkillDriver3 = gameObject.AddComponent<AISkillDriver>();
      aiSkillDriver3.skillSlot = (SkillSlot)1;
      aiSkillDriver3.requireSkillReady = true;
      aiSkillDriver3.requireEquipmentReady = false;
      aiSkillDriver3.moveTargetType = (AISkillDriver.TargetType)0;
      aiSkillDriver3.minDistance = 0.0f;
      aiSkillDriver3.maxDistance = 80f;
      aiSkillDriver3.selectionRequiresTargetLoS = false;
      aiSkillDriver3.activationRequiresTargetLoS = false;
      aiSkillDriver3.activationRequiresAimConfirmation = false;
      aiSkillDriver3.movementType = (AISkillDriver.MovementType)3;
      aiSkillDriver3.aimType = (AISkillDriver.AimType)1;
      aiSkillDriver3.ignoreNodeGraph = false;
      aiSkillDriver3.driverUpdateTimerOverride = -1f;
      aiSkillDriver3.noRepeat = true;
      aiSkillDriver3.shouldSprint = false;
      aiSkillDriver3.shouldFireEquipment = false;
      aiSkillDriver3.buttonPressType = (AISkillDriver.ButtonPressType)0;
      AISkillDriver aiSkillDriver4 = gameObject.AddComponent<AISkillDriver>();
      aiSkillDriver4.skillSlot = (SkillSlot)(-1);
      aiSkillDriver4.requireSkillReady = false;
      aiSkillDriver4.requireEquipmentReady = false;
      aiSkillDriver4.moveTargetType = (AISkillDriver.TargetType)0;
      aiSkillDriver4.minDistance = 20f;
      aiSkillDriver4.maxDistance = float.PositiveInfinity;
      aiSkillDriver4.selectionRequiresTargetLoS = false;
      aiSkillDriver4.activationRequiresTargetLoS = false;
      aiSkillDriver4.activationRequiresAimConfirmation = false;
      aiSkillDriver4.movementType = (AISkillDriver.MovementType)1;
      aiSkillDriver4.aimType = (AISkillDriver.AimType)0;
      aiSkillDriver4.ignoreNodeGraph = false;
      aiSkillDriver4.driverUpdateTimerOverride = -1f;
      aiSkillDriver4.noRepeat = false;
      aiSkillDriver4.shouldSprint = false;
      aiSkillDriver4.shouldFireEquipment = false;
      aiSkillDriver4.buttonPressType = (AISkillDriver.ButtonPressType)0;
    }

    public void ModifyBeetles()
    {
      CharacterBody component = AugmentedBosses.beetleBody.GetComponent<CharacterBody>();
      component.baseMoveSpeed = 8f;
      component.baseAttackSpeed = 1.5f;
    }

    public void ModifyProjectile()
    {
      ProjectileImpactExplosion component1 = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Beetle/BeetleQueenSpit.prefab").WaitForCompletion().GetComponent<ProjectileImpactExplosion>();
      ((ProjectileExplosion)component1).blastRadius = 6f;
      ((ProjectileExplosion)component1).childrenDamageCoefficient = 0.1f;
      GameObject gameObject = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Beetle/BeetleQueenAcid.prefab").WaitForCompletion();
      gameObject.transform.localScale *= 2.5f;
      ProjectileDotZone component2 = gameObject.GetComponent<ProjectileDotZone>();
      component2.overlapProcCoefficient = 0.5f;
      component2.resetFrequency = 5f;
      component2.lifetime = 20f;
    }
  }
}