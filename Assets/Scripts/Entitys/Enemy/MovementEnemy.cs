using UnityEngine;

public class MovementEnemy : MonoBehaviour
{
    [SerializeField] private GameObject trackerPlayer, bodyEnemy;
    [SerializeField] private float offset;
    private float rotateCoefY;
    private Vector2 valuesCalcZ;

    private void Update()
    {
        RotateEnemy();
        bodyEnemy.transform.rotation = Quaternion.Euler(0f, rotateCoefY, 0f);
    }

    private void RotateEnemy()
    {
        Vector3 difference = trackerPlayer.transform.position - transform.position;
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
