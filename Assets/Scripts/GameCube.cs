using UnityEngine;

public class GameCube : MonoBehaviour
{
    private const float baseZ = -1.0f;

    private bool canMove = false;

    public bool CanMove { get => canMove; }

    public void ChangeMoveState(bool state)
    {
        canMove = state;
        ChangeColliderState(state);
    }

    public void ChangeColliderState(bool state)
    {
        GetComponent<Collider2D>().enabled = state;
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = new Vector3(position.x, position.y, baseZ); ;
    }

    public void SetPosition(float x, float y)
    {
        transform.position = new Vector3(x, y, baseZ);
    }
}
