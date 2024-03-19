using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dragSpeed = 1;

    private bool isDragging = false;
    private Vector3 pointOfClick;
    private Vector3 cameraOrigin;
    private Vector3 mapSize;
    private Bounds cameraBounds;
    private int buffer = 20;
    private float cameraHeight;
    private float cameraWidth;
 

    void Start()
    {
        mapSize = LogicManager.SharedInstance.mapSize;

        cameraHeight = Camera.main.orthographicSize;
        cameraWidth = cameraHeight * Camera.main.aspect;

        float minX = -mapSize.x / 2 + cameraWidth;
        float maxX = mapSize.x / 2 - cameraWidth;
        float minY = -mapSize.y / 2 + cameraHeight;
        float maxY = mapSize.y / 2- cameraHeight;

        cameraBounds = new Bounds();
        cameraBounds.SetMinMax(
            new Vector3(minX - buffer, minY - buffer, 0),
            new Vector3(maxX + buffer, maxY + buffer, 0)
        );
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(!isDragging)
            {
                pointOfClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                return;
            }
        }

        if(!Input.GetMouseButton(0))
        {
            isDragging = false;
            return;
        }

        Vector3 distanceMoved = Camera.main.ScreenToWorldPoint(Input.mousePosition) - pointOfClick;
        Vector3 move = new Vector3(distanceMoved.x * -1, distanceMoved.y * -1, 0) + Camera.main.transform.position;

        move = RestrictCameraToBounds(move);

        Camera.main.transform.position = move;

    }

    private Vector3 RestrictCameraToBounds(Vector3 pos)
    {
        return new Vector3(
            Mathf.Clamp(pos.x, cameraBounds.min.x, cameraBounds.max.x),
            Mathf.Clamp(pos.y, cameraBounds.min.y, cameraBounds.max.y),
            transform.position.z
        );
    }
}
        

