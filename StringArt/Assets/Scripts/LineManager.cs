using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManager : MonoBehaviour {
    public Material _material;
    public Color _color;
    private Material _matInstance;
    public int _audioBandMaterial;
    public float _emissionMultiplier;
	// Use this for initialization
	void Start () {
        //apply material
        _matInstance = new Material(_material);
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
