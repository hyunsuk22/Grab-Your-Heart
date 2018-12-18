using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioVisualizer : MonoBehaviour {
    float[] samples;
    public GameObject cube;
    public float visualGap = 1.5f;
    //총 할당할 큐브갯수.
    public float totalCube = 8;
    private GameObject[] visualObjs;
    public GameObject fireworks;
    public float high = 3f;
    public static float[] _bandBuffer = new float[8];
 
    // Use this for initialization
    void Start () {        
        //부모 클래스의 sample 변수에 접근.
        samples = transform.parent.GetComponent<PrepareAudioSourcce>().samples;
        visualObjs = new GameObject[samples.Length];
        //오브젝트 할당.
        for(int i=0; i< totalCube; i++)
        {
            visualObjs[i] = Instantiate(cube, new Vector3(i * visualGap, 0, 0), Quaternion.identity);
        }
	}
	
	// Update is called once per frame
	void Update () {
        BandBuffer();
        Vector3 p;
        for (int i = 0; i < totalCube; i++)
        {
            p = visualObjs[i].transform.localScale;
            p.y = samples[i] * 80f;
            visualObjs[i].transform.localScale = p;
            if (p.y >= high)
            {
                visualObjs[i].GetComponent<Renderer>().material.color = Color.Lerp(Color.white, new Color(1f, 0.07f, 0.46f), Mathf.PingPong(Time.time, 1));
            }
        }
	}
    void BandBuffer()
    {
        for (int i = 0; i < totalCube; i++)
        {
            if (samples[i] > _bandBuffer[i])
            {
                _bandBuffer[i] = samples[i];
            }

            if (samples[i] < _bandBuffer[i])
            {
                _bandBuffer[i] += (0 - _bandBuffer[i]) * 0.05f;
            }
        }
    }
}
