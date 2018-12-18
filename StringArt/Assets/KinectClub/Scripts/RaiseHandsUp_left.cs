using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseHandsUp_left : MonoBehaviour {
    public int playerIndex = 0;
    public UnityEngine.UI.RawImage backgroundImage;

    public InteractionManager interManager;
    public InteractionManager.HandEventType prevHandEvent = InteractionManager.HandEventType.Release;

    public GameObject obj;
    public GameObject throwingFx;
    public KinectManager manager;
    public KinectInterop.JointType leftHandJoint = KinectInterop.JointType.HandLeft;
    public KinectInterop.JointType leftShoulderJoint = KinectInterop.JointType.ShoulderLeft;

    private GameObject newFx;
    private Camera mainCamera;
    private Vector3 handPos;
    private Vector3 shoulderPos;
    private Vector3 prevHandPos;

    private float prevHand_ShoulderGap = -1;
    private bool isThrow;
    void Start()
    {
        mainCamera = Camera.main;
        manager = KinectManager.Instance;
        interManager = InteractionManager.Instance;
    }

    void Update()
    {
        handInteraction(leftHandJoint, leftShoulderJoint);
    }

    void handInteraction(KinectInterop.JointType hand, KinectInterop.JointType shoulder)
    {
        if (manager && manager.IsInitialized())
        {
            backgroundImage.texture = manager.GetUsersClrTex();
            backgroundImage.rectTransform.localScale = manager.GetColorImageScale();
            backgroundImage.color = Color.black;

            Rect cameraRect = mainCamera.pixelRect;
            long userId = manager.GetUserIdByIndex(playerIndex);
            //던지고 난후 다시잡기.
            if (manager.IsJointTracked(userId, (int)hand))
            {
                obj.SetActive(true);
                Vector3 posJoint = manager.GetJointPosColorOverlay(userId, (int)hand, mainCamera, cameraRect);
                if (!isThrow)
                {
                    obj.transform.position = posJoint;
                }
            }
            else
            {
                obj.SetActive(false);
            }


            if (manager.IsJointTracked(userId, (int)shoulder) && manager.IsJointTracked(userId, (int)hand))
            {
                handPos = manager.GetJointPosColorOverlay(userId, (int)hand, mainCamera, cameraRect);
                shoulderPos = manager.GetJointPosColorOverlay(userId, (int)shoulder, mainCamera, cameraRect);
                //Debug.Log("Right Shoulder Y Pos = " + shoulderPos.y.ToString("N2"));
                //Debug.Log("Right Hand Y Pos = " + handPos.y.ToString("N2"));
                //obj.transform.position = handPos;
                float hand_shoulderGap = handPos.y - shoulderPos.y;
                //Debug.Log("Hand_Shoulder Gap: " + hand_shoulderGap);
                //Debug.Log("RightHand Z Pos: " + handPos.z.ToString("N2"));
                obj.transform.localScale = new Vector3(1, 1, 1);
                if (hand_shoulderGap > 0)
                {
                    obj.GetComponent<Renderer>().material.color = new Color(1, 0, 0);
                    //Debug.Log("Throw!");

                    if (prevHand_ShoulderGap < 0)
                    {
                        Vector3 forceVector = handPos - prevHandPos;
                        forceVector = forceVector * 600f;
                        //Debug.Log("force: " + forceVector);

                        newFx = Instantiate(throwingFx, obj.transform.position, Quaternion.identity);
                        ParticleSystem parts = newFx.GetComponent<ParticleSystem>();
                        float totalDuration = parts.duration + parts.startLifetime;
                        Destroy(newFx, totalDuration);

                        isThrow = true;
                        obj.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                        obj.GetComponent<Rigidbody>().AddForce(forceVector);
                    }
                }
                else
                {
                    obj.GetComponent<Renderer>().material.color = new Color(1, 1, 1);
                }
                if (isThrow)
                {
                    if (obj.transform.position.y < -10 || obj.transform.position.y > 10)
                    {
                        isThrow = false;
                    }
                }
                prevHand_ShoulderGap = hand_shoulderGap;
                prevHandPos = handPos;
            }
            if (interManager && interManager.IsInteractionInited())
            {
                //Debug.Log("InterManager Init");
                InteractionManager.HandEventType handEvent = interManager.GetLastLeftHandEvent();

                if (handEvent == InteractionManager.HandEventType.Grip)
                {
                    //Debug.Log("grip");
                    obj.GetComponent<Renderer>().material.color = new Color(0, 0, 1);
                    GameObject.Find("Script").GetComponent<AudioSource>().volume -= 0.01f;
                    //Debug.Log("Volume Down");
                }
                else if (handEvent == InteractionManager.HandEventType.Release)
                {
                    GameObject.Find("Script").GetComponent<AudioSource>().volume += 0.01f;
                    //Debug.Log("Volume Up");
                }

            }
        }
    }
}
