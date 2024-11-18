using EntityStates;
using MandoGamingRewrite.Projectiles;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace MandoGamingRewrite.CustomEntityStates
{
    internal class UnderbarrelShotgunState : BaseSkillState
    {
        public static float baseDuration = 0.15f;
        public float duration;
        public Animator modelAnimator;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            modelAnimator = GetModelAnimator();
            StartAimMode(GetAimRay());
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

            var aimRay = GetAimRay();
            AddRecoil(5f, 5f, 0f, 0f);

            var muzzle = "MuzzleLeft";
            PlayAnimation("Gesture Additive, Left", "FirePistol, Left");

            if (modelAnimator)
            {
                if (EntityStates.Commando.CommandoWeapon.FireBarrage.effectPrefab)
                {
                    EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireBarrage.effectPrefab, gameObject, muzzle, true);
                }
            }

            if (!characterMotor.isGrounded)
                characterMotor.ApplyForce(-2000f * aimRay.direction, false, false);

            Util.PlaySound("play_bandit_M2_shot", gameObject);
            Util.PlaySound("Play_railgunner_m2_alt_fire", gameObject);

            if (isAuthority)
            {
                var fpi = new FireProjectileInfo()
                {
                    damage = damageStat * 8f,
                    crit = RollCrit(),
                    damageColorIndex = DamageColorIndex.Default,
                    owner = gameObject,
                    rotation = Quaternion.LookRotation(aimRay.direction),
                    //position = GetModelChildLocator().FindChild("Muzzle").position,
                    position = transform.position,
                    force = 2500f,
                    procChainMask = default,
                    projectilePrefab = UnderbarrelShotgunProjectile.prefab
                };

                ProjectileManager.instance.FireProjectile(fpi);

                outer.SetNextStateToMain();
            }
        }
    }
}