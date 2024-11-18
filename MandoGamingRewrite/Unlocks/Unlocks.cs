/*
using MandoGaming;
using R2API;
using RoR2;
using RoR2.Achievements;
using UnityEngine;

namespace MandoGamingRewrite.Unlocks
{
    [RegisterAchievement("CommandoHeavyTap", "Commando.Skills_HeavyTap", null, null)]
    public class HeavyTapAchievement : BaseAchievement
    {
        private int primaryUseCount;

        public override BodyIndex LookUpRequiredBodyIndex()
        { return BodyCatalog.FindBodyIndex("CommandoBody"); }

        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            On.RoR2.CharacterBody.OnSkillActivated += CharacterBody_OnSkillActivated;
            TeleporterInteraction.onTeleporterChargedGlobal += TeleporterInteraction_onTeleporterChargedGlobal;
            Run.onRunStartGlobal += Run_onRunStartGlobal;
            Stage.onServerStageBegin += Stage_onServerStageBegin;
        }

        public override void OnBodyRequirementBroken()
        {
            On.RoR2.CharacterBody.OnSkillActivated -= CharacterBody_OnSkillActivated;
            TeleporterInteraction.onTeleporterChargedGlobal -= TeleporterInteraction_onTeleporterChargedGlobal;
            Run.onRunStartGlobal -= Run_onRunStartGlobal;
            Stage.onServerStageBegin -= Stage_onServerStageBegin;
            base.OnBodyRequirementBroken();
        }

        private void Run_onRunStartGlobal(Run obj)
        { primaryUseCount = 0; }

        private void Stage_onServerStageBegin(Stage obj)
        { primaryUseCount = 0; }

        private void TeleporterInteraction_onTeleporterChargedGlobal(TeleporterInteraction _)
        { if (primaryUseCount == 0) Grant(); }

        private void CharacterBody_OnSkillActivated(On.RoR2.CharacterBody.orig_OnSkillActivated orig, CharacterBody self, GenericSkill skill)
        {
            if (localUser?.cachedBody != null && self == localUser.cachedBody && skill == localUser.cachedBody.skillLocator.primary) primaryUseCount++;
            orig(self, skill);
        }
    }

    [RegisterAchievement("CommandoPlasmaTap", "Commando.Skills_PlasmaTap", null, null)]
    public class PlasmaTapAchievement : BaseAchievement
    {
        private float zapCount;

        public override BodyIndex LookUpRequiredBodyIndex()
        { return BodyCatalog.FindBodyIndex("CommandoBody"); }

        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            Run.onRunStartGlobal += Run_onRunStartGlobal;
        }

        public override void OnBodyRequirementBroken()
        {
            GlobalEventManager.onServerDamageDealt -= GlobalEventManager_onServerDamageDealt;
            Run.onRunStartGlobal -= Run_onRunStartGlobal;
            base.OnBodyRequirementBroken();
        }

        private void Run_onRunStartGlobal(Run obj)
        { zapCount = 0; }

        private void GlobalEventManager_onServerDamageDealt(DamageReport damageReport)
        {
            if (localUser?.cachedBody != null && localUser.cachedBody == damageReport.attackerBody)
            {
                // Main.MandoGamingLogger.LogFatal("CHAIN LIGHTNING OnServerDamageDealt attackerBody.name and cachedBody name is CommandoBody(clone)");
                if (damageReport.damageInfo.procChainMask.HasProc(ProcType.ChainLightning)) zapCount++;
                if (zapCount >= 70) Grant();
            }
        }
    }

    [RegisterAchievement("CommandoPRFRVWildfireStorm", "Commando.Skills_PRFRVWildfireStorm", null, null)]
    public class PRFRVWildfireStormAchievement : BaseAchievement
    {
        private float igniteCount;

        public override BodyIndex LookUpRequiredBodyIndex()
        { return BodyCatalog.FindBodyIndex("CommandoBody"); }

        public override void OnBodyRequirementMet()
        {
            base.OnBodyRequirementMet();
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
            On.RoR2.GlobalEventManager.ProcIgniteOnKill += GlobalEventManager_ProcIgniteOnKill;
            Run.onRunStartGlobal += Run_onRunStartGlobal;
            Stage.onServerStageBegin += Stage_onServerStageBegin;
        }

        public override void OnBodyRequirementBroken()
        {
            GlobalEventManager.onServerDamageDealt -= GlobalEventManager_onServerDamageDealt;
            On.RoR2.GlobalEventManager.ProcIgniteOnKill -= GlobalEventManager_ProcIgniteOnKill;
            Run.onRunStartGlobal -= Run_onRunStartGlobal;
            Stage.onServerStageBegin -= Stage_onServerStageBegin;
            base.OnBodyRequirementBroken();
        }

        private void Run_onRunStartGlobal(Run obj)
        { igniteCount = 0; }

        private void Stage_onServerStageBegin(Stage obj)
        { igniteCount = 0; }

        private void GlobalEventManager_ProcIgniteOnKill(On.RoR2.GlobalEventManager.orig_ProcIgniteOnKill orig, DamageReport damageReport, int igniteOnKillCount, CharacterBody victimBody, TeamIndex attackerTeamIndex)
        {
            if (localUser?.cachedBody != null && localUser.cachedBody == damageReport.attackerBody)
            {
                // Main.MandoGamingLogger.LogFatal("ProcIgniteOnKill attackerBody.name and cachedBody name is CommandoBodY(cloenkeo)");
                if (igniteOnKillCount > 0) igniteCount++;
                if (igniteCount >= 30) Grant();
            }
            orig(damageReport, igniteOnKillCount, victimBody, attackerTeamIndex);
        }

        private void GlobalEventManager_onServerDamageDealt(DamageReport damageReport)
        {
            if (localUser?.cachedBody != null && localUser.cachedBody == damageReport.attackerBody)
            {
                // Main.MandoGamingLogger.LogFatal("IGNITE ON HIT OnServerDamageDealt attackerBody.name and cachedBody name is CommandoBody(clone)");
                if ((damageReport.damageInfo.damageType & DamageType.IgniteOnHit) != 0) igniteCount++;
                if (igniteCount >= 30) Grant();
            }
        }
    }

    public static class Unlocks
    {
        public static UnlockableDef heavyTap;
        public static UnlockableDef plasmaTap;
        public static UnlockableDef prfrVWildfireStorm;

        public static void Init()
        {
            heavyTap = ScriptableObject.CreateInstance<UnlockableDef>();
            heavyTap.achievementIcon = Main.bundle.LoadAsset<Sprite>("HeavyTap.png");
            heavyTap.cachedName = "Commando.Skills_HeavyTap";
            heavyTap.nameToken = "ACHIEVEMENT_COMMANDOHEAVYTAP_NAME";

            LanguageAPI.Add("ACHIEVEMENT_COMMANDOHEAVYTAP_NAME", "Commando: Have a Blast");
            LanguageAPI.Add("ACHIEVEMENT_COMMANDOHEAVYTAP_DESCRIPTION", "As Commando, complete a stage without using your Primary skill.");

            plasmaTap = ScriptableObject.CreateInstance<UnlockableDef>();
            plasmaTap.achievementIcon = Main.bundle.LoadAsset<Sprite>("PlasmaTap.png");
            plasmaTap.cachedName = "Commando.Skills_PlasmaTap";
            plasmaTap.nameToken = "ACHIEVEMENT_COMMANDOPLASMATAP_NAME";

            LanguageAPI.Add("ACHIEVEMENT_COMMANDOPLASMATAP_NAME", "Commando: Arch Essence");
            LanguageAPI.Add("ACHIEVEMENT_COMMANDOPLASMATAP_DESCRIPTION", "As Commando, chain lightning 70 times in a single run.");

            prfrVWildfireStorm = ScriptableObject.CreateInstance<UnlockableDef>();
            prfrVWildfireStorm.achievementIcon = Main.bundle.LoadAsset<Sprite>("PRFRVWildfireStorm.png");
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
*/