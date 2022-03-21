using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerComponent : MonoBehaviour
{
    // Update is called once per frame
    void OnGUI()
    {
        Event e = Event.current;
        if (e.type==EventType.KeyDown && e.keyCode.ToString().Length == 1 && char.IsLetter(e.keyCode.ToString()[0]))
        {
            GameObject.Find("Text[1][1]");
        }
    }
}
