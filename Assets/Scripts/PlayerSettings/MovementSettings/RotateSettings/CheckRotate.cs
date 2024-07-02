using UnityEngine;

public class CheckRotate : MonoBehaviour
{
    void Update()
    {
        RotationSettings.RealizeRotation(0f, 0f, new Vector2 (1, 0), this.gameObject);
    }
}
