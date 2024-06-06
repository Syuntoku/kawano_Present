using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Syuntoku.DigMode.Settings;

namespace Syuntoku.DigMode
{

    public class ShopCharactorGenerate : MonoBehaviour
    {
        [SerializeField] BlockScriptable _blockScritable;
        [SerializeField] BlockGenerate _blockGenerate;
        [SerializeField] GameObject _charactorParent;

        [Header("�������ꂽ�ꏊ")]
        public Vector3[] generatePos;

        Vector3 GenerateAjust = new Vector3(0.0f, -3.0f, 0.0f);

        public void GenerateRandomPos(ChunkData[,,] chunkDatas)
        {
            int work = Random.Range(_blockScritable.shopBlock.WorldInCountMin, _blockScritable.shopBlock.WorldInCountMin + 1);

            generatePos = new Vector3[work];

            for (int i = 0; i < work; i++)
            {
                generatePos[i] = new Vector3();
            }

            for (int i = 0; i < work; i++)
            {
                Vector3 workData = new Vector3();
                workData.x = Random.Range(0, _blockScritable.oneChankSize.x);
                workData.y = Random.Range(0, _blockScritable.oneChankSize.y);
                workData.z = Random.Range(0, _blockScritable.oneChankSize.z);
                generatePos[i] = workData;
                chunkDatas[(int)generatePos[i].x, (int)generatePos[i].y, (int)generatePos[i].z].bShop = true;
            }
            generatePos[0] = new Vector3(5, 0, 5);
        }

        /// <summary>
        /// �`�����N���ɃV���b�v�p�̃u���b�N�f�[�^���i�[
        /// </summary>
        /// <param name="_work"></param>
        public void DataGenerate(BlockdataInfo[,,] _work)
        {
            Vector3 pivot = new Vector3(5, 3, 5);

            //���̋󓴂𐶐�
            _blockGenerate.SetAirData(_blockGenerate.Vector3ToBlockIndexData(_work, pivot));

            Vector3[] data =
            {
                    //����
                        new Vector3( 1, 0, 0),
                        new Vector3(-1, 0, 0),
                        new Vector3( 0, 1, 0),
                        new Vector3( 0,-1, 0),
                        new Vector3( 1, 1, 0),
                        new Vector3(-1, 1, 0),
                        new Vector3( 1,-1, 0),
                        new Vector3(-1,-1, 0),

                        //����
                        new Vector3( 0, 0, 1),
                        new Vector3( 1, 0, 1),
                        new Vector3(-1, 0, 1),
                        new Vector3( 0, 1, 1),
                        new Vector3( 0,-1, 1),
                        new Vector3( 1, 1, 1),
                        new Vector3(-1, 1, 1),
                        new Vector3( 1,-1, 1),
                        new Vector3(-1,-1, 1),

                        //3���
                        new Vector3( 0, 0,-1),
                        new Vector3( 1, 0,-1),
                        new Vector3(-1, 0,-1),
                        new Vector3( 0, 1,-1),
                        new Vector3( 0,-1,-1),
                        new Vector3( 1, 1,-1),
                        new Vector3(-1, 1,-1),
                        new Vector3( 1,-1,-1),
                        new Vector3(-1,-1,-1),
                };

            //�R�~�R�̋�C�u���b�N�����
            foreach (var item in data)
            {
                _blockGenerate.SetAirData(_blockGenerate.Vector3ToBlockIndexData(_work, pivot + item));
            }

            //�ǂ𐶐�
            Vector3[] wall =
            {
                        new Vector3(0,2,0),
                        //3*3
                        new Vector3(1,2,0),
                        new Vector3(-1,2,0),
                        new Vector3(0,2,1),
                        new Vector3(0,2,-1),

                        new Vector3(1,2,1),
                        new Vector3(1,2,-1),
                        new Vector3(-1,2,1),
                        new Vector3(-1,2,-1),

                        new Vector3(2,2,0),
                        new Vector3(-2,2,0),
                        new Vector3(0,2,2),
                        new Vector3(0,2,-2),
                        //�O�g
                        new Vector3(0,2,2),
                        new Vector3(0,2,-2),
                        new Vector3(2,2,0),
                        new Vector3(-2,2,0),

                        new Vector3(2,2,2),
                        new Vector3(1,2,2),
                        new Vector3(2,2,1),
                        new Vector3(2,2,-1),
                        new Vector3(1,2,-2),
                        new Vector3(1,2,-2),
                        new Vector3(-2,2,-2),
                        new Vector3(-1,2,-2),
                        new Vector3(-2,2,-1),
                        new Vector3(-2,2,2),
                        new Vector3(-2,2,1),
                        new Vector3(-1,2,2),
                        //�O�g���i
                        new Vector3(0,1,2),
                        new Vector3(0,1,-2),
                        new Vector3(2,1,0),
                        new Vector3(-2,1,0),

                        new Vector3(2,1,2),
                        new Vector3(1,1,2),
                        new Vector3(2,1,1),
                        new Vector3(2,1,-1),
                        new Vector3(1,1,-2),
                        new Vector3(1,1,-2),
                        new Vector3(-2,1,-2),
                        new Vector3(-1,1,-2),
                        new Vector3(-2,1,-1),
                        new Vector3(-2,1,2),
                        new Vector3(-2,1,1),
                        new Vector3(-1,1,2),
                        //�O�g���i2
                        new Vector3(0,0,2),
                        new Vector3(0,0,-2),
                        new Vector3(2,0,0),
                        new Vector3(-2,0,0),

                        new Vector3(2,0,2),
                        new Vector3(1,0,2),
                        new Vector3(2,0,1),
                        new Vector3(2,0,-1),
                        new Vector3(1,0,-2),
                        new Vector3(1,0,-2),
                        new Vector3(-2,0,-2),
                        new Vector3(-1,0,-2),
                        new Vector3(-2,0,-1),
                        new Vector3(-2,0,2),
                        new Vector3(-2,0,1),
                        new Vector3(-1,0,2),
                        //�O�g��i
                        new Vector3(0,-1,2),
                        new Vector3(0,-1,-2),
                        new Vector3(2,-1,0),
                        new Vector3(-2,-1,0),

                        new Vector3(2,-1,2),
                        new Vector3(1,-1,2),
                        new Vector3(2,-1,1),
                        new Vector3(2,-1,-1),
                        new Vector3(1,-1,-2),
                        new Vector3(1,-1,-2),
                        new Vector3(-2,-1,-2),
                        new Vector3(-1,-1,-2),
                        new Vector3(-2,-1,-1),
                        new Vector3(-2,-1,2),
                        new Vector3(-2,-1,1),
                        new Vector3(-1,-1,2),

                        //�ŉ��i
                        new Vector3(0,-2,0),
                        //3*3
                        new Vector3(1,-2,0),
                        new Vector3(-1,-2,0),
                        new Vector3(0,-2,1),
                        new Vector3(0,-2,-1),

                        new Vector3(1,-2,1),
                        new Vector3(1,-2,-1),
                        new Vector3(-1,-2,1),
                        new Vector3(-1,-2,-1),

                        new Vector3(2,-2,0),
                        new Vector3(-2,-2,0),
                        new Vector3(0,-2,2),
                        new Vector3(0,-2,-2),
                        //�O�g
                        new Vector3(2,-2,2),
                        new Vector3(1,-2,2),
                        new Vector3(2,-2,1),
                        new Vector3(2,-2,-1),
                        new Vector3(1,-2,-2),
                        new Vector3(1,-2,-2),
                        new Vector3(-2,-2,-2),
                        new Vector3(-1,-2,-2),
                        new Vector3(-2,-2,-1),
                        new Vector3(-2,-2,2),
                        new Vector3(-2,-2,1),
                        new Vector3(-1,-2,2),
                    };

            foreach (var item in wall)
            {
                Vector3 pos = pivot + item;
                _work[(int)pos.x, (int)pos.y, (int)pos.z].Initialize(new Vector3(pos.x, pos.y, pos.z));
                _work[(int)pos.x, (int)pos.y, (int)pos.z].Initialize(_blockScritable.shopBlock.name, _blockScritable.shopBlock.hp, _blockScritable.shopBlock.category,_blockScritable.shopBlock.objectData,_blockScritable.defaultMesh);
                _work[(int)pos.x, (int)pos.y, (int)pos.z].spescialCategory = BlockScriptable.SpescialCategory.SHOPBLOCK;
            }
        }

        /// <summary>
        /// �V���b�v�L�����N�^�[�𐶐�
        /// </summary>
        /// <param name="GeneratePos"></param>
        public void Generate(Vector3 GeneratePos)
        {
            GameObject generateObject = _blockGenerate.InstanceWorldData(_blockScritable.shopBlock._ShopCharactorPrf, GeneratePos + GenerateAjust, _charactorParent.transform);
            generateObject.transform.SetParent(_charactorParent.transform);
        }
    }
}
