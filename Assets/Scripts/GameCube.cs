using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCube : MonoBehaviour
{
    private bool canMove = false;

    public bool CanMove { get => canMove; set => canMove = value; }

    private void Update()
    {
        GetComponent<Collider2D>().enabled = canMove;
    }
}
