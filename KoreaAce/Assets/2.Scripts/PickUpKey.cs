using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpKey : MonoBehaviour
{
    public GameObject keyOB;
    public GameObject invOB;

    
    public void GetKey()
    {
        keyOB.SetActive(false);
        invOB.SetActive(true);
    }
}
