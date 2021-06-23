using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessMan : MonoBehaviour
{
    public GameObject controller;
    public GameObject movePlate;

    private int xBoard = -1;
    private int yBoard = -1;

    private string player;

    public Sprite w_K, w_Q, w_R, w_B, w_N, w_P, b_K, b_Q, b_R, b_B, b_N, b_P;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetCoords();

        switch (this.name)
        {
            case "w_K": this.GetComponent<SpriteRenderer>().sprite = w_K; player = "white"; break;
            case "w_Q": this.GetComponent<SpriteRenderer>().sprite = w_Q; player = "white"; break;
            case "w_R": this.GetComponent<SpriteRenderer>().sprite = w_R; player = "white"; break;
            case "w_B": this.GetComponent<SpriteRenderer>().sprite = w_B; player = "white"; break;
            case "w_N": this.GetComponent<SpriteRenderer>().sprite = w_N; player = "white"; break;
            case "w_P": this.GetComponent<SpriteRenderer>().sprite = w_P; player = "white"; break;

            case "b_K": this.GetComponent<SpriteRenderer>().sprite = b_K; player = "black"; break;
            case "b_Q": this.GetComponent<SpriteRenderer>().sprite = b_Q; player = "black"; break;
            case "b_R": this.GetComponent<SpriteRenderer>().sprite = b_R; player = "black"; break;
            case "b_B": this.GetComponent<SpriteRenderer>().sprite = b_B; player = "black"; break;
            case "b_N": this.GetComponent<SpriteRenderer>().sprite = b_N; player = "black"; break;
            case "b_P": this.GetComponent<SpriteRenderer>().sprite = b_P; player = "black"; break;
        }
    }

    public void SetCoords()
    {
        float x = xBoard;
        float y = yBoard;

        x += -3.5f;
        y += -3.5f;

        this.transform.position = new Vector3(x, y, 0);
    }

    public int GetXBoard()
    {
        return xBoard;
    }

    public int GetYBoard()
    {
        return yBoard;
    }

    public void SetXBoard(int x)
    {
        xBoard = x;
    }

    public void SetYBoard(int y)
    {
        yBoard = y;
    }

    private void OnMouseUp()
    {
        if (!controller.GetComponent<Game>().IsGameOver() &&
            controller.GetComponent<Game>().GetCurrentPlayer() == player)
        {
            DestroyMovePlates();

            InitiateMovePlates();
        }

    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");

        for (int i = 0; i < movePlates.Length; i++)
        {
            Destroy(movePlates[i]);
        }
    }

    public void InitiateMovePlates()
    {
        controller.GetComponent<Game>().currentPiece = this.name;
        controller.GetComponent<Game>().currentPosition = new Vector3(xBoard, yBoard, 0);
        switch (this.name)
        {
            case "w_Q":
            case "b_Q":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(1, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                LineMovePlate(-1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(1, -1);
                break;
            case "w_N":
            case "b_N":
                LMovePlate();
                break;
            case "w_B":
            case "b_B":
                LineMovePlate(1, 1);
                LineMovePlate(1, -1);
                LineMovePlate(-1, 1);
                LineMovePlate(-1, -1);
                break;
            case "w_K":
                SurroundMovePlate();
                // Rokkade
                bool wKingMoved = controller.GetComponent<Game>().wKingMoved;
                bool wRookLeftMoved = controller.GetComponent<Game>().wRookLeftMoved;
                bool wRookRightMoved = controller.GetComponent<Game>().wRookRightMoved;
                GameObject b1 = controller.GetComponent<Game>().GetPosition(1, 0);
                GameObject c1 = controller.GetComponent<Game>().GetPosition(2, 0);
                GameObject d1 = controller.GetComponent<Game>().GetPosition(3, 0);
                GameObject f1 = controller.GetComponent<Game>().GetPosition(5, 0);
                GameObject g1 = controller.GetComponent<Game>().GetPosition(6, 0);
                if (!wKingMoved && !wRookLeftMoved && b1 == null && c1 == null && d1 == null)
                {
                    PointMovePlate(xBoard - 2, yBoard);
                }
                if (!wKingMoved && !wRookRightMoved && f1 == null && g1 == null)
                {
                    PointMovePlate(xBoard + 2, yBoard);
                }
                break;
            case "b_K":
                SurroundMovePlate();
                // Rokkade
                bool bKingMoved = controller.GetComponent<Game>().bKingMoved;
                bool bRookLeftMoved = controller.GetComponent<Game>().bRookLeftMoved;
                bool bRookRightMoved = controller.GetComponent<Game>().bRookRightMoved;
                GameObject b7 = controller.GetComponent<Game>().GetPosition(1, 7);
                GameObject c7 = controller.GetComponent<Game>().GetPosition(2, 7);
                GameObject d7 = controller.GetComponent<Game>().GetPosition(3, 7);
                GameObject f7 = controller.GetComponent<Game>().GetPosition(5, 7);
                GameObject g7 = controller.GetComponent<Game>().GetPosition(6, 7);
                if (!bKingMoved && !bRookLeftMoved && b7 == null && c7 == null && d7 == null)
                {
                    PointMovePlate(xBoard - 2, yBoard);
                }
                if (!bKingMoved && !bRookRightMoved && f7 == null && g7 == null)
                {
                    PointMovePlate(xBoard + 2, yBoard);
                }
                break;
            case "w_R":
            case "b_R":
                LineMovePlate(1, 0);
                LineMovePlate(0, 1);
                LineMovePlate(-1, 0);
                LineMovePlate(0, -1);
                break;
            case "w_P":
                PawnMovePlate(xBoard, yBoard + 1);
                if (yBoard == 1)
                {
                    PawnMovePlate(xBoard, yBoard + 2);
                }
                break;
            case "b_P":
                PawnMovePlate(xBoard, yBoard - 1);
                if (yBoard == 6)
                {
                    PawnMovePlate(xBoard, yBoard - 2);
                }
                break;
        }
    }

    public void LineMovePlate(int xIncrement, int yIncrement)
    {
        Game sc = controller.GetComponent<Game>();

        int x = xBoard + xIncrement;
        int y = yBoard + yIncrement;

        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            MovePlateSpawn(x, y);
            x += xIncrement;
            y += yIncrement;
        }

        if (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y).GetComponent<ChessMan>().player != player)
        {
            MovePlateAttackSpawn(x, y);
        }
    }

    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }

    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard);
        PointMovePlate(xBoard + 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard - 1, yBoard - 1);
    }

    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);

            if (cp == null)
            {
                MovePlateSpawn(x, y);
            }
            else if (cp.GetComponent<ChessMan>().player != player)
            {
                MovePlateAttackSpawn(x, y);
            }
        }
    }

    public void PawnMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (sc.PositionOnBoard(x, y))
        {
            if (sc.GetPosition(x, y) == null)
            {
                MovePlateSpawn(x, y);
            }

            if (sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null &&
                sc.GetPosition(x + 1, y).GetComponent<ChessMan>().player != player)
            {
                MovePlateAttackSpawn(x + 1, y);
            }

            if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null &&
                sc.GetPosition(x - 1, y).GetComponent<ChessMan>().player != player)
            {
                MovePlateAttackSpawn(x - 1, y);
            }
        }
    }

    public void MovePlateSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x += -3.5f;
        y += -3.5f;

        // GameObject mp = Instantiate(movePlate, new Vector3(x,y,-3.0f), Quaternion.identity);
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, 0), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }

    public void MovePlateAttackSpawn(int matrixX, int matrixY)
    {
        float x = matrixX;
        float y = matrixY;

        x += -3.5f;
        y += -3.5f;

        // GameObject mp = Instantiate(movePlate, new Vector3(x,y,-3.0f), Quaternion.identity);
        GameObject mp = Instantiate(movePlate, new Vector3(x, y, 0), Quaternion.identity);

        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(matrixX, matrixY);
    }
}
