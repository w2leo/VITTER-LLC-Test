using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCell : MonoBehaviour
{
    private int x;
    private int y;
    private bool isEmpty;

    public int X => x;

    public int Y => y;

    public void SetInitData(Transform parent, Vector2 position, string name, int x, int y)
    {
        transform.SetParent(parent);
        transform.localPosition = position;
        gameObject.name = name;
        isEmpty = true;
        this.x = x;
        this.y = y;
    }

    public void SetCube()
    {
        isEmpty = false;
    }   
}
