using UnityEngine;

public class CameraHolder : MonoBehaviour
{
    [SerializeField] private Transform _cameraPos;

    private void Update()
    {
        this.transform.position = _cameraPos.position;
    }
}
