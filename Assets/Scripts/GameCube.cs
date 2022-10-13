using UnityEngine;

public class GameCube : MonoBehaviour
{
    [SerializeField] private Collider2D collider2D;
    
    private bool canMove = false;

    public bool CanMove { get => canMove; }    
    
    public void ChangeMoveState(bool state)
    {
        canMove = state;
        collider2D.enabled = state;
    }
}
