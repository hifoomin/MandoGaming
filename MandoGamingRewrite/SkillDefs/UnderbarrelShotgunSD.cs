using EntityStates;
using RoR2;
using UnityEngine;
using MandoGamingRewrite.CustomEntityStates;

namespace MandoGaming.Skills
{
    public class UnderbarrelShotgunSD : SkillDefBase<UnderbarrelShotgunSD>
    {
        public override string NameToken => "UNDERBARRELSHOTGUN";

        public override string NameText => "Underbarrel Shotgun";

        public override string DescriptionText => "Blast off a large slug for <style=cIsDamage>800% damage</style>.";

        public override SerializableEntityStateType ActivationState => new(typeof(UnderbarrelShotgunState));

        public override string ActivationStateMachineName => "Weapon";

        public override int BaseMaxStock => 1;

        public override float BaseRechargeInterval => 6f;

        public override bool BeginSkillCooldownOnSkillEnd => false;

        public override bool CanceledFromSprinting => false;

        public override bool CancelSprintingOnActivation => true;

        public override InterruptPriority SkillInterruptPriority => InterruptPriority.Any;

        public override bool IsCombatSkill => true;

        public override bool MustKeyPress => false;

        public override int RechargeStock => 1;

        public override Sprite Icon => Main.bundle.LoadAsset<Sprite>("PlasmaTap.png");

        public override int StockToConsume => 1;

        public override string[] KeywordTokens => null;
        public override bool ResetCooldownTimerOnUse => true;
        public override int RequiredStock => 1;

        public override SkillSlot SkillSlot => SkillSlot.Utility;
        public override UnlockableDef UnlockableDef => null;
    }
}