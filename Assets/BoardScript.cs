using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardScript : MonoBehaviour
{
    public GameObject cubiclePrefab;
    GameObject[,] cubicles;
    public GameObject winningAnimationObject;
    public GameObject losingAnimationObject;
    private float timeRemaining;
    private bool hasRanOutOfTime;
    private static bool hasWon;
    private static bool hasLost;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        cubicles = new GameObject[6, 5];
        GenerateCubicles();
        timeRemaining = 120;
        hasRanOutOfTime = false;
        hasLost = false;
        hasWon = false;

        //Invoke("playLosingAnimation", 3.0f); //just to showcase it working
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0 && !hasWon && !hasLost)
        {
            timeRemaining -= Time.deltaTime;
        }
        else if (!hasRanOutOfTime && !hasWon && !hasLost)
        {
            hasRanOutOfTime = true;
            Invoke("playLosingAnimation", 0.0f);
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 20), getRoundedTime());
    }

    private string getRoundedTime()
    {
        if (hasRanOutOfTime)
        {
            return "Out of time!";
        }
        else
        {
            if (timeRemaining <= 0)
            {
                return "Time: 0";
            }
            else
            {
                return "Time: " + Mathf.RoundToInt(timeRemaining).ToString();
            }
        }
    }



    void GenerateCubicles()
    {
        for (int i=0; i<6; i++)
        {
            for (int j=0; j<5; j++)
            {
                cubicles[i,j] = (GameObject)Instantiate(cubiclePrefab);
                cubicles[i, j].name = "Text[" + (5-i) + "][" + j + "]";
                cubicles[i, j].transform.position = new Vector3(j * 1.1f - 2.75f + 0.55f, i * 1.1f - 3.3f, 0.0f);
            }
        }
    }

    public void playWinningAnimation()
    {
        winningAnimationObject.GetComponent<Animator>().Play("sliderAnim");
        hasWon = true;
    }

    public void playLosingAnimation()
    {
        losingAnimationObject.GetComponent<Animator>().Play("sliderAnim");
        hasLost = true;
    }
}
