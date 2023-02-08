using MandoGaming;
using R2API;
using RoR2;
using RoR2.Achievements;
using UnityEngine;

namespace MandoGamingRewrite.Unlocks
{
    public class HeavyTapAchievement : BaseAchievement
    {
        private int primaryUseCount;

        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        private void CharacterBody_OnSkillActivated(On.RoR2.CharacterBody.orig_OnSkillActivated orig, CharacterBody self, GenericSkill skill)
        {
            if (localUser.cachedBody.bodyIndex == LookUpRequiredBodyIndex() && localUser.cachedBody.teamComponent.teamIndex == TeamIndex.Player)
            {
                if (skill != localUser.cachedBody.skillLocator.primary)
                {
                    if (primaryUseCount == 0)
                    {
                        primaryUseCount++;
                    }
                }
            }

            orig(self, skill);
        }

        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        private void Run_onRunStartGlobal(Run obj)
        {
            primaryUseCount = 0;
        }

        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        private void TeleporterInteraction_onTeleporterChargedGlobal(TeleporterInteraction obj)
        {
            if (primaryUseCount == 0)
            {
                Grant();
            }
        }

        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override void OnInstall()
        {
            base.OnInstall();
            On.RoR2.CharacterBody.OnSkillActivated += CharacterBody_OnSkillActivated;
            TeleporterInteraction.onTeleporterChargedGlobal += TeleporterInteraction_onTeleporterChargedGlobal;
            Run.onRunStartGlobal += Run_onRunStartGlobal;
        }

        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override void OnUninstall()
        {
            base.OnUninstall();
            On.RoR2.CharacterBody.OnSkillActivated -= CharacterBody_OnSkillActivated;
            TeleporterInteraction.onTeleporterChargedGlobal -= TeleporterInteraction_onTeleporterChargedGlobal;
            Run.onRunStartGlobal -= Run_onRunStartGlobal;
        }
    }

    public class PlasmaTapAchievement : BaseAchievement
    {
        private float zapCount;

        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override void OnInstall()
        {
            base.OnInstall();
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            Run.onRunStartGlobal += Run_onRunStartGlobal;
        }

        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        private void Run_onRunStartGlobal(Run obj)
        {
            zapCount = 0;
        }

        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        private void GlobalEventManager_onServerDamageDealt(DamageReport damageReport)
        {
            var damageInfo = damageReport.damageInfo;
            var attackerBody = damageReport.attackerBody;
            if (attackerBody && localUser.cachedBody.bodyIndex == LookUpRequiredBodyIndex())
            {
                Main.MandoGamingLogger.LogFatal("attackerBody exists and localUser.cachedBody.bodyIndex is Commando");
                if (attackerBody == localUser.cachedBody)
                {
                    Main.MandoGamingLogger.LogFatal("attackerBody is localUser.cachedBody");
                    if (damageInfo.procChainMask.HasProc(ProcType.ChainLightning))
                    {
                        Main.MandoGamingLogger.LogFatal("added to zap count");
                        zapCount++;
                    }
                    if (zapCount >= 100)
                    {
                        Grant();
                    }
                }
            }
        }

        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override void OnUninstall()
        {
            base.OnUninstall();
            GlobalEventManager.onServerDamageDealt -= GlobalEventManager_onServerDamageDealt;
            Run.onRunStartGlobal -= Run_onRunStartGlobal;
        }
    }

    [RegisterAchievement("CommandoHeavyTap", "Commando.Skills_HeavyTap", null, null)]
    public class CommandoHeavyTapAchievement : HeavyTapAchievement
    {
        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("CommandoBody");
        }
    }

    [RegisterAchievement("CommandoPlasmaTap", "Commando.Skills_PlasmaTap", null, null)]
    public class CommandoPlasmaTapAchievement : PlasmaTapAchievement
    {
        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override BodyIndex LookUpRequiredBodyIndex()
        {
            return BodyCatalog.FindBodyIndex("CommandoBody");
        }
    }

    public static class Unlocks
    {
        public static UnlockableDef heavyTap;
        public static UnlockableDef plasmaTap;

        public static void Create()
        {
            heavyTap = ScriptableObject.CreateInstance<UnlockableDef>();
            heavyTap.achievementIcon = Main.mandogaming.LoadAsset<Sprite>("HeavyTap.png");
            heavyTap.cachedName = "Commando.Skills_HeavyTap";
            heavyTap.nameToken = "ACHIEVEMENT_COMMANDOHEAVYTAP_NAME";

            LanguageAPI.Add("ACHIEVEMENT_COMMANDOHEAVYTAP_NAME", "Commando: Still Here");
            LanguageAPI.Add("ACHIEVEMENT_COMMANDOHEAVYTAP_DESCRIPTION", "As Commando, complete a Teleporter Event without using your Primary skill.");

            plasmaTap = ScriptableObject.CreateInstance<UnlockableDef>();
            plasmaTap.achievementIcon = Main.mandogaming.LoadAsset<Sprite>("PlasmaTap.png");
            plasmaTap.cachedName = "Commando.Skills_PlasmaTap";
            plasmaTap.nameToken = "ACHIEVEMENT_COMMANDOPLASMATAP_NAME";

            LanguageAPI.Add("ACHIEVEMENT_COMMANDOPLASMATAP_NAME", "Commando: Flatline");
            LanguageAPI.Add("ACHIEVEMENT_COMMANDOPLASMATAP_DESCRIPTION", "As Commando, zap enemies with chain lightning 100 times in a single run.");

            Main.MandoGamingLogger.LogFatal("public static unlockableDef heavyTap in Unlocks class is " + heavyTap);
            Main.MandoGamingLogger.LogFatal("public static unlockableDef plasmaTap in Unlocks class is " + plasmaTap);
        }
    }
}