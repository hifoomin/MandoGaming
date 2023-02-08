using EntityStates;
using EntityStates.Mage.Weapon;
using RoR2;
using UnityEngine;

namespace MandoGamingRewrite.EntityStates
{
    internal class PRFRVWildfireStormState : BaseState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            stopwatch = 0f;
            flameInitiated = false;
            entryTimer = baseEntryTimer / attackSpeedStat;
            flameTimer = baseFlameTimer;
            tickFrequency = 3.2f * (attackSpeedStat - 1f) + baseTickFrequency;
            Transform modelTransform = GetModelTransform();
            if (characterBody)
            {
                characterBody.SetAimTimer(entryTimer + flameTimer + 1f);
            }
            if (modelTransform)
            {
                childLocator = modelTransform.GetComponent<ChildLocator>();
                leftMuzzle = childLocator.FindChild("MuzzleLeft");
                rightMuzzle = childLocator.FindChild("MuzzleRight");
            }
            if (isAuthority && characterBody)
            {
                isCrit = Util.CheckRoll(critStat, characterBody.master);
            }
        }

        public override void OnExit()
        {
            Util.PlaySound(endFlameAttackString, gameObject);
            PlayAnimation("Gesture, Additive", "FireFMJ", "FireFMJ.playbackRate", 0.6f);
            PlayAnimation("Gesture, Override", "FireFMJ", "FireFMJ.playbackRate", 0.6f);
            if (leftFlame)
            {
                Destroy(leftFlame.gameObject);
            }
            if (rightFlame)
            {
                Destroy(rightFlame.gameObject);
            }
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            characterBody.isSprinting = false;
            Flamethrower flamethrower = new();
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= entryTimer && !flameInitiated)
            {
                flameInitiated = true;
                Util.PlaySound(startFlameAttackString, gameObject);
                if (childLocator)
                {
                    transformer = childLocator.FindChild("MuzzleLeft");
                    transformer2 = childLocator.FindChild("MuzzleRight");
                }
                if (transformer)
                {
                    leftFlame = Object.Instantiate(flamethrower.flamethrowerEffectPrefab, transformer).transform;
                }
                if (transformer2)
                {
                    rightFlame = Object.Instantiate(flamethrower.flamethrowerEffectPrefab, transformer2).transform;
                }
                if (leftFlame)
                {
                    leftFlame.GetComponent<ScaleParticleSystemDuration>().newDuration = flameTimer;
                }
                if (rightFlame)
                {
                    rightFlame.GetComponent<ScaleParticleSystemDuration>().newDuration = flameTimer;
                }
                Flame("MuzzleCenter");
            }
            if (flameInitiated)
            {
                flameStopwatch += Time.deltaTime;
                if (flameStopwatch > 1f / tickFrequency)
                {
                    flameStopwatch -= 1f / tickFrequency;
                    Flame("MuzzleCenter");
                }
                UpdateFlame();
            }
            if (stopwatch >= flameTimer + entryTimer && isAuthority)
            {
                outer.SetNextStateToMain();
            }
        }

        private void UpdateFlame()
        {
            Ray aimRay = GetAimRay();
            Vector3 direction = aimRay.direction;
            Vector3 direction2 = aimRay.direction;
            if (leftFlame)
            {
                leftFlame.forward = direction;
            }
            if (rightFlame)
            {
                rightFlame.forward = direction2;
            }
        }

        private void Flame(string muzzleString)
        {
            Ray aimRay = GetAimRay();
            if (isAuthority)
            {
                new BulletAttack
                {
                    owner = gameObject,
                    weapon = gameObject,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    minSpread = 0f,
                    damage = tickDamage * damageStat,
                    force = force,
                    muzzleName = muzzleString,
                    hitEffectPrefab = Flamethrower.impactEffectPrefab,
                    isCrit = isCrit,
                    radius = Flamethrower.radius,
                    falloffModel = 0,
                    stopperMask = LayerIndex.world.mask,
                    procCoefficient = tickProc,
                    maxDistance = flameDistance,
                    smartCollision = true,
                    damageType = Util.CheckRoll(30f, characterBody.master) ? DamageType.IgniteOnHit : DamageType.Generic
                }.Fire();
                if (characterMotor)
                {
                    characterMotor.ApplyForce(aimRay.direction * -recoilForce, false, false);
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }

        public static float baseFlameTimer = 1f;

        public static float baseTickFrequency = 11f;

        public static float tickDamage = 0.5f;

        public static float tickProc = 1f;

        public float force = 50f;

        private float stopwatch;

        public static float baseEntryTimer = 0.2f;

        private float entryTimer;

        private bool flameInitiated;

        private float flameStopwatch;

        private float tickFrequency;

        private float flameTimer;

        private static string startFlameAttackString = "Play_item_proc_fireRingTornado_start";

        private static string endFlameAttackString = "Play_item_proc_fireRingTornado_end";

        private ChildLocator childLocator;

        private Transform leftFlame;

        private Transform rightFlame;

        private Transform leftMuzzle;

        private Transform rightMuzzle;

        private Transform transformer;

        private Transform transformer2;

        private bool isCrit;

        private const float flameDistance = 20f;

        public static float recoilForce = 290f;
    }
}