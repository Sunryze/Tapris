using UnityEngine;
using System.Collections;

public class CreateCubeMenu : MonoBehaviour {

    [SerializeField] private GameObject cube;
    [SerializeField] private int spawnX;
    private int prev;
    private Renderer rend;


	// Use this for initialization
	void Start () {
        StartCoroutine(Create());
    }
	
	// Update is called once per frame
	void Update () {

	}

    IEnumerator Create()
    {
        yield return new WaitForSeconds(3);
        Color colour;
        // Prevent cube from spawning in same location twice
        while (spawnX == prev)
            spawnX = Random.Range(1, 10);
        Vector3 newPos = new Vector3(spawnX, 15, 0);
        prev = spawnX;
        GameObject newCube = (GameObject)Instantiate(cube, newPos, Quaternion.identity);

        // Set cube's colour spawn type
        int randInt = Random.Range(0, 6);
        if (randInt == 0)
            colour = new Color(1, 0, 0, 0.5f);
        else if (randInt == 1)
            colour = new Color(0, 1, 0, 0.5f);
        else if (randInt == 2)
            colour = new Color(0, 0, 1, 0.5f);
        else if (randInt == 3)
            colour = new Color(1, 1, 0, 0.5f);
        else if (randInt == 4)
            colour = new Color(1, 0, 1, 0.5f);
        else
            colour = new Color(0, 1, 1, 0.5f);
        
        rend = newCube.transform.GetChild(1).GetComponent<Renderer>();
        rend.material.color = colour;
        newCube.GetComponent<CubeProperties>().colour = colour;
        newCube.GetComponent<CubeProperties>().menuCube = true;
        StartCoroutine(Create());
    }
}
