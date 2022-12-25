using RoR2;
using RoR2.Orbs;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates.BeetleQueenMonster;

namespace AugmentedBosses
{
  public class AugmentedSpawnWards : SpawnWards
  {
    public static float baseDuration = 0.9f;
    public static float orbRange;
    public static float orbTravelSpeed;
    public static int orbCountMax;
    private float stopwatch;
    private int orbCount;
    private float duration;
    private bool hasFiredOrbs;
    private Animator animator;
    private ChildLocator childLocator;

    public override void OnEnter()
    {
      base.OnEnter();
      this.animator = this.GetModelAnimator();
      this.childLocator = this.animator.GetComponent<ChildLocator>();
      this.duration = SpawnWards.baseDuration / this.attackSpeedStat;
      int num = (int)Util.PlayAttackSpeedSound(SpawnWards.attackSoundString, this.gameObject, this.attackSpeedStat);
      this.PlayCrossfade("Gesture", nameof(SpawnWards), "SpawnWards.playbackRate", this.duration, 0.5f);
    }

    public override void FixedUpdate()
    {
      base.FixedUpdate();
      this.stopwatch += Time.fixedDeltaTime;
      if (!this.hasFiredOrbs && (double)this.animator.GetFloat("SpawnWards.active") > 0.5)
      {
        this.hasFiredOrbs = true;
        this.FireOrbs();
      }
      if ((double)this.stopwatch < (double)this.duration || !this.isAuthority)
        return;
      this.outer.SetNextStateToMain();
    }

    private void FireOrbs()
    {
      if (!NetworkServer.active)
        return;
      Transform transform = this.childLocator.FindChild(SpawnWards.muzzleString).transform;
      BullseyeSearch bullseyeSearch = new BullseyeSearch();
      bullseyeSearch.searchOrigin = transform.position;
      bullseyeSearch.searchDirection = transform.forward;
      bullseyeSearch.maxDistanceFilter = SpawnWards.orbRange;
      bullseyeSearch.teamMaskFilter = TeamMask.allButNeutral;
      bullseyeSearch.teamMaskFilter.RemoveTeam(TeamComponent.GetObjectTeam(this.gameObject));
      bullseyeSearch.sortMode = BullseyeSearch.SortMode.Distance;
      bullseyeSearch.RefreshCandidates();
      EffectManager.SimpleMuzzleFlash(SpawnWards.muzzleflashEffectPrefab, this.gameObject, SpawnWards.muzzleString, true);
      List<HurtBox> list = bullseyeSearch.GetResults().ToList<HurtBox>();
      for (int index = 0; index < list.Count; ++index)
      {
        HurtBox hurtBox = list[index];
        BeetleWardOrb beetleWardOrb = new BeetleWardOrb();
        beetleWardOrb.origin = transform.position;
        beetleWardOrb.target = hurtBox;
        beetleWardOrb.speed = SpawnWards.orbTravelSpeed;
        OrbManager.instance.AddOrb((Orb)beetleWardOrb);
      }
    }

    public override void OnExit()
    {
      base.OnExit();
      if ((bool)(Object)this.cameraTargetParams)
        this.cameraTargetParams.fovOverride = -1f;
      int layerIndex = this.animator.GetLayerIndex("Impact");
      if (layerIndex < 0)
        return;
      this.animator.SetLayerWeight(layerIndex, 1.5f);
      this.animator.PlayInFixedTime("LightImpact", layerIndex, 0.0f);
    }
  }
}
