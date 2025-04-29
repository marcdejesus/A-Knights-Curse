using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject swordSwingPrefab; // Prefab with an animation
    public float attackCooldown = 0.5f;
    private float lastAttackTime = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > lastAttackTime + attackCooldown)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 attackDirection = (mousePos - (Vector2)transform.position).normalized;

            GameObject swing = Instantiate(swordSwingPrefab, transform.position, Quaternion.identity);
            swing.transform.right = attackDirection; // Rotate toward click
            lastAttackTime = Time.time;
        }
    }
}
