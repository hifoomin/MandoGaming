using EntityStates;
using R2API;
using RoR2;
using RoR2.Skills;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MandoGaming
{
    public abstract class SkillDefBase
    {
        public abstract string NameToken { get; }
        public abstract string NameText { get; }
        public abstract string DescriptionText { get; }
        public abstract SerializableEntityStateType ActivationState { get; }
        public abstract string ActivationStateMachineName { get; }
        public abstract int BaseMaxStock { get; }
        public abstract float BaseRechargeInterval { get; }
        public abstract bool BeginSkillCooldownOnSkillEnd { get; }
        public abstract bool CanceledFromSprinting { get; }
        public abstract bool CancelSprintingOnActivation { get; }
        public virtual bool FullRestockOnAssign { get; } = true;
        public abstract InterruptPriority SkillInterruptPriority { get; }
        public abstract bool IsCombatSkill { get; }
        public abstract bool MustKeyPress { get; }
        public abstract int RechargeStock { get; }
        public abstract Sprite Icon { get; }
        public abstract int StockToConsume { get; }
        public abstract string[] KeywordTokens { get; }
        public abstract bool ResetCooldownTimerOnUse { get; }
        public abstract int RequiredStock { get; }
        public abstract SkillSlot SkillSlot { get; }

        public virtual bool isEnabled { get; } = true;
        public abstract UnlockableDef UnlockableDef { get; }
        public virtual bool isStepped { get; } = false;
        public virtual int stepCount { get; } = 2;
        public virtual float stepGraceDuration { get; } = 1f;

        public SkillDef skillDef;
        public SteppedSkillDef steppedSkillDef;

        public SkillLocator commandoSkillLocator = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Commando/CommandoBody.prefab").WaitForCompletion().GetComponent<SkillLocator>();

        public T ConfigOption<T>(T value, string name, string description)
        {
            return Main.MGConfig.Bind<T>(NameText, name, value, description).Value;
        }

        public string d(float f)
        {
            return (f * 100f).ToString() + "%";
        }

        public virtual void Init()
        {
            string nameToken = "COMMANDO_" + NameToken.ToUpper() + "_NAME";
            string descriptionToken = "COMMANDO_" + NameToken.ToUpper() + "_DESCRIPTION";
            LanguageAPI.Add(nameToken, NameText);
            LanguageAPI.Add(descriptionToken, DescriptionText);

            if (!isStepped)
            {
                skillDef = ScriptableObject.CreateInstance<SkillDef>();

                skillDef.skillNameToken = nameToken;
                skillDef.skillDescriptionToken = descriptionToken;
                skillDef.activationState = ActivationState;
                skillDef.activationStateMachineName = ActivationStateMachineName;
                skillDef.baseMaxStock = BaseMaxStock;
                skillDef.baseRechargeInterval = BaseRechargeInterval;
                skillDef.beginSkillCooldownOnSkillEnd = BeginSkillCooldownOnSkillEnd;
                skillDef.canceledFromSprinting = CanceledFromSprinting;
                skillDef.cancelSprintingOnActivation = CancelSprintingOnActivation;
                skillDef.fullRestockOnAssign = FullRestockOnAssign;
                skillDef.interruptPriority = SkillInterruptPriority;
                skillDef.isCombatSkill = IsCombatSkill;
                skillDef.mustKeyPress = MustKeyPress;
                skillDef.rechargeStock = RechargeStock;
                skillDef.icon = Icon;
                skillDef.stockToConsume = StockToConsume;
                skillDef.keywordTokens = KeywordTokens;
                skillDef.resetCooldownTimerOnUse = ResetCooldownTimerOnUse;
                skillDef.requiredStock = RequiredStock;

                ContentAddition.AddSkillDef(skillDef);

                var skillFamily = commandoSkillLocator.GetSkill(SkillSlot).skillFamily;
                Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
                skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
                {
                    skillDef = skillDef,
                    unlockableDef = UnlockableDef,
                    viewableNode = new ViewablesCatalog.Node(skillDef.skillNameToken, false, null)
                };
            }
            else
            {
                steppedSkillDef = ScriptableObject.CreateInstance<SteppedSkillDef>();

                steppedSkillDef.skillNameToken = nameToken;
                steppedSkillDef.skillDescriptionToken = descriptionToken;
                steppedSkillDef.activationState = ActivationState;
                steppedSkillDef.activationStateMachineName = ActivationStateMachineName;
                steppedSkillDef.baseMaxStock = BaseMaxStock;
                steppedSkillDef.baseRechargeInterval = BaseRechargeInterval;
                steppedSkillDef.beginSkillCooldownOnSkillEnd = BeginSkillCooldownOnSkillEnd;
                steppedSkillDef.canceledFromSprinting = CanceledFromSprinting;
                steppedSkillDef.cancelSprintingOnActivation = CancelSprintingOnActivation;
                steppedSkillDef.fullRestockOnAssign = FullRestockOnAssign;
                steppedSkillDef.interruptPriority = SkillInterruptPriority;
                steppedSkillDef.isCombatSkill = IsCombatSkill;
                steppedSkillDef.mustKeyPress = MustKeyPress;
                steppedSkillDef.rechargeStock = RechargeStock;
                steppedSkillDef.icon = Icon;
                steppedSkillDef.stockToConsume = StockToConsume;
                steppedSkillDef.keywordTokens = KeywordTokens;
                steppedSkillDef.resetCooldownTimerOnUse = ResetCooldownTimerOnUse;
                steppedSkillDef.requiredStock = RequiredStock;
                steppedSkillDef.stepCount = stepCount;
                steppedSkillDef.stepGraceDuration = stepGraceDuration;

                ContentAddition.AddSkillDef(steppedSkillDef);

                var skillFamily = commandoSkillLocator.GetSkill(SkillSlot).skillFamily;
                Array.Resize(ref skillFamily.variants, skillFamily.variants.Length + 1);
                skillFamily.variants[skillFamily.variants.Length - 1] = new SkillFamily.Variant
                {
                    skillDef = steppedSkillDef,
                    unlockableDef = UnlockableDef,
                    viewableNode = new ViewablesCatalog.Node(steppedSkillDef.skillNameToken, false, null)
                };
            }

            // Main.MandoGamingLogger.LogFatal("public override UnlockableDef UnlockableDef => Unlocks.heavyTap is " + UnlockableDef);

            /*
            var networkStateMachine = commandoSkillLocator.gameObject.GetComponent<NetworkStateMachine>();

            Array.Resize(ref networkStateMachine.stateMachines, networkStateMachine.stateMachines.Length + 1);
            networkStateMachine.stateMachines[networkStateMachine.stateMachines.Length - 1] = EntityStateMachine;
            */

            Main.MGLogger.LogInfo("Added " + NameText);
        }
    }
}