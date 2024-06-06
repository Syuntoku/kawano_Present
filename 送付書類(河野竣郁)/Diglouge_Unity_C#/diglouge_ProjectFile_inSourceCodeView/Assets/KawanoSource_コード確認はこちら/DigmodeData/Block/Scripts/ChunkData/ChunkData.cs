using System.Collections;
using System.Collections.Generic;
using Syuntoku.DigMode.Settings;
using Syuntoku.DigMode;
using UnityEngine;
using Cysharp.Threading.Tasks;
using static BlockScriptable;

/// <summary>
/// 一チャンクの情報が入っています
/// </summary>
public class ChunkData
{
    public int _state;
    public Vector3 _generateWorldPos;
    public BlockPositionInfo _positionInfo;
    public BlockdataInfo[,,] _blockdata;
    public FieldBlockStatus[,,] _specialBlockData; //ブロックとは関係ない配置のデータ
    public bool bShop;
    public bool bGenerated;
    //描画用のMeshの情報を加える
    public Dictionary<FieldBlockStatus, List<Matrix4x4>> _drawMeshBuffer;
    public Dictionary<FieldBlockStatus, List<Matrix4x4>> _newDrawMeshBuffer;

    //マテリアル情報を格納する際の最大キャッシュ量
    const int CASH_MATERIAL_SIZE = 32;

    //DrawMeshInstancedのBuffer
    List<FieldBlockStatus> _chunkinMaterials;
    List<Matrix4x4>[] _sameKindBlockPos;

    public enum ChuncState
    {
        ENABLE,
        DISABLE,
        GENERATE,
    }

    public ChunkData()
    {
        _state = (int)ChuncState.DISABLE;
        _positionInfo = new BlockPositionInfo();
    }

    public ChunkData(ChunkData data)
    {
        _state = data._state;
        _positionInfo = new BlockPositionInfo(data._positionInfo);
    }

    public void Initialize()
    {
        _drawMeshBuffer = new Dictionary<FieldBlockStatus, List<Matrix4x4>>();
        _newDrawMeshBuffer = new Dictionary<FieldBlockStatus, List<Matrix4x4>>();
        _chunkinMaterials = new List<FieldBlockStatus>();
        _sameKindBlockPos = new List<Matrix4x4>[CASH_MATERIAL_SIZE];
        for (int i = 0; i < _sameKindBlockPos.Length; i++)
        {
            _sameKindBlockPos[i] = new List<Matrix4x4>();
        }
    }

    public void GenerateComplate()
    {
        _state = (int)ChuncState.ENABLE;
    }

    public void UpdateBlockBuffer()
    {
        if (_newDrawMeshBuffer.Count == 0 || _newDrawMeshBuffer == null) return;
        if (bGenerated) return;
        _drawMeshBuffer.Clear();
        _drawMeshBuffer = new Dictionary<FieldBlockStatus, List<Matrix4x4>>(_newDrawMeshBuffer);
    }

    /// <summary>
    /// DrawMeshで使うオブジェクトデータを設定する
    /// 別スレッドでワールドの更新を行う
    /// </summary>
    public void UpdateWorldDrawBuffer(BlockGenerate blockGenerate, Vector3 chunkSize)
    {
        if (_state != (int)ChuncState.ENABLE) return;
        if(bGenerated) return;

        bGenerated = true;
        _chunkinMaterials.Clear();
        _newDrawMeshBuffer.Clear();

        for (int i = 0; i < _sameKindBlockPos.Length; i++)
        {
            _sameKindBlockPos[i].Clear();
        }

        for (int i = 0; i < chunkSize.x; i++)
        {
            for (int j = 0; j < chunkSize.y; j++)
            {
                for (int k = 0; k < chunkSize.z; k++)
                {
                    if (_blockdata[i, j, k].category == Category.SPECIAL) continue;
                    if (_blockdata[i, j, k].category == Category.AIR) continue;
                    FieldBlockStatus work = _blockdata[i, j, k].fieldBlockStatus;

                    if (_chunkinMaterials.Count == 0)
                    {
                        _chunkinMaterials.Add(work);
                    }
                    bool boolAdd = false;

                    for (int im = 0; im < _chunkinMaterials.Count; im++)
                    {
                        if (_chunkinMaterials[im] == work)
                        {
                            Matrix4x4 matrix4X4 = new Matrix4x4();
                            matrix4X4.SetTRS(blockGenerate.GetWorldInstanceAjust(new Index3D(_generateWorldPos), _blockdata[i, j, k].fieldBlockStatus.positionInfo._chunkInBlockIndex), Quaternion.identity, _blockdata[i, j, k].fieldBlockStatus.scale);
                            _sameKindBlockPos[im].Add(matrix4X4);
                            //Listのindexを保存する
                            work.bufferListIndex = _sameKindBlockPos.Length - 1;
                            boolAdd = true;
                            break;
                        }
                    }

                    if (!boolAdd)
                    {
                        //検索したマテリアルがない場合フィルター候補として追加する
                        _chunkinMaterials.Add(work);
                        Matrix4x4 matrix4X4 = new Matrix4x4();
                        matrix4X4.SetTRS(blockGenerate.GetWorldInstanceAjust(new Index3D(_generateWorldPos), _blockdata[i, j, k].fieldBlockStatus.positionInfo._chunkInBlockIndex), Quaternion.identity, _blockdata[i, j, k].fieldBlockStatus.scale);
                        _sameKindBlockPos[_chunkinMaterials.Count - 1].Add(matrix4X4);
                    }

                    
                    if(_blockdata[i, j, k].category == Category.JUWELRY)
                    {
                        FieldBlockStatus fieldBlockStatus = new FieldBlockStatus();
                        //宝石のメッシュを登録
                        JuwelryMeshData  mesh = blockGenerate._blkScriptable.GetBreakJuwelryMesh(_blockdata[i, j, k].dropSetting.dropJuwelryKind);
                        fieldBlockStatus.nowMesh = mesh.meshNormal;
                        fieldBlockStatus.useMaterial = mesh.materials;
                        fieldBlockStatus.positionInfo = _blockdata[i, j, k].fieldBlockStatus.positionInfo;
                        boolAdd = false;

                        for (int im = 0; im < _chunkinMaterials.Count; im++)
                        {
                            if (_chunkinMaterials[im] == fieldBlockStatus)
                            {
                                Matrix4x4 matrix4X4 = new Matrix4x4();
                                matrix4X4.SetTRS(blockGenerate.GetWorldInstanceAjust(new Index3D(_generateWorldPos), _blockdata[i, j, k].fieldBlockStatus.positionInfo._chunkInBlockIndex), Quaternion.identity, _blockdata[i, j, k].fieldBlockStatus.scale);
                                _sameKindBlockPos[im].Add(matrix4X4);
                                //Listのindexを保存する
                                work.bufferListIndex = _sameKindBlockPos.Length - 1;
                                boolAdd = true;
                                break;
                            }
                        }

                        if (!boolAdd)
                        {
                            //宝石も描画対象にする
                            _chunkinMaterials.Add(fieldBlockStatus);
                            Matrix4x4 matrix4X4 = new Matrix4x4();
                            matrix4X4.SetTRS(blockGenerate.GetWorldInstanceAjust(new Index3D(_generateWorldPos), _blockdata[i, j, k].fieldBlockStatus.positionInfo._chunkInBlockIndex), Quaternion.identity, _blockdata[i, j, k].fieldBlockStatus.scale);
                            _sameKindBlockPos[_chunkinMaterials.Count - 1].Add(matrix4X4);
                        }
                    }
                }
            }
        }

        //バッファに登録
        int count = 0;
        foreach (FieldBlockStatus work in _chunkinMaterials)
        {
            _newDrawMeshBuffer.Add(work, _sameKindBlockPos[count]);
            count++;
        }
        bGenerated = false;
    }
}

