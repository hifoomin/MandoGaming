using R2API;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MandoGamingRewrite.Crosshairs
{
    public static class ProjectileCrosshair
    {
        public static GameObject prefab;

        public static void Init()
        {
            prefab = PrefabAPI.InstantiateClone(Addressables.LoadAssetAsync<GameObject>("RoR2/Base/UI/SimpleDotCrosshair.prefab").WaitForCompletion(), "Paradigm Handgun Crosshair", false);
            prefab.transform.GetChild(0).GetComponent<RectTransform>().localScale = Vector3.one * 1.5f;
        }
    }
}