using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using System;


namespace Syuntoku.DigMode.Settings
{
    //�u���b�N�̍��W���`�����N�̈ʒu��ۑ�
    public class BlockPositionInfo
    {
        public BlockPositionInfo()
        {
            _chunkPos = new Index3D();
            _chunkInBlockIndex = new Index3D();
        }

        public BlockPositionInfo(BlockPositionInfo refInfo)
        {
            _chunkPos = refInfo._chunkPos;
            _chunkInBlockIndex = refInfo._chunkInBlockIndex;
        }

        public Index3D _chunkPos;
        public Index3D _chunkInBlockIndex;

        public void SetPosition(Index3D chankPos, Index3D blockIndex)
        {
            _chunkPos = chankPos;
            _chunkInBlockIndex = blockIndex;
        }

        public void SetChunk(Vector3 chankPos)
        {
            _chunkPos = new Index3D(chankPos);
        }
        public void SetChunk(Index3D chankPos)
        {
            _chunkPos = new Index3D(chankPos);
        }
        public void SetChunk(int x, int y, int z)
        {
            _chunkPos.x = x;
            _chunkPos.y = y;
            _chunkPos.z = z;
        }

        public void SetIndex(Vector3 blockIndex)
        {
            _chunkInBlockIndex = new Index3D(blockIndex);
        }
        public void SetIndex(int x, int y, int z)
        {
            _chunkInBlockIndex.x = x;
            _chunkInBlockIndex.y = y;
            _chunkInBlockIndex.z = z;
        }
        public void SetIndex(Index3D chankPos)
        {
            _chunkInBlockIndex.x = chankPos.x;
            _chunkInBlockIndex.y = chankPos.y;
            _chunkInBlockIndex.z = chankPos.z;
        }

        /// <summary>
        /// �C���f�b�N�X���X���C�h����@�`�����N�𒴂���ꍇ�͈ړ�����
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public BlockPositionInfo SlideIndex(Index3D vec)
        {
            BlockPositionInfo work = this;

            work._chunkInBlockIndex += vec;

            if (work._chunkInBlockIndex.x >= WorldData._defaultWorldSize.x)
            {
                float slide = work._chunkInBlockIndex.x - WorldData._defaultWorldSize.x;
                work._chunkPos.x++;
                work._chunkInBlockIndex.x = (int)slide;
            }
            if (work._chunkInBlockIndex.y >= WorldData._defaultWorldSize.y)
            {
                float slide = work._chunkInBlockIndex.y - WorldData._defaultWorldSize.y;
                work._chunkPos.y++;
                work._chunkInBlockIndex.y = (int)slide;
            }
            if (work._chunkInBlockIndex.z >= WorldData._defaultWorldSize.z)
            {
                float slide = work._chunkInBlockIndex.z - WorldData._defaultWorldSize.z;
                work._chunkPos.z++;
                work._chunkInBlockIndex.z = (int)slide;
            }

            return work;
        }
        /// <summary>
        /// �C���f�b�N�X���X���C�h����@�`�����N�𒴂���ꍇ�͈ړ�����@X
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public BlockPositionInfo SlideIndexX(float vec)
        {
            BlockPositionInfo work = this;

            work._chunkInBlockIndex.x += (int)vec;

            if (work._chunkInBlockIndex.x >= WorldData._defaultWorldSize.x)
            {
                float slide = work._chunkInBlockIndex.x - WorldData._defaultWorldSize.x;
                work._chunkPos.x++;
                work._chunkInBlockIndex.x = (int)slide;
            }

            return work;
        }
        /// <summary>
        /// �C���f�b�N�X���X���C�h����@�`�����N�𒴂���ꍇ�͈ړ�����@Y
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public BlockPositionInfo SlideIndexY(float vec)
        {
            BlockPositionInfo work = this;

            work._chunkInBlockIndex.y += (int)vec;

            if (work._chunkInBlockIndex.y >= WorldData._defaultWorldSize.y)
            {
                float slide = work._chunkInBlockIndex.y - WorldData._defaultWorldSize.y;
                work._chunkPos.y--;
                work._chunkInBlockIndex.y = (int)slide;
            }

            return work;
        }
        /// <summary>
        /// �C���f�b�N�X���X���C�h����@�`�����N�𒴂���ꍇ�͈ړ�����@Z
        /// </summary>
        /// <param name="vec"></param>
        /// <returns></returns>
        public BlockPositionInfo SlideIndexZ(float vec)
        {
            BlockPositionInfo work = this;

            work._chunkInBlockIndex.z += (int)vec;

            if (work._chunkInBlockIndex.z >= WorldData._defaultWorldSize.z)
            {
                float slide = work._chunkInBlockIndex.z - WorldData._defaultWorldSize.z;
                work._chunkPos.z++;
                work._chunkInBlockIndex.z = (int)slide;
            }

            return work;
        }

        /// <summary>
        /// �`�����N�̏ꏊ�����o��
        /// </summary>
        /// <param name="chunkDatas"></param>
        /// <returns></returns>
        public ChunkData ExtractionWorldData(ChunkData[,,] chunkDatas)
        {
            return chunkDatas[(int)_chunkPos.x, (int)_chunkPos.y, (int)_chunkPos.z];
        }

        /// <summary>
        /// �`�����N�ƃu���b�N�̏ꏊ���Ăяo��
        /// </summary>
        /// <param name="chunkDatas"></param>
        /// <returns></returns>
        public BlockdataInfo ExtractionBlockData(ChunkData[,,] chunkDatas)
        {
            return chunkDatas[(int)_chunkPos.x, (int)_chunkPos.y, (int)_chunkPos.z]._blockdata[(int)_chunkInBlockIndex.x, (int)_chunkInBlockIndex.y, (int)_chunkInBlockIndex.z];
        }
    }

    /// <summary>
    /// �u���b�N�������Ǘ�����N���X
    /// </summary>
    public class WorldData : MonoBehaviour
    {
        #region CashVariables
        [SerializeField] BlockScriptable _blkScriptable;
        [SerializeField] WorldSettingScriptable _worldSettingScriptable;
        [SerializeField] GameObject _chunkPivotPrf;
        [SerializeField] GameObject FieldObj;
        [SerializeField] BlockGenerate _blockGenerate;
        [SerializeField] WorldSettingScriptable _worldSettingScrptble;
        [SerializeField] GameObject _juwelyParent;
        [SerializeField] GameObject _playerObject;
        [SerializeField] GameObject _directionalLight;
        #endregion
        WorldSize _worldsizeSetting;
        ChunkData[,,] _world;
        Light _dirLight;

        float _count = 0;
        Vector3 _ajustPos;
        int _sizeSetting;
        public static Index3D _defaultWorldSize;
        const float DELAYCOUNT = 0.05f;
        const int WORLD_CHANK_PLUS = 53;
        const int WORLD_CHANK_MINUS = -43;
        const int WORLD_CHANK_Y = -245;

        /*
          �u���b�N�����̍œK���ɂ��R�{�̌������ɐ���
         */

        //======================
        //Unity
        //======================
        private void Awake()
        {
            _worldsizeSetting = _worldSettingScrptble.worldSizes[_sizeSetting];
            _ajustPos = new Vector3(_blkScriptable.oneChankSize.x / 2 * _blkScriptable.block_spaceX * 10, _blkScriptable.oneChankSize.y / 2 * _blkScriptable.block_spaceY, _blkScriptable.oneChankSize.z / 2 * _blkScriptable.block_spaceZ * 10);


            _defaultWorldSize = _worldSettingScriptable.worldSizes[0].size;
        }
        private void Start()
        {
            //�u���b�N�����p��������
            _blockGenerate.Initialize(_blkScriptable, _worldSettingScriptable);
            //���[���h�𐶐�
            _world = new ChunkData[_worldsizeSetting.size.x, _worldsizeSetting.size.y, _worldsizeSetting.size.z];
            Ray ray = new Ray();
            ray.origin = _playerObject.transform.position;
            WorldSet();

            _dirLight = _directionalLight.GetComponent<Light>();
        }
        private void Update()
        {
            GenerateCheck();
            _count += Time.deltaTime;

            if (_count < DELAYCOUNT) return;

            UniTask.SwitchToThreadPool();

            for (int i = 0; i < _worldsizeSetting.size.x; i++)
            {
                for (int j = 0; j < _worldsizeSetting.size.y; j++)
                {
                    for (int k = 0; k < _worldsizeSetting.size.z; k++)
                    {
                        if (_world[i,j,k]._state == (int)ChunkData.ChuncState.ENABLE)
                        {
                            //�`�����N���X�V����
                           _world[i, j, k].UpdateWorldDrawBuffer(_blockGenerate, _blkScriptable.oneChankSize);
                        }
                    }
                }
            }
            _count = 0;
        }

        //==============================
        //public
        //==============================
        /// <summary>
        /// ���[���h�̏����f�[�^������
        /// </summary>
        public void WorldSet()
        {
            for (int i = 0; i < _worldsizeSetting.size.x; i++)
            {
                for (int j = 0; j < _worldsizeSetting.size.y; j++)
                {
                    for (int k = 0; k < _worldsizeSetting.size.z; k++)
                    {
                        Index3D chunkPos = new Index3D(i, j, k);
                        Vector3 pivotPos = new Vector3(i * _blkScriptable.oneChankSize.x, -(j * _blkScriptable.oneChankSize.y), k * _blkScriptable.oneChankSize.z) - _ajustPos;
                        ChunkData chunk = new ChunkData();
                        chunk.Initialize();
                        chunk._generateWorldPos = pivotPos;
                        //�`�����N�̍��W���Z�b�g
                        chunk._positionInfo.SetChunk(chunkPos);
                        chunk._positionInfo.SetIndex(i, j, k);
                        _world[i, j, k] = chunk;
                    }
                }
            }

            //���ʂȃf�[�^�𐶐�
            _blockGenerate.SpecialDataSetting(_world);

            //���ʂȃf�[�^���܂߂ău���b�N�̐������s��
            for (int i = 0; i < _worldsizeSetting.size.x; i++)
            {
                for (int j = 0; j < _worldsizeSetting.size.y; j++)
                {
                    for (int k = 0; k < _worldsizeSetting.size.z; k++)
                    {
                        _world[i, j, k]._blockdata = _blockGenerate.Generate(j * _worldSettingScriptable.worldSizes[0].size.y
                            , new Index3D(_world[i, j, k]._generateWorldPos)
                            , _world[i, j, k]);
                        _world[i, j, k].UpdateWorldDrawBuffer(_blockGenerate, _blkScriptable.oneChankSize);
                    }
                }
            }

            GC.Collect();
#if UNITY_EDITOR
            Debug.Log("WorldGenerate OK");
#endif
        }

        GameObject PivotGenerate(Vector3 pivotPos)
        {
            return Instantiate(_chunkPivotPrf, pivotPos, Quaternion.identity);
        }

        /// <summary>
        /// �u���b�N�𐶐�����N���X
        /// </summary>
        void GenerateCheck()
        {
            Vector3 position = _playerObject.transform.position;
#if true 
            Index3D worldIndex = new Index3D();

            for (int i = 0; i < _worldsizeSetting.size.x; i++)
            {
                for (int j = 0; j < _worldsizeSetting.size.y; j++)
                {
                    for (int k = 0; k < _worldsizeSetting.size.z; k++)
                    {
                        //�`�����N���擾
                        worldIndex.SetFullIndex(i, j, k);
                        ChunkData work = GetChukData(worldIndex);

                        //�u���b�N�𐶐����͉������Ȃ�
                        if (work._state == (int)ChunkData.ChuncState.GENERATE) continue;

                        Vector3 vec = work._generateWorldPos - position;


                        if (work._state == (int)ChunkData.ChuncState.ENABLE)
                        {
                            work.UpdateBlockBuffer();

                            //�X�V�����o�b�t�@�����Ƀ��b�V����`�悷��
                            foreach (var item in work._drawMeshBuffer)
                            {
                                for (int mi = 0; mi < item.Key.useMaterial.Length; mi++)
                                {
                                    Graphics.DrawMeshInstanced(item.Key.nowMesh, mi, item.Key.useMaterial[mi], item.Value);
                                }
                            }
                        }

                        //�v���C���[�Ƃ̋����𑪂�
                        if (Mathf.Abs(vec.sqrMagnitude) <= _worldSettingScriptable.enableChankDistance * _worldSettingScriptable.enableChankDistance)
                        {
                            //���������͂��̃`�����N����������Ă��Ȃ��ꍇ
                            if (work._state == (int)ChunkData.ChuncState.DISABLE)
                            {
                                //�`�����N�̐e�𐶐�
                                GameObject pivot = PivotGenerate(work._generateWorldPos);
                                pivot.name = work._positionInfo._chunkInBlockIndex.ConvertToVector3().ToString();
                                pivot.transform.SetParent(FieldObj.transform);
                                //�u���b�N�𐶐�
                                _blockGenerate.GenerateHieralchy(pivot, _world[i, j, k]);
                                work._state = (int)ChunkData.ChuncState.GENERATE;
#if UNITY_EDITOR
                                Debug.Log("GenerateChank" + _world[i, j, k]._positionInfo._chunkPos);
#endif
                            }
                        }
                        //������͂��̃f�[�^����������Ă����ꍇ�̓I�u�W�F�N�g������
                        else if (work._state == (int)ChunkData.ChuncState.ENABLE)
                        {
                            DeleteChunkData(_world[i, j, k]);
                        }
                    }
                }
            }
#endif 
        }

        public static bool IsInWorld(Vector3 position)
        {
            if (position.x >= WORLD_CHANK_PLUS || position.x <= WORLD_CHANK_MINUS) return false;
            if (position.y >= 0 || position.y <= WORLD_CHANK_Y) return false;
            if (position.z >= WORLD_CHANK_PLUS || position.z <= WORLD_CHANK_MINUS) return false;
            return true;
        }

        //==============================
        //private
        //==============================
        /// <summary>
        /// �n���ꂽ�`�����N���폜����
        /// </summary>
        /// <param name="chunkPos"></param>
        void DeleteChunkData(ChunkData chunkPos)
        {
            foreach (Transform chunkData in FieldObj.transform)
            {
                if (chunkData.name == chunkPos._positionInfo._chunkInBlockIndex.ConvertToVector3().ToString())
                {
                    if (chunkPos._state == (int)ChunkData.ChuncState.GENERATE) return;

                    foreach (Transform work in chunkData.transform)
                    {
                        Destroy(work.gameObject);
                    }

                    Destroy(chunkData.gameObject);
                    chunkPos._state = (int)ChunkData.ChuncState.DISABLE;
                }
            }
        }

        public void ChangeHomeLightPower()
        {
            _dirLight.intensity = _worldSettingScriptable.homeLightPower;
        }
        public void ChangeDigLightPower()
        {
            _dirLight.intensity = _worldSettingScriptable.digLightPower;
        }

        /// <summary>
        /// �C���f�b�N�X����`�����N�f�[�^���擾
        /// </summary>
        public ChunkData GetChukData(Index3D index)
        {
            return _world[index.x, index.y, index.z];
        }
    }
}