using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class AudioPeer1 : MonoBehaviour {
    AudioSource _audioSource;
    //다른 스크립트에서 접근이 가능하도록 static을 사용.
    public static float[] _samples = new float[512];
    public static float[] _freqBand = new float[8];
    public static float[] _bandBuffer = new float[8];
    float[] _bufferDecrease = new float[8];

    //Amplitude variables
    [HideInInspector]
    public float _Amplitude, _AmplitudeBuffer;
    private float _AmplitudeHighest;

    // Use this for initialization
    void Start () {
        //계속해서 불러오는것보다 효율적으로 불러오기
        _audioSource = GetComponent<AudioSource>();
        //Debug.Log(_audioSource.clip.frequency);
	}
	
	// Update is called once per frame
	void Update () {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();

    
    }

    void GetSpectrumAudioSource()
    {
        _audioSource.GetSpectrumData(_samples, 0, FFTWindow.Blackman);
    }
    void BandBuffer()
    {
        for(int g = 0; g < 8; g++)
        {
            if(_freqBand[g] > _bandBuffer[g])
            {
                _bandBuffer[g] = _freqBand[g];
                _bufferDecrease[g] = 0.005f;
            }
            if(_freqBand[g] < _bandBuffer[g])
            {
                _bandBuffer[g] -= _bufferDecrease[g];
                //큰값을 받았을때 더 빠르게 내려갈수 있도록.
                _bufferDecrease[g] *= 1.2f;
            }
        }
    }

    void MakeFrequencyBands()
    {
        /* 현재 곡의 최대 Hertz(이건 상이할수 있음. Audio Peer Tutorial 기준) - AudioClip.frequency(Read Only) 로 읽을수있음.
         * 22050 / 512 = 43 hertz per sample
         * 현재 나의 프로젝트에서 쓰이는 BTS-DNA는 44100 Hertz를 가지고 있음.
         * 44100 / 512 = 86 헤르츠 (샘플당).
         * 
         * 아래는 Audio Spectrum(samples)에 해당하는 7단위의 소리 샘플 구간.
         * 20 - 60 hertz
           60 - 250 hertz
           500 - 2000 hertz
           2000 - 4000 hertz
           4000 - 6000 hertz
           6000 - 20000 hertz
         * 
         * 어떻게 8개로 나눌것인가? -> 위의 소리구간과 비슷한 구조로 제작.
         * (근데 내가한건 좀망했음. 아예 샘플오디오부터 달라서..)
         * 0 - 2 samples = 86 * 2 = 172 sampels
         * 1 - 4 = 344 samples (Range 87 - 430)
         *          //왜 4인가? - 위 소리구간의 간격은 높아지는것을 감안해 밸류의 갭도 이를따라야함.
         *          //Range 는 Cube의 Index라고 생각하면 쉬울것같다. 87부터 344개니까 전체로봤을때 87번째부터 430번째가
         *          //해당 구간이 된다.
         * 2 - 8 = 688 (431 - 1118)
         * 3 - 16 = 1376 (1119 - 2494)
         * 4 - 32 = 2752 (2495 - 5246)
         * 5 - 64 = 5504 (5247 - 10750)
         * 6 - 128 = 11008 (10751 - 21758)
         * 7 - 256 = 22016 (21759 - 43774)
         * 510 ( 8+ .... + 256)
         */

        //현재 _sample을 파악하기위한 count 변수
        int count = 0;
        for (int i = 0; i < 8; i++)
        {
            float average = 0;
            //2,4,8,16,32 ... 256 을 받아오기위한 식.
            //*2를 한이유. i = 0 일때 2의 0 승으로 1이되고 * 2 하면 위의 첫번째 구간인 0 - 2. 이런식으로..!
            int sampleCount = (int)Mathf.Pow(2, i) * 2;
            //우리가 얻는 _sample의 배열데이터갯수는 512이나 위의 구간을 나눈공식에선 510으로 끝나므로 2를 더해줌(궂이 하지않아도되긴함)

            if (i == 7)
            {
                sampleCount += 2;
            }
            //sample을 FreqBand로 대입시키기위해 내부 for loop 작성.
            //512까지 샘플카운트를 돌려서 모든 샘플데이터가 들어갈수있게끔.
            for(int j = 0; j < sampleCount; j++)
            {
                //샘플데이터를 average로
                //count + 1 을 곱한이유는 frequency가 높아질때 value drop을 보상하기위해서임.
                //그래서 visualize band가 같은 높이로 보이게.
                average += _samples[count] * (count + 1);
                count++;
            }
            //Debug.Log("count로 나누기전 average = " + average);
            average /= count;
            //Debug.Log("count로 나눈후 average = " + average);
            _freqBand[i] = average * 10;
            //Debug.Log("_freqBand[" + i + "] 의 값" + _freqBand[i]);
        }
    }

   
}
