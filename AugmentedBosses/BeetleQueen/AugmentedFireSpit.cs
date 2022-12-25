using RoR2;
using RoR2.Projectile;
using UnityEngine;
using EntityStates;
using EntityStates.BeetleQueenMonster;

namespace AugmentedBosses
{
  public class AugmentedFireSpit : FireSpit
  {
    public new static int projectileCount = 6;
    private new Ray aimRay;
    private new float duration;

    public override void OnEnter()
    {
      string muzzleName = "Mouth";
      this.duration = baseDuration / this.attackSpeedStat;
      if ((bool)(Object)effectPrefab)
        EffectManager.SimpleMuzzleFlash(FireSpit.effectPrefab, this.gameObject, muzzleName, false);
      this.PlayCrossfade("Gesture", nameof(FireSpit), "FireSpit.playbackRate", this.duration, 0.1f);
      this.aimRay = this.GetAimRay();
      float speed = FireSpit.projectileHSpeed;
      Ray aimRay = this.aimRay with
      {
        origin = this.aimRay.GetPoint(6f)
      };
      GameObject gameObject = this.gameObject;
      Ray ray = aimRay;
      RaycastHit raycastHit;
      LayerIndex layerIndex = LayerIndex.world;
      int mask1 = (int)layerIndex.mask;
      layerIndex = LayerIndex.entityPrecise;
      int mask2 = (int)layerIndex.mask;
      LayerMask layerMask = (LayerMask)(mask1 | mask2);
      if (Util.CharacterRaycast(gameObject, ray, out raycastHit, float.PositiveInfinity, layerMask, QueryTriggerInteraction.Ignore))
      {
        float num = speed;
        Vector3 vector3_1 = raycastHit.point - this.aimRay.origin;
        Vector2 vector2 = new Vector2(vector3_1.x, vector3_1.z);
        float magnitude = vector2.magnitude;
        float initialYspeed = Trajectory.CalculateInitialYSpeed(magnitude / num, vector3_1.y);
        Vector3 vector3_2 = new Vector3(vector2.x / magnitude * num, initialYspeed, vector2.y / magnitude * num);
        speed = vector3_2.magnitude;
        this.aimRay.direction = vector3_2 / speed;
      }
      EffectManager.SimpleMuzzleFlash(FireSpit.effectPrefab, this.gameObject, muzzleName, false);
      if (!this.isAuthority)
        return;
      for (int index = 0; index < AugmentedFireSpit.projectileCount; ++index)
        this.FireBlob(this.aimRay, 0.0f, ((float)AugmentedFireSpit.projectileCount / 2f - (float)index) * FireSpit.yawSpread, speed);
    }

    private new void FireBlob(Ray aimRay, float bonusPitch, float bonusYaw, float speed)
    {
      Vector3 forward = Util.ApplySpread(aimRay.direction, FireSpit.minSpread, FireSpit.maxSpread, 1f, 1f, bonusYaw, bonusPitch);
      ProjectileManager.instance.FireProjectile(FireSpit.projectilePrefab, aimRay.origin, Util.QuaternionSafeLookRotation(forward), this.gameObject, this.damageStat * FireSpit.damageCoefficient, 0.0f, Util.CheckRoll(this.critStat, this.characterBody.master), speedOverride: speed);
    }

    public override void OnExit() => base.OnExit();

    public override void FixedUpdate()
    {
      if ((double)this.fixedAge < (double)this.duration || !this.isAuthority)
        return;
      this.outer.SetNextStateToMain();
    }

    public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Skill;
  }
}
