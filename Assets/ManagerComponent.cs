using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerComponent : MonoBehaviour
{
    // Update is called once per frame
    void OnGUI()
    {
        int line = 1;
        int column = 1;
        Event e = Event.current;
        if(Input.GetKey(KeyCode.RightAlt && e.keyCode.ToString().Length == 1 && char.IsLetter(e.keyCode.ToString()[0]))
            {
            switch (e.keyCode)
                {
                case (KeyCode.E):
                    FillIn("Ę");
                    break;
                case (KeyCode.O):
                    FillIn("Ó");
                    break;
                case (KeyCode.L):
                    FillIn("Ł");
                    break;
                case (KeyCode.S):
                    FillIn("Ś");
                    break;
                case (KeyCode.A):
                    FillIn("Ą");
                    break;
                case (KeyCode.Z):
                    FillIn("Ż");
                    break;
                case (KeyCode.X):
                    FillIn("Ź");
                    break;
                case (KeyCode.N):
                    FillIn("Ń");
                    break;
                }
            }
        else if (e.type==EventType.KeyDown && e.keyCode.ToString().Length == 1 && char.IsLetter(e.keyCode.ToString()[0]))
        {
            FillIn(e.keyCode.ToString());
        }
        if (Input.GetKeyDown(KeyCode.Return) && column==6)
        {
            column = 1;
            line++;
        }
    }

    public void FillIn (string c)
    {
        Text text = GameObject.Find("Text[" + line + "][" + column + "]").GetComponent<Text>();
        text.text = c;
        column++;
    }
}
