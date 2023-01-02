using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AugmentedBosses
{
  public class WanderingVagrant
  {
    private static GameObject vagrant = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Vagrant/VagrantBody.prefab").WaitForCompletion();
    private static Material cannonRed = Addressables.LoadAssetAsync<Material>("RoR2/Base/Vagrant/matVagrantCannonRed.mat").WaitForCompletion();
    private static GameObject cannonGhost = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Vagrant/VagrantCannonGhost.prefab").WaitForCompletion();

    public WanderingVagrant()
    {
      ModifyStats();
      cannonGhost.transform.GetChild(1).GetComponent<MeshRenderer>().material = cannonRed;
      vagrant.transform.GetChild(0).GetChild(0).GetChild(3).GetComponent<SkinnedMeshRenderer>().material = cannonRed;
    }

    private void ModifyStats()
    {
      CharacterBody vagrantBody = vagrant.GetComponent<CharacterBody>();
      vagrantBody.baseAcceleration = 20f; // Vanilla 15
      vagrantBody.baseMoveSpeed = 8f; // Vanilla 6
      vagrantBody.baseAttackSpeed = 1.25f;
      vagrantBody.baseDamage = 4;
    }
  }
}