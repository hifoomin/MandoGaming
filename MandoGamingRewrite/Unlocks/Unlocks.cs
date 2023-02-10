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
            if (localUser.cachedBody.name == "CommandoBody(Clone)" && localUser.cachedBody.teamComponent.teamIndex == TeamIndex.Player)
            {
                // Main.MandoGamingLogger.LogFatal("OnSkillActivated cachedBody name is CommandoBody(Clone)");
                if (skill == localUser.cachedBody.skillLocator.primary)
                {
                    // Main.MandoGamingLogger.LogFatal("Added primary skill usage");
                    primaryUseCount++;
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
        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            On.RoR2.CharacterBody.OnSkillActivated += CharacterBody_OnSkillActivated;
            TeleporterInteraction.onTeleporterChargedGlobal += TeleporterInteraction_onTeleporterChargedGlobal;
            Run.onRunStartGlobal += Run_onRunStartGlobal;
            Stage.onServerStageBegin += Stage_onServerStageBegin;
        }

        private void Stage_onServerStageBegin(Stage obj)
        {
            primaryUseCount = 0;
        }

        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override void OnBodyRequirementBroken()
        {
            base.OnBodyRequirementBroken();
            On.RoR2.CharacterBody.OnSkillActivated -= CharacterBody_OnSkillActivated;
            TeleporterInteraction.onTeleporterChargedGlobal -= TeleporterInteraction_onTeleporterChargedGlobal;
            Run.onRunStartGlobal -= Run_onRunStartGlobal;
            Stage.onServerStageBegin -= Stage_onServerStageBegin;
        }
    }

    public class PlasmaTapAchievement : BaseAchievement
    {
        private float zapCount;

        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
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
            if (attackerBody && attackerBody.name == "CommandoBody(Clone)" && localUser.cachedBody.name == "CommandoBody(Clone)")
            {
                // Main.MandoGamingLogger.LogFatal("CHAIN LIGHTNING OnServerDamageDealt attackerBody.name and cachedBody name is CommandoBody(clone)");
                if (damageInfo.procChainMask.HasProc(ProcType.ChainLightning))
                {
                    // Main.MandoGamingLogger.LogFatal("added to zap count");
                    zapCount++;
                }
                if (zapCount >= 70)
                {
                    Grant();
                }
            }
        }

        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override void OnBodyRequirementBroken()
        {
            base.OnBodyRequirementBroken();
            GlobalEventManager.onServerDamageDealt -= GlobalEventManager_onServerDamageDealt;
            Run.onRunStartGlobal -= Run_onRunStartGlobal;
        }
    }

    public class PRFRVWildfireStormAchievement : BaseAchievement
    {
        private float igniteCount;

        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            On.RoR2.GlobalEventManager.ProcIgniteOnKill += GlobalEventManager_ProcIgniteOnKill;
            Run.onRunStartGlobal += Run_onRunStartGlobal;
            Stage.onServerStageBegin += Stage_onServerStageBegin;
        }

        private void GlobalEventManager_ProcIgniteOnKill(On.RoR2.GlobalEventManager.orig_ProcIgniteOnKill orig, DamageReport damageReport, int igniteOnKillCount, CharacterBody victimBody, TeamIndex attackerTeamIndex)
        {
            var attackerBody = damageReport.attackerBody;
            if (attackerBody && attackerBody.name == "CommandoBody(Clone)" && localUser.cachedBody.name == "CommandoBody(Clone)")
            {
                // Main.MandoGamingLogger.LogFatal("ProcIgniteOnKill attackerBody.name and cachedBody name is CommandoBodY(cloenkeo)");
                if (igniteOnKillCount > 0)
                {
                    igniteCount++;
                }
                if (igniteCount >= 30)
                {
                    Grant();
                }
            }

            orig(damageReport, igniteOnKillCount, victimBody, attackerTeamIndex);
        }

        private void Stage_onServerStageBegin(Stage obj)
        {
            igniteCount = 0;
        }

        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        private void Run_onRunStartGlobal(Run obj)
        {
            igniteCount = 0;
        }

        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        private void GlobalEventManager_onServerDamageDealt(DamageReport damageReport)
        {
            var damageInfo = damageReport.damageInfo;
            var attackerBody = damageReport.attackerBody;
            if (attackerBody && attackerBody.name == "CommandoBody(Clone)" && localUser.cachedBody.name == "CommandoBody(Clone)")
            {
                // Main.MandoGamingLogger.LogFatal("IGNITE ON HIT OnServerDamageDealt attackerBody.name and cachedBody name is CommandoBody(clone)");
                if ((damageInfo.damageType & DamageType.IgniteOnHit) == DamageType.IgniteOnHit)
                {
                    // Main.MandoGamingLogger.LogFatal("Added to ServerDamageDealt igniteCount");
                    igniteCount++;
                }
                if (igniteCount >= 30)
                {
                    Grant();
                }
            }
        }

        [SystemInitializer(typeof(HG.Reflection.SearchableAttribute.OptInAttribute))]
        public override void OnBodyRequirementBroken()
        {
            base.OnBodyRequirementBroken();
            GlobalEventManager.onServerDamageDealt -= GlobalEventManager_onServerDamageDealt;
            On.RoR2.GlobalEventManager.ProcIgniteOnKill -= GlobalEventManager_ProcIgniteOnKill;
            Run.onRunStartGlobal -= Run_onRunStartGlobal;
            Stage.onServerStageBegin -= Stage_onServerStageBegin;
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

    [RegisterAchievement("CommandoPRFRVWildfireStorm", "Commando.Skills_PRFRVWildfireStorm", null, null)]
    public class CommandoPRFRVWildfireStormAchievement : PRFRVWildfireStormAchievement
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
        public static UnlockableDef prfrVWildfireStorm;

        public static void Create()
        {
            heavyTap = ScriptableObject.CreateInstance<UnlockableDef>();
            heavyTap.achievementIcon = Main.mandogaming.LoadAsset<Sprite>("HeavyTap.png");
            heavyTap.cachedName = "Commando.Skills_HeavyTap";
            heavyTap.nameToken = "ACHIEVEMENT_COMMANDOHEAVYTAP_NAME";

            LanguageAPI.Add("ACHIEVEMENT_COMMANDOHEAVYTAP_NAME", "Commando: Have a Blast");
            LanguageAPI.Add("ACHIEVEMENT_COMMANDOHEAVYTAP_DESCRIPTION", "As Commando, complete a stage without using your Primary skill.");

            plasmaTap = ScriptableObject.CreateInstance<UnlockableDef>();
            plasmaTap.achievementIcon = Main.mandogaming.LoadAsset<Sprite>("PlasmaTap.png");
            plasmaTap.cachedName = "Commando.Skills_PlasmaTap";
            plasmaTap.nameToken = "ACHIEVEMENT_COMMANDOPLASMATAP_NAME";

            LanguageAPI.Add("ACHIEVEMENT_COMMANDOPLASMATAP_NAME", "Commando: Arch Essence");
            LanguageAPI.Add("ACHIEVEMENT_COMMANDOPLASMATAP_DESCRIPTION", "As Commando, chain lightning 70 times in a single run.");

            prfrVWildfireStorm = ScriptableObject.CreateInstance<UnlockableDef>();
            prfrVWildfireStorm.achievementIcon = Main.mandogaming.LoadAsset<Sprite>("PRFRVWildfireStorm.png");
            prfrVWildfireStorm.cachedName = "Commando.Skills_PRFRVWildfireStorm";
            prfrVWildfireStorm.nameToken = "ACHIEVEMENT_COMMANDOPRFRVWILDFIRESTORM_NAME";
            prfrVWildfireStorm.sortScore = plasmaTap.sortScore + 1;

            LanguageAPI.Add("ACHIEVEMENT_COMMANDOPRFRVWILDFIRESTORM_NAME", "Commando: Catch Fire");
            LanguageAPI.Add("ACHIEVEMENT_COMMANDOPRFRVWILDFIRESTORM_DESCRIPTION", "As Commando, burn enemies 30 times on a single stage.");

            ContentAddition.AddUnlockableDef(heavyTap);
            ContentAddition.AddUnlockableDef(plasmaTap);
            ContentAddition.AddUnlockableDef(prfrVWildfireStorm);
        }
    }
}