using UnityEngine;

public class RotationTowardsMouse : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
       

   Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
   Vector3 localDirection = transform.parent.InverseTransformPoint(mousePosition) - transform.localPosition;
   float angle = Mathf.Atan2(localDirection.y, localDirection.x) * Mathf.Rad2Deg;
   float moveAngle = 0f;
   float delta = Mathf.DeltaAngle(moveAngle, angle);
   delta = Mathf.Clamp(delta, -90f, 90f);
   angle = moveAngle + delta;


transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}