using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;


public class ReadNotes : MonoBehaviour
{
    public GameObject player;
    public GameObject noteUI;
    public GameObject hud;




    void Start()
    {
        noteUI.SetActive(false);
        hud.SetActive(true);
    }

    public void ReadNote()
    {
        noteUI.SetActive(true);
        hud.SetActive(false);
        player.GetComponent<FirstPersonController>().enabled = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ExitButton()
    {
        noteUI.SetActive(false);
        hud.SetActive(true);
        player.GetComponent<FirstPersonController>().enabled = true;

    }
}
