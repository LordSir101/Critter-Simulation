using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnowledgeGeneration : MonoBehaviour
{
    protected float knowledgeGainInterval = 8;
    protected float timeLastKnowledgeGained;
    protected int knowledgeGainRate = 20;
    protected int knowledgeGainRateIncrease = 2;
    protected int maxKnowledgeGainrate = 100;

    // Start is called before the first frame update
    void Start()
    {
        timeLastKnowledgeGained = Time.time;
    }

    // Update is called once per frame
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
        CritterKnowledgePoints.SharedInstance.AddKnowledgePoints(speciesNum, rate);

        if(rate < maxKnowledgeGainrate)
        {
            knowledgeGainRate += knowledgeGainRateIncrease;
        }

        timeLastKnowledgeGained = Time.time;
    }
}
