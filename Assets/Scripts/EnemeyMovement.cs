using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemeyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    Rigidbody2D enemyBody;
    void Start()
    {
        enemyBody = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        enemyBody.velocity = new Vector2(moveSpeed,0);
    }

    private void OnTriggerExit2D(Collider2D other) {
        moveSpeed = -moveSpeed;
        flipEnemyFacing();
    }

    void flipEnemyFacing()
    {
        transform.localScale = new Vector2 (-(Mathf.Sign(enemyBody.velocity.x)), 1f);
    }

}
