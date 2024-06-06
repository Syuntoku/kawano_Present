using System;
using System.Collections.Generic;
using Syuntoku.DigMode.Settings;
using UnityEngine;

namespace Syuntoku.DigMode
{
    /// <summary>
    /// ゲーム中のブロック状態
    /// このデータを元に描画します
    /// </summary>
    public class FieldBlockStatus
    {
        public FieldBlockStatus()
        {
            instancePostion = new Index3D();
            scale = Vector3.one;
            positionInfo = new BlockPositionInfo();
        }

        public BlockPositionInfo positionInfo;
        public Index3D instancePostion;
        public Vector3 scale;
        public Mesh nowMesh;
        public Material[] useMaterial;

        //バッファに必要な情報
        public int bufferListIndex;

        /// <summary>
        /// 必要なデータのみコピーする
        /// ブロックのポジションなど変更禁止
        /// </summary>
        public static FieldBlockStatus CopyData(FieldBlockStatus copyData)
        {
            FieldBlockStatus outData = new FieldBlockStatus();
            outData.scale = copyData.scale;
            outData.nowMesh = copyData.nowMesh;
            outData.useMaterial = copyData.useMaterial;
            return outData;
        }

        public static bool operator ==(FieldBlockStatus fieldBlockStatus, FieldBlockStatus check)
        {
            if (fieldBlockStatus.useMaterial != check.useMaterial) return false;
            if (fieldBlockStatus.nowMesh != check.nowMesh) return false;
            if (fieldBlockStatus.scale != check.scale) return false;
            return true;
        }
        public static bool operator !=(FieldBlockStatus fieldBlockStatus, FieldBlockStatus check)
        {
            if (fieldBlockStatus.useMaterial != check.useMaterial) return true;
            if (fieldBlockStatus.nowMesh != check.nowMesh) return true;
            if (fieldBlockStatus.scale != check.scale) return true;
            return false;
        }

        public override bool Equals(object obj)
        {
            return obj is FieldBlockStatus status &&
                   EqualityComparer<BlockPositionInfo>.Default.Equals(positionInfo, status.positionInfo) &&
                   instancePostion.Equals(status.instancePostion) &&
                   scale.Equals(status.scale) &&
                   EqualityComparer<Mesh>.Default.Equals(nowMesh, status.nowMesh) &&
                   EqualityComparer<Material[]>.Default.Equals(useMaterial, status.useMaterial);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(positionInfo, instancePostion, scale, nowMesh, useMaterial);
        }
    }
}
