using System;
using Unity.VisualScripting;
using UnityEngine;

public class DestroyOnTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
