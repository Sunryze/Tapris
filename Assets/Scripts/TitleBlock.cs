using UnityEngine;
using System.Collections;

public class TitleBlock : MonoBehaviour {

    public Color colour;

	// Use this for initialization
	void Start () {
        transform.GetChild(1).GetComponent<Renderer>().material.color = colour;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
