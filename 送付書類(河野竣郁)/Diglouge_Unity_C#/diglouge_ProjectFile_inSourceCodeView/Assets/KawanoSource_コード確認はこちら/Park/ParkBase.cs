using System.IO;
using Syuntoku.Status;

namespace Syuntoku.DigMode.ParkData
{
    [System.Serializable]
    public class Park
    {
        public Park()
        {
            parkNo = 0;
            nowLevel = -1;
            PopKind = "";
            levelParkData = null;
            bPopActive = true;
        }

        public Park(Park park)
        {
            parkNo = park.parkNo;
            nowLevel = park.nowLevel;
            levelParkData = park.levelParkData;
            PopKind = park.PopKind;
        }
        public bool bValid;
        public int parkNo;
        public int nowLevel;
        public int maxLevels;

        public string PopKind;

        public string name;

        /// <summary>
        /// ポップを許可するフラグ
        /// </summary>
        public bool bPopActive;
        /// <summary>
        /// 取得したフラグ
        /// </summary>
        public bool bAcquired;

        public ParkBase[] levelParkData;

        /// <summary>
        /// ファイルからベースのデータを呼び出す
        /// </summary>
        /// <param name="stringReader"></param>
        public void ReadBaseParkData(StringReader stringReader)
        {
            string work = stringReader.ReadLine();

            //説明部と値部を分ける
            string[] spitData = work.Split(':');
            parkNo = int.Parse(spitData[1]);

            work = stringReader.ReadLine();
            spitData = work.Split(':');
            name = spitData[1];

            work = stringReader.ReadLine();
            spitData = work.Split(':');
            PopKind = spitData[1];

            work = stringReader.ReadLine();
            spitData = work.Split(':');

            maxLevels = int.Parse(spitData[1]);
        }

        /// <summary>
        /// パークをゲットした時にアクティブかをチェック
        /// </summary>
        public void GetToActiveCheck(StatusManage statusManage, ParkConditionsManage parkConditionManage)
        {
            bAcquired = true;

            if (nowLevel != -1)
            {
                levelParkData[nowLevel].Disable(statusManage, parkConditionManage);
            }

            nowLevel++;

            levelParkData[nowLevel].Start(statusManage, parkConditionManage);

            if (nowLevel >= maxLevels - 1)
            {
                bPopActive = false;
            }
        }

        public string GetNowLevelExplain()
        {
            return levelParkData[nowLevel].explanation;
        }
        public string GetNextLevelExplain()
        {
            int work = 0;

            if (nowLevel + 1 >= maxLevels)
            {
                work = maxLevels;
            }
            else
            {
                work = nowLevel + 1;
            }

            return levelParkData[work].explanation;
        }
    }

    public class ParkBase
    {
        public string explanation;

        public ParkBase()
        {
            explanation = "null";
        }

        virtual public void Loading(StringReader reader)
        {
            //データでは使用しない行を飛ばす
            reader.ReadLine();

            //共通の説明文を読み込む
            string work = reader.ReadLine();

            string[] splitData = work.Split(":");

            explanation = splitData[1];

        }

        virtual public void Start(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {

        }

        virtual public void Update(StatusManage baseStatus, ParkConditionsManage parkConditionsManage, float addTimier)
        {

        }

        virtual public void Disable(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {

        }

        virtual public void EnemyKill(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {

        }

        /// <summary>
        /// 敵からダメージを受けた時に実行される
        /// </summary>
        /// <param name="baseStatus"></param>
        /// <param name="parkConditionsManage"></param>
        /// <param name="addTimier"></param>
        virtual public void OnDamage(StatusManage baseStatus, ParkConditionsManage parkConditionsManage, float addTimier)
        {

        }

        /// <summary>
        /// ブロックにダメージを与えた時に実行されるイベント関数
        /// </summary>
        /// <param name="baseStatus"></param>
        /// <param name="parkConditionsManage"></param>
        /// <param name="addTimier"></param>
        virtual public void OnHitBlock(StatusManage baseStatus, ParkConditionsManage parkConditionsManage, float addTimier)
        {

        }
        virtual public void OnExecuteMainSkill(StatusManage baseStatus, ParkConditionsManage parkConditionsManage, float addTimier)
        {

        }

        /// <summary>
        /// ブロックを破壊したときの処理
        /// </summary>
        /// <param name="baseStatus"></param>
        /// <param name="parkConditionsManage"></param>
        virtual public void BreakUpDate(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {
        }

        /// <summary>
        /// 装備を振ったときの処理
        /// </summary>
        /// <param name="baseStatus"></param>
        /// <param name="parkConditionsManage"></param>
        virtual public void SwingUpdate(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {

        }

        virtual public void EndSwing(StatusManage baseStatus)
        {

        }

        /// <summary>
        /// 武器を撃った時にステータスを変更
        /// </summary>
        /// <param name="baseStatus"></param>
        /// <param name="parkConditionsManage"></param>
        virtual public void WeaponUpdate(StatusManage baseStatus, ParkConditionsManage parkConditionsManage)
        {

        }

        /// <summary>
        /// 一行読み込み、必要なデータだけ取得する
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected string LoadLine(StringReader reader, bool enptyLoad = false)
        {
            string work = reader.ReadLine();

            if (enptyLoad) return "";

            string[] splitData = work.Split(":");

            return splitData[1];
        }
    }
}