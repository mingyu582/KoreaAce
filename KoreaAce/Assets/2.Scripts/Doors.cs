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

    public void DoorOpen_Close()
    {
        isOpen = !isOpen;
        doorAnimator.SetBool("isOpen", isOpen);
    }
}