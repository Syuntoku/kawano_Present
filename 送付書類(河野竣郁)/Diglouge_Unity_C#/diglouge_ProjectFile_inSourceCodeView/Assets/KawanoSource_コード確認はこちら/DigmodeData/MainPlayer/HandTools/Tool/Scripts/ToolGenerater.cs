using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Inventory;
using Syuntoku.DigMode.Tool.Unique;
using Syuntoku.DigMode.Tool.Scriptable;
using Syuntoku.Status;
using Syuntoku.DigMode.Weapon;

namespace Syuntoku.DigMode.Tool
{
    public class ToolGenerater : MonoBehaviour
    {
        public enum ToolName
        {
            PICK_AXE,
            HAMMER,
            GUN,
            MAX,
        }

        public enum WeaponName
        {
            Revolver,
            Riffle,
            Shotgun,
            Minigun,
            Railgun,
            Rocketlauncher,
        }

        [SerializeField] ParkConditionsManage _conditionsManage;
        [SerializeField] StatusManage _statusManage;
        [SerializeField] BlockManage _blockManage;
        [SerializeField] UniqueCharacteristicsScriptable _characteristicsScriptable;

        //ツールのデータ
        [SerializeField] PickAxeInfoScriptable _pickAxeInfoScriptable;
        [SerializeField] HammerInfoScriptable _hammerInfoScriptable;
        [SerializeField] GunInfoScriptable _gunInfoScriptable;

        //武器のデータ
        [SerializeField] RevolverScriptable _revolverScriptable;
        [SerializeField] MinigunScriptable _minigunScriptable;
        [SerializeField] RiffleScriptable _riffleScriptable;
        [SerializeField] RailgunScriptable _railgunScriptable;
        [SerializeField] RocketLauncherScriptable _rocketLauncherScriptable;
        [SerializeField] ShotgunScriptable _shotgunScriptable;

        public const string ATTACH_OBJECT_NAME = "ToolGenerator";
        /// <summary>
        /// ツール情報が入っていないPrefabObjectを生成
        /// </summary>
        public GameObject InstanceEmptyDataTool(ToolName generateKind, Vector3 position, Quaternion rotate, Transform parent)
        {
            switch (generateKind)
            {
                case ToolName.PICK_AXE:
                    return Instantiate(_pickAxeInfoScriptable.tooldata.toolObject, position, rotate, parent);
                case ToolName.HAMMER:
                    return Instantiate(_hammerInfoScriptable.tooldata.toolObject, position, rotate, parent);
                case ToolName.GUN:
                    return Instantiate(_gunInfoScriptable.tooldata.toolObject, position, rotate, parent);
#if UNITY_EDITOR
                default:
                    Debug.Log("ツールのIndexを超えています");
                    break;
#endif
            }
            return null;
        }

        /// <summary>
        /// ツールのデータを生成
        /// </summary>
        /// <param name="generateKind"></param>
        /// <returns></returns>
        public ToolInfo InstanceToolData(ToolName generateKind)
        {
            ToolInfo toolInfo = new ToolInfo();

            switch (generateKind)
            {
                case ToolName.PICK_AXE:
                    PickAxeInfo pickAxeInfo = new PickAxeInfo();
                    pickAxeInfo.SetUp(_pickAxeInfoScriptable);
                    toolInfo = pickAxeInfo;
                    break;

                case ToolName.HAMMER:
                    HammerInfo hammerInfo = new HammerInfo();
                    hammerInfo.Setup(_hammerInfoScriptable);
                    toolInfo = hammerInfo;
                    break;

                case ToolName.GUN:
                    GunInfo gunInfo = new GunInfo();
                    gunInfo.Setup(_gunInfoScriptable);
                    toolInfo = gunInfo;
                    break;
            }

            toolInfo.Initialize(_characteristicsScriptable, _conditionsManage, _statusManage, gameObject);
            toolInfo.ActiveOrLevelUp();
            return toolInfo;
        }

        /// <summary>
        /// 武器のゲームオブジェクトを生成
        /// </summary>
        /// <param name="weaponInfo"></param>
        /// <param name="position"></param>
        /// <param name="rotate"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject InstanceWeapon(WeaponInfo weaponInfo, Vector3 position, Quaternion rotate, Transform parent)
        {
            GameObject gameObject = null;
            WeaponBase weaponData = null;
#if UNITY_EDITOR
            if (weaponInfo == null)
            {
                Debug.Log("武器データがありません");
                return null;
            }
#endif
            switch (weaponInfo._weaponBaseStatus.weaponKind)
            {
                case WeaponName.Revolver:
                    gameObject = Instantiate(_revolverScriptable.weaponPrf, position, rotate, parent);
                    weaponData = gameObject.GetComponent<WeaponBase>();
                    weaponData.Initialize(_statusManage);
                    break;
                case WeaponName.Riffle:
                    gameObject = Instantiate(_riffleScriptable.weaponPrf, position, rotate, parent);
                    weaponData = gameObject.GetComponent<WeaponBase>();
                    weaponData.Initialize(_statusManage);
                    break;
                case WeaponName.Shotgun:
                    gameObject = Instantiate(_shotgunScriptable.weaponPrf, position, rotate, parent);
                    weaponData = gameObject.GetComponent<WeaponBase>();
                    weaponData.Initialize(_statusManage);
                    break;
                case WeaponName.Minigun:
                    gameObject = Instantiate(_minigunScriptable.weaponPrf, position, rotate, parent);
                    weaponData = gameObject.GetComponent<WeaponBase>();
                    weaponData.Initialize(_statusManage);
                    break;
                case WeaponName.Railgun:
                    gameObject = Instantiate(_railgunScriptable.weaponPrf, position, rotate, parent);
                    weaponData = gameObject.GetComponent<WeaponBase>();
                    weaponData.Initialize(_statusManage);
                    break;
                case WeaponName.Rocketlauncher:
                    gameObject = Instantiate(_rocketLauncherScriptable.weaponPrf, position, rotate, parent);
                    weaponData = gameObject.GetComponent<WeaponBase>();
                    weaponData.Initialize(_statusManage);
                    break;
#if UNITY_EDITOR
                default:
                    Debug.Log("武器のIndexを超えています");
                    return null;
#endif
            }
            weaponData.SetStatus(weaponInfo);
            return gameObject;
        }

        /// <summary>
        /// 武器のデータを生成しゲームオブジェクトを生成
        /// </summary>
        /// <param name="weaponInfo"></param>
        /// <param name="position"></param>
        /// <param name="rotate"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public GameObject InstanceWeapon(WeaponName weaponName, Vector3 position, Quaternion rotate, Transform parent)
        {
            GameObject gameObject = null;
            WeaponBase weaponData = null;
            WeaponInfo weaponInfo;

            weaponInfo = InstanceWeaponData(weaponName);

            switch (weaponName)
            {
                case WeaponName.Revolver:
                    gameObject = Instantiate(_revolverScriptable.weaponPrf, position, rotate, parent);
                    weaponData = gameObject.GetComponent<WeaponBase>();
                    weaponData.Initialize(_statusManage);
                    break;
                case WeaponName.Riffle:
                    gameObject = Instantiate(_riffleScriptable.weaponPrf, position, rotate, parent);
                    weaponData = gameObject.GetComponent<WeaponBase>();
                    weaponData.Initialize(_statusManage);
                    break;
                case WeaponName.Shotgun:
                    gameObject = Instantiate(_shotgunScriptable.weaponPrf, position, rotate, parent);
                    weaponData = gameObject.GetComponent<WeaponBase>();
                    weaponData.Initialize(_statusManage);
                    break;
                case WeaponName.Minigun:
                    gameObject = Instantiate(_minigunScriptable.weaponPrf, position, rotate, parent);
                    weaponData = gameObject.GetComponent<WeaponBase>();
                    weaponData.Initialize(_statusManage);
                    break;
                case WeaponName.Railgun:
                    gameObject = Instantiate(_railgunScriptable.weaponPrf, position, rotate, parent);
                    weaponData = gameObject.GetComponent<WeaponBase>();
                    weaponData.Initialize(_statusManage);
                    break;
                case WeaponName.Rocketlauncher:
                    gameObject = Instantiate(_rocketLauncherScriptable.weaponPrf, position, rotate, parent);
                    weaponData = gameObject.GetComponent<WeaponBase>();
                    weaponData.Initialize(_statusManage);
                    break;
#if UNITY_EDITOR
                default:
                    Debug.Log("武器のIndexを超えています");
                    return null;
#endif
            }
            weaponData.SetStatus(weaponInfo);
            return gameObject;
        }

        /// <summary>
        /// ツールの武器データのみを生成
        /// </summary>
        /// <param name="weaponName"></param>
        /// <returns></returns>
        public WeaponInfo InstanceWeaponData(WeaponName weaponName)
        {
            switch (weaponName)
            {
                case WeaponName.Revolver:
                    return new RevolverInfo(_revolverScriptable);
                case WeaponName.Riffle:
                    return new RiffleInfo(_riffleScriptable);
                case WeaponName.Shotgun:
                    return new ShotgunInfo(_shotgunScriptable);
                case WeaponName.Minigun:
                    return new MinigunInfo(_minigunScriptable);
                case WeaponName.Railgun:
                    return new RailgunInfo(_railgunScriptable);
                case WeaponName.Rocketlauncher:
                    return new RocketLaucherInfo(_rocketLauncherScriptable);
                default:
#if UNITY_EDITOR
                    Debug.Log("武器のIndexを超えています");
#endif
                    return null;
            }
        }
    }
}
