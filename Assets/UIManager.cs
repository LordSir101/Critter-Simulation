using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public CritterManager critterManager;
    public EnvironmentManager environmentManager;
    public GameObject speciesCountUI;
    public TextMeshProUGUI dayDisplay;
    private Transform[] countElements = new Transform [10];

    // Start is called before the first frame update
    void Start()
    {
        int index = 0;
        foreach (Transform element in speciesCountUI.transform)
        {
            countElements[index] = element;
            index++;
        }
        speciesCountUI.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // update species counter
         if(speciesCountUI.GetComponent<VerticalLayoutGroup>().enabled)
        {
            int elementNum = 1;

            // species num and the amount of cirtters that are alive
            foreach(KeyValuePair<int, int> entry in critterManager.speciesCount)
            {
                // we reverse through count elements so that in the UI, the elements expand from bottom to top
                Image background = countElements[countElements.Length - elementNum].GetComponent<Image>();
                TextMeshProUGUI text = background.gameObject.GetComponentInChildren<TextMeshProUGUI>();

                // Show species color and how many of that species is alive
                int key = entry.Key;
                text.text = entry.Value.ToString();
                Color color = critterManager.colors[key];
                color.a= 0.6f;
                background.color = color;

                elementNum++;
            }

            // hide unused elements
            for(int i = 0; i < countElements.Length - critterManager.speciesCount.Count; i++)
            {
                Image background = countElements[i].GetComponent<Image>();
                TextMeshProUGUI text = background.gameObject.GetComponentInChildren<TextMeshProUGUI>();
                background.enabled = false;
                text.enabled = false;
            }
        }

        dayDisplay.text = "Day: " + environmentManager.day;
        
    }

    public void ToggleSpeciesCount()
    {
        speciesCountUI.SetActive(!speciesCountUI.activeInHierarchy);
    }
}
