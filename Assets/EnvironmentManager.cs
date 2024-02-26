using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class EnvironmentManager : MonoBehaviour
{
    public Vector3 mapSize;
    private Button visionToggle;
    public bool showLines = false;
    // Start is called before the first frame update
    void Start()
    {
        mapSize = GameObject.FindGameObjectWithTag("PlayableArea").GetComponent<Tilemap>().size;

        visionToggle = GameObject.FindGameObjectWithTag("VisionToggle").GetComponent<Button>();
        visionToggle.onClick.AddListener(ToggleShowLines);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ToggleShowLines()
    {
        showLines = !showLines;
    }
}
