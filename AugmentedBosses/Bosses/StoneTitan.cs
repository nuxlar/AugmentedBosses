using RoR2;
using RoR2.Projectile;
using RoR2.CharacterAI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using EntityStates;
using EntityStates.TitanMonster;

namespace AugmentedBosses
{
  public class StoneTitan
  {
    // Accel 20
    // Base Move speed 5
    // Level Move Speed 0
    public void ModifyStats()
    {
      AugmentedBosses.titan.transform.localScale = new Vector3(1, 1, 1);
      CharacterBody titanBody = AugmentedBosses.titan.GetComponent<CharacterBody>();
      SkillLocator titanSkillLocator = AugmentedBosses.titan.GetComponent<SkillLocator>();
      titanSkillLocator.special.skillFamily.variants[0].skillDef.activationState = new SerializableEntityStateType(typeof(ChargeGoldMegaLaser));
      titanBody.baseAttackSpeed = 1.25f;
      titanBody.baseAcceleration = 30f;
      titanBody.baseMoveSpeed = 7f;
      titanBody.levelMoveSpeed = 0.25f;
    }
  }
}