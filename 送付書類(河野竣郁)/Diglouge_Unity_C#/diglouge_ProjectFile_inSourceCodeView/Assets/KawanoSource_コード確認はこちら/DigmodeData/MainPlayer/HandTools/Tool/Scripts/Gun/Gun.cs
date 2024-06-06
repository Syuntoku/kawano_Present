using UnityEngine;
using Syuntoku.Status;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine.ProBuilder;

namespace Syuntoku.DigMode.Tool
{
    //======================================
    //銃　ボールを発射する
    //======================================
    class Gun : ToolBase
    {
        [SerializeField] GameObject _shotPoint;
        [Header("ボールの設定")]
        public Vector3 _startSize;
        LineRenderer _lineRenderer;
        const int POINT_COUNT = 2;
        uint rayMask;

        //コンストラクタ
        public Gun()
        {

        }

        private void Start()
        {
            _lineRenderer = GameObject.Find("LineRenderManage").GetComponent<LineRenderer>();
            _lineRenderer.positionCount = POINT_COUNT;
            _toolData._toolStatus.addDrawLineCount = 1;
            rayMask =  (uint)~LayerMask.GetMask("PostEfect");
        }

        /// <summary>
        /// Update　掘るインターバルは標準
        /// </summary>
        public override void ToolUpdate()
        {
            base.ToolUpdate();

            RaycastHit hit;
#if UNITY_EDITOR
            Debug.DrawRay(_shotPoint.transform.position, Camera.main.transform.forward * 100.0f);
#endif
            Physics.Raycast(_shotPoint.transform.position, Camera.main.transform.forward, out hit, rayMask);

            _lineRenderer.SetPosition(0, _shotPoint.transform.position);
            _lineRenderer.SetPosition(1, hit.point);

            _lineRenderer.positionCount = POINT_COUNT + _toolData._toolStatus.addDrawLineCount;

            RayReflect(hit, Camera.main.transform.forward, rayMask);
        }

        /// <summary>
        /// 反射後のRayを追加
        /// </summary>
        /// <param name="raycastHit"></param>
        /// <param name="drawCount"></param>
        void RayReflect(RaycastHit raycastHit,Vector3 inDirection, uint mask)
        {
            RaycastHit hit;
            Vector3 reflectVec = Vector3.Reflect(inDirection, raycastHit.normal);

            if (!Physics.Raycast(raycastHit.point, reflectVec, out hit, mask))
            {
                _lineRenderer.SetPosition(0 + POINT_COUNT, raycastHit.point);
                return;
            }
            _lineRenderer.SetPosition(0 + POINT_COUNT, hit.point);
#if UNITY_EDITOR
            Debug.DrawRay(_shotPoint.transform.position, Camera.main.transform.forward * 100.0f);
#endif
        }

        /// <summary>
        ///  掘る　
        /// </summary>
        public override bool Dig(GameObject gameObject, DigStatus digStatus, ToolInfo toolInfo)
        {
            GunInfo gunInfo = (GunInfo)toolInfo;

            if (!_bDig)
            {
                Shoot(gunInfo, (int)gunInfo._toolStatus.interval);

                return true;
            }
            return false;
        }

        async void Shoot(GunInfo toolInfo, int Interval)
        {
            GameObject shotBall = null;

            for (int i = 0; i < toolInfo._shotCount; i++)
            {
                if (i != 0)
                {
                    await UniTask.Delay(Interval);
                }

                shotBall = Instantiate(toolInfo._shotPrefab, _shotPoint.transform.position, _shotPoint.transform.rotation);
                var dig = shotBall.AddComponent<DigBall>();
                dig.Initialzie(_blockManage, Camera.main.transform.forward, _statusManage.digmodeStatus.ballStatus,toolInfo);
                //掘る
                _bDig = true;
            }
        }
    }
}