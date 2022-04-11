using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubicleScript : MonoBehaviour
{

    TMPro.TextMeshPro textMesh;

    // Start is called before the first frame update
    void Start()
    {
        textMesh = this.GetComponentInChildren<TMPro.TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        SetLetter('B');
        SetIfCorrect(IsLetterCorrect.Incorrect);
    }

    void SetLetter(char letter)
    {
        textMesh.text = letter.ToString();
    }

    void SetIfCorrect(IsLetterCorrect isLetterCorrect)
    {
        switch(isLetterCorrect)
        {
            case IsLetterCorrect.CorrectInGoodPlace:
                textMesh.color = new Color(0, 255, 0, 255);
                break;

            case IsLetterCorrect.CorrectInWrongPlace:
                textMesh.color = new Color(255, 255, 0, 255);
                break;

            case IsLetterCorrect.Incorrect:
                textMesh.color = new Color(255, 0, 0, 255);
                break;
        }
    }
}

enum IsLetterCorrect
{
    CorrectInGoodPlace,
    CorrectInWrongPlace,
    Incorrect
}
