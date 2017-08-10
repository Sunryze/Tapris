using UnityEngine;
using System.Collections;

public class Wireframe : MonoBehaviour {

    public float growth;
    private float size;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        size += Time.deltaTime * growth;
        transform.localScale = new Vector3(size, size, 1);

        if (size >= 3)
            Destroy(gameObject);
	}
}
