using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MandoGamingRewrite.VFX
{
    public static class HeavyTapVFX
    {
        public static GameObject prefab;

        public static void Init()
        {
            prefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Junk/Bandit/TracerBanditShotgun.prefab").WaitForCompletion(), "HeavyTapTracer", false);

            var tracer = prefab.GetComponent<Tracer>();
            tracer.length *= 1.8f;
            tracer.beamDensity *= 1.5f;
            foreach (LineRenderer lineRenderer in tracer.GetComponentsInChildren<LineRenderer>())
            {
                if (lineRenderer)
                {
                    lineRenderer.endColor = new Color(0.025f, 0.02f, 0.3f);
                    lineRenderer.startColor = new Color(0.2f, 0.175f, 0.7f);
                }
            }
            tracer.gameObject.AddComponent<VFXAttributes>();
            tracer.GetComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Medium;
            tracer.GetComponent<VFXAttributes>().vfxIntensity = VFXAttributes.VFXIntensity.Medium;
            ContentAddition.AddEffect(tracer.gameObject);
        }
    }
}