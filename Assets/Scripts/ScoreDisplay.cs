using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour {

    public Vector3 startPos;
    //public Vector3 targetPos;
    public float speed = 0.01f;
    private int score;
    public int Score
    {
        set
        {
            GetComponent<TextMeshPro>().text = "+" + value.ToString();
        }
    }

    private Color colour;
    private float lifetime;

    private void Start()
    {
        Debug.Log("alive");
        colour = GetComponent<TextMeshPro>().color;
    }

    public void Update()
    {
        if (lifetime <= Globals.SCORE_DURATION)
        {
            transform.Translate(new Vector3(0, speed, 0));
            GetComponent<TextMeshPro>().color = Color.Lerp(colour, new Color(colour.r, colour.g, colour.b, 0.0f), lifetime);
            lifetime += Time.deltaTime;
        }
        else
            Destroy(this.gameObject);
    }
}
