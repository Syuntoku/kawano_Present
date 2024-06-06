using UnityEngine;

public class LightScript : MonoBehaviour
{
    Light m_light;
    public GameObject slender;
    int a, b=0,c=0;
    Renderer m_renderer;
    void Start()
    {
        m_light = GetComponent<Light>();
        m_renderer = slender.GetComponent<Renderer>();
        m_renderer.enabled = false;
    }

    void Update()
    {
        a = Random.Range(1, 100);
        if (b == 4)
        {
            b = 0;
            c++;
        }
        if (a < 4)
            b++;
        
        m_light.enabled = b<=2;
        m_renderer.enabled = c % 5 == 0;
    }
}
