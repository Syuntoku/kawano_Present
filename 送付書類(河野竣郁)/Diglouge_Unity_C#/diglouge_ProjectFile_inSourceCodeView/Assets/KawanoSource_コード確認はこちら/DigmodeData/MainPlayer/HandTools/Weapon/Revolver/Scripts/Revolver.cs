using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.Status;
using Syuntoku.DigMode.Tool;

namespace Syuntoku.DigMode.Weapon
{
    public class Revolver : WeaponBase
    {
        private AudioSource _audioSource;
        [SerializeField] private AudioClip _reloadCompleteSE;
        [SerializeField] private AudioClip _íeçûSE;

        [SerializeField] ParticleSystem muzzleFlashParticle = null;
        [SerializeField] public Transform WeaponParentSocket;

        [SerializeField] RevolverScriptable revolverScriptable;
        RevolverInfo _revolverInfo;

        [Tooltip("ïêäÌÇÃîΩìÆóÕ")]
        public float RecoilForce = 1f;

        [Tooltip("îΩìÆÇ™ïêäÌÇ…âeãøÇó^Ç¶ÇÈç≈ëÂãóó£")]
        public float MaxRecoilDistance = 0.5f;
        [Tooltip("îΩìÆÇ™ïêäÌÇìÆÇ©Ç∑ë¨Ç≥")]
        public float RecoilSharpness = 50f;
        [Tooltip("îΩìÆå„ÇÃïêäÌÇ™å≥ÇÃà íuÇ…ñﬂÇÈë¨Ç≥")]
        public float RecoilRestitutionSharpness = 10f;

        private Vector3 _accumulatedRecoil;
        private Vector3 _weaponRecoilLocalPosition;
        private Vector3 _weaponMainLocalPosition;
        public Revolver()
        {

        }

        private void Start()
        {
            WeaponParentSocket = transform.parent;
            _weaponMainLocalPosition = WeaponParentSocket.localPosition;

            if (!TryGetComponent(out _audioSource))
            {
                _audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        public override void ReroadStart()
        {
            base.ReroadStart();
            StartCoroutine(ReloadSECoroutine());
        }

        private IEnumerator ReloadSECoroutine()
        {
            var magazineSize = _weaponInfo._weaponBaseStatus.magazineSize;
            var d = _weaponInfo._weaponBaseStatus.reloadTime - _reloadCompleteSE.length - REROADING_MOSION_TIME;

            var delay = d / magazineSize;


            yield return new WaitForSeconds(REROADING_MOSION_TIME);

            for (int i = 0; i < magazineSize; i++)
            {
                _audioSource.PlayOneShot(_íeçûSE);
                yield return new WaitForSeconds(delay);
            }
            _audioSource.PlayOneShot(_reloadCompleteSE);
        }

        public override void OnCallShot(GameObject bulletPrefab, Vector3 instancePosition, Vector3 targetDir, bool activeShotInterval = true)
        {
            base.OnCallShot(bulletPrefab, instancePosition, targetDir, activeShotInterval);

            if (muzzleFlashParticle) muzzleFlashParticle.Play();

            _accumulatedRecoil += Vector3.back * RecoilForce;
            _accumulatedRecoil = Vector3.ClampMagnitude(_accumulatedRecoil, MaxRecoilDistance);
        }

        private void LateUpdate()
        {
            UpdateWeaponRecoil();

            WeaponParentSocket.localPosition = _weaponMainLocalPosition + _weaponRecoilLocalPosition;
        }

        private void UpdateWeaponRecoil()
        {
            if (_weaponRecoilLocalPosition.z >= _accumulatedRecoil.z * 0.99f)
            {
                _weaponRecoilLocalPosition = Vector3.Lerp(_weaponRecoilLocalPosition, _accumulatedRecoil, RecoilSharpness * Time.deltaTime);
            }
            else
            {
                _weaponRecoilLocalPosition = Vector3.Lerp(_weaponRecoilLocalPosition, Vector3.zero, RecoilRestitutionSharpness * Time.deltaTime);
                _accumulatedRecoil = _weaponRecoilLocalPosition;
            }
        }

        public override void SetStatus(WeaponInfo weaponInfo)
        {
            base.SetStatus(weaponInfo);
            _revolverInfo = (RevolverInfo)weaponInfo;
        }

        public override void SetHoldTool()
        {
            base.SetHoldTool();
            _revolverInfo._weaponUpgrade.ChangeStatus(base._statusManage, _revolverInfo._weaponBaseStatus);
        }

        public override void PutItAway()
        {
            base.PutItAway();
            _revolverInfo._weaponUpgrade.DisableStatus(base._statusManage, _revolverInfo._weaponBaseStatus);
        }

    }
}
