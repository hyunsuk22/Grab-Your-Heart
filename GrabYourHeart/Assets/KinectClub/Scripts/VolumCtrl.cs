using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumCtrl : MonoBehaviour {
    private void OnCollisionStay(Collision col)
    {
        GameObject.Find("Script").GetComponent<AudioSource>().volume -= 0.1f;
        Debug.Log("Volume Down");
    }
    private void OnCollisionExit(Collision col)
    {
        GameObject.Find("Script").GetComponent<AudioSource>().volume += 0.1f;
        Debug.Log("Volume Up");
    }
}
