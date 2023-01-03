using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;
using EntityStates;
using EntityStates.RoboBallBoss.Weapon;

namespace AugmentedBosses
{
  public class SolusControlUnit
  {
    // accel 14
    // movespd 7
    // dmg 15
    // lvl dmg 3
    // FireSuperDelayKnockup
    public static GameObject solusControlUnit = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/RoboBallBoss/RoboBallBossBody.prefab").WaitForCompletion();

    public SolusControlUnit()
    {
      ModifyStats();
      On.EntityStates.RoboBallBoss.Weapon.EnableEyebeams.OnEnter += EnableEyebeams_OnEnter;
      On.EntityStates.RoboBallBoss.Weapon.FireSpinningEyeBeam.GetLaserRay += FireSpinningEyeBeam_GetLaserRay;
      On.EntityStates.RoboBallBoss.Weapon.FireDelayKnockup.OnEnter += FireDelayKnockup_OnEnter;
    }

    private void ModifyStats()
    {
      CharacterBody solusBody = solusControlUnit.GetComponent<CharacterBody>();
      SkillLocator solusSkillLocator = solusControlUnit.GetComponent<SkillLocator>();
      solusBody.baseAcceleration = 25;
      solusBody.baseMoveSpeed = 10;
      solusSkillLocator.utility.skillFamily.variants[0].skillDef.activationState = new SerializableEntityStateType(typeof(EnableEyebeams));
    }

    private Ray FireSpinningEyeBeam_GetLaserRay(On.EntityStates.RoboBallBoss.Weapon.FireSpinningEyeBeam.orig_GetLaserRay orig, EntityStates.RoboBallBoss.Weapon.FireSpinningEyeBeam self)
    {
      Ray laserRay = new Ray();
      if ((bool)(Object)self.eyeBeamOriginTransform)
      {
        laserRay.origin = self.eyeBeamOriginTransform.position;
        laserRay.direction = self.GetAimRay().direction;
      }
      return laserRay;
    }

    private void EnableEyebeams_OnEnter(On.EntityStates.RoboBallBoss.Weapon.EnableEyebeams.orig_OnEnter orig, EntityStates.RoboBallBoss.Weapon.EnableEyebeams self)
    {
      self.duration = EnableEyebeams.baseDuration / self.attackSpeedStat;
      int num = (int)Util.PlaySound(EnableEyebeams.soundString, self.gameObject);
      EntityStateMachine[] components = self.gameObject.GetComponents<EntityStateMachine>();
      int counter = 1;
      for (int i = 0; i < components.Length; i++)
      {
        if (components[i].customName.Contains("EyeBeam") || components[i].customName.Contains("EyeballMuzzle"))
        {
          components[i].customName = "EyeballMuzzle" + counter.ToString();
          counter++;
          components[i].SetNextState((EntityState)new FireSpinningEyeBeam());
        }
      }
    }

    private void FireDelayKnockup_OnEnter(On.EntityStates.RoboBallBoss.Weapon.FireDelayKnockup.orig_OnEnter orig, EntityStates.RoboBallBoss.Weapon.FireDelayKnockup self)
    {
      if (self.characterBody.name == "RoboBallBossBody(Clone)")
      {
        self.characterBody.AddTimedBuff(RoR2Content.Buffs.ArmorBoost, 5);
        self.knockupCount = 3;
        self.randomPositionRadius = 30;
      }
      orig(self);
    }
  }
}