using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using MandoGamingRewrite.CustomEntityStates;
using MandoGamingRewrite.Keywords;
using MandoGamingRewrite.VFX;
using R2API;
using R2API.ContentManagement;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using MandoGamingRewrite.Projectiles;
using MandoGaming.Skills;
using MandoGamingRewrite.Crosshairs;

namespace MandoGaming
{
    [BepInDependency(R2APIContentManager.PluginGUID)]
    [BepInDependency(LanguageAPI.PluginGUID)]
    [BepInDependency(PrefabAPI.PluginGUID)]
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Main : BaseUnityPlugin
    {
        public const string PluginGUID = PluginAuthor + "." + PluginName;

        public const string PluginAuthor = "HIFU";
        public const string PluginName = "MandoGaming";
        public const string PluginVersion = "1.5.0";

        public static ConfigFile MGConfig;
        public static ManualLogSource MGLogger;
        public static AssetBundle bundle;

        public static BodyIndex commandoBodyIndex;

        public void Awake()
        {
            MGLogger = Logger;
            MGConfig = Config;

            bundle = AssetBundle.LoadFromFile(Assembly.GetExecutingAssembly().Location.Replace("MandoGamingRewrite.dll", "mandogaming"));

            // Unlocks.Init();
            ProjectileCrosshair.Init();
            Keywords.Init();
            HeavyTapVFX.Init();
            PlasmaTapVFX.Init();
            UnderbarrelShotgunVFX.Init();
            UnderbarrelShotgunProjectile.Init();
            ParadigmHandgunProjectile.Init();

            IEnumerable<Type> enumerable = from type in Assembly.GetExecutingAssembly().GetTypes()
                                           where !type.IsAbstract && type.IsSubclassOf(typeof(SkillDefBase))
                                           select type;

            MGLogger.LogInfo("==+----------------==SKILLS==----------------+==");

            foreach (Type type in enumerable)
            {
                SkillDefBase based = (SkillDefBase)Activator.CreateInstance(type);
                if (ValidateSkillDef(based))
                {
                    based.Init();
                }
            }

            ContentAddition.AddEntityState(typeof(HeavyTapState), out _);
            ContentAddition.AddEntityState(typeof(PlasmaTapState), out _);
            ContentAddition.AddEntityState(typeof(PRFRVWildfireStormState), out _);
            ContentAddition.AddEntityState(typeof(PointBlankState), out _);
            ContentAddition.AddEntityState(typeof(UnderbarrelShotgunState), out _);
            ContentAddition.AddEntityState(typeof(ParadigmHandgunState), out _);

            On.RoR2.BodyCatalog.Init += BodyCatalog_Init;
            CharacterBody.onBodyStartGlobal += CharacterBody_onBodyStartGlobal;
        }

        private void BodyCatalog_Init(On.RoR2.BodyCatalog.orig_Init orig)
        {
            orig();
            commandoBodyIndex = BodyCatalog.FindBodyIndex("CommandoBody(Clone)");
        }

        private void CharacterBody_onBodyStartGlobal(CharacterBody body)
        {
            if (body.bodyIndex != commandoBodyIndex)
            {
                return;
            }

            var skillLocator = body.skillLocator;
            if (!skillLocator)
            {
                return;
            }

            if (skillLocator.primary.skillDef == ParadigmHandgunSD.instance.skillDef && body.GetComponent<ParadigmHandgunIdentifier>() == null)
            {
                body.gameObject.AddComponent<ParadigmHandgunIdentifier>();
                var crosshairOverrideBehavior = body.gameObject.AddComponent<RoR2.UI.CrosshairUtils.CrosshairOverrideBehavior>();
                crosshairOverrideBehavior.AddRequest(ProjectileCrosshair.prefab, RoR2.UI.CrosshairUtils.OverridePriority.Skill);
            }
        }

        public bool ValidateSkillDef(SkillDefBase sdb)
        {
            if (sdb.isEnabled)
            {
                bool enabledfr = Config.Bind(sdb.NameText, "Enable?", true, "Vanilla is false").Value;
                if (enabledfr)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class ParadigmHandgunIdentifier : MonoBehaviour
    {
    }
}