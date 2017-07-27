using UnityEngine;
using System.Collections;

public class CreateCube : MonoBehaviour {

    public GameObject cube;
    private int spawnX;
    private int prev;
    private Renderer rend;


	// Use this for initialization
	void Start () {
        StartCoroutine(create());
    }
	
	// Update is called once per frame
	void Update () {
        
	}

    IEnumerator create()
    {
        yield return new WaitForSeconds(Globals.createTime);
        while(spawnX == prev)
            spawnX = Random.Range(1, 10);
        Vector3 newPos = new Vector3(spawnX, 14, 0);
        prev = spawnX;
        GameObject newCube = (GameObject)Instantiate(cube, newPos, Quaternion.identity);
        int rand = Random.Range(0, 6);
        rend = newCube.transform.GetChild(1).GetComponent<Renderer>();
        Color colour;
        if (rand == 0)
            colour = new Color(1, 0, 0);
        else if (rand == 1)
            colour = new Color(0, 1, 0);
        else if (rand == 2)
            colour = new Color(0, 0, 1);
        else if (rand == 3)
            colour = new Color(1, 1, 0);
        else if (rand == 4)
            colour = new Color(1, 0, 1);
        else
            colour = new Color(0, 1, 1);
        rend.material.color = colour;
        newCube.GetComponent<CubeProperties>().colour = colour;

        if (!Globals.gameOver)
            StartCoroutine(create());
    }
}
