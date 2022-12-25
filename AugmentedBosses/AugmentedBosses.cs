using BepInEx;
using BepInEx.Configuration;
using RoR2;
using RoR2.CharacterAI;
using RoR2.Skills;
using RoR2.Projectile;
using EntityStates.NullifierMonster;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

namespace AugmentedBosses
{
  [BepInPlugin("com.Nuxlar.AugmentedBosses", "AugmentedBosses", "1.0.0")]

  public class AugmentedBosses : BaseUnityPlugin
  {
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

    // Beetle Queen: deadlier poison zones, bug zones slow more?, summons only 2 BG
    // Titan: bigger X fist, maybe zappies from Auri
    // Wandering Vagrant: more movement, tesla coil-like attack?, change projectile trajectory
    // Dunestrider: fuck this piece of shit
    // Imp Overlord: throw sends out more shards, blitz is tracking, ult is faster and sends out shards?
    // Magma Worm: Add magma orb or flamethrower attack
    // Grovetender: tracking chains, faster wisps, summons greater wisps?
    // Solus Control Unit: alloy worship unit but less shield?
    // Xi Construct: i hate this fucking thing
    // Grandparent: bring back the dead abilities PortalFist, PortalJump, SpiritPull, etc...
    // Overloading Worm: Add Vagrant orb attack similar to P4 umbral wurm
    // Scavenger:
    // Void Devastator: 
    public void Awake()
    {
      new AugmentedBeetleQueen().AugmentBeetleQueen();
    }
  }
}