using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    public GameObject controller;

    GameObject reference = null;

    // Board positions, not world positions
    int matrixX;
    int matrixY;

    // false: movement, true: attacking
    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    public void OnMouseUp()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        if (attack)
        {
            GameObject cp = controller.GetComponent<Game>().GetPosition(matrixX, matrixY);

            // Hvis kongen blir tatt => Sjakk matt
            if (cp.name == "w_K")
            {
                controller.GetComponent<Game>().Winner("black");
                controller.GetComponent<Game>().PlayMateSound();
            }
            if (cp.name == "b_K")
            {
                controller.GetComponent<Game>().Winner("white");
                controller.GetComponent<Game>().PlayMateSound();
            }

            Destroy(cp);

            // Spill av lyd
            controller.GetComponent<Game>().PlayAttackSound();
        }

        controller.GetComponent<Game>().SetPositionEmpty(reference.GetComponent<ChessMan>().GetXBoard(),
                                                        reference.GetComponent<ChessMan>().GetYBoard());

        reference.GetComponent<ChessMan>().SetXBoard(matrixX);
        reference.GetComponent<ChessMan>().SetYBoard(matrixY);
        reference.GetComponent<ChessMan>().SetCoords();

        controller.GetComponent<Game>().SetPosition(reference);

        // Flytt tårn ved rokkade
        string currentPiece = controller.GetComponent<Game>().currentPiece;
        Vector3 currentPosition = controller.GetComponent<Game>().currentPosition;

        // Lang rokkade for hvit
        if (currentPiece == "w_K" && matrixX - currentPosition.x == -2)
        {
            Debug.Log("Lang rokkade hvit");
        }
        // Kort rokkade for hvit
        else if (currentPiece == "w_K" && matrixX - currentPosition.x == 2)
        {
            Debug.Log("Kort rokkade hvit");
        }
        // Lang rokkade for svart
        else if (currentPiece == "b_K" && matrixX - currentPosition.x == -2)
        {
            Debug.Log("Lang rokkade svart");
        }
        // Kort rokkade for svart
        else if (currentPiece == "b_K" && matrixX - currentPosition.x == 2)
        {
            Debug.Log("Kort rokkade svart");
        }

        controller.GetComponent<Game>().NextTurn();

        reference.GetComponent<ChessMan>().DestroyMovePlates();

        // Spill av lyd
        controller.GetComponent<Game>().PlayMoveSound();
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }
}
