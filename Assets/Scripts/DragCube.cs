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

    private void ActivateRaycast(Vector2 position)
    {
        ray = Camera.main.ScreenPointToRay(position);
        hit = Physics2D.Raycast(ray.origin, ray.direction);
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    ActivateRaycast(touch.position);

                    if (hit.collider != null && hit.collider.gameObject.TryGetComponent<GameCube>(out cube))
                    {
                        startPosition = cube.transform.position;
                        if (!cube.CanMove)
                        {
                            cube = null;
                        }
                        else
                        {
                            cube.GetComponent<Collider2D>().enabled = false;
                            ActivateRaycast(touch.position);
                            if (hit.collider != null)
                            {
                                hit.collider.gameObject.TryGetComponent<PocketCell>(out oldPocket);
                            }
                        }
                    }
                    break;

                case TouchPhase.Moved:
                    if (cube != null && cube.CanMove)
                    {
                        Vector2 position = Camera.main.ScreenToWorldPoint(touch.position);
                        cube.SetPosition(position);
                        
                    }
                    break;

                case TouchPhase.Ended:

                    if (cube != null)
                    {
                        var cubeCollider = cube.gameObject.GetComponent<Collider2D>();
                        cubeCollider.enabled = false; // Update Cube state

                        ActivateRaycast(touch.position);

                        // Try put to field
                        if (hit.collider != null && hit.collider.gameObject.TryGetComponent<FieldCell>(out field) &&
                            fieldSpawner.SetCubeToField(field.X, field.Y, cube))
                        {
                            oldPocket.TakeCube();
                            if (fieldSpawner.RemainCubesInField == 0)
                            {
                                winPanel.ActivateEndGame(fieldSpawner.CheckAnswer());
                            }
                        }
                        //Try put to pocket
                        else if (hit.collider != null && hit.collider.gameObject.TryGetComponent<PocketCell>(out pocket) &&
                            pocket.IsEmpty)
                        {
                            oldPocket.TakeCube();
                            pocket.SetCube(cube);
                            cube.SetPosition(pocket.transform.position.x, pocket.transform.position.y);
                            cubeCollider.enabled = true; //Update Cube state
                            cube.ChangeMoveState(true);
                        }

                        else
                        {
                            cubeCollider.enabled = true; //Update Cube state
                            cube.SetPosition(startPosition);
                            cube.ChangeMoveState(true);
                        }
                        cube = null;
                    }
                    break;
            }
        }
    }

    private void UpdateCubeState(GameCube cube, CubeState state)
    {
        // По стэйт менять cube
    }
}
