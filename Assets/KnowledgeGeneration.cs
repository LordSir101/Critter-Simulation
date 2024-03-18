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

    void Start()
    {
        timeLastKnowledgeGained = Time.time;
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
        SpeciesKnowledgePoints.SharedInstance.AddKnowledgePoints(speciesNum, rate);

        if(rate < maxKnowledgeGainrate)
        {
            knowledgeGainRate += knowledgeGainRateIncrease;
        }

        timeLastKnowledgeGained = Time.time;
    }
}
