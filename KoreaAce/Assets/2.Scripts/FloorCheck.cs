using UnityEngine;

public class FloorCheck : MonoBehaviour
{
    public string floorTag;

    private void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1f))
        {
            if (hit.collider.gameObject.tag != "Floor")
            {
                return;
            }

            floorTag = hit.collider.gameObject.GetComponent<Floor>().FNum;
        }
    }
}
