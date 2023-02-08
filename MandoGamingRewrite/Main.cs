using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using MandoGamingRewrite.EntityStates;
using MandoGamingRewrite.Keywords;
using MandoGamingRewrite.Projectiles;
using MandoGamingRewrite.Unlocks;
using R2API;
using R2API.ContentManagement;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

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
        public const string PluginVersion = "1.4.0";

        public static ConfigFile MandoGamingConfig;
        public static ManualLogSource MandoGamingLogger;
        public static AssetBundle mandogaming;

        public void Awake()
        {
            MandoGamingLogger = Logger;
            MandoGamingConfig = Config;

            mandogaming = AssetBundle.LoadFromFile(Assembly.GetExecutingAssembly().Location.Replace("MandoGamingRewrite.dll", "mandogaming"));

            Unlocks.Create();
            Keywords.Create();
            HeavyTapTracer.Create();
            PlasmaTapTracer.Create();

            IEnumerable<Type> enumerable = from type in Assembly.GetExecutingAssembly().GetTypes()
                                           where !type.IsAbstract && type.IsSubclassOf(typeof(SkillDefBase))
                                           select type;

            MandoGamingLogger.LogInfo("==+----------------==SKILLS==----------------+==");

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
}