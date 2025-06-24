using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWorldInfo", menuName = "Scriptable Objects/PlayerWorldInfo")]
public class PlayerWorldInfo : ScriptableObject
{
    public Transform ProjectileInstantiationPoint;
    public Vector3 PlayerCamOrientation;

    public void UpdatePlayerTransform(Transform instPoint)
    {
        this.ProjectileInstantiationPoint = instPoint;
    }

    public void UpdatePlayerOrientation(Vector3 orientation)
    {
        this.PlayerCamOrientation = orientation;
    }
}
