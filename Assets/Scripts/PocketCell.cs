using UnityEngine;

public class PocketCell : MonoBehaviour
{
    [SerializeField] PocketType pocketType;
    
    private const float bigPocketScale = 1.2f;
    private GameCube currentCube;

    public bool IsEmpty { get => (GetCubeInfo() == null); }

    private void Awake()
    {
        SetPocketScale();
    }

    public void SetCube(GameCube cube)
    {
        if (IsEmpty)
        {
            currentCube = cube;
        }
    }

    private GameCube GetCubeInfo()
    {
        return currentCube;
    }

    public void TakeCube()
    {
        currentCube = null;
    }

    private void SetPocketScale()
    {
        float newScale = 1.0f;
        if (pocketType == PocketType.Big)
        {
            newScale = bigPocketScale;
        }
        transform.localScale = new Vector2(newScale, newScale);
    }
}
