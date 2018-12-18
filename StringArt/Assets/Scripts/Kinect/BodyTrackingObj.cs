using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyTrackingObj : MonoBehaviour {

    public int playerIndex = 0;
    public GameObject bodyObj;
    public GameObject _camera;
    public GameObject _selectStage;
    public AudioSource _audioSource;
    public InteractionManager _interactionManager;

    /* Kinect */
    public KinectManager manager;

    private Camera mainCamera;
    public KinectInterop.JointType bodyCenterJoint = KinectInterop.JointType.SpineMid;

    [Header("Distance")]
    [SerializeField]
    private float maxDistance_z = 2.0f;


    void Start () {
        manager = KinectManager.Instance;
        mainCamera = Camera.main;
        bodyObj.SetActive(false);
        _selectStage.SetActive(false);
    }
	
	void Update () {
        if (manager && manager.IsInitialized())
        {
            Rect cameraRect = mainCamera.pixelRect;
            long userId = manager.GetUserIdByIndex(playerIndex);
            if (manager.IsJointTracked(userId, (int)bodyCenterJoint))
            {
                bodyObj.SetActive(true);
                Vector3 jointPos = manager.GetJointPosColorOverlay(userId, (int)bodyCenterJoint, mainCamera, cameraRect);
                //String Art보다 앞에 위치하게함.
                //jointPos.z = -0.2f;
                bodyObj.transform.position = jointPos;
                //플레이어와 카메라 간격에따른 인터렉션.
                //Debug.Log("jointPos" + jointPos);
                //Debug.Log("Volume" + _audioSource.volume);

                //사운드 볼륨 컨트롤 및 카메라 이동
                if (jointPos.z >= maxDistance_z)
                {
                    _audioSource.volume -= 0.01f;
                    if (_audioSource.volume <= 0.1f) _audioSource.Pause();
                    StartCoroutine(_cameraZoomIn());
                    
                }
                else
                {
                    _audioSource.UnPause();
                    if (_audioSource.volume < 0.5f) _audioSource.volume += 0.01f;
                    StartCoroutine(_cameraZoomOut());

                }

                /*
                //그립 이벤트
                if (_interactionManager.GetRightHandEvent() == InteractionManager.HandEventType.Grip)
                {
                    Debug.Log("Grip");
                }
                */
                
            }
            else
            {
                bodyObj.SetActive(false);
            }
        }
       
    }
    private IEnumerator _cameraZoomIn()
    {
        yield return new WaitForSeconds(1.5f);
        iTween.MoveTo(_camera, iTween.Hash(
            "z", 2.5f, 
            "easeType", "easeOutQuad",
            "time", 3.5f
            ));
        _selectStage.SetActive(true);
        
    }

    private IEnumerator _cameraZoomOut()
    {
        yield return new WaitForSeconds(1.5f);
        iTween.MoveTo(_camera, iTween.Hash(
            "z", -2.5f,
            "easeType", "easeInQuad",
            "time", 3.5f
            ));
        _selectStage.SetActive(false);
    }
}
