using EntityStates;
using RoR2;
using UnityEngine;
using MandoGamingRewrite.EntityStates;

namespace MandoGaming.Skills
{
    public class PlasmaTapSD : SkillDefBase
    {
        public override string NameToken => "PLASMATAP";

        public override string NameText => "Plasma Tap";

        public override string DescriptionText => "<style=cIsDamage>Arcing</style>. Fire a burst of lightning that deals <style=cIsDamage>100% damage</style> in a cone.";

        public override SerializableEntityStateType ActivationState => new(typeof(PlasmaTapState));

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

        public override Sprite Icon => Main.mandogaming.LoadAsset<Sprite>("PlasmaTap.png");

        public override int StockToConsume => 0;

        public override string[] KeywordTokens => new string[] { "KEYWORD_ARC" };
        public override bool ResetCooldownTimerOnUse => true;
        public override int RequiredStock => 1;

        public override SkillSlot SkillSlot => SkillSlot.Primary;
    }
}