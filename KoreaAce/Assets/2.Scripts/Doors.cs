using UnityEngine;
using TMPro;

public class Doors : MonoBehaviour
{
    public bool isOpen;
    public Animator doorAnimator;

    private void Awake()
    {
        isOpen = false;
    }

    public void DoorOpen()
    {
        doorAnimator.SetBool("isOpen", true);
    }
}