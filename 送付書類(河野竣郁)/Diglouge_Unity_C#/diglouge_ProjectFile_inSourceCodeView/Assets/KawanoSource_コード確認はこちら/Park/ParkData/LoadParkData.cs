using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Syuntoku.DigMode.ParkData;

namespace Syuntoku.DigMode.ParkData
{

    public enum POPKIND
    {
        SHARE,
        DIG,
        BATTLE,
    }

    /// <summary>
    /// パーク読み込み用クラス
    /// </summary>
    public class LoadParkData : MonoBehaviour
    {
        private TextAsset csvFile;

        public Dictionary<int, Park> _fullParkData = new Dictionary<int, Park>();
        public List<Park> _parkFullInfo = new List<Park>();

        /// <summary>
        /// パーク関連のエラー時に返されるパークデータ
        /// </summary>
        public Park _notFindData = new Park();

        const string POP_KIND_DIG = "採掘";
        const string POP_KIND_BATTLE = "戦闘";

        bool bInit;
        int loadCount = 0;
        const int MAX_COUNT = 30 + 12;

        // 使用可能なパークID
        private static int[] AvailableParkID = new int[] { 1, 2, 4, 17, 12, 8 };
        void Start()
        {
            //パーク関連のエラー時に返されるデータを初期化
            _notFindData.maxLevels = 0;
            _notFindData.name = "NotFind";
            _notFindData.levelParkData = new ParkBase[1];
            _notFindData.levelParkData[0] = new ParkBase();
            _notFindData.nowLevel = 0;
            _notFindData.levelParkData[0].explanation = "取得できるパークがありません";

            //パークを読み込み
            csvFile = Resources.Load("ParkData") as TextAsset; // ResourcesにあるCSVファイルを格納
#if UNITY_EDITOR
            if (csvFile == null)
            {
                Debug.Log("fileLoadingEroor");
                return;
            }
#endif
            StringReader reader = new StringReader(csvFile.text); // TextAssetをStringReaderに変換

            while (reader.Peek() != -1)
            {
                //区分け用のデータを読み込む
                reader.ReadLine();

                Park parkWork = new Park();

                //パーク共通のデータを読み込む
                parkWork.ReadBaseParkData(reader);

                //idからパーク用専用データを読み込む
                parkWork.levelParkData = ParkIdToParkLevelData(reader, parkWork.parkNo, parkWork.maxLevels);

                _fullParkData.Add(parkWork.parkNo, parkWork);
                _parkFullInfo.Add(parkWork);

                loadCount++;
            }
#if UNITY_EDITOR
            Debug.Log("ParkLoadComplate");
#endif
        }

        ParkBase[] ParkIdToParkLevelData(StringReader stringReader, int id, int levelCount)
        {
            ParkBase[] parkbase = null;

            switch (id)
            {
                case 1:
                    parkbase = new Park_001_Fierce_Blow[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_001_Fierce_Blow();
                    }
                    break;
                case 2:
                    parkbase = new Park_002_Good_Luck[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_002_Good_Luck();
                    }
                    break;
                case 3:
                    parkbase = new Park_003_SuccessFully[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_003_SuccessFully();
                    }
                    break;
                case 4:
                    parkbase = new Park_004_Steady[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_004_Steady();
                    }
                    break;
                case 5:
                    parkbase = new Park_005_Innocent[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_005_Innocent();
                    }
                    break;
                case 6:
                    parkbase = new Park_006_Badger[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_006_Badger();
                    }
                    break;
                case 7:
                    parkbase = new Park_007_Dedication[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_007_Dedication();
                    }
                    break;
                case 8:
                    parkbase = new Park_008_Mole[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_008_Mole();
                    }
                    break;
                case 9:
                    parkbase = new Park_009_Rationality[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_009_Rationality();
                    }
                    break;
                case 10:
                    parkbase = new Park_010_RippleEffect[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_010_RippleEffect();
                    }
                    break;
                case 11:
                    parkbase = new Park_011_Impatience[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_011_Impatience();
                    }
                    break;
                case 12:
                    parkbase = new Park_012_HeavenAndEarth[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_012_HeavenAndEarth();
                    }
                    break;
                case 13:
                    parkbase = new Park_013_PerfectShadow[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_013_PerfectShadow();
                    }
                    break;
                case 14:
                    parkbase = new Park_014_Tide[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_014_Tide();
                    }
                    break;
                case 15:
                    parkbase = new Park_015_RigidBody[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_015_RigidBody();
                    }
                    break;
                case 16:
                    parkbase = new Park_016_WideAngle[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_016_WideAngle();
                    }
                    break;
                case 17:
                    parkbase = new Park_017_GradualIncrease[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_017_GradualIncrease();
                    }
                    break;
                case 18:
                    parkbase = new Park_018_YinAndYang[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_018_YinAndYang();
                    }
                    break;
                case 19:
                    parkbase = new Park_019_Brilliant[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_019_Brilliant();
                    }
                    break;
                case 20:
                    parkbase = new Park_020_LeapForward[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_020_LeapForward();
                    }
                    break;
                case 21:
                    parkbase = new Park_021_Synergy[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_021_Synergy();
                    }
                    break;
                case 22:
                    parkbase = new Park_022_[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_022_();
                    }
                    break;
                case 23:
                    parkbase = new Park_023_[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_023_();
                    }
                    break;
                case 24:
                    parkbase = new Park_024_[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_024_();
                    }
                    break;
                case 25:
                    parkbase = new Park_025_[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_025_();
                    }
                    break;
                case 100:
                    parkbase = new Park_100_DistantThunder[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_100_DistantThunder();
                    }
                    break;
                case 101:
                    parkbase = new Park_101_Exaltation[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_101_Exaltation();
                    }
                    break;
                case 102:
                    parkbase = new Park_102_ComeClose[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_102_ComeClose();
                    }
                    break;
                case 103:
                    parkbase = new Park_103_Boredom[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_103_Boredom();
                    }
                    break;
                case 104:
                    parkbase = new Park_104_BigPicture[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_104_BigPicture();
                    }
                    break;
                case 105:
                    parkbase = new Park_105_Ignition[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_105_Ignition();
                    }
                    break;
                case 106:
                    parkbase = new Park_106_ExplosiveArmor[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_106_ExplosiveArmor();
                    }
                    break;
                case 107:
                    parkbase = new Park_107_WideAreaStrategy[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_107_WideAreaStrategy();
                    }
                    break;
                case 108:
                    parkbase = new Park_108_Whirlwind[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_108_Whirlwind();
                    }
                    break;
                case 109:
                    parkbase = new Park_109_Hakhiro[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_109_Hakhiro();
                    }
                    break;
                case 110:
                    parkbase = new Park_110_Renewal[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_110_Renewal();
                    }
                    break;
                case 111:
                    parkbase = new Park_111_Skilled[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_111_Skilled();
                    }
                    break;
                case 112:
                    parkbase = new Park_112_GiantWhale[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_112_GiantWhale();
                    }
                    break;
                case 113:
                    parkbase = new Park_113_Collect[levelCount];
                    for (int i = 0; i < parkbase.Length; i++)
                    {
                        parkbase[i] = new Park_113_Collect();
                    }
                    break;

            }

            for (int i = 0; i < levelCount; i++)
            {
                parkbase[i].Loading(stringReader);
            }

            return parkbase;
        }

        /// <summary>
        /// POPKindに対応したパークのKeyを取得
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public List<int> GetTypePark(int kind)
        {
            List<int> keylist = new List<int>(_fullParkData.Keys);
            List<int> resultlist = new List<int>();

            switch (kind)
            {
                case (int)POPKIND.DIG:

                    foreach (var item in keylist)
                    {
                        if (!_fullParkData[item].bPopActive) continue;
                        if (_fullParkData[item].PopKind == POP_KIND_DIG)
                        {

                            resultlist.Add(item);
                        }
                    }

                    break;
                case (int)POPKIND.BATTLE:

                    foreach (var item in keylist)
                    {
                        if (!_fullParkData[item].bPopActive) continue;
                        if (_fullParkData[item].PopKind == POP_KIND_BATTLE)
                        {
                            resultlist.Add(item);
                        }
                    }
                    break;
                case (int)POPKIND.SHARE:

                    foreach (var item in keylist)
                    {
                        if (!_fullParkData[item].bPopActive) continue;

                        resultlist.Add(item);
                    }

                    break;
            }

            return resultlist;
        }

        public void KeyToEnablePark(int key, Status.StatusManage statusManage, ParkConditionsManage parkConditionsManage)
        {
            _fullParkData[key].GetToActiveCheck(statusManage, parkConditionsManage);
        }

        /// <summary>
        /// keyからランダムなパークを受け取る
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Park GetKeyToParkDataAndChangeEnable(int key, Status.StatusManage statusManage, ParkConditionsManage parkConditionsManage)
        {
            _fullParkData[key].GetToActiveCheck(statusManage, parkConditionsManage);
            return _fullParkData[key];
        }

        /// <summary>
        /// アクティブなパークの数を数える
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public int PopUpCount(int kind)
        {
            int cout = 0;
            switch (kind)
            {
                case (int)POPKIND.DIG:

                    foreach (var item in _fullParkData)
                    {
                        if (!item.Value.bPopActive) continue;

                        if (item.Value.PopKind == POP_KIND_DIG)
                        {
                            cout++;
                        }
                    }

                    break;
                case (int)POPKIND.BATTLE:

                    foreach (var item in _fullParkData)
                    {
                        if (!item.Value.bPopActive) continue;

                        if (item.Value.PopKind == POP_KIND_BATTLE)
                        {
                            cout++;
                        }
                    }

                    break;
                case (int)POPKIND.SHARE:
                    foreach (var item in _fullParkData)
                    {
                        if (!item.Value.bPopActive) continue;

                        cout++;
                    }

                    break;
            }
            return cout;
        }

        /// <summary>
        /// ランダムなパークを取得する
        /// </summary>
        /// <returns></returns>
        public Park GetRandomPark()
        {
            Park result = new Park();
#if UNITY_EDITOR
            if (_fullParkData.Count == 0)
            {
                Debug.Log("アクティブなパークがありません");
                return _notFindData;
            }
#endif
            List<int> data = new List<int>(_fullParkData.Keys);

            while (true)
            {

                //result = _fullParkData[data[Random.Range(0, _fullParkData.Count)]];
                var id = AvailableParkID[Random.Range(0, AvailableParkID.Length)];
                result = _fullParkData[id];

                if (!result.bPopActive) continue;

                if (result.nowLevel >= 0)
                {
#if UNITY_EDITOR
                    Debug.Log("パーク：レベルアップ");
#endif
                }
                else
                {
#if UNITY_EDITOR
                    Debug.Log("パーク：初めて取得");
#endif
                }

                break;
            }
            return result;
        }


        /// <summary>
        /// IDでパークを取得する
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Park FindPark(int id)
        {
            var park = _fullParkData[id];
            return park;
        }
    }
}
