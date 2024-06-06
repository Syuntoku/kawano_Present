using UnityEngine;
using DG.Tweening;

public class PVControll : MonoBehaviour
{
    [SerializeField] Transform parent;

    [Header("�s�b�P���̃I�u�W�F�N�g")]
    [SerializeField] GameObject _pickAxe;
    [Header("�s�b�P���̃q�b�g�G�t�F�N�g��ς���ꍇ�͂�����")]
    [Header("�K���G�t�F�N�g�Ɂ@���G�t�F�N�g�I����ɏ����ݒ��t���Ă�������" +
        "\n�I�u�W�F�N�g��ParticleSystem�Ƃ����R���|�[�l���g������܂��B\nDuration��Looping������ݒ�̘g�̒���StopAction�Ƃ����ݒ肪����܂�(������3�Ԗ�)\n�I������ƃv���_�E�����o��̂ł�����uDesyroy�v�ɕς��Ă�������")]
    [SerializeField] GameObject _hitEfect;
    [Header("--------------�s�b�P���̃��[�V�����ݒ�-------------------")]
    [Header("�s�b�P���̃��[�V�����ݒ�")]
    [Header("�s�b�P���̃I�u�W�F�N�g������ɂ̓C���X�y�N�^�[�ɐݒ肳��Ă���GameObject���N���b�N�@" +
        "\nTransform��Rotation��Z���̉�]��ݒ肷�邱�ƂŐU��p�x��ς����܂�")]
    [Header("�X�^�[�g�p�x\n�\������̊p�x\n�\������̊p�x�ɂ�������\n�U��܂ł̒x������(�����قǗ\�����삪�����Ȃ�)\n�s�b�P����U�������̊p�x\n�U�鎞��\n�ŏ��̊p�x�ɖ߂��x������")]
    public float pickAxeStartRotate;
    public float swingPreliminary;
    public float swingPreliminaryTime;
    public float swingDeley;
    public float pickAxeEndRotate;
    public float swingTime;
    public float delayRerotate;
    [Header("------------�q�b�g�G�t�F�N�g�ݒ�---------------------")]

    [Header("�����ꏊ\n�傫��\n��]")]
    public Vector3 instanceEfectPosition;
    public Vector3 efectScale;
    public Vector3 efectRotate;
    [Header("--------------�u���b�N�̃��b�V���ύX-------------------")]
    public Mesh startMesh;
    public Mesh firstHitMesh;
    public Mesh secondHitMesh;
    public GameObject blockHitEfectPrefab;
    [Header("--------------�h���b�v���-------------------")]
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
        //���Z�b�g
        if(Input.GetKeyDown(KeyCode.R))
        {

            blockMesh.gameObject.SetActive(true);
            hitCount = 0;
            ChangeBlockMesh(hitCount);
            ResetJuwelry();
        }

        //�s�b�P���𓮂���
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
