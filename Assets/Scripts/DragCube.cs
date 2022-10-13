using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum CubeState
{
    Enabled,
    Disabled
}

public class DragCube : MonoBehaviour
{
    [SerializeField] FieldSpawner fieldSpawner;
    [SerializeField] EndGamePanel winPanel;

    private Vector2 startPosition;
    private GameCube cube;
    private FieldCell field;
    private PocketCell pocket;
    private PocketCell oldPocket;
    private Ray ray;
    private RaycastHit2D hit;

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    StartTouchPhase(touch.position);
                    break;

                case TouchPhase.Moved:
                    MoveTouchPhase(touch.position);
                    break;

                case TouchPhase.Ended:
                    EndTouchPhase(touch.position);
                    break;
            }
        }
    }

    private void StartTouchPhase(Vector2 touchPosition)
    {
        ActivateRaycast(touchPosition);
        if (hit.collider != null && hit.collider.gameObject.TryGetComponent<GameCube>(out cube) && cube.CanMove)
        {
            startPosition = cube.transform.position;
            cube.ChangeColliderState(false);
            oldPocket = GetPocket(touchPosition);
        }
        else
        {
            cube = null;
        }
    }

    private PocketCell GetPocket(Vector2 touchPosition)
    {
        PocketCell pocket = new PocketCell();
        ActivateRaycast(touchPosition);
        if (hit.collider != null)
        {
            hit.collider.gameObject.TryGetComponent<PocketCell>(out pocket);
        }
        return pocket;
    }

    private void MoveTouchPhase(Vector2 touchPosition)
    {
        if (cube != null && cube.CanMove)
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(touchPosition);
            cube.SetPosition(position);
        }
    }

    private void EndTouchPhase(Vector2 touchPosition)
    {
        if (cube != null)
        {
            cube.ChangeMoveState(false);
            ActivateRaycast(touchPosition);
            if (TrySetInField())
            {
                return;
            }
            else if (TrySetInPocket())
            {
                return;
            }
            else
            {
                cube.SetPosition(startPosition);
                cube.ChangeMoveState(true);
            }
            cube = null;
        }
    }

    private bool TrySetInField()
    {
        if (hit.collider != null && hit.collider.gameObject.TryGetComponent<FieldCell>(out field) && fieldSpawner.SetCubeToField(field.X, field.Y, cube))
        {
            oldPocket.TakeCube();
            if (fieldSpawner.RemainCubesInField == 0)
            {
                winPanel.ActivateEndGame(fieldSpawner.CheckAnswer());
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool TrySetInPocket()
    {
        if (hit.collider != null && hit.collider.gameObject.TryGetComponent<PocketCell>(out pocket) &&
                 pocket.IsEmpty)
        {
            oldPocket.TakeCube();
            pocket.SetCube(cube);
            cube.SetPosition(pocket.transform.position.x, pocket.transform.position.y);
            cube.ChangeMoveState(true);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ActivateRaycast(Vector2 position)
    {
        ray = Camera.main.ScreenPointToRay(position);
        hit = Physics2D.Raycast(ray.origin, ray.direction);
    }
}
