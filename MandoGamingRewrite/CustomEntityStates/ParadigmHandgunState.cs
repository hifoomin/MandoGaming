using EntityStates;
using MandoGamingRewrite.Projectiles;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace MandoGamingRewrite.CustomEntityStates
{
    internal class ParadigmHandgunState : BaseSkillState
    {
        public static float baseDuration = 0.33f;
        public float duration;
        public Animator modelAnimator;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            modelAnimator = GetModelAnimator();
            StartAimMode(GetAimRay());

            var aimRay = GetAimRay();
            AddRecoil(2.5f, 2.5f, 0.5f, 1f);

            var muzzle = "MuzzleLeft";
            PlayAnimation("Gesture Additive, Left", "FirePistol, Left");

            if (modelAnimator)
            {
                if (EntityStates.Commando.CommandoWeapon.FireBarrage.effectPrefab)
                {
                    EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireBarrage.effectPrefab, gameObject, muzzle, true);
                }
            }

            Util.PlaySound("play_bandit_M2_shot", gameObject);

            if (isAuthority)
            {
                var fpi = new FireProjectileInfo()
                {
                    damage = damageStat * 4f,
                    crit = RollCrit(),
                    damageColorIndex = DamageColorIndex.Default,
                    owner = gameObject,
                    rotation = Quaternion.LookRotation(aimRay.direction),
                    //position = GetModelChildLocator().FindChild("Muzzle").position,
                    position = transform.position,
                    force = 500f,
                    procChainMask = default,
                    projectilePrefab = ParadigmHandgunProjectile.prefab
                };

                ProjectileManager.instance.FireProjectile(fpi);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge < duration)
            {
                return;
            }

            outer.SetNextStateToMain();
        }
    }
}