using MandoGaming;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace MandoGamingRewrite.VFX
{
    public static class ParadigmHandgunVFX
    {
        public static GameObject impactPrefab;
        public static GameObject ghostPrefab;

        public static void Init()
        {
            impactPrefab = CreateImpact(new Color32(23, 66, 234, 255), new Color32(23, 30, 211, 94), new Color32(0, 132, 255, 255));
            ghostPrefab = CreateGhost(new Color32(0, 82, 255, 255), new Color32(0, 13, 197, 255), new Color32(20, 48, 217, 255));
        }

        public static GameObject CreateImpact(Color32 hotPinkEquivalent, Color32 redEquivalent, Color32 spikeColor, float brighnessBoost = 11.08f, float alphaBoost = 4.3f)
        {
            // hotPinkEquivalent = new Color32(226, 27, 128, 255);
            // redEquivalent = new Color32(209, 21, 15, 96);
            // spikeColor = new Color32(255,255,255,255);
            var impact = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Railgunner/ImpactRailgunLight.prefab").WaitForCompletion().InstantiateClone("Paradigm Handgun Impact", false);

            impact.GetComponent<EffectComponent>().soundName = "Play_item_proc_armorReduction_hit";

            var trans = impact.transform;

            var beamParticles = trans.GetChild(0);
            beamParticles.gameObject.SetActive(false);

            var shockWave = trans.GetChild(1).GetComponent<ParticleSystem>().main.startColor;
            shockWave.color = hotPinkEquivalent;

            var flashWhite = trans.GetChild(2);
            // flashWhite.gameObject.SetActive(false);

            var daggers = trans.GetChild(3).GetComponent<ParticleSystemRenderer>();

            var daggersPS = daggers.GetComponent<ParticleSystem>().main.startLifetime;
            daggersPS.constantMin = 1f;
            daggersPS.constantMax = 1f;

            var newMat = Object.Instantiate(Addressables.LoadAssetAsync<Material>("RoR2/DLC1/Railgunner/matRailgunImpactSpikesLight.mat").WaitForCompletion());
            newMat.SetColor("_TintColor", spikeColor);
            newMat.SetFloat("_Boost", brighnessBoost);
            newMat.SetFloat("_AlphaBoost", alphaBoost);

            daggers.material = newMat;

            var flashWhite3 = trans.GetChild(7).GetComponent<ParticleSystem>().main.startColor;
            flashWhite3.color = redEquivalent;

            ContentAddition.AddEffect(impact);
            // 9,0,0
            return impact;
        }

        public static GameObject CreateGhost(Color32 saturatedAquaEquivalent, Color32 saturatedBlueEquivalent, Color32 mutedAquaEquivalent)
        {
            // saturatedAquaEquivalent = new Color32(0, 255, 167, 255);
            // saturatedBlueEquivalent = new Color32(0,141,197,255);
            // mutedAquaEquivalent = new Color32(111,170,151,154);

            var ghost = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/LunarSun/LunarSunProjectileGhost.prefab").WaitForCompletion().InstantiateClone("Paradigm Handgun Ghost", false);

            Main.MGLogger.LogDebug(ghost);
            Main.MGLogger.LogDebug(ghost.transform);
            Main.MGLogger.LogDebug(ghost.transform.GetChild(0));

            ghost.transform.localScale = Vector3.one * 0.35f;

            var ramp = Main.bundle.LoadAsset<Texture2D>("texRampUnderbarrelShotgun.png");
            var fresnel = Main.bundle.LoadAsset<Texture2D>("texRampUnderbarrelShotgunFresnel.png");

            var green = saturatedAquaEquivalent;

            var mdl = ghost.transform.GetChild(0);
            var objectScaleCurve = mdl.GetComponent<ObjectScaleCurve>();
            objectScaleCurve.overallCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.15f, 0.75f), new Keyframe(1f, 2.5f));

            var backdrop = mdl.GetChild(0).GetComponent<ParticleSystemRenderer>();

            var newBackdropMat = Object.Instantiate(Addressables.LoadAssetAsync<Material>("RoR2/DLC1/LunarSun/matLunarSunProjectileBackdrop.mat").WaitForCompletion());

            newBackdropMat.SetTexture("_RemapTex", ramp);
            newBackdropMat.SetInt("_Cull", 1); // used to appear as a white square behind terrain so I fixed it

            backdrop.material = newBackdropMat;

            var quad = mdl.GetChild(1).GetComponent<MeshRenderer>();

            var newQuadMat = Object.Instantiate(Addressables.LoadAssetAsync<Material>("RoR2/DLC1/LunarSun/matLunarSunProjectile.mat").WaitForCompletion());

            newQuadMat.SetColor("_EmColor", green); // 0, 187, 255, 255
            newQuadMat.SetTexture("_FresnelRamp", fresnel);

            quad.material = newQuadMat;

            var particles = ghost.transform.GetChild(1);

            var closeParticles = particles.GetChild(0).GetComponent<ParticleSystem>().main.startColor;
            closeParticles.colorMin = green;
            closeParticles.colorMax = saturatedBlueEquivalent;

            var distantParticles = particles.GetChild(1).GetComponent<ParticleSystem>().main.startColor;
            distantParticles.color = green;

            var pop = particles.GetChild(2).GetComponent<ParticleSystem>().main.startColor;
            pop.color = green;

            var trail = particles.GetChild(3).GetComponent<TrailRenderer>();
            trail.startWidth = 0.6f;
            trail.endWidth = 0.25f;
            trail.widthMultiplier = 0.2f;
            trail.time = 0.1f;

            var newTrailMat = Object.Instantiate(Addressables.LoadAssetAsync<Material>("RoR2/DLC1/LunarSun/matLunarSunProjectileTrail.mat").WaitForCompletion());

            newTrailMat.SetTexture("_RemapTex", ramp);
            newTrailMat.SetFloat("_Boost", 1f);
            newTrailMat.SetFloat("_AlphaBoost", 4.710526f);
            newTrailMat.SetFloat("_AlphaBias", 0.3349282f);
            newTrailMat.SetTexture("_MainTex", Addressables.LoadAssetAsync<Texture2D>("RoR2/Base/Common/VFX/texAlphaGradient1.png").WaitForCompletion());
            newTrailMat.SetColor("_TintColor", mutedAquaEquivalent);

            trail.material = newTrailMat;

            return ghost;
        }
    }
}