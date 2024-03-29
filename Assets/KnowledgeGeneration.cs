using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeGeneration : MonoBehaviour
{
    protected float knowledgeGainInterval = 8;
    protected float timeLastKnowledgeGained;
    protected int knowledgeGainRate = 20; // knowledge per tick
    protected int knowledgeGainRateIncrease = 2; // increase rate of knowledge per tick
    protected int maxKnowledgeGainrate = 100;

    private SpeciesKnowledgePoints speciesKnowledgePoints;

    void Start()
    {
        timeLastKnowledgeGained = Time.time;
        speciesKnowledgePoints = CritterManager.SharedInstance.GetComponent<SpeciesKnowledgePoints>();
    }

    void Update()
    {
        if(Time.time - timeLastKnowledgeGained >= knowledgeGainInterval)
        {
            GenerateKnowledge(knowledgeGainRate);
        }
    }

    void GenerateKnowledge(int rate)
    {
        int speciesNum = gameObject.GetComponent<Critter>().speciesNum;
        speciesKnowledgePoints.AddKnowledgePoints(speciesNum, rate);

        if(rate < maxKnowledgeGainrate)
        {
            knowledgeGainRate += knowledgeGainRateIncrease;
        }

        timeLastKnowledgeGained = Time.time;
    }
}
