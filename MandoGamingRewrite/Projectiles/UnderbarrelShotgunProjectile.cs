using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MandoGamingRewrite.Projectiles
{
    public static class UnderbarrelShotgunProjectile
    {
        public static GameObject prefab;

        public static void Init()
        {
            prefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mage/MageLightningBombProjectile.prefab").WaitForCompletion().InstantiateClone("Underbarrel Shotgun Slug", true);

            var proximityDetonator = prefab.transform.GetChild(0).GetComponent<SphereCollider>();
            proximityDetonator.radius = 0.6f;

            GameObject.Destroy(prefab.GetComponent<ProjectileProximityBeamController>());

            GameObject.Destroy(prefab.GetComponent<AkEvent>());
            GameObject.Destroy(prefab.GetComponent<AkGameObj>());

            var sphereCollider = prefab.GetComponent<SphereCollider>();
            // sphereCollider.material = Assets.PhysicMaterial.physmatEngiGrenade;
            sphereCollider.radius = 0.5f;
            prefab.layer = LayerIndex.projectile.intVal;

            prefab.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            var projectileDamage = prefab.GetComponent<ProjectileDamage>();
            projectileDamage.damageType = DamageType.Generic;

            var projectileImpactExplosion = prefab.GetComponent<ProjectileImpactExplosion>();
            projectileImpactExplosion.falloffModel = BlastAttack.FalloffModel.None;
            projectileImpactExplosion.blastRadius = 1f;
            projectileImpactExplosion.bonusBlastForce = Vector3.zero;
            projectileImpactExplosion.lifetime = 5f;
            projectileImpactExplosion.impactEffect = VFX.UnderbarrelShotgunVFX.impactPrefab;

            var projectileSimple = prefab.GetComponent<ProjectileSimple>();
            projectileSimple.lifetime = 5f;
            projectileSimple.desiredForwardSpeed = 250f;
            projectileSimple.enableVelocityOverLifetime = true;
            projectileSimple.velocityOverLifetime = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.04f, 0.75f), new Keyframe(0.1f, 1f));

            var antiGravityForce = prefab.GetComponent<AntiGravityForce>();
            antiGravityForce.antiGravityCoefficient = -0.4f;

            var projectileController = prefab.GetComponent<ProjectileController>();

            var newGhost = VFX.UnderbarrelShotgunVFX.ghostPrefab;

            projectileController.ghostPrefab = newGhost;

            prefab.RegisterNetworkPrefab();
            ContentAddition.AddProjectile(prefab);
        }
    }
}