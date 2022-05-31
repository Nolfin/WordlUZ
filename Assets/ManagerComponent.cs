using System;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Random = System.Random;

public class ManagerComponent : MonoBehaviour
{
    private Vector3 targetPosition = new Vector3(18f, -0.5f, 10.6f);
    private Vector3 startPosition = new Vector3(36f, -0.5f, 9f);
    public GameObject cubiclePrefab;
    GameObject[,] cubicles;
    public GameObject winningAnimationObject;
    public GameObject losingAnimationObject;
    private float timeRemaining;
    private bool hasRanOutOfTime;
    private static bool hasWon;
    private static bool hasLost;
    private Text text;
    private short activeMode = 0;
    private int column;
    private readonly string[] inputWord = new string[5];
    private int line;

    private string theWord;
    private string[] theWordArray = new string[5];

    public GameObject board;

    // Update is called once per frame

    void Start()
    {
        cubicles = new GameObject[6, 5];
        GenerateCubicles();
        timeRemaining = 120;
        hasRanOutOfTime = false;
        hasLost = false;
        hasWon = false;
    }
    void Update()
    {
        if (timeRemaining > 0 && !hasWon && !hasLost && activeMode==2)
        {
            
            timeRemaining -= Time.deltaTime;
        }
        else if (!hasRanOutOfTime && !hasWon && !hasLost && activeMode==2)
        {
            hasRanOutOfTime = true;
            playLosingAnimation();
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
        iTween.MoveTo(winningAnimationObject, iTween.Hash("position", targetPosition, "time", 1.0f, "easetype", iTween.EaseType.easeInOutSine));
        hasWon = true;
    }

    public void playLosingAnimation()
    {
        iTween.MoveTo(losingAnimationObject, iTween.Hash("position", targetPosition, "time", 1.0f, "easetype", iTween.EaseType.easeInOutSine));
        hasLost = true;
    }
    private string getRoundedTime()
    {
        if (timeRemaining <= 0 || activeMode!=2) return null;
        return "Time: " + Mathf.RoundToInt(timeRemaining).ToString();
    }
    private void OnGUI()
    {
        
        var e = Event.current;
        if (Input.GetKeyDown(KeyCode.Return) && column == 5 && e.type == EventType.KeyDown)
        {
            if (!IsCorrectWord()) return;
            column = 0;
            if (line == 0)
            {
                theWord = RollAWord();
                Debug.Log(theWord);
                for (int i = 0; i < 5; i++)
                {
                    theWordArray[i] = theWord.Substring(i, 1);
                }
            }
            CheckForSimilarities();
            if (activeMode == 1) ChangeRandomLetterColour();
            line++;
            if(line==6) playLosingAnimation();
        }

        if (Input.GetKey(KeyCode.Backspace) && e.type == EventType.KeyDown)
        {
            if (column == 0) return;
            column--;
            FillIn(" ");
            column--;
        }
        GUI.Label(new Rect(0, 0, 100, 20), getRoundedTime());
        if (column == 5) return;

        if (Input.GetKey(KeyCode.RightAlt) && e.keyCode.ToString().Length == 1 && e.type == EventType.KeyDown)
            switch (e.keyCode)
            {
                case KeyCode.E:
                    FillIn("Ę");
                    break;
                case KeyCode.O:
                    FillIn("Ó");
                    break;
                case KeyCode.L:
                    FillIn("Ł");
                    break;
                case KeyCode.S:
                    FillIn("Ś");
                    break;
                case KeyCode.A:
                    FillIn("Ą");
                    break;
                case KeyCode.Z:
                    FillIn("Ż");
                    break;
                case KeyCode.X:
                    FillIn("Ź");
                    break;
                case KeyCode.N:
                    FillIn("Ń");
                    break;
                case KeyCode.C:
                    FillIn("Ć");
                    break;
            }
        else if (e.type == EventType.KeyDown && e.keyCode.ToString().Length == 1 &&
                 char.IsLetter(e.keyCode.ToString()[0])) FillIn(e.keyCode.ToString());

        EventSystem.current.SetSelectedGameObject(null);
        


    }
    void FillIn(string c)
    {
        var text = GameObject.Find("Text[" + line + "][" + column + "]").transform.GetChild(0).transform.GetChild(1)
            .GetComponent<TextMeshPro>();
        text.text = c;
        inputWord[column] = c;
        column++;
    }
    public string RollAWord()
    {
        string result;
        var path = "wordluz_passwords.txt";
        var lineCount = File.ReadLines(path).Count();
        var random = new Random();
        var lineChosen = random.Next(lineCount / 2);
        result = File.ReadLines(path).Skip(lineChosen * 2).Take(1).First();
        return result;
    }

    public bool IsCorrectWord()
    {
        var path = "wordluz_all_words.txt";
        var word = "";
        for (var i = 0; i < 5; i++)
        {
            word += inputWord[i].ToLower();
        }
        

        using (var sr = new StreamReader(path))
        {
            string lInE;
            while ((lInE = sr.ReadLine()) != null)
                if (word.Trim().Equals(lInE))
                    return true;
        }
        //TODO: Make sign for user that the word is incorrect
        return false;
    }

    public void CheckForSimilarities()
    {
        bool[] usedLetters = new bool[5];
        bool fullCorrect = true;
        for (int i = 0; i < 5; i++)
        {
            usedLetters[i] = false;
            if(inputWord[i].ToLower()==theWordArray[i]) {
                GameObject.Find("Text[" + line + "][" + i + "]").transform.GetChild(0).transform.GetChild(1)
                .GetComponent<TextMeshPro>().color = new Color(0, 255, 0, 255);
                usedLetters[i] = true;
            }
            else
            {
                fullCorrect = false;
            }
        }
        
        if (fullCorrect)
        {
            playWinningAnimation();
        }

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (i == j) continue;
                if (inputWord[i].ToLower() == theWordArray[j] && usedLetters[j] == false && 
                    GameObject.Find("Text[" + line + "][" + i + "]").transform.GetChild(0).transform.GetChild(1)
                    .GetComponent<TextMeshPro>().color != new Color(0, 255, 0, 255))
                {
                    GameObject.Find("Text[" + line + "][" + i + "]").transform.GetChild(0).transform.GetChild(1)
                        .GetComponent<TextMeshPro>().color = new Color(255, 255, 0, 255);
                    usedLetters[j] = true;
                    break;
                }
            }

            if (GameObject.Find("Text[" + line + "][" + i + "]").transform.GetChild(0).transform.GetChild(1)
                    .GetComponent<TextMeshPro>().color != new Color(255, 255, 0, 255) &&
                GameObject.Find("Text[" + line + "][" + i + "]").transform.GetChild(0).transform.GetChild(1)
                    .GetComponent<TextMeshPro>().color != new Color(0, 255, 0, 255))
            {
                GameObject.Find("Text[" + line + "][" + i + "]").transform.GetChild(0).transform.GetChild(1)
                    .GetComponent<TextMeshPro>().color = new Color(220,220,220, 255);
            }
            
            
        }
    }

    public void ChangeModeTo1()
    {
        for (double i = 0; i < 30; i++)
        {
            GameObject.Find("Text[" + Math.Floor(i/5) + "][" + i%5 + "]").transform.GetChild(0).transform.GetChild(1)
                .GetComponent<TextMeshPro>().SetText(" ");
            GameObject.Find("Text[" + Math.Floor(i / 5) + "][" + i % 5 + "]").transform.GetChild(0).transform
                .GetChild(1)
                .GetComponent<TextMeshPro>().color = new Color(220, 220, 220, 255);
            Debug.Log("Liars");
        }

        line = 0;
        column = 0;
        activeMode = 1;
        Reset();
    }
    
    public void ChangeModeTo0()
    {
        for (double i = 0; i < 30; i++)
        {
            GameObject.Find("Text[" + Math.Floor(i/5) + "][" + i%5 + "]").transform.GetChild(0).transform.GetChild(1)
                .GetComponent<TextMeshPro>().SetText(" ");
            GameObject.Find("Text[" + Math.Floor(i / 5) + "][" + i % 5 + "]").transform.GetChild(0).transform
                .GetChild(1)
                .GetComponent<TextMeshPro>().color = new Color(220, 220, 220, 255);
            Debug.Log("Normal");
        }

        line = 0;
        column = 0;
        activeMode = 0;
        Reset();
    }
    
    public void ChangeModeTo2()
    {
        for (double i = 0; i < 30; i++)
        {
            GameObject.Find("Text[" + Math.Floor(i/5) + "][" + i%5 + "]").transform.GetChild(0).transform.GetChild(1)
                .GetComponent<TextMeshPro>().SetText(" ");
            GameObject.Find("Text[" + Math.Floor(i / 5) + "][" + i % 5 + "]").transform.GetChild(0).transform
                .GetChild(1)
                .GetComponent<TextMeshPro>().color = new Color(220, 220, 220, 255);
            Debug.Log("Ranked");
        }

        line = 0;
        column = 0;
        activeMode = 2;
        Reset();
    }
    public void NewGame()
    {
        for (double i = 0; i < 30; i++)
        {
            GameObject.Find("Text[" + Math.Floor(i/5) + "][" + i%5 + "]").transform.GetChild(0).transform.GetChild(1)
                .GetComponent<TextMeshPro>().SetText(" ");
            GameObject.Find("Text[" + Math.Floor(i / 5) + "][" + i % 5 + "]").transform.GetChild(0).transform
                .GetChild(1)
                .GetComponent<TextMeshPro>().color = new Color(220, 220, 220, 255);
        }

        line = 0;
        column = 0;
        Reset();
    }

    public void ChangeRandomLetterColour()
    {
        var random = new Random();
        var letterToLie = random.Next(0,4);
        var colorToChange = GameObject.Find("Text[" + line + "][" + letterToLie + "]").transform.GetChild(0).transform
            .GetChild(1)
            .GetComponent<TextMeshPro>().color;
        if (colorToChange == new Color(0, 255, 0, 255))
        {
            if(random.Next(2)==1) colorToChange = new Color(220, 220, 220, 255);
            else colorToChange = new Color(255, 255, 0, 255);
            Debug.Log("Lied green at position "+letterToLie);
            GameObject.Find("Text[" + line + "][" + letterToLie + "]").transform.GetChild(0).transform
                .GetChild(1)
                .GetComponent<TextMeshPro>().color = colorToChange;
            return;
        }
        if (colorToChange == new Color(220, 220, 220, 255))
        {
            if(random.Next(2)==1) colorToChange = new Color(0, 255, 0, 255);
            else colorToChange = new Color(255, 255, 0, 255);
            Debug.Log("Lied gray at position "+letterToLie);
            GameObject.Find("Text[" + line + "][" + letterToLie + "]").transform.GetChild(0).transform
                .GetChild(1)
                .GetComponent<TextMeshPro>().color = colorToChange;
            return;
        }
        if (colorToChange == new Color(255, 255, 0, 255))
        {
            if(random.Next(2)==1) colorToChange = new Color(220, 220, 220, 255);
            else colorToChange = new Color(0, 255, 0, 255);
            Debug.Log("Lied yellow at position "+letterToLie);
            GameObject.Find("Text[" + line + "][" + letterToLie + "]").transform.GetChild(0).transform
                .GetChild(1)
                .GetComponent<TextMeshPro>().color = colorToChange;
            return;
        }
    }

    public void Reset()
    {

        if (hasLost) iTween.MoveTo(losingAnimationObject, iTween.Hash("position", startPosition, "time", 1.0f, "easetype", iTween.EaseType.easeInOutSine));
        if (hasWon) iTween.MoveTo(winningAnimationObject, iTween.Hash("position", startPosition, "time", 1.0f, "easetype", iTween.EaseType.easeInOutSine));
        Debug.Log("Reset called");
        hasLost = false;
        hasWon = false;
        timeRemaining = 120;

    }
}