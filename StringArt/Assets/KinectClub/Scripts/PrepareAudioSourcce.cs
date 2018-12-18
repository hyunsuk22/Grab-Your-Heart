using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareAudioSourcce : MonoBehaviour {
    //미니멈 64개.
    public float[] samples = new float[64];
   
    AudioSource audio;
	// Use this for initialization
	void Start () {
        audio = GetComponent<AudioSource>();
	}

    private void Update()
    {
       
    }
    //정한 횟수만큼만 업데이트 됨.
    private void FixedUpdate()
    {
        //audio.GetSpectrumData(samples, channel(왼쪽=0 오른쪽=1), window)
        audio.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

  
}
