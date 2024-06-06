using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Inventory;
using Syuntoku.Status;
using System;

namespace Syuntoku.DigMode.Tool.Unique
{

    public class UniqueCharacteristics
    {
        public enum Rarelity
        {
            NORMAL,
            RARE,
            LEGEND,
        }

        protected UniqueCharacteristicsScriptable _toolUniqueScriptable;
        protected ParkConditionsManage _parkConditionsManage;
        protected StatusManage _baseStatus;

        public uint activeFlag_Normal;
        public uint activeFlag_Rare;
        public uint activeFlag_Legend;

        protected int activeCount;
        protected int maxActive;

        virtual public void Initialize(UniqueCharacteristicsScriptable toolScriptable, ParkConditionsManage parkConditionsManage, StatusManage baseStatus)
        {
            activeFlag_Normal = 0x00;
            activeFlag_Rare = 0x00;
            activeFlag_Legend = 0x00;
            activeCount = 0;
            _toolUniqueScriptable = toolScriptable;
            _parkConditionsManage = parkConditionsManage;
            _baseStatus = baseStatus;
        }

        public void ZeroInitialize()
        {
            activeFlag_Normal = 0x00;
            activeFlag_Rare = 0x00;
            activeFlag_Legend = 0x00;
            activeCount = 0;
        }

        /// <summary>
        /// ���̑����̌ŗL�����߂�
        /// </summary>
        virtual public void RollUniqueData()
        {
            maxEmptyCheck();
            activeFlag_Normal = 0x00;
            activeFlag_Rare = 0x00;
            activeFlag_Legend = 0x00;
            activeCount = 0;
        }

        /// <summary>
        /// �c�[������������
        /// </summary>
        virtual public void ActiveSetting(ToolInfo toolData)
        {

        }

        /// <summary>
        /// �c�[���������Ȃ��Ȃ�����
        /// </summary>
        virtual public void DisableSetiing(ToolInfo toolData)
        {

        }

        /// <summary>
        /// �w�肵���ŗL�X�L�����N������
        /// </summary>
        /// <param name="rarelity"></param>
        /// <param name="num"></param>
        virtual public bool Active(StatusManage magnificationStatus,Rarelity rarelity , int num)
        {
            return false;
        }

        virtual public string ExplainSet()
        {
            return "null";
        }

        /// <summary>
        /// �L���ɂȂ�ŗL�̘g�𒲂ׂ�
        /// </summary>
        public void maxEmptyCheck()
        {
            maxActive = 0;

            foreach (var item in _toolUniqueScriptable.uniqueSettings)
            {
               if(_parkConditionsManage.waveCount >= item.startWaveCount && _parkConditionsManage.waveCount <= item.endWaveCount)
                {
                    //��g�ڂ͊m��
                    maxActive++;

                    if (item.SecondFlame <= GameUtility.GetRandomParsent()) maxActive++;
                    if (item.ThirdFlame <= GameUtility.GetRandomParsent()) maxActive++;
                    break;
                }
            }
#if UNITY_EDITOR
            Debug.Log(maxActive);
#endif
        }

        /// <summary>
        /// �ۗL�ł���ŗL�f�[�^�̌����`�F�b�N����
        /// </summary>
        /// <returns>�g���J���Ă���@false : �g���J���Ă��Ȃ��@true </returns>
        protected bool ActiveEmptyFrameCheck()
        {
            activeCount++;

            if(activeCount >= maxActive)
            {
                return true;
            }

            return false;
        }

        public static UniqueCharacteristics InstanceUniqueData(ToolGenerater.ToolName ToolKind, UniqueCharacteristicsScriptable toolUniqueScriptable,ParkConditionsManage parkConditionsManage, StatusManage statusBase)
        {
            UniqueCharacteristics uniqueCharacteristics = null;

            switch (ToolKind)
            {
                case ToolGenerater.ToolName.PICK_AXE:
                    uniqueCharacteristics = new Unique_PickAxe();
                    break;
                case ToolGenerater.ToolName.HAMMER:
                    uniqueCharacteristics = new Unique_Hammer();
                    break;
                case ToolGenerater.ToolName.GUN:
                    uniqueCharacteristics = new Unique_Gun();
                    break;
            }

            uniqueCharacteristics.Initialize(toolUniqueScriptable, parkConditionsManage, statusBase);
            uniqueCharacteristics.maxEmptyCheck();
            uniqueCharacteristics.RollUniqueData();

            return uniqueCharacteristics;
        }
    }
}
