using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Inventory.Juwelry;
using Syuntoku.DigMode.Inventory;
using Syuntoku.DigMode.Tool;
using Syuntoku.DigMode.Weapon;
using Syuntoku.DigMode.UI;

namespace Syuntoku.DigMode.Player
{
    /// <summary>
    /// �v���C���[�̎莝���̃C���x���g��
    /// </summary>
    [System.Serializable]
    public class PlayerInventory
    {
        DropManager _dropManager;

        public const int ON_HAND_TOOL_COUNT = 2;
        public const int ON_WEAPON_COUNT = 2;
        
        [SerializeField] JuwelryInventory _juwelryInventory;
        [SerializeField] List<ToolInfo> _toolInventory;
        [SerializeField] List<WeaponInfo> _weaponInventory;

        ToolGenerater _toolGenerater;
        GameObject _playerObject;
        GameObject _dropItemParent;
        MainUI _mainUI;

        ToolInfo[] _equipmentTools;
        WeaponInfo[] _equipmentWeapons;
        public int _maxWeight;

        public PlayerInventory()
        {
            _juwelryInventory = new JuwelryInventory();
            _toolInventory = new List<ToolInfo>();
            _weaponInventory = new List<WeaponInfo>();
            _maxWeight = 300;
            _equipmentTools = new ToolInfo[ON_HAND_TOOL_COUNT];
            _equipmentWeapons = new WeaponInfo[ON_HAND_TOOL_COUNT];
        }

        //===========================================
        //public
        //===========================================
        public void SetMainUI(MainUI mainUI)
        {
            _mainUI = mainUI;
        }

        /// <summary>
        /// ���݂̏d�ʂ��擾
        /// </summary>
        public int WeightCheck()
        {
            int resultWeight = 0;
            uint work = 0;
            //��΂̏d�����v�Z
            int count = 0;

            do
            {
                //��Έ�̏d���Ǝ莝���ɂ��������Z
                resultWeight += (int)(_juwelryInventory.GetJuwelryWeight((JuwelryInventory.JUWELRY_KIND)work)
                                      * _juwelryInventory.GetjuwelryData((JuwelryInventory.JUWELRY_KIND)work));
                count++;
            } while (count < (uint)JuwelryInventory.JUWELRY_KIND.JUWELRY_MAX);

            foreach (ToolInfo item in _toolInventory)
            {
                resultWeight += item._toolStatus.weight;
            }

            foreach (WeaponInfo item in _weaponInventory)
            {
                resultWeight += item._weaponBaseStatus.weight;
            }

            return resultWeight;
        }

        public int GetMaxWeight()
        {
            return _maxWeight;
        }

        public void AddMaxWeight(int addWeight)
        {
            _maxWeight += addWeight;
        }

        public void ChangeEquipmentTool(ToolInfo toolInfo, int equipmetIndex)
        {
            _equipmentTools[equipmetIndex]._isEquipmet = false;
            toolInfo._isEquipmet = true;
            _equipmentTools[equipmetIndex] = toolInfo;
        }

        public void ChangeEquipmentWeapon(WeaponInfo weaponBase, int equipmetIndex)
        {
            _equipmentWeapons[equipmetIndex]._bEquipment = false;
            weaponBase._bEquipment = true;
            _equipmentWeapons[equipmetIndex] = weaponBase;
        }

        /// <summary>
        ///�C���x���g���ɕ�΂�����
        /// </summary>
        /// <param name="getJuwelryKind"></param>
        public void AddItem(JuwelryInventory.JUWELRY_KIND getJuwelryKind)
        {
            Sprite sprite = _juwelryInventory.GetIconData(getJuwelryKind);
            _mainUI.InstanceGetItemUI(sprite, getJuwelryKind.ToString());
            _juwelryInventory.AddJuwelryData(getJuwelryKind);
        }

        /// <summary>
        ///�C���x���g���ɕ��������
        /// </summary>
        public void AddWeaon(WeaponInfo weaponInfo)
        {
            _weaponInventory.Add(weaponInfo);

            for (int i = 0; i < ON_WEAPON_COUNT; i++)
            {
                if(_equipmentWeapons[i] == null)
                {
                    weaponInfo._bEquipment = true;
                    _equipmentWeapons[i] = weaponInfo;
                    return;
                }
            }
        }

        /// <summary>
        ///�C���x���g���ɍ̌@���������
        /// </summary>
        public void AddTool(ToolInfo toolInfo)
        {
            _toolInventory.Add(toolInfo);

            for (int i = 0; i < ON_WEAPON_COUNT; i++)
            {
                if (_equipmentTools[i] == null)
                {
                    toolInfo._isEquipmet = true;
                    _equipmentTools[i] = toolInfo;
                    return;
                }
            }
        }

        public void DropItem(JuwelryInventory.JUWELRY_KIND juwelryKind)
        {
          uint count = _juwelryInventory.GetjuwelryData(juwelryKind);
#if UNITY_EDITOR
            if(count <= 0)
            {
                Debug.Log("�h���b�v�F��΂������Ă��܂���");
                return;
            }
#endif
            _juwelryInventory.RemoveJuwelry(juwelryKind, 1);
            _dropManager.InstanceJuwelry(juwelryKind, _playerObject.transform.position + _playerObject.transform.forward / 10);
        }

        public void DropTool(int index)
        {
#if UNITY_EDITOR
            if (index >= _toolInventory.Count)
            {
                Debug.Log("�h���b�v�F�c�[���̃C���f�b�N�X�𒴂��Ă��܂�");
                return;
            }
#endif
            ToolInfo toolInfo = _toolInventory[index];
            _toolInventory.RemoveAt(index);
            GameObject generateObject = _toolGenerater.InstanceEmptyDataTool(toolInfo._toolStatus.toolKind, _playerObject.transform.position + _playerObject.transform.forward / 10, Quaternion.identity, _dropItemParent.transform);
            generateObject.GetComponent<ToolBase>().SetStatus(toolInfo);
        }

        public void DropWeapon(int index)
        {
#if UNITY_EDITOR
            if (index >= _toolInventory.Count)
            {
                Debug.Log("�h���b�v�F����̃C���f�b�N�X�𒴂��Ă��܂�");
                return;
            }
#endif
            WeaponInfo weaponInfo = _weaponInventory[index];
            _weaponInventory.RemoveAt(index);
            GameObject generateObject = _toolGenerater.InstanceWeapon(weaponInfo._weaponBaseStatus.weaponKind, _playerObject.transform.position + _playerObject.transform.forward / 10, Quaternion.identity, _dropItemParent.transform);
            generateObject.GetComponent<WeaponBase>().SetStatus(weaponInfo);
        }

        //==================================
        //public geter
        //==================================
        public ToolInfo[] GetEquipmentTools()
        {
            return _equipmentTools;
        }

        public WeaponInfo[] GetEquipmentWeapons()
        {
            return _equipmentWeapons;
        }

        public List<ToolInfo> GetToolInventory()
        {
            return _toolInventory;
        }

        public List<WeaponInfo> GetWeaponInventory()
        {
            return _weaponInventory;
        }

        public JuwelryInventory GetjuwelryInventory()
        {
            return _juwelryInventory;
        }
    }
}