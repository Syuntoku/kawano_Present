using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Syuntoku.DEBUG;
using Syuntoku.DigMode.Tool;
using Syuntoku.DigMode.Settings;
using Syuntoku.DigMode.Input;
using Syuntoku.DigMode.UI;
using Syuntoku.DigMode.Inventory;
using System.Threading;
using Syuntoku.Status;

namespace Syuntoku.DigMode.Player
{
    //=====================================
    //ñ⁄ê¸ä«óùÉNÉâÉX
    //=====================================
    public class LineOfSight : MonoBehaviour
    {
        #region VARIABLES

        [SerializeField] Camera _camera;

        [SerializeField] PlayerData _playerData;

        [SerializeField] ToolSetHand toolSetHand;

        [SerializeField] UIManage _uIManage;

        [SerializeField] GameSetting gameSetting;

        [SerializeField] ParkConditionsManage parkConditions;

        [SerializeField] InventoryManage _inventoryManage;

        [SerializeField] StatusManage _statusManage;

        [SerializeField] BlockManage _blockManage;

        public LayerMask m_aylayerMask;

        public float m_RayLength = 5;

        public static bool bBreakerSkill;
        Ray m_ray;

        bool _bNotDig;
        #endregion

        void Start()
        {
            m_RayLength = _playerData.sightLength;

            m_ray = new Ray(_camera.transform.position, _camera.transform.forward * m_RayLength);
        }

        public void ActiveDig(bool set)
        {
            _bNotDig = set;
        }

    }
}