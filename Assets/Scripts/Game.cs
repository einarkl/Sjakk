using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    public GameObject chessPiece;

    public AudioSource source;
    public AudioClip clipMove, clipAttack;

    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerWhite = new GameObject[16];
    private GameObject[] playerBlack = new GameObject[16];

    private string currentPlayer = "white";

    private bool gameOver = false;
    public bool wKingMoved, wRookLeftMoved, wRookRightMoved, bKingMoved, bRookLeftMoved, bRookRightMoved = false;

    public string currentPiece;
    public Vector3 currentPosition;

    void Start()
    {
        //Instantiate(chessPiece, new Vector3(0, 0, 0), Quaternion.identity);
        playerWhite = new GameObject[] {
            Create("w_R",0,0), Create("w_N",1,0), Create("w_B",2,0), Create("w_Q",3,0),
            Create("w_K",4,0), Create("w_B",5,0), Create("w_N",6,0), Create("w_R",7,0),
            Create("w_P",0,1), Create("w_P",1,1), Create("w_P",2,1), Create("w_P",3,1),
            Create("w_P",4,1), Create("w_P",5,1), Create("w_P",6,1), Create("w_P",7,1)
        };
        playerBlack = new GameObject[] {
            Create("b_R",0,7), Create("b_N",1,7), Create("b_B",2,7), Create("b_Q",3,7),
            Create("b_K",4,7), Create("b_B",5,7), Create("b_N",6,7), Create("b_R",7,7),
            Create("b_P",0,6), Create("b_P",1,6), Create("b_P",2,6), Create("b_P",3,6),
            Create("b_P",4,6), Create("b_P",5,6), Create("b_P",6,6), Create("b_P",7,6)
        };

        for (int i = 0; i < playerWhite.Length; i++)
        {
            SetPosition(playerWhite[i]);
            SetPosition(playerBlack[i]);
        }
    }

    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chessPiece, new Vector3(0, 0, 0), Quaternion.identity);
        ChessMan cm = obj.GetComponent<ChessMan>();
        cm.name = name;
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.Activate();

        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        ChessMan cm = obj.GetComponent<ChessMan>();

        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }
    // Fiks denne for tårn
    public void SetPositionCastling(GameObject obj)
    {
        ChessMan cm = obj.GetComponent<ChessMan>();

        positions[cm.GetXBoard(), cm.GetYBoard()] = obj;
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        if (x < 0 || y < 0 || x >= positions.GetLength(0) || y >= positions.GetLength(1))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        if (currentPlayer == "white")
        {
            currentPlayer = "black";
        }
        else
        {
            currentPlayer = "white";
        }
    }

    public void Update()
    {
        if (gameOver == true && Input.GetMouseButtonDown(0))
        {
            gameOver = false;

            SceneManager.LoadScene("Game");
        }
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;

        char[] charArr = playerWinner.ToCharArray();
        string first = charArr[0] + "";
        string rest = "";
        for (int i = 1; i < charArr.Length; i++)
        {
            rest += charArr[i];
        }
        string winner = first.ToUpper() + rest;


        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = winner + " is the winner!";

        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }

    public void PlayMoveSound()
    {
        source.PlayOneShot(clipMove);
    }

    public void PlayAttackSound()
    {
        source.PlayOneShot(clipAttack);
    }
}
