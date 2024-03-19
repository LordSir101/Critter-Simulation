using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public CritterManager critterManager;
    [SerializeField] CritterBuilder critterBuilder;
    //public LogicManager logicManager;

    public GameObject speciesCountUI;
    
    [SerializeField] private TextMeshProUGUI dayDisplay;
    [SerializeField] private TextMeshProUGUI knowledgeDisplay;

    [SerializeField] private GameObject preview; // prefab of Images in the shape of critters
    [SerializeField] private SpeciesKnowledgePoints speciesKnowledgePoints;

    public int day;
    [SerializeField] private Transform[] countElements = new Transform [10];

    private List<GameObject> previewPool = new List<GameObject>();
    public int amountToPool = 10;

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

        GameObject tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(preview);
            tmp.SetActive(false);
            previewPool.Add(tmp);
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(PauseControl.gameIsPaused){return;}
        UpdateSpeciesCountUI();
        DisplayInfo();
        
    }

    public void ToggleSpeciesCount()
    {
        speciesCountUI.SetActive(!speciesCountUI.activeInHierarchy);
        if(speciesCountUI.GetComponent<VerticalLayoutGroup>().enabled)
        {
            BuildCritterCountUI();
        }
    }

    public void SetDay(int newDay)
    {
        day = newDay;
    }

    public void DisplayInfo()
    {
        dayDisplay.text = "Day: " + day;
        knowledgeDisplay.text = speciesKnowledgePoints.GetKnowledgeOfSpecies(PlayerGameInfo.currSpeciesNum).ToString();
    }

     public GameObject GetPooledPreview()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if(!previewPool[i].activeInHierarchy)
            {
                return previewPool[i];
            }
        }
        return null;
    }

    public void BuildCritterCountUI()
    {
        if(speciesCountUI.GetComponent<VerticalLayoutGroup>().enabled)
        {
            int elementNum = 1;
            foreach(KeyValuePair<int, int> entry in CritterManager.SharedInstance.speciesCount)
            {
                // loop backward so that the first species appears at bottom of UI
                GameObject box = countElements[countElements.Length - elementNum].gameObject;
                Image background = box.GetComponent<Image>();

                Color color = critterManager.colors[entry.Key];
                color.a= 0.6f;
                background.color = color;

                (int speed, int sense, int breed) = CritterManager.SharedInstance.GetPooledCritterStats(entry.Key);
                GameObject icon = box.transform.GetChild(1).gameObject;
                critterBuilder.CreateCritterIcon(speed, sense, breed, icon);

                box.SetActive(true);

                elementNum++;

            }

            // Hide the unused boxes
            for(int i = 0; i < countElements.Length - elementNum + 1; i++)
            {
                countElements[i].gameObject.SetActive(false);
            }
        }

    }

    public void UpdateSpeciesCountUI()
    {
        // update species counter
        if(speciesCountUI.GetComponent<VerticalLayoutGroup>().enabled)
        {
            int elementNum = 1;

            foreach(KeyValuePair<int, int> entry in CritterManager.SharedInstance.speciesCount)
            {
                TextMeshProUGUI text = countElements[countElements.Length - elementNum].GetComponentInChildren<TextMeshProUGUI>();

                int key = entry.Key;
                text.text = entry.Value.ToString();
                text.enabled = true;

                elementNum++;
            }
        }
    }
}
