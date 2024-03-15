using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float dragSpeed = 1;

    private bool isDragging = false;
    private Vector3 pointOfClick;
    private Vector3 cameraOrigin;
    //float buffer = 0.5f;
 
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
        Camera.main.transform.position = move;

        
    }
    // void LateUpdate()
    // {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     dragOrigin = Input.mousePosition;
        //     return;
        // }
 
        // if (!Input.GetMouseButton(0)) return;
 
        // Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        // Vector3 move = new Vector3(pos.x * dragSpeed * -1, pos.y * dragSpeed * -1, 0);
 
        // transform.Translate(move, Space.World);

        // void LateUpdate () {
        //     if (Input.GetMouseButton (0)) {
        //         Diference=(Camera.main.ScreenToWorldPoint (Input.mousePosition))- Camera.main.transform.position;
        //             if (Drag==false){
        //             Drag=true;
        //             Origin=Camera.main.ScreenToWorldPoint (Input.mousePosition);
        //         }
        //     } else {
        //         Drag=false;
        //     }
        //     if (Drag==true){
        //         Camera.main.transform.position = Origin-Diference;
        //     }
        // }


        // if(!isDragging) return;
        
       

        //     //Vector3 roundedToPixel = transform.position;

        //     // roundedToPixel.x = (int)(roundedToPixel.x * PixelsPerUnit) / PixelsPerUnit;
        //     // roundedToPixel.y = (int)(roundedToPixel.y * PixelsPerUnit) / PixelsPerUnit;
        //     // roundedToPixel.z = (int)(roundedToPixel.z * PixelsPerUnit) / PixelsPerUnit;
        
        //     // camera.transform.position = roundedToPixel;
        // }
        //Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition)- Camera.main.transform.position;
    }
        

