using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MandoGamingRewrite.VFX
{
    public static class PlasmaTapVFX
    {
        public static GameObject prefab;

        public static void Init()
        {
            prefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Huntress/TracerHuntressSnipe.prefab").WaitForCompletion(), "PlasmaTapTracer", false);

            var tracer = prefab.GetComponent<Tracer>();
            tracer.length = 40f;
            tracer.beamDensity = 5f;

            tracer.gameObject.AddComponent<VFXAttributes>();
            tracer.GetComponent<VFXAttributes>().vfxPriority = VFXAttributes.VFXPriority.Always;
            tracer.GetComponent<VFXAttributes>().vfxIntensity = VFXAttributes.VFXIntensity.High;
            var tracerHeadLineRenderer = tracer.transform.Find("TracerHead").GetComponent<LineRenderer>();
            tracerHeadLineRenderer.startColor = new Color(0.1f, 0.2f, 0.7f);
            var tracerMaterial = Object.Instantiate(tracerHeadLineRenderer.material);
            tracerMaterial.SetColor("_TintColor", new Color(0.1f, 0.2f, 0.7f));
            tracerHeadLineRenderer.material = tracerMaterial;
            tracerHeadLineRenderer.startWidth *= 0.4f;
            tracerHeadLineRenderer.endWidth *= 0.4f;
            ContentAddition.AddEffect(tracer.gameObject);
        }
    }
}