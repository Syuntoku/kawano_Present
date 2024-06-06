using System.Collections;
using System.Collections.Generic;
using Syuntoku.DigMode.Inventory;
using UnityEngine;

namespace Syuntoku.DigMode.Player.MainSkill
{
    public class MainSkillManage
    {
        public uint _activeFlag;
        public uint _nowSkillFlag;
        public uint _flagState;
        GameObject _respornEfect;
        GameObject _playerObject;
        private InventoryManage _inventoryManage;
        MainSkillBase[] _mainSkillData = new MainSkillBase[(int)MainSkill_Kind_Num.MAX];

        public enum MainSkill_Kind
        {
            WARP = 0x01,
            MAGNET = 0x02,
            OVERHEAT = 0x04,
            BREAKER = 0x08,
            MAX = 4,
        }
        public enum MainSkill_Kind_Num
        {
            WARP = 1,
            MAGNET,
            OVERHEAT,
            BREAKER,
            MAX,
        }

        string[] _mainSkillName ={
         "選択なし",
         "ワープ",
         "マグネット",
         "オーバーヒート",
         "ブレイカー",
        };

        public void Initialize(GameObject playerObject, GameObject respornEfect)
        {
            _playerObject = playerObject;
            _inventoryManage = GameObject.FindObjectOfType<InventoryManage>();

            for (int i = 0; i < (int)MainSkill_Kind.MAX; i++)
            {
                AddFlag();
            }
            InstanceData();
            _respornEfect = respornEfect;
            _mainSkillData[1].Initialize(_respornEfect);
        }

        public void Update()
        {
            foreach (var item in _mainSkillData)
            {
                if (item == null) continue;
                item.Update();
            }
            
            if (UnityEngine.Input.GetKeyDown(KeyCode.T))
            {
                GetUseReset();
                NextChenge(1);
#if UNITY_EDITOR
                Debug.Log(GetSkillName(_nowSkillFlag));
#endif
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.Y))
            {
                GetUseReset();
                NextChenge(-1);
#if UNITY_EDITOR
                Debug.Log(GetSkillName(_nowSkillFlag));
#endif
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.G))
            {
                if (Action(_playerObject))
                {
                    _inventoryManage.OnExecuteMainSkill();
#if UNITY_EDITOR
                    Debug.Log(GetSkillName(_nowSkillFlag));
#endif
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log("スキルを使用できません");
#endif
                }
            }
        }

        /// <summary>
        /// 次のフラグを追加する
        /// </summary>
        public void AddFlag()
        {
            if ((MainSkill_Kind)_activeFlag == MainSkill_Kind.BREAKER) return;

            //フラグが何もない場合フラグを立てる
            if (_flagState == 0x00)
            {
                _flagState++;
                _activeFlag = _flagState;
                _nowSkillFlag++;
                return;
            }

            uint holdflag = _activeFlag;

            holdflag = _activeFlag & ~_flagState;
            _activeFlag = _flagState << 1;
            _flagState++;
            _activeFlag++;
            _activeFlag |= holdflag;
            _flagState = _activeFlag;
            InstanceData();
        }

        /// <summary>
        /// 指定したスキルを追加する
        /// </summary>
        /// <param name="kind"></param>
        public void AddFlag(MainSkill_Kind kind)
        {
            _activeFlag |= (uint)kind;
            InstanceData();
        }

        /// <summary>
        /// 今選択されているスキルを使用
        /// </summary>
        public bool Action(GameObject playerObject)
        {
            return _mainSkillData[GetSkillIndex()].Active(playerObject);
        }

        /// <summary>
        /// 無条件でスキルを使用　*フラグで判定しないため,どのスキルでも使用可能
        /// </summary>
        /// <param name="kind"></param>
        public void SelectAction(GameObject playerObject, MainSkill_Kind_Num kind)
        {
            _mainSkillData[(int)kind].Active(playerObject);
        }

        /// <summary>
        /// 現在のスキルから変更する
        /// </summary>
        /// <param name="amount"></param>
        public void NextChenge(int amount)
        {
            if (_activeFlag == 0x00) return;

            while (true)
            {
                if (Mathf.Sign(amount) > 0)
                {
                    uint nextFlag = _nowSkillFlag << 1;

                    if (nextFlag == (int)MainSkill_Kind.BREAKER << 1 || nextFlag == 0x00)
                    {
                        _nowSkillFlag = (int)MainSkill_Kind.WARP;
                        return;
                    }
                    _nowSkillFlag = nextFlag;

                    if ((nextFlag & _activeFlag) == 0) continue;
                    break;
                }
                else
                {
                    uint nextFlag = _nowSkillFlag >> 1;

                    if (nextFlag == 0x00)
                    {
                        nextFlag = (int)MainSkill_Kind.BREAKER;
                    }

                    if ((nextFlag & _activeFlag) == 0) continue;

                    _nowSkillFlag = nextFlag;

                    break;
                }
            }
        }

        public bool IsActive()
        {
            return _mainSkillData[GetSkillIndex()].IsUse();
        }

        void InstanceData()
        {
            if (((MainSkill_Kind)_activeFlag & MainSkill_Kind.WARP) != 0)
            {
                _mainSkillData[(int)MainSkill_Kind_Num.WARP] = new MainSkill_Warp();
                _mainSkillData[(int)MainSkill_Kind_Num.WARP].Initialize(_respornEfect);
            }

            if (((MainSkill_Kind)_activeFlag & MainSkill_Kind.MAGNET) != 0)
            {
                _mainSkillData[(int)MainSkill_Kind_Num.MAGNET] = new MainSkill_Magnet();
            }

            if (((MainSkill_Kind)_activeFlag & MainSkill_Kind.OVERHEAT) != 0)
            {
                _mainSkillData[(int)MainSkill_Kind_Num.OVERHEAT] = new MainSkill_OverHeat();
            }

            if (((MainSkill_Kind)_activeFlag & MainSkill_Kind.BREAKER) != 0)
            {
                _mainSkillData[(int)MainSkill_Kind_Num.BREAKER] = new MainSkill_Breaker();
            }
        }

        string GetSkillName(uint flag)
        {
            if ((MainSkill_Kind)flag == MainSkill_Kind.WARP)
            {
                return _mainSkillName[(int)MainSkill_Kind_Num.WARP];
            }
            if ((MainSkill_Kind)flag == MainSkill_Kind.MAGNET)
            {
                return _mainSkillName[(int)MainSkill_Kind_Num.MAGNET];
            }
            if ((MainSkill_Kind)flag == MainSkill_Kind.OVERHEAT)
            {
                return _mainSkillName[(int)MainSkill_Kind_Num.OVERHEAT];
            }
            if ((MainSkill_Kind)flag == MainSkill_Kind.BREAKER)
            {
                return _mainSkillName[(int)MainSkill_Kind_Num.BREAKER];
            }

            return "";
        }

        int GetSkillIndex()
        {
            if ((MainSkill_Kind)_nowSkillFlag == MainSkill_Kind.WARP)
            {
                return (int)MainSkill_Kind_Num.WARP;
            }
            if ((MainSkill_Kind)_nowSkillFlag == MainSkill_Kind.MAGNET)
            {
                return (int)MainSkill_Kind_Num.MAGNET;
            }
            if ((MainSkill_Kind)_nowSkillFlag == MainSkill_Kind.OVERHEAT)
            {
                return (int)MainSkill_Kind_Num.OVERHEAT;
            }
            if ((MainSkill_Kind)_nowSkillFlag == MainSkill_Kind.BREAKER)
            {
                return (int)MainSkill_Kind_Num.BREAKER;
            }
            return int.MaxValue;
        }

        public float GetnowCoolTime()
        {
            return _mainSkillData[GetSkillIndex()]._timer;
        }
        public float GetMaxCoolTime()
        {
            return _mainSkillData[GetSkillIndex()]._coolTime;
        }

        public void GetUseReset()
        {
            _mainSkillData[GetSkillIndex()].Reset();
        }
    }
}
