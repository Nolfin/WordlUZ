using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagerComponent : MonoBehaviour
{
    int line = 0;
    int column = 0;
    // Update is called once per frame
    void OnGUI()
    {
        if (Input.GetKeyDown(KeyCode.Return) && column == 5)
        {
            column = 0;
            line++;
        }
        if (column == 5) return;
        Event e = Event.current;
        if(Input.GetKey(KeyCode.RightAlt) && e.keyCode.ToString().Length == 1 && char.IsLetter(e.keyCode.ToString()[0]))
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

        void FillIn(string c)
        {
            TextMeshPro text = GameObject.Find("Text[" + line + "][" + column + "]").transform.GetChild(0).transform.GetChild(1).GetComponent<TextMeshPro>();
            text.text = c;
            column++;
            Debug.Log(column + " " + line);
        }
    }


}
