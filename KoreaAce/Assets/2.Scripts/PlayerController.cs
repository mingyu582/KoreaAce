using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform cam;

    public TMP_Text doorText;

    private void Update()
    {
        Ray ray = new Ray(transform.position, cam.forward);
        RaycastHit hit;

        Debug.Log("SDLFKJSDFKJ");
        if(Physics.Raycast(ray, out hit, 7f, 1 << 6))
        {
            Debug.Log("SLDKFJ");
            doorText.text = "¿­±â / ´Ý±â";
            if (Input.GetKeyDown(KeyCode.E))
            {
                hit.collider.GetComponent<Doors>().DoorOpen_Close();
            }
        }
        else
        {
            doorText.text = "";
        }

        Debug.DrawRay(transform.position + new Vector3(0, 3, 0), cam.forward * 7f, Color.blue, 0.3f);
    }
}
