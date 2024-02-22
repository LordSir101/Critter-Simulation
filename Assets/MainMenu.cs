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
    public int size = 0;

    public TextMeshProUGUI speedText;
    public TextMeshProUGUI senseText;
    public TextMeshProUGUI breedText;


    public critterBuilder critterBuilder;

    public GameObject critterTemplate;

    public GameObject preview;

    
    public void Start()
    {
        // Instatiate a critter so the player can see a preview of what it will look like
        preview = Instantiate(critterTemplate, new Vector3(transform.position.x+5, transform.position.y + 1, 0), transform.rotation);
    }

    public void PlayGame()
    {
        //TODO: don't let player start unless size >= 3
        SceneManager.LoadSceneAsync("Game");
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
        critterBuilder.CreateCritter(MenuInput.speed, MenuInput.sense, MenuInput.breed, preview);
    }

}
