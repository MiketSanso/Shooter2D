using UnityEngine;

public class RotateGuns : MonoBehaviour
{
    [SerializeField] private float offset;
    [SerializeField] private GameObject player, checkerRotate;
    private float rotateCoefY;
    private Vector2 valuesCalcZ;

    private void Update()
    {
        if (PlayerPrefs.GetInt("IsActivePanels") == 0)
        {
            if (checkerRotate.transform.localRotation.z >= 0.7f || checkerRotate.transform.localRotation.z <= -0.7f)
            {
                valuesCalcZ = new Vector2(-1, 180);
                rotateCoefY = 180;
            }
            else
            {
                valuesCalcZ = new Vector2(1, 0);
                rotateCoefY = 0;
            }
            RotationSettings.RealizeRotation(rotateCoefY, offset, valuesCalcZ, this.gameObject);
            player.transform.rotation = Quaternion.Euler(0f, rotateCoefY, 0f);
        }
    }
    #warning Добавь сюда нормальную переменную
}

public static class RotationSettings
{
    public static void RealizeRotation(float rotateCoefY, float offset, Vector2 valuesCalcZ, GameObject objectRotation)
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - objectRotation.transform.position;
        float rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        float rotateCoefZ = valuesCalcZ.x * rotateZ + valuesCalcZ.y + offset;
        objectRotation.transform.rotation = Quaternion.Euler(0f, rotateCoefY, rotateCoefZ);
    }
}
