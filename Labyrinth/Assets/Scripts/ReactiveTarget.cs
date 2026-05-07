using System;
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[RequireComponent(typeof(AudioSource))]
public class ReactiveTarget : MonoBehaviour
{

    [SerializeField] private float _defeatAnimTime = 1.5f;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip _enemyDefeat;


    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void ReactToHit()
    {
        Wandering nav = GetComponent<Wandering>();

        if (nav != null)
            nav.IsAlive = false;
        _audioSource.PlayOneShot(_enemyDefeat);
        
        StartCoroutine(DefeatAnim());
    }

    IEnumerator DefeatAnim()
    {
        float elapsedTime = 0.0f;

        Quaternion initRotation = transform.rotation;

        // Multiplying quaternions combines the effects of both
        Quaternion endRotation = transform.rotation * Quaternion.Euler(-75.0f, 0.0f, 0.0f);

        // Interpolation logic
        while (elapsedTime < _defeatAnimTime)
        {
            transform.rotation = Quaternion.Lerp(
                initRotation,
                endRotation,
                elapsedTime / _defeatAnimTime
            );
            
            elapsedTime += Time.deltaTime;

            yield return null; // Skip frame
        }
        
        // Reach destination
        transform.rotation = endRotation;

        yield return new WaitForSeconds(1.0f);
        
        Destroy(gameObject);
    }
    
    // void OnDestroy()
}

