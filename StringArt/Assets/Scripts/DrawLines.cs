using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLines : MonoBehaviour
{
    public GameObject LinePrefab;
    public GameObject outerLine;
    public int sideNum = 4;
    public float Radius = 1f;
    public int _multiplier;
    public float _multiplyBuffer = 0.5f;
    public Color _stageColor;

    private float _increase;
    private float _emssionBlue;
    private float _defaultEmssionBlue;
    private float _audioBuffer2;
    private float _audioBuffer3;

    private List<Vector3> LinePos;
    void Start()
    {
        _multiplier = 2;
        _defaultEmssionBlue = 0.4771027f;
        DrawLine();
    }

    private void Update()
    {
        _multiplier = (int)AudioPeer._bandBuffer[1];
        for (int i = 0; i < LinePos.Count - 1; i++)
        {
            Destroy(GameObject.Find("innerLine" + i));
            GameObject line = Instantiate(LinePrefab);
            line.name = "innerLine" + i;
            line.GetComponent<LineRenderer>().SetPosition(0, LinePos[i]);
            int n = i * _multiplier;
            //moduler연산자 %. 지정된 방을 초과하면 다시 0 으로 갈수있게.
            n = n % sideNum;
            line.GetComponent<LineRenderer>().SetPosition(1, LinePos[n]);

            //색변화.
            Material _material = line.GetComponent<Renderer>().material;
            //_emssionBlue = _defaultEmssionBlue + AudioPeer._bandBuffer[1] * 2;
            _audioBuffer2 = AudioPeer._bandBuffer[2];
            _audioBuffer3 = AudioPeer._bandBuffer[3];
            _material.SetColor("_EmissionColor", _stageColor * _audioBuffer2);

 
            
        }
        //Debug.Log("emission blue = " + _emssionBlue);
        //_increase += 0.1f;


    }

    void DrawLine()
    {
        LinePos = new List<Vector3>();
        // +1 해서 시작점으로 돌아가게함.
        for (int i = 0; i < sideNum + 1; i++)
        {
            // 1PI = 180 Degree (프로그래밍에서는 Radian 각도체계를 사용함)
            // Mathf 는 수학과 관계된 모든것들을 포함하고있음.
            //Debug.Log("PI = " + Mathf.PI);
            float angle = i * 2 * Mathf.PI / sideNum;
            float z = Radius * 2 * i / sideNum;
            LinePos.Add(new Vector3(Radius * Mathf.Cos(angle), Mathf.Sin(angle), 0));
        }
        //Debug.Log("LinePos 개수 =" + LinePos.Count);

        //원
        for (int i = 0; i < LinePos.Count - 1; i++)
        {
            GameObject line = Instantiate(outerLine);
            line.name = "line" + i;
            line.GetComponent<LineRenderer>().SetPosition(0, LinePos[i]);
            line.GetComponent<LineRenderer>().SetPosition(1, LinePos[i + 1]);
            // line.GetComponent<LineRenderer>().SetPosition(1, LinePos[i + 2]);
        }
        //선긋기
        for (int i = 0; i < LinePos.Count - 1; i++)
        {
            GameObject line = Instantiate(LinePrefab);
            line.name = "innerLine" + i;
            line.GetComponent<LineRenderer>().SetPosition(0, LinePos[i]);
            int n = i * _multiplier;
            //moduler연산자 %. 지정된 방을 초과하면 다시 0 으로 갈수있게.
            n = n % sideNum;
            line.GetComponent<LineRenderer>().SetPosition(1, LinePos[n]);
        }
    }
}
