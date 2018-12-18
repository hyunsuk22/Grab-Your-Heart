using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointLineRenderer : MonoBehaviour {
	public LineRenderer lineRenderer;
	public float TimeToDestroy = -1f;

	// Use this for initialization
	void Start () {
		//lineRenderer = GetComponent<LineRenderer>();
		if (TimeToDestroy > 0)
			Destroy(gameObject, TimeToDestroy);
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void SetPosition(Vector3 p1, Vector3 p2)
	{
		lineRenderer.SetPosition(0, p1);
		lineRenderer.SetPosition(1, p2);
	}

}
