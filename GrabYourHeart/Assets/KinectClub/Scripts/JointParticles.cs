using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointParticles : MonoBehaviour
{
	public int playerIndex = 0;
	public GameObject jointPrefab;
	public GameObject linePrefab;
	public Vector3 scaleFactors = Vector3.one;
	public KinectManager manager;
	private GameObject[] joints = null;
	private GameObject[] lines = null;
	private int jointsCount = 0;

	// Use this for initialization
	void Start()
	{
		//material = GetComponent<MeshRenderer>().material;
		manager = KinectManager.Instance;
		if (manager && manager.IsInitialized())
		{
			jointsCount = manager.GetJointCount();
			joints = new GameObject[jointsCount];
			lines = new GameObject[jointsCount];
			for (int i = 0; i < joints.Length; i++)
			{
				joints[i] = Instantiate(jointPrefab) as GameObject;
				joints[i].transform.parent = transform;
				joints[i].name = i.ToString() + "." + ((KinectInterop.JointType)i).ToString();
				joints[i].SetActive(false);

				lines[i] = Instantiate(linePrefab) as GameObject;
				lines[i].transform.parent = transform;
				lines[i].name = i.ToString() + ".line_" + ((KinectInterop.JointType)i).ToString();
				lines[i].SetActive(false);
			}
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (manager && manager.IsInitialized() && manager.IsUserDetected(playerIndex))
		{
			long userId = manager.GetUserIdByIndex(playerIndex);
			for (int joint = 0; joint < jointsCount; joint++)
			{
				if (manager.IsJointTracked(userId, joint))
				{
                    Vector3 posJoint = manager.GetJointPosition(userId, joint);
					posJoint = new Vector3(posJoint.x * scaleFactors.x,
						posJoint.y * scaleFactors.y, posJoint.z * scaleFactors.z);
					joints[joint].SetActive(true);
					joints[joint].transform.position = posJoint;

					int jointParent = (int)manager.GetParentJoint((KinectInterop.JointType)joint);
					//Debug.Log("(joint, parent)=" + joint + "," + jointParent);
					if (manager.IsJointTracked(userId, jointParent))
					{
						Vector3 posParent = manager.GetJointPosition(userId, jointParent);
						posParent = new Vector3(posParent.x * scaleFactors.x,
							posParent.y * scaleFactors.y, posParent.z * scaleFactors.z);
						lines[joint].GetComponent<JointLineRenderer>().SetPosition(posJoint, posParent);
						//lines[joint].SetActive(true);
					}

				}
				else
				{
					joints[joint].SetActive(false);
					//lines[joint].SetActive(false);
				}
			}
		}
	}
}
