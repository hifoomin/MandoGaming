using EntityStates;
using RoR2;
using UnityEngine;
using MandoGamingRewrite.CustomEntityStates;

namespace MandoGaming.Skills
{
    public class PointBlankSD : SkillDefBase<PointBlankSD>
    {
        public override string NameToken => "POINTBLANK";

        public override string NameText => "Point-Blank";

        public override string DescriptionText => "Fire <style=cIsDamage>4</style> quick bursts of <style=cIsDamage>piercing</style> shells for <style=cIsDamage>5x90%</style> damage each.";

        public override SerializableEntityStateType ActivationState => new(typeof(PointBlankState));

        public override string ActivationStateMachineName => "Weapon";

        public override int BaseMaxStock => 1;

        public override float BaseRechargeInterval => 7f;

        public override bool BeginSkillCooldownOnSkillEnd => false;

        public override bool CanceledFromSprinting => false;

        public override bool CancelSprintingOnActivation => true;

        public override InterruptPriority SkillInterruptPriority => InterruptPriority.Any;

        public override bool IsCombatSkill => true;

        public override bool MustKeyPress => false;

        public override int RechargeStock => 1;

        public override Sprite Icon => Main.bundle.LoadAsset<Sprite>("HeavyTap.png");

        public override int StockToConsume => 1;

        public override string[] KeywordTokens => null;
        public override bool ResetCooldownTimerOnUse => true;
        public override int RequiredStock => 1;

        public override SkillSlot SkillSlot => SkillSlot.Special;
        public override UnlockableDef UnlockableDef => null;
        public override bool isStepped => true;
    }
}