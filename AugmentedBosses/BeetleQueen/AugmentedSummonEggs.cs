using RoR2;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using EntityStates.BeetleQueenMonster;

namespace AugmentedBosses
{
  public class AugmentedSummonEggs : SummonEggs
  {
    public static float baseDuration = 3.5f;
    public static string attackSoundString;
    public static float randomRadius = 8f;
    public static GameObject spitPrefab;
    public static int maxSummonCount = 2;
    public static float summonInterval = 1f;
    private static float summonDuration = 3.26f;
    public static SpawnCard spawnCard;
    private Animator animator;
    private Transform modelTransform;
    private ChildLocator childLocator;
    private float duration;
    private float summonTimer;
    private int summonCount;
    private bool isSummoning;
    private BullseyeSearch enemySearch;

    public override void OnEnter()
    {
      base.OnEnter();
      this.animator = this.GetModelAnimator();
      this.modelTransform = this.GetModelTransform();
      this.childLocator = this.modelTransform.GetComponent<ChildLocator>();
      this.duration = SummonEggs.baseDuration;
      this.PlayCrossfade("Gesture", nameof(SummonEggs), 0.5f);
      int num = (int)Util.PlaySound(SummonEggs.attackSoundString, this.gameObject);
      if (!NetworkServer.active)
        return;
      this.enemySearch = new BullseyeSearch();
      this.enemySearch.filterByDistinctEntity = false;
      this.enemySearch.filterByLoS = false;
      this.enemySearch.maxDistanceFilter = float.PositiveInfinity;
      this.enemySearch.minDistanceFilter = 0.0f;
      this.enemySearch.minAngleFilter = 0.0f;
      this.enemySearch.maxAngleFilter = 180f;
      this.enemySearch.teamMaskFilter = TeamMask.GetEnemyTeams(this.GetTeam());
      this.enemySearch.sortMode = BullseyeSearch.SortMode.Distance;
      this.enemySearch.viewer = this.characterBody;
    }

    private void SummonEgg()
    {
      Vector3 vector3 = this.GetAimRay().origin;
      RaycastHit hitInfo;
      if ((bool)(UnityEngine.Object)this.inputBank && this.inputBank.GetAimRaycast(float.PositiveInfinity, out hitInfo))
        vector3 = hitInfo.point;
      if (this.enemySearch == null)
        return;
      this.enemySearch.searchOrigin = vector3;
      this.enemySearch.RefreshCandidates();
      HurtBox hurtBox = this.enemySearch.GetResults().FirstOrDefault<HurtBox>();
      Transform transform = !(bool)(UnityEngine.Object)hurtBox || !(bool)(UnityEngine.Object)hurtBox.healthComponent ? this.characterBody.coreTransform : hurtBox.healthComponent.body.coreTransform;
      if (!(bool)(UnityEngine.Object)transform)
        return;
      SpawnCard spawnCard = SummonEggs.spawnCard;
      DirectorPlacementRule placementRule = new DirectorPlacementRule();
      placementRule.placementMode = DirectorPlacementRule.PlacementMode.Approximate;
      placementRule.minDistance = 3f;
      placementRule.maxDistance = 20f;
      placementRule.spawnOnTarget = transform;
      Xoroshiro128Plus rng = RoR2Application.rng;
      DirectorSpawnRequest directorSpawnRequest = new DirectorSpawnRequest(spawnCard, placementRule, rng);
      directorSpawnRequest.summonerBodyObject = this.gameObject;
      directorSpawnRequest.onSpawnedServer += (Action<SpawnCard.SpawnResult>)(spawnResult =>
      {
        if (!spawnResult.success || !(bool)(UnityEngine.Object)spawnResult.spawnedInstance || !(bool)(UnityEngine.Object)this.characterBody)
          return;
        Inventory component = spawnResult.spawnedInstance.GetComponent<Inventory>();
        if (!(bool)(UnityEngine.Object)component)
          return;
        component.CopyEquipmentFrom(this.characterBody.inventory);
      });
      DirectorCore.instance?.TrySpawnObject(directorSpawnRequest);
    }

    public override void FixedUpdate()
    {
      base.FixedUpdate();
      bool flag = (double)this.animator.GetFloat("SummonEggs.active") > 0.899999976158142;
      if (flag && !this.isSummoning)
      {
        string muzzleName = "Mouth";
        EffectManager.SimpleMuzzleFlash(SummonEggs.spitPrefab, this.gameObject, muzzleName, false);
      }
      if (this.isSummoning)
      {
        this.summonTimer += Time.fixedDeltaTime;
        if (NetworkServer.active && (double)this.summonTimer > 0.0 && this.summonCount < SummonEggs.maxSummonCount)
        {
          ++this.summonCount;
          this.summonTimer -= SummonEggs.summonInterval;
          this.SummonEgg();
        }
      }
      this.isSummoning = flag;
      if ((double)this.fixedAge < (double)this.duration || !this.isAuthority)
        return;
      this.outer.SetNextStateToMain();
    }

    public override void OnExit() => base.OnExit();

    public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.PrioritySkill;
  }
}
