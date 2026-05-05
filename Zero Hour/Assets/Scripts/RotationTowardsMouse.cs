using UnityEngine;
using UnityEngine.Rendering;

public class RotationTowardsMouse : MonoBehaviour
{
    [SerializeField] PlayerMovement pMove;
    [SerializeField] private SpriteRenderer sr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        pMove = GetComponentInParent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    Vector2 direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    

    Vector2 movement = pMove.MoveDirection;
     float moveAngle = 0f;
     if(movement.x > 0 ) moveAngle = 0f;
     else if(movement.x < 0 ) moveAngle = 180f;
     else if (movement.y > 0) moveAngle = 90f;
     else if (movement.y < 0) moveAngle = 270f;

     float delta = Mathf.DeltaAngle(moveAngle, angle);
     delta = Mathf.Clamp(delta, -90f, 90f);
     angle = moveAngle + delta;
     transform.rotation = Quaternion.Euler(0,0, angle);
    }
}
