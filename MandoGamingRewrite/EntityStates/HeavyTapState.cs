using EntityStates;
using EntityStates.Commando.CommandoWeapon;
using MandoGamingRewrite.Projectiles;
using RoR2;
using UnityEngine;

namespace MandoGamingRewrite.EntityStates
{
    internal class HeavyTapState : BaseSkillState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            if (finalShot)
            {
                duration = secondDuration / attackSpeedStat;
            }
            aimRay = GetAimRay();
            StartAimMode(aimRay, 3f, false);
            if (remainingShots % 2 == 0)
            {
                base.PlayAnimation("Gesture Additive, Left", "FirePistol, Left");
                FireBullet("MuzzleLeft");
                return;
            }
            base.PlayAnimation("Gesture Additive, Right", "FirePistol, Right");
            FireBullet("MuzzleRight");
        }

        public void FireBullet(string targetMuzzle)
        {
            Util.PlaySound(fireHeavyPistolSoundString, gameObject);
            if (FirePistol2.muzzleEffectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(FirePistol2.muzzleEffectPrefab, gameObject, targetMuzzle, false);
            }
            AddRecoil(-0.45f * recoilAmplitude, 0.9f * recoilAmplitude, -0.34f * recoilAmplitude, 0.34f * recoilAmplitude);
            if (isAuthority)
            {
                new BulletAttack
                {
                    owner = gameObject,
                    weapon = gameObject,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    minSpread = 0f,
                    maxSpread = 0f,
                    damage = damageCoefficient * damageStat,
                    force = force,
                    tracerEffectPrefab = tracerEffectPrefab,
                    muzzleName = targetMuzzle,
                    hitEffectPrefab = hitEffectPrefab,
                    isCrit = Util.CheckRoll(critStat, characterBody.master),
                    radius = 0.4f,
                    smartCollision = true,
                    falloffModel = BulletAttack.FalloffModel.None,
                    maxDistance = maxRange,
                    procCoefficient = 0.9f
                }.Fire();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if ((fixedAge < duration) || !isAuthority)
            {
                return;
            }
            remainingShots--;
            if (remainingShots == 0)
            {
                duration = baseDuration;
                finalShot = false;
                outer.SetNextStateToMain();
                return;
            }
            HeavyTapState firePistol = new();
            if (remainingShots == 1)
            {
                firePistol.finalShot = true;
            }
            firePistol.remainingShots = remainingShots;
            outer.SetNextState(firePistol);
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public float duration;
        public static float baseDuration = 0.15f;
        public static float secondDuration = 0.35f;
        public int remainingShots = 2;
        public bool finalShot = false;
        public static string fireHeavyPistolSoundString = "play_bandit_M2_shot";
        public GameObject tracerEffectPrefab = HeavyTapTracer.prefab;
        public GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/Hitspark1");
        public static float recoilAmplitude = 1f;
        private Ray aimRay;
        public static float maxRange = 250f;
        public static float damageCoefficient = 1.55f;
        public static float force = 40f;
    }
}