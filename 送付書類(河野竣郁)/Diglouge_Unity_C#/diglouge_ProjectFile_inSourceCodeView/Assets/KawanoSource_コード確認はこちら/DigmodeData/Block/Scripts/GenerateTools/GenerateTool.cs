using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Settings;

namespace Syuntoku
{
    namespace DigMode
    {

        /// <summary>
        /// ブロック生成のツール
        /// </summary>
        public class GenerateTool
        {

            /// <summary>
            /// 方向についてのクラス
            /// </summary>
            public class Direction
            {
                /// <summary>
                /// 各方面の番号
                /// </summary>
                public enum DirectionNum
                {
                    FORWORD,
                    RIGHT_FORWORD,
                    RIGHT,
                    RIGHT_BACK,
                    BACK,
                    LEFT_BACK,
                    LEFT,
                    LEFT_FORWORD,
                    END,
                }

                /// <summary>
                /// 傾きの番号
                /// </summary>
                public enum Inclination
                {
                    UP,
                    PARALLEL,
                    DOWN,
                }

                public static Index3D forword = new Index3D(1, 0, 0);
                public static Index3D rightForword = new Index3D(1, 0, 1);
                public static Index3D right = new Index3D(0, 1, 0);
                public static Index3D backRight = new Index3D(-1, 0, 1);
                public static Index3D back = new Index3D(-1, 0, 0);
                public static Index3D backLeft = new Index3D(-1, 0, -1);
                public static Index3D left = new Index3D(0, 0, -1);
                public static Index3D leftForword = new Index3D(1, 0, -1);
            }

            /// <summary>
            /// チャンク関連のツール
            /// </summary>
            public class ChunkTool
            {
                /// <summary>
                /// インデックスデータからワールドデータのチャンクデータを取得
                /// </summary>
                /// <param name="worldDatas"></param>
                /// <param name="data"></param>
                /// <returns></returns>
                public static BlockdataInfo WorldIndexDataToworldData(ChunkData[,,] worldDatas, WorldIndexData data)
                {
                    if (data.IsOutOfBounds) return null;

                    return worldDatas[data._chankIndex.x, data._chankIndex.y, data._chankIndex.z]._blockdata[data._blockIndex.x, data._blockIndex.y, data._blockIndex.z];
                }
                /// <summary>
                /// インデックスデータからワールドデータのチャンクデータを取得
                /// </summary>
                /// <param name="worldDatas"></param>
                /// <param name="data"></param>
                /// <returns></returns>
                public static BlockdataInfo[,,] WorldIndexDataToworldDataReturnChunkData(ChunkData[,,] worldDatas, WorldIndexData data)
                {
                    return worldDatas[data._chankIndex.x, data._chankIndex.y, data._chankIndex.z]._blockdata;
                }

                /// <summary>
                /// ワールドのデータからインデックスデータに変換する　引数１,2,3 はチャンクのインデックス　引数4,5,6はブロックのインデックス
                /// </summary>
                /// <param name="i"></param>
                /// <param name="j"></param>
                /// <param name="k"></param>
                /// <param name="randomX"></param>
                /// <param name="randomY"></param>
                /// <param name="randomZ"></param>
                /// <returns></returns>
                public static WorldIndexData WorldDataToWorldIndecData(WorldIndexData worldIndexData, int i, int j, int k, int randomX, int randomY, int randomZ)
                {

                    worldIndexData._chankIndex.x = i;
                    worldIndexData._chankIndex.y = j;
                    worldIndexData._chankIndex.z = k;

                    worldIndexData._blockIndex.x = randomX;
                    worldIndexData._blockIndex.y = randomY;
                    worldIndexData._blockIndex.z = randomZ;

                    return worldIndexData;


                }
                /// <summary>
                ///インデックスデータから移動する　チャンクも移動可
                /// </summary>
                /// <param name="_blockScriptable"></param>
                /// <param name="data"></param>
                /// <param name="MoveData"></param>
                public static WorldIndexData ArrayMoveChunk(BlockScriptable _blockScriptable, WorldIndexData data, Index3D MoveData)
                {
                    //Xの軸を動かす
                    data._blockIndex.x += MoveData.x;

                    if (data._blockIndex.x <= 0)
                    {
                        int outCount = data._blockIndex.x - 0;

                        data._chankIndex.x--;

                    }
                    else if (data._blockIndex.x >= _blockScriptable.oneChankSize.x)
                    {
                        data._chankIndex.x++;

                    }

                    //Yの軸を動かす
                    data._blockIndex.y += MoveData.y;

                    if (data._blockIndex.y <= 0)
                    {
                        int outCount = data._blockIndex.y - 0;

                        data._chankIndex.y--;

                    }
                    else if (data._blockIndex.x >= _blockScriptable.oneChankSize.y)
                    {
                        data._chankIndex.y++;

                    }

                    //Zの軸を動かす
                    data._blockIndex.z += MoveData.z;

                    if (data._blockIndex.z <= 0)
                    {
                        int outCount = data._blockIndex.z - 0;

                        data._chankIndex.z--;

                    }
                    else if (data._blockIndex.x >= _blockScriptable.oneChankSize.z)
                    {
                        data._chankIndex.z++;

                    }

                    //OutOfBoundsチェック
                    WorldChunkOutOfBoundsCheck(_blockScriptable, data);

                    return data;
                }

                /// <summary>
                /// もし設定したチャンクより外に出ている場合にIndexData内のIsOutOfBoundsをtrueにする
                /// </summary>
                /// <param name="blockScriptable"></param>
                /// <param name="data"></param>
                public static void WorldChunkOutOfBoundsCheck(BlockScriptable blockScriptable, WorldIndexData data)
                {
                    if (data._chankIndex.x < 0 || data._chankIndex.y < 0 || data._chankIndex.z < 0)
                    {
                        data.IsOutOfBounds = true;
                    }

                    if (data._chankIndex.x >= blockScriptable.oneChankSize.x || data._chankIndex.y >= blockScriptable.oneChankSize.y || data._chankIndex.z >= blockScriptable.oneChankSize.z)
                    {
                        data.IsOutOfBounds = true;
                    }
                }

                /// <summary>
                /// もし設定したチャンクより外に出ている場合にtrueを返す
                /// </summary>
                /// <param name="blockScriptable"></param>
                /// <param name="data"></param>
                public static bool WorldChunkOutOfBoundsCheckToIndex(BlockScriptable blockScriptable, Index3D data)
                {
                    if (data.x < 0 || data.y < 0 || data.z < 0)
                    {
                        return true;
                    }

                    if (data.x >= blockScriptable. oneChankSize.x || data.y >= blockScriptable.oneChankSize.y || data.z >= blockScriptable.oneChankSize.z)
                    {
                        return true;
                    }

                    return false;
                }
            }
        }


        public class WorldIndexData
        {
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public WorldIndexData()
            {

            }
            /// <summary>
            ///　コンストラクタ　コピー
            /// </summary>
            /// <param name="copyData"></param>
            public WorldIndexData(WorldIndexData copyData)
            {
                _chankIndex = copyData._chankIndex;
                _blockIndex = copyData._blockIndex;
            }

            public bool IsOutOfBounds = false;
            public Index3D _chankIndex = new Index3D();
            public Index3D _blockIndex = new Index3D();
        }


        /// <summary>
        /// int型３つのクラス
        /// </summary>
        [System.Serializable]
        public class Index3D
        {
            public int x;
            public int y;
            public int z;

            public enum Axis
            {
                X,
                Y,
                Z,
            }

            static public int length = 3;

            /// <summary>
            /// コンストラクタ　
            /// </summary>
            public Index3D()
            {
            }

            /// <summary>
            /// コンストラクタ2
            /// </summary>
            public Index3D(int setX, int setY, int setZ)
            {
                x = setX;
                y = setY;
                z = setZ;
            }


            /// <summary>
            /// コンストラクタ3　
            /// </summary>
            public Index3D(Index3D CopyData)
            {
                x = CopyData.x;
                y = CopyData.y;
                z = CopyData.z;
            }
            /// <summary>
            /// コンストラクタ4　vector3からVariableClassのデータに変換
            /// </summary>
            /// <param name="data"></param>
            public Index3D(Vector3 data)
            {
                x = (int)data.x;
                y = (int)data.y;
                z = (int)data.z;
            }


            public Vector3 ConvertToVector3()
            {
                Vector3 work = new Vector3(x, y, z);

                return work;
            }

            public void SetFullIndex(int inX,int inY,int inZ)
            {

                x = inX;
                y = inY;
                z = inZ;
            }

            /// <summary>
            /// すべての変数に同じ値を設定する
            /// </summary>
            /// <param name="allSettingNum"></param>
            public void FullSetting(int allSettingNum)
            {
                x = allSettingNum;
                y = allSettingNum;
                z = allSettingNum;
            }


            /// <summary>
            /// クラス同士の足し算
            /// </summary>
            public static Index3D operator +(Index3D data, Index3D data2)
            {
                Index3D work = new Index3D();

                work.x = data.x + data2.x;
                work.y = data.y + data2.y;
                work.z = data.z + data2.z;

                return work;
            }
            /// <summary>
            /// クラス同士の引き算
            /// </summary>
            public static Index3D operator -(Index3D data, Index3D data2)
            {
                data.x -= data2.x;
                data.y -= data2.y;
                data.z -= data2.z;

                return data;
            }
            /// <summary>
            /// クラス同士の掛け算
            /// </summary>
            public static Index3D operator *(Index3D data, Index3D data2)
            {
                data.x *= data2.x;
                data.y *= data2.y;
                data.z *= data2.z;

                return data;
            }

            /// <summary>
            /// クラス同士の掛け算 すべてに共通のデータを入れる
            /// </summary>
            public static Index3D operator *(Index3D data, int data2)
            {
                data.x *= data2;
                data.y *= data2;
                data.z *= data2;

                return data;
            }
        }
    }
}