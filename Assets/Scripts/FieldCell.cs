using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldCell : MonoBehaviour
{
    private int x;
    private int y;

    public int X => x;

    public int Y => y;

    public void SetInitData(Transform parent, Vector2 position, string name, int x, int y)
    {
        transform.SetParent(parent);
        transform.localPosition = position;
        gameObject.name = name;
        this.x = x;
        this.y = y;
    }
}
