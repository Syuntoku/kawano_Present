using UnityEngine;
using DG.Tweening;

public class PVControll : MonoBehaviour
{
    [SerializeField] Transform parent;

    [Header("ピッケルのオブジェクト")]
    [SerializeField] GameObject _pickAxe;
    [Header("ピッケルのヒットエフェクトを変える場合はこちら")]
    [Header("必ずエフェクトに　をエフェクト終了後に消す設定を付けてください" +
        "\nオブジェクトにParticleSystemというコンポーネントがあります。\nDurationやLoopingがある設定の枠の中にStopActionという設定があります(下から3番目)\n選択するとプルダウンが出るのでそれを「Desyroy」に変えてください")]
    [SerializeField] GameObject _hitEfect;
    [Header("--------------ピッケルのモーション設定-------------------")]
    [Header("ピッケルのモーション設定")]
    [Header("ピッケルのオブジェクトを見るにはインスペクターに設定されているGameObjectをクリック　" +
        "\nTransformのRotationのZ軸の回転を設定することで振る角度を変えられます")]
    [Header("スタート角度\n予備動作の角度\n予備動作の角度にいく時間\n振るまでの遅延時間(長いほど予備動作が長くなる)\nピッケルを振った時の角度\n振る時間\n最初の角度に戻す遅延時間")]
    public float pickAxeStartRotate;
    public float swingPreliminary;
    public float swingPreliminaryTime;
    public float swingDeley;
    public float pickAxeEndRotate;
    public float swingTime;
    public float delayRerotate;
    [Header("------------ヒットエフェクト設定---------------------")]

    [Header("生成場所\n大きさ\n回転")]
    public Vector3 instanceEfectPosition;
    public Vector3 efectScale;
    public Vector3 efectRotate;
    [Header("--------------ブロックのメッシュ変更-------------------")]
    public Mesh startMesh;
    public Mesh firstHitMesh;
    public Mesh secondHitMesh;
    public GameObject blockHitEfectPrefab;
    [Header("--------------ドロップ宝石-------------------")]
    public float upPower;

    public GameObject[] JuwelryObject;
    public MeshFilter blockMesh;
    int hitCount;

    private void Start()
    {
        ResetJuwelry();
    }

    private void Update()
    {
        //リセット
        if(Input.GetKeyDown(KeyCode.R))
        {

            blockMesh.gameObject.SetActive(true);
            hitCount = 0;
            ChangeBlockMesh(hitCount);
            ResetJuwelry();
        }

        //ピッケルを動かす
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _pickAxe.transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y, swingPreliminary), swingPreliminaryTime).OnComplete(() =>
            {
                _pickAxe.transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y, pickAxeEndRotate), swingTime).SetDelay(swingDeley).OnComplete(()=> {
                   GameObject instance = Instantiate(_hitEfect, instanceEfectPosition, Quaternion.identity, parent);
                    instance.transform.localPosition = instanceEfectPosition;
                    instance.transform.localScale = efectScale;
                    instance.transform.localRotation =Quaternion.Euler(efectRotate);
                    hitCount++;
                    ChangeBlockMesh(hitCount);
                    Instantiate(blockHitEfectPrefab,blockMesh.gameObject.transform.position,Quaternion.identity);
                    _pickAxe.transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y, pickAxeStartRotate), swingTime).SetDelay(delayRerotate);
                });
            });
        }

        if( Input.GetKeyDown(KeyCode.B))
        {
            _pickAxe.transform.DORotate(new Vector3(transform.rotation.x, transform.rotation.y, pickAxeStartRotate), swingTime);
        }

    }

    void ChangeBlockMesh(int hitCount)
    {
        switch (hitCount)
        {
            case 0:
                blockMesh.mesh = startMesh;
                break;
            case 1:
                blockMesh.mesh = firstHitMesh;
                break;
            case 2:
                blockMesh.mesh = secondHitMesh;
                break;
            case 3:
                blockMesh.gameObject.SetActive(false);
                foreach (var item in JuwelryObject)
                {
                    item.gameObject.SetActive(true);
                    item.gameObject.transform.Rotate(new Vector3(UnityEngine.Random.Range(0.0f, 360.0f), UnityEngine.Random.Range(0.0f, 360.0f), UnityEngine.Random.Range(0.0f, 360.0f)));
                    Rigidbody rigidbody = item.gameObject.GetComponent<Rigidbody>();
                    Vector3 velosity = new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), upPower, UnityEngine.Random.Range(-1.0f, 1.0f));
                    rigidbody.AddForce(velosity, ForceMode.Impulse);
                }
                break;
        }
    }

    void ResetJuwelry()
    {
        foreach (GameObject item in JuwelryObject)
        {
            item.transform.localPosition = Vector3.zero;
            item.gameObject.SetActive(false);
        }
    }
}
