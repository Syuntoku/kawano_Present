using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyGenerater : MonoBehaviour
{
    bool bStart;

    float waveTime;
    float generateTime;

    bool bDigMode = true;

    const float Digtime = 180.0f;
    const float BattleTime = 120.0f;

    [SerializeField] GameObject enemy;

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.F10))
        {
            bStart = true;
        }

        if (!bStart) return;

        waveTime += Time.deltaTime;
        generateTime += Time.deltaTime;

        if(bDigMode)
        {
            if (waveTime >= Digtime) bDigMode = !bDigMode;
            waveTime = 0.0f;
        }
        else
        {
            if (waveTime >= BattleTime) bDigMode = !bDigMode;

            waveTime = 0.0f;


        }

        if(bDigMode)
        {
            if(generateTime >= 1)
            {
                generateTime = 0;
                InstanceEnemy();
            }
        }
    }

    void InstanceEnemy()
    {
        Vector3 vector3 = transform.position;

        vector3.x = Random.Range(-2, 3);
        vector3.z = Random.Range(-2, 3);

        Instantiate(enemy, transform.position + vector3, Quaternion.identity);
    }
}
