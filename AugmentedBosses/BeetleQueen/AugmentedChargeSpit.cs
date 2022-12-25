using RoR2;
using UnityEngine;
using EntityStates;
using EntityStates.BeetleQueenMonster;

namespace AugmentedBosses
{
  public class AugmentedChargeSpit : ChargeSpit
  {
    public override void OnEnter()
    {
      this.duration = ChargeSpit.baseDuration / this.attackSpeedStat;
      Transform modelTransform = this.GetModelTransform();
      this.PlayCrossfade("Gesture", nameof(ChargeSpit), "ChargeSpit.playbackRate", this.duration, 0.2f);
      int num = (int)Util.PlaySound(ChargeSpit.attackSoundString, this.gameObject);
      if (!(bool)(Object)modelTransform)
        return;
      ChildLocator component1 = modelTransform.GetComponent<ChildLocator>();
      if (!(bool)(Object)component1 || !(bool)(Object)ChargeSpit.effectPrefab)
        return;
      Transform child = component1.FindChild("Mouth");
      if (!(bool)(Object)child)
        return;
      this.chargeEffect = Object.Instantiate<GameObject>(ChargeSpit.effectPrefab, child.position, child.rotation);
      this.chargeEffect.transform.parent = child;
      ScaleParticleSystemDuration component2 = this.chargeEffect.GetComponent<ScaleParticleSystemDuration>();
      if (!(bool)(Object)component2)
        return;
      component2.newDuration = this.duration;
    }

    public override void OnExit()
    {
      base.OnExit();
      EntityState.Destroy((Object)this.chargeEffect);
    }

    public override void Update() => base.Update();

    public override void FixedUpdate()
    {
      if ((bool)(Object)this.characterDirection)
        this.characterDirection.moveVector = this.GetAimRay().direction;
      if ((double)this.fixedAge < (double)this.duration || !this.isAuthority)
        return;
      this.outer.SetNextState((EntityState)new AugmentedFireSpit());
    }

    public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Skill;
  }
}