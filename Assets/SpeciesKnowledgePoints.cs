using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeciesKnowledgePoints : MonoBehaviour
{
    //public static SpeciesKnowledgePoints SharedInstance;

    public Dictionary<int, int> speciesKnowledge = new Dictionary<int, int>(){
            {0, 0}, 
            {1, 0}, 
            {2, 0}, 
            {3, 0} 
        };

    public void AddKnowledgePoints(int speciesNum, int knowledgeGained)
    {
        if(!speciesKnowledge.ContainsKey(speciesNum))
        {
            speciesKnowledge.Add(speciesNum,0);
        }

        speciesKnowledge[speciesNum] += knowledgeGained;
    }

    public void UseKnowledgePoints(int speciesNum, int amountToUse)
    {
        if(speciesKnowledge.ContainsKey(speciesNum))
        {
            speciesKnowledge[speciesNum] -= amountToUse;
        }
    }

    public int GetKnowledgeOfSpecies(int SpeciesNum)
    {
        return speciesKnowledge[SpeciesNum];
    }

    public void RemoveSpecies(int speciesNum)
    {
        speciesKnowledge.Remove(speciesNum);
    }
    


}
