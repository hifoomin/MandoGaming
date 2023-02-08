using EntityStates;
using RoR2;
using UnityEngine;
using MandoGamingRewrite.EntityStates;
using MandoGamingRewrite.Unlocks;

namespace MandoGaming.Skills
{
    public class PRFRVWildfireStormSD : SkillDefBase
    {
        public override string NameToken => "PRFRVWILDFIRESTORM";

        public override string NameText => "PRFR-V Wildfire Storm";

        public override string DescriptionText => "Fire a continuous stream of flame that deals <style=cIsDamage>550% damage</style> per second and has a chance to <style=cIsDamage>ignite</style> enemies.";

        public override SerializableEntityStateType ActivationState => new(typeof(PRFRVWildfireStormState));

        public override string ActivationStateMachineName => "Weapon";

        public override int BaseMaxStock => 1;

        public override float BaseRechargeInterval => 4f;

        public override bool BeginSkillCooldownOnSkillEnd => false;

        public override bool CanceledFromSprinting => false;

        public override bool CancelSprintingOnActivation => true;

        public override InterruptPriority SkillInterruptPriority => InterruptPriority.Any;

        public override bool IsCombatSkill => true;

        public override bool MustKeyPress => true;

        public override int RechargeStock => 1;

        public override Sprite Icon => Main.mandogaming.LoadAsset<Sprite>("PRFRVWildfireStorm.png");

        public override int StockToConsume => 1;

        public override string[] KeywordTokens => null;
        public override bool ResetCooldownTimerOnUse => false;
        public override int RequiredStock => 1;

        public override SkillSlot SkillSlot => SkillSlot.Secondary;
        public override UnlockableDef UnlockableDef => Unlocks.prfrVWildfireStorm;
    }
}