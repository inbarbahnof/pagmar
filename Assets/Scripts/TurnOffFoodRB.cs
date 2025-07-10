using System;
using System.Collections;
using UnityEngine;

public class TurnOffFoodRB : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            StartCoroutine(WaitToTurnOffRB(other.gameObject.GetComponent<Rigidbody2D>()));
        }
    }

    private IEnumerator WaitToTurnOffRB(Rigidbody2D rb)
    {
        yield return new WaitForSeconds(0.5f);
        rb.bodyType = RigidbodyType2D.Kinematic;
    }
}
