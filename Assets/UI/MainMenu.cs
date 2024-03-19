using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //public int size = 0;

    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI senseText;
    [SerializeField] private TextMeshProUGUI breedText;

    [SerializeField] private TextMeshProUGUI helpText;    

    [SerializeField] private CritterBuilder critterBuilder;

    [SerializeField] private GameObject critterTemplate;

    private GameObject preview;

    public void PlayGame()
    {
        if(MenuInput.speed + MenuInput.sense + MenuInput.breed >= 3)
        {
            SceneManager.LoadSceneAsync("Game");
        }
        else
        {
            helpText.text = "Add at least 3 parts to your critter to start";
        }
    }

    public void IncreaseSpeed()
    {
        MenuInput.speed = MenuInput.speed + MenuInput.sense + MenuInput.breed < 9 ? MenuInput.speed+1 : MenuInput.speed;
        speedText.text = MenuInput.speed.ToString();
        BuildPreview();
    }
    public void IncreaseSense()
    {
        MenuInput.sense = MenuInput.speed + MenuInput.sense + MenuInput.breed < 9 ? MenuInput.sense+1 : MenuInput.sense;
        senseText.text = MenuInput.sense.ToString();
        BuildPreview();
    }
    public void IncreaseBreed()
    {
        MenuInput.breed = MenuInput.speed + MenuInput.sense + MenuInput.breed < 9 ? MenuInput.breed+1 : MenuInput.breed;
        breedText.text = MenuInput.breed.ToString();
        BuildPreview();
    }
    public void DecreaseSpeed()
    {
        MenuInput.speed = MenuInput.speed == 0 ? 0 : MenuInput.speed -1;
        speedText.text = MenuInput.speed.ToString();
        BuildPreview();
    }
    public void DecreaseSense()
    {
        MenuInput.sense = MenuInput.sense == 0 ? 0 : MenuInput.sense -1;
        senseText.text = MenuInput.sense.ToString();
        BuildPreview();
    }
    public void DecreaseBreed()
    {
        MenuInput.breed = MenuInput.breed == 0 ? 0 : MenuInput.breed -1;
        breedText.text = MenuInput.breed.ToString();
        BuildPreview();
    }

    private void BuildPreview()
    {
        critterBuilder.CreateCritterIcon(MenuInput.speed, MenuInput.sense, MenuInput.breed, critterTemplate);
    }

}
