using UnityEngine;
using System.Collections;

public class CreateCube : MonoBehaviour {

    public GameObject cube;
    private int chanceOfSpecial = 20;       // 1 in 'chanceOfSpecial'
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
        Color colour;
        // Prevent cube from spawning in same location twice
        while (spawnX == prev)
            spawnX = Random.Range(-1, 12);
        Vector3 newPos = new Vector3(spawnX, 13, 0);
        prev = spawnX;
        GameObject newCube = (GameObject)Instantiate(cube, newPos, Quaternion.identity);

        // Set cube's colour spawn type
        int randInt = Random.Range(0, chanceOfSpecial);
        if (randInt == 0 && Globals.elapsedTime >= Globals.allowWhite)
            colour = new Color(1, 1, 1);
        else if (randInt == 1 && Globals.elapsedTime >= Globals.allowGrey)
            colour = new Color(0.5f, 0.5f, 0.5f);
        else {
            randInt = Random.Range(0, 6);
            if (randInt == 0)
                colour = new Color(1, 0, 0);
            else if (randInt == 1)
                colour = new Color(0, 1, 0);
            else if (randInt == 2)
                colour = new Color(0, 0, 1);
            else if (randInt == 3)
                colour = new Color(1, 1, 0);
            else if (randInt == 4)
                colour = new Color(1, 0, 1);
            else
                colour = new Color(0, 1, 1);
        }
        rend = newCube.transform.GetChild(1).GetComponent<Renderer>();
        rend.material.color = colour;
        newCube.GetComponent<CubeProperties>().colour = colour;

        if (!Globals.gameOver)
            StartCoroutine(create());
    }
}
