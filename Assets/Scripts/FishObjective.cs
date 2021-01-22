using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishObjective : MonoBehaviour
{
    [SerializeField] private List<GameObject> tokens;
    
    public void EarnToken(int index)
    {
        tokens[index].SetActive(true);
    }
}
