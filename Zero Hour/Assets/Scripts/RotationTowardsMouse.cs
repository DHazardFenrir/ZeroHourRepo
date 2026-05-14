using UnityEngine;

public class RotationTowardsMouse : MonoBehaviour
{
    [SerializeField] PlayerMovement pMove;
    [SerializeField] private SpriteRenderer sr;

    void Awake()
    {
        pMove = GetComponentInParent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 1. Calculamos la dirección RELATIVA al arma
        Vector3 localDirection = transform.parent.InverseTransformPoint(mousePosition) - transform.localPosition;

        // 2. Calculamos el ángulo basándonos en esa dirección local
        float angle = Mathf.Atan2(localDirection.y, localDirection.x) * Mathf.Rad2Deg;

        // 3. El moveAngle siempre será 0 porque estamos hablando en "idioma local" del arma
        // Si el padre mira a la izquierda, para el arma (localmente) eso sigue siendo "adelante"
        float moveAngle = 0f;

        float delta = Mathf.DeltaAngle(moveAngle, angle);
        delta = Mathf.Clamp(delta, -90f, 90f);
        angle = moveAngle + delta;

        // 4. Aplicamos la rotación
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}