using System.Collections;
using UnityEngine;

public class RayShooter : MonoBehaviour
{
    private Camera _cam;
    private AudioSource _audioSource;
    [SerializeField] AudioClip _playerShoot;
    
    void Start()
    {
        _cam = GetComponent<Camera>();
        _audioSource = GetComponent<AudioSource>();

        Cursor.lockState = CursorLockMode.Locked;
        //locked locks the cursor to the very center
        //this should go in the game manager so that it can be contrianed or none during pause
        Cursor.visible = false;
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _audioSource.PlayOneShot(_playerShoot);
            
            //calculate the middle of the screen
            Vector3 point = new(_cam.pixelWidth * 0.5f, _cam.pixelHeight * 0.5f, 0.0f);
            
            // Turn that coordinate into an actual ray
            Ray ray = _cam.ScreenPointToRay(point);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                GameObject hitObj = hit.transform.gameObject;
                ReactiveTarget target = hitObj.GetComponent<ReactiveTarget>();

                if (target != null)
                    target.ReactToHit();
                else
                    StartCoroutine(SphereIndicator(hit.point));
            }
                
        }
        
    }
    
    IEnumerator SphereIndicator(Vector3 pos)
    {
        // Create and place sphere
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Renderer sphereRenderer = sphere.GetComponent<Renderer>();
        sphereRenderer.material.color = Color.darkGreen;
        
        sphere.transform.position = pos;
        sphere.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        
        // Destroy after a second
        yield return new WaitForSeconds(1.0f);
        Destroy(sphere);
    }
}