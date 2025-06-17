using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    [SerializeField] private Transform orientation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float yRot = this.transform.eulerAngles.y;
        orientation.rotation = Quaternion.Euler(0f, yRot, 0f);
    }
}
