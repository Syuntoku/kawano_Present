using UnityEngine;
using Syuntoku.DigMode.Inventory.Juwelry;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Jobs;
using Syuntoku.DigMode.Drop;

namespace Syuntoku.DigMode.Inventory
{
    /// <summary>
    /// フィールドのドロップアイテム管理
    /// </summary>
    public class DropManager : MonoBehaviour
    {
        [SerializeField] JuwelryScriptable _juwelryScriptable;
        [SerializeField] Transform _dropParent;
        [SerializeField] Transform _playerTrans;
        [SerializeField] float _activeLength;

        const float ROTATE_AMOUNT = 0.1f;
        const float GROUND_LENGTH = 0.01f;
        const float GRAVITY_STRENGTH = 1.0f;
        const float ACTIVE_LENGTH = 20.0f;
        const float GET_LENGTH = 0.7f;

        /// <summary>
        /// ワールドに宝石を生成する
        /// </summary>
        /// <param name="juwelryKind"></param>
        /// <param name="generatePosition"></param>
        /// <param name="dropPlibility"></param>
        /// <param name="count"></param>    
        public GameObject[] InstanceJuwelry(JuwelryInventory.JUWELRY_KIND juwelryKind, Vector3 generatePosition, float dropPlibility = 100, int count = 1)
        {
            GameObject[] instanceJuwelry = new GameObject[count];

            if (GameUtility.CheckUnderParsent(dropPlibility)) return null;

            for (int i = 0; i < count; i++)
            {
                instanceJuwelry[i] = Instantiate(_juwelryScriptable.GetJuwelryPrefab(juwelryKind), generatePosition, Quaternion.identity, _dropParent);
            }
            return instanceJuwelry;
        }
        /// <summary>
        /// ワールドに宝石を生成する
        /// </summary>
        public void InstanceJuwelry(DropSetting dropSetting, Vector3 generatePosition)
        {
            if (dropSetting == null)
            {
#if UNITY_EDITOR
                Debug.Log("dropDataNull");
#endif
                return;
            }
            //宝石をドロップ
            if (!GameUtility.CheckUnderParsent(dropSetting.dropPlibability)) return;

            for (int i = 0; i < dropSetting.dropCount; i++)
            {
                if (!dropSetting.bSetDropItem)
                {
                   Instantiate(_juwelryScriptable.GetJuwelryPrefab(dropSetting.dropJuwelryKind), generatePosition, Quaternion.identity, _dropParent);
                }
                else
                {
                    //設定されたオブジェクトを生成
                    Instantiate(dropSetting.setDropObject, generatePosition, Quaternion.identity, _dropParent);
                }
            }
        }

        private void Update()
        {
            if (_dropParent.childCount == 0) return;
            var inputs = new NativeArray<InputJob>(_dropParent.childCount, Allocator.TempJob);
            var results = new NativeArray<ResultJob>(_dropParent.childCount, Allocator.TempJob);
            var rayResult = new NativeArray<RaycastCommand>(_dropParent.childCount, Allocator.TempJob);
            var result = new NativeArray<RaycastHit>(_dropParent.childCount, Allocator.TempJob);

            RotateJob rotateJob = new RotateJob();

            for (int i = 0; i < _dropParent.childCount; i++)
            {
                Transform transform = _dropParent.GetChild(i).transform;
                InputJob inputJob = new InputJob();

                inputJob.playerPosition = _playerTrans.position;
                inputJob.position = transform.position;
                inputJob.angle = ROTATE_AMOUNT;
                inputJob.activeLength = _activeLength;
                inputJob.groundLengh = GROUND_LENGTH;
                inputJob.gravityStrength = GRAVITY_STRENGTH * Time.deltaTime;
                inputJob.activeLength = ACTIVE_LENGTH;
                inputJob.getLength = GET_LENGTH;
                inputs[i] = inputJob;

                RaycastCommand raycastCommand = new RaycastCommand();
                raycastCommand.direction = Vector3.down;
                raycastCommand.from = transform.position;
                raycastCommand.distance = 1.0f;
                rayResult[i] = raycastCommand;
            }

            rotateJob.inputs = inputs;
            rotateJob.rayResult = result;
            rotateJob.resultJobs = results;

            var raycastJobHandle = RaycastCommand.ScheduleBatch( rayResult, result, 20,default(JobHandle));

            raycastJobHandle.Complete();

            var rotateHandle = rotateJob.Schedule(_dropParent.childCount,20);

            /*
             アイテムの重力を別スレッドで再現する
             時間が掛かる＆優先度低のため一旦RigitBodyで対応
             */
            //JobHandle.ScheduleBatchedJobs();
            rotateHandle.Complete();

            for (int i = 0; i < results.Length; i++)
            {
                Transform transform = _dropParent.GetChild(i).transform;
                if (results[i].bGetItem)
                {
                    transform.GetComponent<DropItem>().GetItem();
                    continue;
                }
                transform.gameObject.SetActive(results[i].active);
            }

            inputs.Dispose();
            results.Dispose();
            rayResult.Dispose();
            result.Dispose();
        }

        struct RotateJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<RaycastHit> rayResult;
            public NativeArray<InputJob> inputs;
            public NativeArray<ResultJob> resultJobs;

            public void Execute(int index)
            {
                ResultJob resultJob = resultJobs[index];
                float length = (inputs[index].playerPosition - inputs[index].position).magnitude;

                //存在設定をする
                resultJob.active = length <= inputs[index].activeLength ? true : false;

                if (length < inputs[index].getLength)
                {
                    resultJob.bGetItem = true;
                }

                if (rayResult[index].distance >= inputs[index].groundLengh)
                {
                    // resultJob.position = inputs[index].position + Vector3.down * inputs[index].gravityStrength;
                }
                else
                {
                    //resultJob.position = inputs[index].position;
                }

                resultJobs[index] = resultJob;
            }
        }

        struct InputJob
        {
            public Vector3 playerPosition;
            public Vector3 position;
            public float activeLength;
            public float angle;
            public ResultJob dropObject;
            public float groundLengh;
            public float gravityStrength;
            public float getLength;
        }

        struct ResultJob
        {
            public bool active;
            public bool bGetItem;
            public Vector3 position;
        }
    }
}
