using EntityStates;
using RoR2;
using UnityEngine;
using MandoGamingRewrite.EntityStates;
using MandoGamingRewrite.Unlocks;

namespace MandoGaming.Skills
{
    public class HeavyTapSD : SkillDefBase
    {
        public override string NameToken => "HEAVYTAP";

        public override string NameText => "Heavy Tap";

        public override string DescriptionText => "<style=cIsDamage>Frictionless</style>. Shoot twice for <style=cIsDamage>2x155% damage</style>.";

        public override SerializableEntityStateType ActivationState => new(typeof(HeavyTapState));

        public override string ActivationStateMachineName => "Weapon";

        public override int BaseMaxStock => 1;

        public override float BaseRechargeInterval => 1f;

        public override bool BeginSkillCooldownOnSkillEnd => true;

        public override bool CanceledFromSprinting => false;

        public override bool CancelSprintingOnActivation => true;

        public override InterruptPriority SkillInterruptPriority => InterruptPriority.Any;

        public override bool IsCombatSkill => true;

        public override bool MustKeyPress => false;

        public override int RechargeStock => 0;

        public override Sprite Icon => Main.mandogaming.LoadAsset<Sprite>("HeavyTap.png");

        public override int StockToConsume => 0;

        public override string[] KeywordTokens => new string[] { "KEYWORD_FRICTIONLESS" };
        public override bool ResetCooldownTimerOnUse => true;
        public override int RequiredStock => 1;

        public override SkillSlot SkillSlot => SkillSlot.Primary;
        public override UnlockableDef UnlockableDef => Unlocks.heavyTap;
    }
}