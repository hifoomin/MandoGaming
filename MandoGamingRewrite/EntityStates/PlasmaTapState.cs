using EntityStates;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using RoR2.Skills;
using RoR2;
using RoR2.Orbs;
using MandoGamingRewrite.Projectiles;
using EntityStates.Commando.CommandoWeapon;
using MandoGaming;
using System.Linq;

namespace MandoGamingRewrite.EntityStates
{
    internal class PlasmaTapState : BaseSkillState, SteppedSkillDef.IStepSetter
    {
        void SteppedSkillDef.IStepSetter.SetStep(int i)
        {
            pistol = i;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            aimRay = GetAimRay();
            StartAimMode(aimRay, 3f, false);
            modelTransform = GetModelTransform();
            if (modelTransform)
            {
                childLocator = modelTransform.GetComponent<ChildLocator>();
            }
            if (pistol % 2 == 0)
            {
                base.PlayAnimation("Gesture Additive, Left", "FirePistol, Left");
                FireBullet("MuzzleLeft");
            }
            else
            {
                base.PlayAnimation("Gesture Additive, Right", "FirePistol, Right");
                FireBullet("MuzzleRight");
            }
            SearchForTarget(aimRay);
            FireOrb();
        }

        private void SearchForTarget(Ray aimRay)
        {
            search.teamMaskFilter = TeamMask.GetUnprotectedTeams(teamComponent.teamIndex);
            search.filterByLoS = true;
            search.searchOrigin = aimRay.origin;
            search.searchDirection = aimRay.direction;
            search.sortMode = BullseyeSearch.SortMode.Distance;
            search.maxDistanceFilter = 100f;
            search.maxAngleFilter = 360f;
            search.RefreshCandidates();
            search.FilterOutGameObject(gameObject);
            initialOrbTarget = search.GetResults().FirstOrDefault();
        }

        private void FireOrb()
        {
            if (!NetworkServer.active)
            {
                return;
            }

            LightningOrb lightningOrb = new()
            {
                lightningType = LightningOrb.LightningType.Ukulele,
                damageValue = damageArcing * damageStat,
                isCrit = Util.CheckRoll(critStat, characterBody.master),
                teamIndex = TeamComponent.GetObjectTeam(gameObject),
                attacker = gameObject,
                procCoefficient = 0.15f,
                bouncesRemaining = 1,
                speed = 120f,
                bouncedObjects = new List<HealthComponent>(),
                range = 30f,
                targetsToFindPerBounce = 3,
            };
            var hurtBox = initialOrbTarget;
            if (hurtBox)
            {
                var handR = childLocator.FindChild("HandR");
                var handL = childLocator.FindChild("HandL");
                lightningOrb.origin = pistol % 2 == 0 ? handL.position : handR.position;
                lightningOrb.target = hurtBox;
                OrbManager.instance.AddOrb(lightningOrb);
            }
        }

        public void FireBullet(string targetMuzzle)
        {
            Util.PlaySound(attackSoundString, gameObject);
            Util.PlaySound("Play_mage_m1_cast_lightning", gameObject);
            if (FirePistol2.muzzleEffectPrefab)
            {
                EffectManager.SimpleMuzzleFlash(FirePistol2.muzzleEffectPrefab, gameObject, targetMuzzle, false);
            }
            AddRecoil(-0.4f * recoilAmplitude, 0.8f * recoilAmplitude, -0.3f * recoilAmplitude, 0.3f * recoilAmplitude);

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
                    tracerEffectPrefab = PlasmaTapTracer.prefab,
                    muzzleName = targetMuzzle,
                    hitEffectPrefab = hitEffectPrefab,
                    isCrit = RollCrit(),
                    radius = 1f,
                    smartCollision = true,
                    falloffModel = BulletAttack.FalloffModel.Buckshot,
                    maxDistance = maxRange
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
            if (base.fixedAge < this.duration || !base.isAuthority)
            {
                return;
            }
            if (base.activatorSkillSlot.stock <= 0)
            {
                this.outer.SetNextState(new ReloadPistols());
                return;
            }
            this.outer.SetNextStateToMain();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            writer.Write(HurtBoxReference.FromHurtBox(initialOrbTarget));
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            initialOrbTarget = reader.ReadHurtBoxReference().ResolveHurtBox();
        }

        public float duration;

        public static float baseDuration = 0.25f;

        public static float maxRange = 100f;

        public int pistol = 2;

        private Ray aimRay;

        public static float damageCoefficient = 1f;

        public static float damageArcing = 0.3f;

        public GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/LightningFlash");

        public static string attackSoundString = "Play_item_proc_chain_lightning";
        public static float recoilAmplitude = 1f;
        private HurtBox initialOrbTarget;
        private readonly BullseyeSearch search = new();
        private ChildLocator childLocator;
        private Transform modelTransform;
    }
}