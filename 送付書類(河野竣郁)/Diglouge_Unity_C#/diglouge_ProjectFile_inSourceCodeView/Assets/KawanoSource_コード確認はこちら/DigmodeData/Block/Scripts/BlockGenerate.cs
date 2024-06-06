using UnityEngine;
using Cysharp.Threading.Tasks;
using Syuntoku.DigMode.Settings;
using System.Threading;

//===================================
//�u���b�N�̐����Ɋւ���ݒ�
//===================================
namespace Syuntoku.DigMode
{
    enum BLOCKKIND
    {
        BLOCK,
        JUWELRY,
        SPESIAL,
    }

    enum SPECIALGENERATE
    {
        TRESUTURE_CHEST,
        SHOP_CHARACTOR,
    }

    enum GenerateCheck
    {
        NOT_SEARCH,
        GENERATED,
    }

    enum Direction
    {
        UP,
        DOWN,
        RIGHT,
        LEFT,
        FORWARD,
        BACK,
        MAX,
    }

    /// <summary>
    /// �u���b�N�̃f�[�^�����{�C���X�^���X���p�N���X
    /// </summary>
    public class BlockGenerate : MonoBehaviour
    {
        [SerializeField] ShopCharactorGenerate _shopCharactorGenerate;
        CancellationToken _token;
        CancellationTokenSource _cts;
        public BlockScriptable _blkScriptable;
        WorldSettingScriptable _worldSettingScriptable;
        BlockdataInfo[,,] _work;
        Vector3 _arraySize;
        int _delayFlameCount;
        readonly Index3D GENERATE_AJUST_POS = new Index3D(5, 5, 5);
        const int DELAY_BLOCK = 100;
        const int HALF = 2;

        //===========================
        //Unity
        //===========================
        private void OnApplicationQuit()
        {
            if (_cts == null) return;
            _cts.Cancel();
        }

        //===========================
        //public
        //===========================
        public void Initialize(BlockScriptable scriptableData
            , WorldSettingScriptable worldSettingScriptable)
        {
            _blkScriptable = scriptableData;
            _worldSettingScriptable = worldSettingScriptable;
            _arraySize = scriptableData.oneChankSize;
            _delayFlameCount = 0;
            _cts = new CancellationTokenSource();
            _token = _cts.Token;
#if UNITY_EDITOR
            Debug.Log("BlockGenerateInit OK");
#endif
        }

        //==================================
        //�f�[�^����
        //==================================
        public BlockdataInfo[,,] Generate(int depth, Index3D chunkPos, ChunkData chunkData)
        {
            //������������
            _work = new BlockdataInfo[(int)_arraySize.x, (int)_arraySize.y, (int)_arraySize.z];
            BlockdataInfo workData = null;

            BlockInitialize();
            //�󂯎�����K�w����K�w�ݒ���w�肷��ԍ����󂯎��
            int hirarchySettingIndex = GetBlockProbabilityIndex(depth);
#if UNITY_EDITOR
            Debug.Log(depth);
#endif
            BaseBlockSet(chunkPos.y);

            //�R�s�[�p�̃x�[�X�f�[�^���쐬
            workData = new BlockdataInfo();
            //�J�e�S���["block"���i�[
            BlockScriptable.Category cateGory = BlockScriptable.Category.NORMAL;
            
            //=====================================
            //�T�u�̃u���b�N�𐶐�
            //=====================================
            for (int i = 0; i < _blkScriptable.blockData.Length; i++)
            {
                //�󂯎�����K�w�ł̐������A�N�e�B�u�ł͂Ȃ��ꍇ�͎��֐i��
                if (!IsGenerateBlockActive(depth, (int)BLOCKKIND.BLOCK, i)) continue;
                //�x�[�X�f�[�^���u���b�N�p�ɏ�����
                workData.Initialize(_blkScriptable.blockData[i].name
                    , (int)(_blkScriptable.blockData[i].hp * _blkScriptable.blockData[i].blockHardnessMagnification[hirarchySettingIndex].hardPlibability)
                    , cateGory
                    , _blkScriptable.blockData[i].blockKind
                    , _blkScriptable.blockData[i].objectData
                    , _blkScriptable.defaultMesh);
                //�T�u�̃u���b�N�𐶐�
                RandomBlockSetting(_work
                    , workData
                    , _blkScriptable.blockData[i].brockProbabilityData[hirarchySettingIndex].probility
                    , _blkScriptable.blockData[i].brockProbabilityData[hirarchySettingIndex].probabilityDecrease
                    , _blkScriptable.blockData[i].brockProbabilityData[hirarchySettingIndex].generateNum);
            }

            //======================================
            //��΂�ݒ�
            //======================================

            //�J�e�S���["block"���i�[
            cateGory = BlockScriptable.Category.JUWELRY;

            for (int i = 0; i < _blkScriptable.jewelryData.Length; i++)
            {
                //�󂯎�����K�w�ł̕�ΐݒ肪�A�N�e�B�u�ł͂Ȃ��ꍇ�͎��֐i��
                if (!IsGenerateBlockActive(depth, (int)BLOCKKIND.JUWELRY, i)) continue;
                //�x�[�X�f�[�^���Ηp�ɏ�����
                workData.Initialize(_blkScriptable.jewelryData[i].name
                    , (int)(_blkScriptable.jewelryData[i].hp * _blkScriptable.jewelryData[i].blockHardnessMagnification[hirarchySettingIndex].hardPlibability)
                    , cateGory
                    , _blkScriptable.jewelryData[i].dropSetting);
#if UNITY_EDITOR
                if (_blkScriptable.jewelryData[i].brockProbabilityData.Length <= hirarchySettingIndex)
                {
                    Debug.Log("IndexError");
                }
#endif
                //��΂̐���
                RandomJuwerySetting(
                    _work
                    , workData
                    , _blkScriptable.jewelryData[i].brockProbabilityData[hirarchySettingIndex].generateNum);
            }
            //=======================================
            //���ʂȃI�u�W�F�N�g�𐶐�����
            //=======================================
            //�J�e�S���["Special"���i�[
            cateGory = BlockScriptable.Category.SPECIAL;

            //�󔠃f�[�^�𐶐�
            workData.spescialCategory = BlockScriptable.SpescialCategory.TRESUREBOX;
            workData.Initialize(_blkScriptable.tresureChest.name
                , _blkScriptable.tresureChest.hp
                , cateGory
                , _blkScriptable.tresureChest.objectData
                , _blkScriptable.defaultMesh);
            workData.dropSetting = _blkScriptable.tresureChest.dropSetting;
            GenerateTureChesst(_work, workData);

            //�����u���b�N�f�[�^�𐶐�
            workData.spescialCategory = BlockScriptable.SpescialCategory.EXPLOSION_BLOCK;

            workData.Initialize(_blkScriptable.explosionBlock.name
                , _blkScriptable.explosionBlock.hp
                , cateGory
                , _blkScriptable.explosionBlock.objectData
                , _blkScriptable.defaultMesh);
            workData.dropSetting = null;
            RandomBlockSetting(_work, workData, _blkScriptable.explosionBlock.generateCount);

            //�񕜃u���b�N�f�[�^�𐶐�
            workData.spescialCategory = BlockScriptable.SpescialCategory.HEAL_BLOCK;

            workData.Initialize(_blkScriptable.healBlock.name
                , _blkScriptable.healBlock.hp
                , cateGory
                , _blkScriptable.healBlock.objectData
                , _blkScriptable.defaultMesh);
            workData.dropSetting = null;
            RandomBlockSetting(_work, workData, _blkScriptable.healBlock.generateCount);

            //�V���b�v�̃L�����N�^�[�𐶐�
            if (_blkScriptable.shopBlock.bActive)
            {
                workData.spescialCategory = BlockScriptable.SpescialCategory.SHOPBLOCK;

                if (chunkData.bShop) _shopCharactorGenerate.DataGenerate(_work);
            }

            //���Ȃ��u���b�N�𐶐�����
            UnBreakGenerate(_work, chunkData._positionInfo._chunkPos);

            //�ォ��ʂ��悤�Ɍ���������(�X�^�[�g�n�_)
            Index3D chunkPosWork = chunkData._positionInfo._chunkPos;
            if (chunkPosWork.x == 5
                && chunkPosWork.y == 0
                && chunkPosWork.z == 5)
            {
                SetAirData(_work[4, 9, 4]);
                SetAirData(_work[4, 9, 5]);
                SetAirData(_work[4, 9, 6]);
                SetAirData(_work[5, 9, 4]);
                SetAirData(_work[5, 9, 5]);
                SetAirData(_work[5, 9, 6]);
                SetAirData(_work[6, 9, 4]);
                SetAirData(_work[6, 9, 5]);
                SetAirData(_work[6, 9, 6]);
            }
            return _work;
        }

        /// <summary>
        /// �`�����N�̃��[�J�����W�����[���h�̍��W�ɕϊ�����
        /// </summary>
        public GameObject InstanceWorldData(GameObject prefab, BlockPositionInfo blockPositionInfo, Transform parent)
        {
            Index3D generateWorldPos = new Index3D();
            generateWorldPos.x = blockPositionInfo._chunkPos.x * _worldSettingScriptable.worldSizes[0].size.x;
            generateWorldPos.y = blockPositionInfo._chunkPos.y * -_worldSettingScriptable.worldSizes[0].size.y;
            generateWorldPos.z = blockPositionInfo._chunkPos.z * _worldSettingScriptable.worldSizes[0].size.z;
            generateWorldPos = (generateWorldPos + GENERATE_AJUST_POS) + blockPositionInfo._chunkInBlockIndex;
            return Instantiate(prefab, generateWorldPos.ConvertToVector3(), Quaternion.identity);
        }

        /// <summary>
        /// �C���X�^���X
        /// </summary>
        public GameObject InstanceWorldData(GameObject prefab, Vector3 chankPosition, Transform parent)
        {
            return Instantiate(prefab, chankPosition, Quaternion.identity, parent);
        }

        /// <summary>
        /// ���ʂȃu���b�N�𐶐�
        /// </summary>
        /// <param name="chunkDatas"></param>
        public void SpecialDataSetting(ChunkData[,,] chunkDatas)
        {
            _shopCharactorGenerate.GenerateRandomPos(chunkDatas);
        }

        async public void GenerateHieralchy(GameObject chunc, ChunkData chunkData)
        {
            int count = 0;

            for (int i = 0; i < _arraySize.y; i++)
            {
                await UniTask.Delay(1, cancellationToken: _token);
                await GenerateBlock(chunc, chunkData._blockdata, i);
                count++;
            }

            if (chunkData.bShop)
            {
                _shopCharactorGenerate.Generate(chunc.transform.position);
            }

            if (_arraySize.y >= count)
            {
                chunkData.GenerateComplate();
#if UNITY_EDITOR
                Debug.Log("generateComplate");
#endif
            }
        }

        /// <summary>
        /// �f�[�^�����ƂɃu���b�N���C���X�^���X����
        /// </summary>
        async public UniTask<bool> GenerateBlock(GameObject chunkPivot
            , BlockdataInfo[,,] _work
            , int hieralchy)
        {
            for (int i = 0; i < _arraySize.x; i++)
            {
                for (int k = 0; k < _arraySize.z; k++)
                {
                    BlockdataInfo instanceInfo = _work[i, hieralchy, k];

                    if (instanceInfo.category == BlockScriptable.Category.AIR
                        || instanceInfo.bitFlagBlockState != (int)BlockState.ENABLE)
                    {
                        continue;
                    }

                    Vector3 generatePos = GetWorldInstanceAjust(new Index3D(chunkPivot.transform.position), instanceInfo.fieldBlockStatus.positionInfo._chunkInBlockIndex);

                    instanceInfo.fieldBlockStatus.instancePostion =new Index3D(generatePos);

                    //��ΈȊO�̃u���b�N�𐶐�����
                    if (instanceInfo.category == BlockScriptable.Category.NORMAL)
                    {
                        InstanceNormalBlock(generatePos, instanceInfo, chunkPivot.transform);
                    }
                    

                    //��΂̃u���b�N�����b�V���Ɛ�������
                    if (instanceInfo.category == BlockScriptable.Category.JUWELRY)
                    {
                        GameObject block = InstanceNormalBlock(generatePos, instanceInfo, chunkPivot.transform);
                        Instantiate(_blkScriptable.JuwelryObject, generatePos, Quaternion.identity, block.transform);
                    }

                    //���ʂȃu���b�N�𐶐�����
                    if (instanceInfo.category == BlockScriptable.Category.SPECIAL)
                    {
                        if (instanceInfo.spescialCategory == BlockScriptable.SpescialCategory.TRESUREBOX)
                        {
                            GameObject instanceObject = Instantiate(_blkScriptable.tresureChest.tresureChestPrefab, generatePos, Quaternion.identity, chunkPivot.transform);
                            instanceObject.GetComponent<BlockData>().Initialize(ref instanceInfo);
                        }
                        if (instanceInfo.spescialCategory == BlockScriptable.SpescialCategory.SHOPBLOCK)
                        {
                            InstancePrivateNormalBlock(generatePos, _blkScriptable.shopBlock._shopWallPrf, instanceInfo, chunkPivot.transform);
                        }                 
                        if (instanceInfo.spescialCategory == BlockScriptable.SpescialCategory.EXPLOSION_BLOCK)
                        {
                            InstancePrivateNormalBlock(generatePos, _blkScriptable.explosionBlock.explosionBlockPrf, instanceInfo, chunkPivot.transform);
                        }
                        if (instanceInfo.spescialCategory == BlockScriptable.SpescialCategory.HEAL_BLOCK)
                        {
                            InstancePrivateNormalBlock(generatePos, _blkScriptable.healBlock.healBlockPrf, instanceInfo, chunkPivot.transform);
                        }
                    }

                    if (instanceInfo.category == BlockScriptable.Category.STATIC)
                    {
                        InstanceNormalBlock(generatePos, instanceInfo, chunkPivot.transform);
                    }

                    _delayFlameCount++;

                    if (_delayFlameCount >= DELAY_BLOCK)
                    {
                        _delayFlameCount = 0;
                        await UniTask.DelayFrame(1, default, _token);
                    }
                }
            }
            return true;
        }

        GameObject InstanceNormalBlock(Vector3 pos, BlockdataInfo blockdataInfo, Transform parent)
        {
            GameObject generate = Instantiate(_blkScriptable.mainBlockPrefab, pos, Quaternion.identity, parent);
            generate.GetComponent<BlockData>().Initialize(ref blockdataInfo);
            return generate;
        }
        GameObject InstancePrivateNormalBlock(Vector3 pos, GameObject prefab, BlockdataInfo blockdataInfo, Transform parent)
        {
            GameObject generate = Instantiate(prefab, pos, Quaternion.identity, parent);
           
            generate.GetComponent<BlockData>().Initialize(ref blockdataInfo);
            return generate;
        }

        public Vector3 GetWorldInstanceAjust(Index3D chunkPos, Index3D index)
        {
           return ((chunkPos + index) - GENERATE_AJUST_POS).ConvertToVector3();
        }

        public static void GenerateIndexToPos(BlockdataInfo data, int i, int j, int k)
        {
            data.fieldBlockStatus.positionInfo._chunkInBlockIndex = new Index3D(i, j, k);
        }

        public BlockdataInfo Vector3ToBlockIndexData(BlockdataInfo[,,] _world, Vector3 pos)
        {
            return _world[(int)pos.x, (int)pos.y, (int)pos.z];
        }
        public BlockdataInfo Vector3ToBlockIndexData(BlockdataInfo[,,] _world, Index3D pos)
        {
            return _world[pos.x, pos.y, pos.z];
        }
        public BlockdataInfo Vector3ToBlockIndexData(BlockdataInfo[,,] _world, BlockPositionInfo pos)
        {
            return _world[pos._chunkInBlockIndex.x,pos._chunkInBlockIndex.y, pos._chunkInBlockIndex.z];
        }

        /// <summary>
        /// �󂹂Ȃ��u���b�N�̐���
        /// </summary>
        /// <param name="_world"></param>
        /// <param name="chunkPos"></param>
        public void UnBreakGenerate(BlockdataInfo[,,] _world, Index3D chunkPos)
        {
            Vector3 worldSize = _arraySize;

            if (chunkPos.x == 0 || chunkPos.x == worldSize.x - 1)
            {
                if (chunkPos.x == 0)
                {
                    for (int i = 0; i < worldSize.z; i++)
                    {
                        for (int j = 0; j < worldSize.y; j++)
                        {
                            _world[0, j, i].SetBedRock(_blkScriptable.unBreaks.generateData);
                        }
                    }
                }
                if (chunkPos.x == worldSize.x - 1)
                {
                    for (int i = 0; i < worldSize.z; i++)
                    {
                        for (int j = 0; j < worldSize.y; j++)
                        {
                            _world[(int)_arraySize.x - 1, j, i].SetBedRock(_blkScriptable.unBreaks.generateData);
                        }
                    }
                }
            }

            if (chunkPos.y == 0 || chunkPos.y == worldSize.y - 1)
            {
                if (chunkPos.y == 0)
                {
                    for (int i = 0; i < worldSize.x; i++)
                    {
                        for (int j = 0; j < worldSize.z; j++)
                        {
                            _world[i, (int)_arraySize.y - 1, j].SetBedRock(_blkScriptable.unBreaks.generateData);

                        }
                    }
                }
                if (chunkPos.y == worldSize.y - 1)
                {
                    for (int i = 0; i < worldSize.x; i++)
                    {
                        for (int j = 0; j < worldSize.z; j++)
                        {
                            _world[i, 0, j].SetBedRock(_blkScriptable.unBreaks.generateData);

                        }
                    }
                }
            }

            if (chunkPos.z == 0 || chunkPos.z == worldSize.z - 1)
            {
                if (chunkPos.z == 0)
                {
                    for (int i = 0; i < _arraySize.x; i++)
                    {
                        for (int j = 0; j < _arraySize.y; j++)
                        {
                            _world[i, j, 0].SetBedRock(_blkScriptable.unBreaks.generateData);

                        }
                    }
                }
                if (chunkPos.z == worldSize.z - 1)
                {
                    for (int i = 0; i < worldSize.x; i++)
                    {
                        for (int j = 0; j < worldSize.y; j++)
                        {
                            _world[i, j, (int)_arraySize.z - 1].SetBedRock(_blkScriptable.unBreaks.generateData);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ��C�u���b�N�ɏ���������
        /// </summary>
        /// <param name="_world"></param>
        public void SetAirData(BlockdataInfo _world)
        {
            _world.SetAirData();
        }

        //==================================
        //private
        //==================================
        void BlockInitialize()
        {
            for (int i = 0; i < _arraySize.x; i++)
            {
                for (int j = 0; j < _arraySize.y; j++)
                {
                    for (int k = 0; k < _arraySize.z; k++)
                    {
                        BlockdataInfo blockdataInfo = new BlockdataInfo();
                        blockdataInfo.SetAirData();
                        //�C���f�b�N�X����u���b�N�f�[�^�̃|�W�V�����ɕύX
                        GenerateIndexToPos(blockdataInfo, i, j, k);
                        _work[i, j, k] = blockdataInfo;
                    }
                }
            }
        }

        /// <summary>
        /// �q���̃u���b�N�𐶐�
        /// </summary>
        void ChildGenerate(BlockdataInfo[,,] _work
            , BlockdataInfo data
            , int probility
            , int probabilityDecrease
            , Index3D position)
        {

            Index3D[] CubeGeneratePivot =
            {
                    new Index3D(0,0,0),
                    new Index3D(1,0,0),
                    new Index3D(0,-1,0),
                    new Index3D(1,-1,0),
                    new Index3D(0,0,1),
                    new Index3D(1,0,1),
                    new Index3D(0,-1,1),
                    new Index3D(1,-1,1),
                };
            //�z��O�ł�߂�
            if (position.x < 0 || position.x >= _arraySize.x) return;
            if (position.y < 0 || position.y >= _arraySize.y) return;
            if (position.z < 0 || position.z >= _arraySize.z) return;

            //�m��Ő�������u���b�N�@�Q���Q
            for (int i = 0; i < CubeGeneratePivot.Length; i++)
            {
                Index3D arrayPos = CubeGeneratePivot[i] + position;
                //����̃u���b�N�̐����`�F�b�N
               ChainGenerateBlockCheck(_work, data, probility, probabilityDecrease, arrayPos);
            }
        }

        void ChainGenerateBlockCheck(BlockdataInfo[,,] _work
            , BlockdataInfo data
            , int probility
            , int probabilityDecrease
            , Index3D position)
        {
            Index3D[] CheckPos =
            {       
                    //���̏\��
                    new Index3D(0,0,0),
                    new Index3D(1,0,0),
                    new Index3D(-1,0,0),
                    new Index3D(0,0,1),
                    new Index3D(0,0,-1),
                    //���̎l�p
                    new Index3D(1,0,1),
                    new Index3D(-1,0,1),
                    new Index3D(1,0,-1),
                    new Index3D(-1,0,-1),
                    //��̏\��
                    new Index3D(0,1,0),
                    new Index3D(1,1,0),
                    new Index3D(-1,1,0),
                    new Index3D(0,1,1),
                    new Index3D(0,1,-1),
                    //��̎l�p
                    new Index3D(1,1,1),
                    new Index3D(-1,1,1),
                    new Index3D(1,1,-1),
                    new Index3D(-1,1,-1),
                    //���̏\��
                    new Index3D(0,-1,0),
                    new Index3D(1,-1,0),
                    new Index3D(-1,-1,0),
                    new Index3D(0,-1,1),
                    new Index3D(0,-1,-1),
                    //���̎l�p
                    new Index3D(1,-1,1),
                    new Index3D(-1,-1,1),
                    new Index3D(1,-1,-1),
                    new Index3D(-1,-1,-1),
                };

            for (int i = 0; i < CheckPos.Length; i++)
            {
                Index3D arrayPos = CheckPos[i] + position;

                //�z��O�ł�߂�
                if (!InField(arrayPos)) continue;

                //���̃C���f�b�N�X�Ƀu���b�N����������Ă��Ȃ��ꍇ
                if (probility <= 0) return;

                if (GameUtility.CheckUnderParsent(probility)) return;
                //�u���b�N�𐶐�
                data.fieldBlockStatus.positionInfo._chunkInBlockIndex = Vector3ToBlockIndexData(_work, arrayPos).fieldBlockStatus.positionInfo._chunkInBlockIndex;
                _work[arrayPos.x, arrayPos.y, arrayPos.z] = new BlockdataInfo(data, data.fieldBlockStatus.positionInfo);

                //�ċA�@���̃u���b�N�����ƂɍĐ���
                ChainGenerateBlockCheck(_work, data, probility - probabilityDecrease, probabilityDecrease, arrayPos);
            }
        }

        /// <summary>
        /// �x�[�X�̔z���ݒ�
        /// </summary>
        void BaseBlockSet(int num)
        {
            int result = -(num / 10);

            var generateSetting = _blkScriptable.baseObjectGenerates[result];
            for (int i = 0; i < _arraySize.x; i++)
            {
                for (int j = 0; j < _arraySize.y; j++)
                {
                    for (int k = 0; k < _arraySize.z; k++)
                    {
                        _work[i, j, k].Initialize(generateSetting.name, generateSetting.hp, generateSetting.category, generateSetting.blockKind, generateSetting.objectData,_blkScriptable.defaultMesh);
                    }
                }
            }
        }

        /// <summary>
        /// �t�B�[���h���̃����_���ȏꏊ���擾
        /// </summary>
        /// <returns></returns>
        Index3D RandomArrayIndex()
        {
            Index3D index = new Index3D();
            index.x = Random.Range(1, (int)_arraySize.x - 1);
            index.y = Random.Range(1, (int)_arraySize.y - 1);
            index.z = Random.Range(1, (int)_arraySize.z - 1);
            return index;
        }

        /// <summary>
          /// �t�B�[���h���ɂȂ��Ă��邩
          /// </summary>
          /// <param name="checkNum"></param>
          /// <returns></returns>
        bool InField(Index3D checkNum)
        {
            if (checkNum.x >= _arraySize.x) return false;
            if (checkNum.y >= _arraySize.y) return false;
            if (checkNum.z >= _arraySize.z) return false;
            if (checkNum.x < 0) return false;
            if (checkNum.y < 0) return false;
            if (checkNum.z < 0) return false;
            return true;
        }

        /// <summary>
        /// ���͂����u���b�N�����̊K�w�ŃA�N�e�B�u���ǂ����@�E�[���@�E�u���b�N�̎�ށ@�E�ǂ̃u���b�N��T�����邩
        /// </summary>
        bool IsGenerateBlockActive(int depth, int blockKind, int blockdataIndex)
        {
            //�󂯎�����K�w����K�p���鐶���ݒ�����߂�
            int depthIndex = GetBlockProbabilityIndex(depth);

            //�󂯎�����u���b�N��񂩂炱�̊K�w�̐������A�N�e�B�u����Ԃ�
            if (blockKind == (int)BLOCKKIND.BLOCK)
            {
                return _worldSettingScriptable.generateSettings[depthIndex].blockstatus[blockdataIndex].active;
            }

            if (blockKind == (int)BLOCKKIND.JUWELRY)
            {
                return _worldSettingScriptable.generateSettings[depthIndex].juwelrystatus[blockdataIndex].active;
            }

            return false;
        }

        /// <summary>
        /// ���̊K�w�̃u���b�N�̐����m����\���m����Index�𐶐�
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        int GetBlockProbabilityIndex(int depth)
        {
            for (int i = 0; i < _worldSettingScriptable.generateSettings.Length; i++)
            {
                //�K�w���Ƃ̃}�b�N�X�̊K�w�����ł��������ꍇ�͎���
                if (_worldSettingScriptable.generateSettings[i].hirarchyMax <= depth) continue;
                return i;
            }
            return 0;
        }

        /// <summary>
        ///�@�z��̒��̃����_���ȏꏊ�ɕ��𐶐�
        /// </summary
        void RandomBlockSetting(BlockdataInfo[,,] _work
            , BlockdataInfo data
            , int probility
            , int probabilityDecrease
            , int generateCnt)
        {
            Index3D randomPosition;

            for (int i = 0; i < generateCnt; i++)
            {
                randomPosition = RandomArrayIndex();
                //�q������
                ChildGenerate(_work, data, probility, probabilityDecrease, randomPosition);
            }
        }

        /// <summary>
        ///�@�z��̒��̃����_���ȏꏊ�ɕ��𐶐�
        /// </summary
        void RandomBlockSetting(BlockdataInfo[,,] _work
            , BlockdataInfo data
            , int generateCount)
        {
            Index3D randomPosition;
            for (int i = 0; i < generateCount; i++)
            {
                randomPosition = RandomArrayIndex();
                data.fieldBlockStatus.positionInfo = _work[randomPosition.x, randomPosition.y, randomPosition.z].fieldBlockStatus.positionInfo;
                _work[randomPosition.x,randomPosition.y,randomPosition.z] = new BlockdataInfo(data, data.fieldBlockStatus.positionInfo);
            }
        }

        /// <summary>
        ///�@�z��̒��̃����_���ȏꏊ�ɕ��𐶐�
        /// </summary
        void RandomJuwerySetting(BlockdataInfo[,,] _work, BlockdataInfo data, int generateCnt)
        {
            Index3D randomPosition;

            for (int i = 0; i < generateCnt; i++)
            {
                randomPosition = RandomArrayIndex();
                JuweryChildGenerate(_work, randomPosition, data);
            }
        }

        /// <summary>
        /// �g���W���[�`�F�X�g�𐶐�
        /// </summary>
        /// <param name="_work"></param>
        /// <param name="data"></param>
        public void GenerateTureChesst(BlockdataInfo[,,] _work
            , BlockdataInfo data)
        {
            for (int i = 0; i < _blkScriptable.tresureChest.generateCount; i++)
            {
                Index3D randomPosition;
                randomPosition = RandomArrayIndex();

                if (randomPosition.y == 0 || randomPosition.y == _arraySize.y - 1)
                {
                    continue;
                }

                data.fieldBlockStatus.positionInfo._chunkInBlockIndex = _work[randomPosition.x, randomPosition.y, randomPosition.z].fieldBlockStatus.positionInfo._chunkInBlockIndex;
                _work[randomPosition.x, randomPosition.y, randomPosition.z] = new BlockdataInfo(data, data.fieldBlockStatus.positionInfo);
            }
        }


        /// <summary>
        /// ��Ηp�̃f�[�^����
        /// </summary>
        void JuweryChildGenerate(BlockdataInfo[,,] work, Index3D chankPos, BlockdataInfo data)
        {
            Index3D[] checkPos =
            {
                    new Index3D(1,0,0) ,
                    new Index3D(-1,0,0),
                    new Index3D(0,1,0) ,
                    new Index3D(0,-1,0),
                    new Index3D(0,0,1) ,
                    new Index3D(0,0,-1),
            };

            foreach (Index3D pos in checkPos)
            {
                Index3D chankIndex = pos + chankPos;

                if (Random.Range(0, HALF) == 1) continue;

                //�z��O�ł�߂�
                if (!InField(chankIndex)) return;

                data.fieldBlockStatus.positionInfo._chunkInBlockIndex = _work[chankIndex.x, chankIndex.y, chankIndex.z].fieldBlockStatus.positionInfo._chunkInBlockIndex;
                work[chankIndex.x, chankIndex.y, chankIndex.z] = new BlockdataInfo(data, data.fieldBlockStatus.positionInfo);
            }
        }
    }
}