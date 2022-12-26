using RoR2;
using RoR2.Projectile;
using RoR2.CharacterAI;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace AugmentedBosses
{
  public class WanderingVagrant
  {
    private static GameObject cannonGhost = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Vagrant/VagrantCannonGhost.prefab").WaitForCompletion();
    private static Material cannonBlue = Addressables.LoadAssetAsync<Material>("RoR2/Base/Vagrant/matVagrantCannonBlue.mat").WaitForCompletion();
    private static Material cannonGreen = Addressables.LoadAssetAsync<Material>("RoR2/Base/Vagrant/matVagrantCannonGreen.mat").WaitForCompletion();
    private static Material cannonRed = Addressables.LoadAssetAsync<Material>("RoR2/Base/Vagrant/matVagrantCannonRed.mat").WaitForCompletion();


  }
}