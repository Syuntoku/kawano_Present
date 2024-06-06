using UnityEngine;
using Cysharp.Threading.Tasks;
using Syuntoku.DigMode.Settings;
using System.Threading;

//===================================
//ブロックの生成に関する設定
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
    /// ブロックのデータ生成＋インスタンス化用クラス
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
        //データ生成
        //==================================
        public BlockdataInfo[,,] Generate(int depth, Index3D chunkPos, ChunkData chunkData)
        {
            //生成＆初期化
            _work = new BlockdataInfo[(int)_arraySize.x, (int)_arraySize.y, (int)_arraySize.z];
            BlockdataInfo workData = null;

            BlockInitialize();
            //受け取った階層から階層設定を指定する番号を受け取る
            int hirarchySettingIndex = GetBlockProbabilityIndex(depth);
#if UNITY_EDITOR
            Debug.Log(depth);
#endif
            BaseBlockSet(chunkPos.y);

            //コピー用のベースデータを作成
            workData = new BlockdataInfo();
            //カテゴリー"block"を格納
            BlockScriptable.Category cateGory = BlockScriptable.Category.NORMAL;
            
            //=====================================
            //サブのブロックを生成
            //=====================================
            for (int i = 0; i < _blkScriptable.blockData.Length; i++)
            {
                //受け取った階層での生成がアクティブではない場合は次へ進む
                if (!IsGenerateBlockActive(depth, (int)BLOCKKIND.BLOCK, i)) continue;
                //ベースデータをブロック用に初期化
                workData.Initialize(_blkScriptable.blockData[i].name
                    , (int)(_blkScriptable.blockData[i].hp * _blkScriptable.blockData[i].blockHardnessMagnification[hirarchySettingIndex].hardPlibability)
                    , cateGory
                    , _blkScriptable.blockData[i].blockKind
                    , _blkScriptable.blockData[i].objectData
                    , _blkScriptable.defaultMesh);
                //サブのブロックを生成
                RandomBlockSetting(_work
                    , workData
                    , _blkScriptable.blockData[i].brockProbabilityData[hirarchySettingIndex].probility
                    , _blkScriptable.blockData[i].brockProbabilityData[hirarchySettingIndex].probabilityDecrease
                    , _blkScriptable.blockData[i].brockProbabilityData[hirarchySettingIndex].generateNum);
            }

            //======================================
            //宝石を設定
            //======================================

            //カテゴリー"block"を格納
            cateGory = BlockScriptable.Category.JUWELRY;

            for (int i = 0; i < _blkScriptable.jewelryData.Length; i++)
            {
                //受け取った階層での宝石設定がアクティブではない場合は次へ進む
                if (!IsGenerateBlockActive(depth, (int)BLOCKKIND.JUWELRY, i)) continue;
                //ベースデータを宝石用に初期化
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
                //宝石の生成
                RandomJuwerySetting(
                    _work
                    , workData
                    , _blkScriptable.jewelryData[i].brockProbabilityData[hirarchySettingIndex].generateNum);
            }
            //=======================================
            //特別なオブジェクトを生成する
            //=======================================
            //カテゴリー"Special"を格納
            cateGory = BlockScriptable.Category.SPECIAL;

            //宝箱データを生成
            workData.spescialCategory = BlockScriptable.SpescialCategory.TRESUREBOX;
            workData.Initialize(_blkScriptable.tresureChest.name
                , _blkScriptable.tresureChest.hp
                , cateGory
                , _blkScriptable.tresureChest.objectData
                , _blkScriptable.defaultMesh);
            workData.dropSetting = _blkScriptable.tresureChest.dropSetting;
            GenerateTureChesst(_work, workData);

            //爆発ブロックデータを生成
            workData.spescialCategory = BlockScriptable.SpescialCategory.EXPLOSION_BLOCK;

            workData.Initialize(_blkScriptable.explosionBlock.name
                , _blkScriptable.explosionBlock.hp
                , cateGory
                , _blkScriptable.explosionBlock.objectData
                , _blkScriptable.defaultMesh);
            workData.dropSetting = null;
            RandomBlockSetting(_work, workData, _blkScriptable.explosionBlock.generateCount);

            //回復ブロックデータを生成
            workData.spescialCategory = BlockScriptable.SpescialCategory.HEAL_BLOCK;

            workData.Initialize(_blkScriptable.healBlock.name
                , _blkScriptable.healBlock.hp
                , cateGory
                , _blkScriptable.healBlock.objectData
                , _blkScriptable.defaultMesh);
            workData.dropSetting = null;
            RandomBlockSetting(_work, workData, _blkScriptable.healBlock.generateCount);

            //ショップのキャラクターを生成
            if (_blkScriptable.shopBlock.bActive)
            {
                workData.spescialCategory = BlockScriptable.SpescialCategory.SHOPBLOCK;

                if (chunkData.bShop) _shopCharactorGenerate.DataGenerate(_work);
            }

            //壊れないブロックを生成する
            UnBreakGenerate(_work, chunkData._positionInfo._chunkPos);

            //上から通れるように穴をあける(スタート地点)
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
        /// チャンクのローカル座標をワールドの座標に変換する
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
        /// インスタンス
        /// </summary>
        public GameObject InstanceWorldData(GameObject prefab, Vector3 chankPosition, Transform parent)
        {
            return Instantiate(prefab, chankPosition, Quaternion.identity, parent);
        }

        /// <summary>
        /// 特別なブロックを生成
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
        /// データをもとにブロックをインスタンスする
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

                    //宝石以外のブロックを生成する
                    if (instanceInfo.category == BlockScriptable.Category.NORMAL)
                    {
                        InstanceNormalBlock(generatePos, instanceInfo, chunkPivot.transform);
                    }
                    

                    //宝石のブロックをメッシュと生成する
                    if (instanceInfo.category == BlockScriptable.Category.JUWELRY)
                    {
                        GameObject block = InstanceNormalBlock(generatePos, instanceInfo, chunkPivot.transform);
                        Instantiate(_blkScriptable.JuwelryObject, generatePos, Quaternion.identity, block.transform);
                    }

                    //特別なブロックを生成する
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
        /// 壊せないブロックの生成
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
        /// 空気ブロックに初期化する
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
                        //インデックスからブロックデータのポジションに変更
                        GenerateIndexToPos(blockdataInfo, i, j, k);
                        _work[i, j, k] = blockdataInfo;
                    }
                }
            }
        }

        /// <summary>
        /// 子供のブロックを生成
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
            //配列外でやめる
            if (position.x < 0 || position.x >= _arraySize.x) return;
            if (position.y < 0 || position.y >= _arraySize.y) return;
            if (position.z < 0 || position.z >= _arraySize.z) return;

            //確定で生成するブロック　２＊２
            for (int i = 0; i < CubeGeneratePivot.Length; i++)
            {
                Index3D arrayPos = CubeGeneratePivot[i] + position;
                //周りのブロックの生成チェック
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
                    //中の十字
                    new Index3D(0,0,0),
                    new Index3D(1,0,0),
                    new Index3D(-1,0,0),
                    new Index3D(0,0,1),
                    new Index3D(0,0,-1),
                    //中の四つ角
                    new Index3D(1,0,1),
                    new Index3D(-1,0,1),
                    new Index3D(1,0,-1),
                    new Index3D(-1,0,-1),
                    //上の十字
                    new Index3D(0,1,0),
                    new Index3D(1,1,0),
                    new Index3D(-1,1,0),
                    new Index3D(0,1,1),
                    new Index3D(0,1,-1),
                    //上の四つ角
                    new Index3D(1,1,1),
                    new Index3D(-1,1,1),
                    new Index3D(1,1,-1),
                    new Index3D(-1,1,-1),
                    //下の十字
                    new Index3D(0,-1,0),
                    new Index3D(1,-1,0),
                    new Index3D(-1,-1,0),
                    new Index3D(0,-1,1),
                    new Index3D(0,-1,-1),
                    //下の四つ角
                    new Index3D(1,-1,1),
                    new Index3D(-1,-1,1),
                    new Index3D(1,-1,-1),
                    new Index3D(-1,-1,-1),
                };

            for (int i = 0; i < CheckPos.Length; i++)
            {
                Index3D arrayPos = CheckPos[i] + position;

                //配列外でやめる
                if (!InField(arrayPos)) continue;

                //このインデックスにブロックが生成されていない場合
                if (probility <= 0) return;

                if (GameUtility.CheckUnderParsent(probility)) return;
                //ブロックを生成
                data.fieldBlockStatus.positionInfo._chunkInBlockIndex = Vector3ToBlockIndexData(_work, arrayPos).fieldBlockStatus.positionInfo._chunkInBlockIndex;
                _work[arrayPos.x, arrayPos.y, arrayPos.z] = new BlockdataInfo(data, data.fieldBlockStatus.positionInfo);

                //再帰　このブロックをもとに再生成
                ChainGenerateBlockCheck(_work, data, probility - probabilityDecrease, probabilityDecrease, arrayPos);
            }
        }

        /// <summary>
        /// ベースの配列を設定
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
        /// フィールド内のランダムな場所を取得
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
          /// フィールド内になっているか
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
        /// 入力したブロックがこの階層でアクティブかどうか　・深さ　・ブロックの種類　・どのブロックを探索するか
        /// </summary>
        bool IsGenerateBlockActive(int depth, int blockKind, int blockdataIndex)
        {
            //受け取った階層から適用する生成設定を決める
            int depthIndex = GetBlockProbabilityIndex(depth);

            //受け取ったブロック情報からこの階層の生成がアクティブかを返す
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
        /// その階層のブロックの生成確立を表す確率のIndexを生成
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        int GetBlockProbabilityIndex(int depth)
        {
            for (int i = 0; i < _worldSettingScriptable.generateSettings.Length; i++)
            {
                //階層ごとのマックスの階層数よりでかかった場合は次へ
                if (_worldSettingScriptable.generateSettings[i].hirarchyMax <= depth) continue;
                return i;
            }
            return 0;
        }

        /// <summary>
        ///　配列の中のランダムな場所に母岩を生成
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
                //子供生成
                ChildGenerate(_work, data, probility, probabilityDecrease, randomPosition);
            }
        }

        /// <summary>
        ///　配列の中のランダムな場所に母岩を生成
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
        ///　配列の中のランダムな場所に母岩を生成
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
        /// トレジャーチェストを生成
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
        /// 宝石用のデータ生成
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

                //配列外でやめる
                if (!InField(chankIndex)) return;

                data.fieldBlockStatus.positionInfo._chunkInBlockIndex = _work[chankIndex.x, chankIndex.y, chankIndex.z].fieldBlockStatus.positionInfo._chunkInBlockIndex;
                work[chankIndex.x, chankIndex.y, chankIndex.z] = new BlockdataInfo(data, data.fieldBlockStatus.positionInfo);
            }
        }
    }
}