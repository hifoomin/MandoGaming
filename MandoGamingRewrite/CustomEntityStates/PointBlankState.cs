using EntityStates;
using RoR2;
using RoR2.Skills;
using System.Collections;
using UnityEngine;

namespace MandoGamingRewrite.CustomEntityStates
{
    internal class PointBlankState : BaseSkillState, SteppedSkillDef.IStepSetter
    {
        public static float baseDurationPerBurst = 0.2f;
        public float durationPerBurst;
        public Animator modelAnimator;
        public int pistol;

        void SteppedSkillDef.IStepSetter.SetStep(int i)
        {
            pistol = i;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            durationPerBurst = baseDurationPerBurst / attackSpeedStat;
            modelAnimator = GetModelAnimator();
            StartAimMode(GetAimRay());
            outer.StartCoroutine(FireFuckingBullets());
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (fixedAge < durationPerBurst * 5 || !isAuthority)
            {
                return;
            }
            outer.SetNextStateToMain();
        }

        public IEnumerator FireFuckingBullets()
        {
            // Util.PlaySound("Play_lunar_wisp_attack1_shoot_impact", gameObject);
            // Util.PlaySound("Play_lunar_wisp_attack1_shoot_impact", gameObject);
            // yield return new WaitForSeconds(durationPerBurst);
            for (int i = 0; i < 4; i++)
            {
                var aimRay = GetAimRay();
                AddRecoil(-4f, 4f, -2f, 2f);

                string muzzle = "MuzzleRight";
                if (pistol % 2 == 0)
                {
                    muzzle = "MuzzleLeft";
                    PlayAnimation("Gesture Additive, Left", "FirePistol, Left");
                }
                else
                {
                    PlayAnimation("Gesture Additive, Right", "FirePistol, Right");
                }

                if (modelAnimator)
                {
                    if (EntityStates.Commando.CommandoWeapon.FireBarrage.effectPrefab)
                    {
                        EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireBarrage.effectPrefab, gameObject, muzzle, true);
                    }
                }

                characterMotor.ApplyForce(-600f * aimRay.direction, false, false);

                for (int j = 0; j < 5; j++)
                {
                    if (isAuthority)
                    {
                        new BulletAttack()
                        {
                            bulletCount = 1U,
                            minSpread = 2f,
                            maxSpread = 5f,
                            falloffModel = BulletAttack.FalloffModel.DefaultBullet,
                            damageType = DamageType.Generic,
                            force = 500f,
                            damageColorIndex = DamageColorIndex.Default,
                            damage = characterBody.damage * 0.9f,
                            isCrit = RollCrit(),
                            procCoefficient = 1,
                            smartCollision = false,
                            radius = 1f,
                            weapon = gameObject,
                            owner = gameObject,
                            origin = aimRay.origin,
                            aimVector = aimRay.direction,
                            tracerEffectPrefab = EntityStates.Commando.CommandoWeapon.FireBarrage.tracerEffectPrefab,
                            muzzleName = muzzle,
                            hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FireBarrage.hitEffectPrefab,
                            stopperMask = LayerIndex.world.mask,
                            maxDistance = 50f
                        }.Fire();
                    }
                }
                Util.PlaySound("play_bandit_M2_shot", gameObject);
                Util.PlaySound("Play_grandParent_attack1_boulderSmall_impact", gameObject);
                yield return new WaitForSeconds(durationPerBurst);
            }
        }
    }
}