using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class InformationDisplay : MonoBehaviour
{
    protected LineRenderer lineRenderer;
    protected Button visionToggle;

    public Color color = new Color32(0, 0, 0, 255 );
    // Start is called before the first frame update
    void Start()
    {
        setup();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(lineRenderer.enabled){
            DrawVision();
        }
        
    }

    public void ToggleLineRenderer()
    {
        lineRenderer.enabled = !lineRenderer.enabled;
    }

    protected void DrawVision()
    {
        
        int sense = gameObject.GetComponent<Critter>().sense;
        int baseSense = gameObject.GetComponent<Critter>().baseSense;
        float senseScale = gameObject.GetComponent<Critter>().senseScale;

        // draw a circle based on radius and subdivision
        // the line renderer is attatched to the critter and the circle will automatically move with it
        int subdivisions = 15;
        float radius = (sense+3)*senseScale;

        float angleStop = 2f * Mathf.PI / subdivisions;
        lineRenderer.positionCount = subdivisions;

        for(int i = 0; i < subdivisions; i++)
        {
            float x = radius * Mathf.Cos(angleStop*i);
            float y = radius * Mathf.Sin(angleStop * i);

            Vector3 pointInCircle = new Vector3(gameObject.transform.position.x + x,gameObject.transform.position.y + y,0);

            lineRenderer.SetPosition(i,pointInCircle);
        }
    }

    protected void setup()
    {
        
        gameObject.GetComponent<Light2D>().color = color;
        
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(color, 0.0f), new GradientColorKey(color, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
        );
        lineRenderer.colorGradient = gradient;
        lineRenderer.enabled = EnvironmentManager.SharedInstance.showLines;
        //critterManager = GameObject.FindGameObjectWithTag("CritterManager");

        visionToggle = GameObject.FindGameObjectWithTag("VisionToggle").GetComponent<Button>();
        visionToggle.onClick.AddListener(ToggleLineRenderer);

        // toggle the initial state of line renderer to avoid desync when critters spawn
        //EnvironmentManager manager = GameObject.FindGameObjectWithTag("EnvironmentManager").GetComponent<EnvironmentManager>();
        
    }
}
