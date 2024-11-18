using EntityStates;
using MandoGamingRewrite.CustomEntityStates;
using RoR2;
using UnityEngine;

namespace MandoGaming.Skills
{
    public class ParadigmHandgunSD : SkillDefBase<ParadigmHandgunSD>
    {
        public override string NameToken => "PARADIGMHANDGUN";

        public override string NameText => "Paradigm Handgun";

        public override string DescriptionText => "Fire off a spread of <style=cIsDamage>4</style> slugs for <style=cIsDamage>200% damage</style> each.";

        public override SerializableEntityStateType ActivationState => new(typeof(ParadigmHandgunState));

        public override string ActivationStateMachineName => "Weapon";

        public override int BaseMaxStock => 1;

        public override float BaseRechargeInterval => 1f;

        public override bool BeginSkillCooldownOnSkillEnd => false;

        public override bool CanceledFromSprinting => false;

        public override bool CancelSprintingOnActivation => true;

        public override InterruptPriority SkillInterruptPriority => InterruptPriority.Any;

        public override bool IsCombatSkill => true;

        public override bool MustKeyPress => false;

        public override int RechargeStock => 1;

        public override Sprite Icon => Main.bundle.LoadAsset<Sprite>("PlasmaTap.png");

        public override int StockToConsume => 0;

        public override string[] KeywordTokens => null;
        public override bool ResetCooldownTimerOnUse => true;
        public override int RequiredStock => 1;

        public override SkillSlot SkillSlot => SkillSlot.Primary;
        public override UnlockableDef UnlockableDef => null;
    }
}