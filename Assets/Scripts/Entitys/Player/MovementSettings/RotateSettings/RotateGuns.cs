using UnityEngine;

public class RotateGuns : MonoBehaviour
{
    [SerializeField] private float offset;
    [SerializeField] private GameObject player;
    private float rotateCoefY;
    private Vector2 valuesCalcZ;

    private void Update()
    {
        if (PlayerPrefs.GetInt("IsActivePanels") == 0)
        {
            RealizeRotation();
            player.transform.rotation = Quaternion.Euler(0f, rotateCoefY, 0f);
        }
    }

    public void RealizeRotation()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float rotateZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        if (rotateZ >= 90 || rotateZ <= -90)
        {
            valuesCalcZ = new Vector2(-1, 180);
            rotateCoefY = 180;
        }
        else
        {
            valuesCalcZ = new Vector2(1, 0);
            rotateCoefY = 0;
        }

        float rotateCoefZ = valuesCalcZ.x * rotateZ + valuesCalcZ.y + offset;
        transform.rotation = Quaternion.Euler(0f, rotateCoefY, rotateCoefZ);
    }
}

