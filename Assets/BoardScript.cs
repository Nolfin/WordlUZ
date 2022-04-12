using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScript : MonoBehaviour
{
    public GameObject cubiclePrefab;
    GameObject[,] cubicles;

    // Start is called before the first frame update
    void Start()
    {
        cubicles = new GameObject[6, 5];
        GenerateCubicles();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateCubicles()
    {
        for (int i=0; i<6; i++)
        {
            for (int j=0; j<5; j++)
            {
                cubicles[i,j] = (GameObject)Instantiate(cubiclePrefab);
                cubicles[i, j].name = "Text[" + (5-i) + "][" + j + "]";
                cubicles[i, j].transform.position = new Vector3(j * 1.1f - 2.75f, i * 1.1f - 3.3f, 0.0f);
            }
        }    
    }
}
