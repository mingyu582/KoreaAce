using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cam;

    public TMP_Text InteractText;

    private void Update()
    {
        OpenDoor();
    }

    public void OpenDoor()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, 1.7f, 0), cam.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 7f, 1 << 6)) //layermask 가 6일때 (Door일때)
        {
            InteractText.text = "열기 / 닫기 (E)";
            if (Input.GetKeyDown(KeyCode.E))
            {
                hit.collider.GetComponent<Doors>().DoorOpen_Close();
            }
        }
        else if (Physics.Raycast(ray, out hit, 7f, 1 << 7)) //layermask 가 7일때 (Key일때)
        {
            Debug.Log("SDFLKJ");
            InteractText.text = "열쇠 획득 (E)";
            if (Input.GetKeyDown(KeyCode.E))
            {
                hit.collider.GetComponent<PickUpKey>().GetKey();
            }
        }
        else if (Physics.Raycast(ray, out hit, 7f, 1 << 8)) //layermask 가 8일때 (Box일때)
        {
            Debug.Log("SDFLKJ");
            InteractText.text = "박스 열기 (E)";
            if (Input.GetKeyDown(KeyCode.E))
            {
                hit.collider.GetComponent<OpenBoxScript>().OpenBox();
            }
        }
        else if (Physics.Raycast(ray, out hit, 7f, 1 << 9)) //layermask 가 8일때 (Box일때)
        {
            Debug.Log("SDFLKJ");
            InteractText.text = "노트 보기 (E)";
            if (Input.GetKeyDown(KeyCode.E))
            {
                hit.collider.GetComponent<ReadNotes>().ReadNote();
            }
        }
        else
        {
            InteractText.text = "";
        }

        Debug.DrawRay(transform.position + new Vector3(0, 1.7f, 0), cam.forward * 7f, Color.blue, 0.3f);
    }
}
