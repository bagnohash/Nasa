using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(LineRenderer))]
public class LaserScript : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask layerMask;
    public float defaultLength = 50;
    public int numOfReflections = 10;

    private LineRenderer _lineRenderer;
    private Camera _myCam;
    private RaycastHit   hit;

    private Ray ray;
    private Vector3 direction;

    public static bool stopkurwa = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _myCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopkurwa) return;
        
        ReflectLaser();
    }

    void ReflectLaser()
    {
        ray = new Ray(transform.position, transform.forward);

        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, transform.position);

        float remainLength = defaultLength;

        for (int i = 0; i < numOfReflections; i++)
        {
            if (Physics.Raycast(ray.origin, ray.direction, out hit, remainLength, layerMask))
            {
                Debug.Log(hit.transform.gameObject.name);
                
                
                _lineRenderer.positionCount += 1;
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, hit.point);

                remainLength -= Vector3.Distance(ray.origin, hit.point);

                ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
                
                if (hit.transform.gameObject.layer == 15) break;
                if (hit.transform.gameObject.layer == 16)
                {
                    Debug.Log("dupa koniec");
                    var outline = hit.transform.gameObject;
                    StartCoroutine(SwitchScene(outline));
                    break;
                }
            }
            else
            {
                _lineRenderer.positionCount += 1;
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, ray.origin + (ray.direction * remainLength));
            }
        }

        void NormalLaser()
        {
            _lineRenderer.SetPosition(0, transform.position);

            // Does the ray intersect any objects
            if (Physics.Raycast(transform.position, transform.forward, out hit, defaultLength, layerMask))
            {
                _lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                _lineRenderer.SetPosition(1, transform.position + (transform.forward * defaultLength));
            }
        }

        IEnumerator SwitchScene(GameObject outlineGameObject)
        {
            var outline = outlineGameObject.GetComponent<Outline>();
            stopkurwa = true;
            int color = 255;
            for (int j = 0; j < 3; j++)
            {
                while (outline.OutlineColor.a > 0f)
                {
                    outline.OutlineColor = new Color(0, 1, 0, outline.OutlineColor.a - 0.01f);
                    yield return new WaitForSeconds(0.003f);
                }
                while (outline.OutlineColor.a < 1f)
                {
                    outline.OutlineColor = new Color(0, 1, 0, outline.OutlineColor.a + 0.01f);
                    yield return new WaitForSeconds(0.003f);
                }
            }
            
            yield return new WaitForSeconds(2f);

            SceneManager.LoadScene(1);
        }
    }
}
