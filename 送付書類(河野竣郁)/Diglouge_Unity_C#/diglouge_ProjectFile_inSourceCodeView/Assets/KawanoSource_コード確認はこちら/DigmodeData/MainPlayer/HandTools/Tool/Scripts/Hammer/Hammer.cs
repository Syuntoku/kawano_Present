using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using Syuntoku.DigMode.Inventory;
using Syuntoku.Status;

namespace Syuntoku.DigMode.Tool
{
    //======================================
    //ハンマー　5*5マス
    //======================================

    class Hammer : ToolBase
    {
        GameObject[] _breakObject;
        ToolWorks toolWorks = new ToolWorks();
        public readonly int FIRST_BREAK = 9;
        public readonly int MAX_BREAK = 25;

        public Hammer()
        {
            _breakObject = new GameObject[MAX_BREAK];
        }

        /// <summary>
        /// Update　掘るインターバルは標準
        /// </summary>
        public override void ToolUpdate()
        {
            base.ToolUpdate();
        }

        /// <summary>
        ///  掘る　
        /// </summary>
        public override bool Dig(GameObject digBlockData, DigStatus digStatus, ToolInfo toolData)
        {
            HammerInfo hammerInfo = (HammerInfo)toolData;
            const int DOUBLE_RAYLENGTH = 2;

            if (digBlockData == null)
            {
                return false;
            }

            if (!_bDig)
            {
                _breakObject[0] = digBlockData;

                Vector3 pos = digBlockData.transform.position - _playerObject.transform.position;

                float dot = Vector3.Dot(new Vector3(1.0f, 0.0f, 0.0f), pos);
                Vector3 cross = Vector3.Cross(new Vector3(1.0f, 0.0f, 0.0f), pos);

                if (Mathf.Abs(dot) <= Mathf.Abs(cross.y))
                {
                    //Debug.Log(dot);
                    //十字方向のブロックを取得
                    _breakObject[1] = toolWorks.RayBlocks(digBlockData.transform.position, (int)Axis.UP);
                    _breakObject[2] = toolWorks.RayBlocks(digBlockData.transform.position, (int)Axis.DOWN);
                    _breakObject[3] = toolWorks.RayBlocks(digBlockData.transform.position, (int)Axis.RIGHT);
                    _breakObject[4] = toolWorks.RayBlocks(digBlockData.transform.position, (int)Axis.LEFT);

                    //十字の左右から上と下のブロックを取得
                    Vector3 rigthtPos = digBlockData.transform.position + Vector3.right;
                    Vector3 LefttPos = digBlockData.transform.position + Vector3.left;

                    _breakObject[5] = toolWorks.RayBlocks(rigthtPos, (int)Axis.UP);
                    _breakObject[6] = toolWorks.RayBlocks(rigthtPos, (int)Axis.DOWN);
                    _breakObject[7] = toolWorks.RayBlocks(LefttPos, (int)Axis.UP);
                    _breakObject[8] = toolWorks.RayBlocks(LefttPos, (int)Axis.DOWN);

                    if (hammerInfo._breakRange >= 2)
                    {

                        Vector3 work;

                        //右方向のブロック取得
                        work = digBlockData.transform.position + Vector3.up;
                        _breakObject[9] = toolWorks.RayBlocks(work, (int)Axis.UP);

                        work = digBlockData.transform.position + Vector3.up * DOUBLE_RAYLENGTH;
                        _breakObject[13] = toolWorks.RayBlocks(work, (int)Axis.RIGHT);
                        work = work + Vector3.right;
                        _breakObject[14] = toolWorks.RayBlocks(work, (int)Axis.RIGHT);

                        work = digBlockData.transform.position + Vector3.up * DOUBLE_RAYLENGTH;
                        _breakObject[15] = toolWorks.RayBlocks(work, (int)Axis.LEFT);
                        work = work + Vector3.left;
                        _breakObject[16] = toolWorks.RayBlocks(work, (int)Axis.LEFT);

                        //左方向を取得
                        work = digBlockData.transform.position + Vector3.down;
                        _breakObject[10] = toolWorks.RayBlocks(work, (int)Axis.DOWN);


                        work = digBlockData.transform.position + Vector3.down * DOUBLE_RAYLENGTH;
                        _breakObject[17] = toolWorks.RayBlocks(work, (int)Axis.RIGHT);
                        work = work + Vector3.right;
                        _breakObject[18] = toolWorks.RayBlocks(work, (int)Axis.RIGHT);

                        work = digBlockData.transform.position + Vector3.down * DOUBLE_RAYLENGTH;
                        _breakObject[19] = toolWorks.RayBlocks(work, (int)Axis.LEFT);
                        work = work + Vector3.left;
                        _breakObject[20] = toolWorks.RayBlocks(work, (int)Axis.LEFT);


                        work = digBlockData.transform.position + Vector3.right;
                        _breakObject[11] = toolWorks.RayBlocks(work, (int)Axis.RIGHT);

                        //右のブロックから上下のブロックを取得
                        work = digBlockData.transform.position + Vector3.right * DOUBLE_RAYLENGTH;
                        _breakObject[21] = toolWorks.RayBlocks(work, (int)Axis.UP);
                        _breakObject[22] = toolWorks.RayBlocks(work, (int)Axis.DOWN);

                        work = digBlockData.transform.position + Vector3.left;
                        _breakObject[12] = toolWorks.RayBlocks(work, (int)Axis.LEFT);

                        //左のブロックから上下のブロックを取得
                        work = digBlockData.transform.position + Vector3.left * DOUBLE_RAYLENGTH;
                        _breakObject[23] = toolWorks.RayBlocks(work, (int)Axis.UP);
                        _breakObject[24] = toolWorks.RayBlocks(work, (int)Axis.DOWN);
                    }
                }
                else
                {

                    //Debug.Log(dot);
                    //十字方向のブロックを取得
                    _breakObject[1] = toolWorks.RayBlocks(digBlockData.transform.position, (int)Axis.UP);
                    _breakObject[2] = toolWorks.RayBlocks(digBlockData.transform.position, (int)Axis.DOWN);
                    _breakObject[3] = toolWorks.RayBlocks(digBlockData.transform.position, (int)Axis.FORWARD);
                    _breakObject[4] = toolWorks.RayBlocks(digBlockData.transform.position, (int)Axis.BACK);

                    //十字の左右から上と下のブロックを取得
                    Vector3 rigthtPos = digBlockData.transform.position + Vector3.forward;
                    Vector3 LefttPos = digBlockData.transform.position + Vector3.back;

                    _breakObject[5] = toolWorks.RayBlocks(rigthtPos, (int)Axis.UP);
                    _breakObject[6] = toolWorks.RayBlocks(rigthtPos, (int)Axis.DOWN);

                    _breakObject[7] = toolWorks.RayBlocks(LefttPos, (int)Axis.UP);
                    _breakObject[8] = toolWorks.RayBlocks(LefttPos, (int)Axis.DOWN);


                    if (hammerInfo._breakRange >= 2)
                    {

                        Vector3 work;

                        //右方向のブロック取得
                        work = digBlockData.transform.position + Vector3.up;
                        _breakObject[9] = toolWorks.RayBlocks(work, (int)Axis.UP);


                        work = digBlockData.transform.position + Vector3.up * 2;
                        _breakObject[13] = toolWorks.RayBlocks(work, (int)Axis.FORWARD);
                        work = work + Vector3.forward;
                        _breakObject[14] = toolWorks.RayBlocks(work, (int)Axis.FORWARD);

                        work = digBlockData.transform.position + Vector3.up * 2;
                        _breakObject[15] = toolWorks.RayBlocks(work, (int)Axis.BACK);
                        work = work + Vector3.back;
                        _breakObject[16] = toolWorks.RayBlocks(work, (int)Axis.BACK);

                        //左方向を取得
                        work = digBlockData.transform.position + Vector3.down;
                        _breakObject[10] = toolWorks.RayBlocks(work, (int)Axis.DOWN);


                        work = digBlockData.transform.position + Vector3.down * 2;
                        _breakObject[17] = toolWorks.RayBlocks(work, (int)Axis.FORWARD);
                        work = work + Vector3.forward;
                        _breakObject[18] = toolWorks.RayBlocks(work, (int)Axis.FORWARD);

                        work = digBlockData.transform.position + Vector3.down * 2;
                        _breakObject[19] = toolWorks.RayBlocks(work, (int)Axis.BACK);
                        work = work + Vector3.back;
                        _breakObject[20] = toolWorks.RayBlocks(work, (int)Axis.BACK);


                        work = digBlockData.transform.position + Vector3.forward;
                        _breakObject[11] = toolWorks.RayBlocks(work, (int)Axis.FORWARD);

                        //右のブロックから上下のブロックを取得
                        work = digBlockData.transform.position + Vector3.forward * 2;
                        _breakObject[21] = toolWorks.RayBlocks(work, (int)Axis.UP);
                        _breakObject[22] = toolWorks.RayBlocks(work, (int)Axis.DOWN);

                        work = digBlockData.transform.position + Vector3.back;
                        _breakObject[12] = toolWorks.RayBlocks(work, (int)Axis.BACK);

                        //左のブロックから上下のブロックを取得
                        work = digBlockData.transform.position + Vector3.back * 2;
                        _breakObject[23] = toolWorks.RayBlocks(work, (int)Axis.UP);
                        _breakObject[24] = toolWorks.RayBlocks(work, (int)Axis.DOWN);
                    }
                }

                DamageManager[] damageData = hammerInfo.GetDamage();

                //壊す 中央
                var noWait = toolWorks.ArrayBreak(_breakObject, damageData[0],_blockManage,_toolData, 0, 1, 0);

                int waitTime = HammerInfo.DELAY_FIRST;
                //壊す 内周
                noWait = toolWorks.ArrayBreak(_breakObject, damageData[1], _blockManage, _toolData, 1, 9, waitTime);

                waitTime += HammerInfo.DELAY_SECOND;

                //壊す 外周
                noWait = toolWorks.ArrayBreak(_breakObject, damageData[2], _blockManage, _toolData, 9, MAX_BREAK, waitTime);
#if UNITY_EDITOR
                Debug.Log("Closs = " + cross);
#endif
                _bDig = true;
                return true;
            }
            return false;
        }
    }
}