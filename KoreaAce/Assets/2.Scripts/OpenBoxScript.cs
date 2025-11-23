using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OpenBoxScript : MonoBehaviour
{
    public Animator boxOB;
    public GameObject keyOBNeeded;
    public TMP_Text InteractText;

    public bool isOpen;


    public void OpenBox()
    {
        if (keyOBNeeded.activeInHierarchy == true) //키가 inventory에 활성화 되어있으면, E키 눌렀을때
        {
            keyOBNeeded.SetActive(false);
            boxOB.SetBool("isOpen", true);
            Debug.Log("SDL:FKJLS:DKFJ");
            isOpen = true;
            DisableBox();
        }
        else if (keyOBNeeded.activeInHierarchy == false)
        {
            InteractText.text = "";
        }
    }

    public void DisableBox()
    {
        boxOB.GetComponent<BoxCollider>().enabled = false;
        boxOB.GetComponent<OpenBoxScript>().enabled = false;
    }
}
