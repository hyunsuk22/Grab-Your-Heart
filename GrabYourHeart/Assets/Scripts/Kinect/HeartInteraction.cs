using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartInteraction : MonoBehaviour {
    public AudioPeer _audioPeer;
    public float _div = 2.5f;
    private float _audioBuffer;

    public Vector3 _heartScale;
  
	void Start () {
	}
	
	
	void Update () {
        //AudioPeer -> Object Scale
        _audioBuffer = _audioPeer._audioBandBuffer[0] / _div;
        _heartScale = new Vector3(_audioBuffer, _audioBuffer, _audioBuffer);
        gameObject.transform.localScale = _heartScale;
    }
}
