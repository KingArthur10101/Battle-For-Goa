using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
public class clickLogger : MonoBehaviour
{
    public bool clicksAllowed;
    private GameObject selectedGO;
    public GameObject debugDot;
    [SerializeField] private AudioClip cPing;
    [SerializeField] private AudioClip aPing;
    [SerializeField] private float zoomSpeed = 1f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 8f;
    [SerializeField] private float panSpeed = 10f;
    private bool isDragging = false;
    private Vector3 lastMousePosition;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (mainCamera == null) return;

        float scrollDelta = Input.mouseScrollDelta.y;
        if (scrollDelta != 0)
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 beforeZoom = mainCamera.ScreenToWorldPoint(mousePos);
            
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - (scrollDelta * zoomSpeed), minZoom, maxZoom);
            
            Vector3 afterZoom = mainCamera.ScreenToWorldPoint(mousePos);
            mainCamera.transform.position -= afterZoom - beforeZoom;
        }

        if (Input.GetMouseButtonDown(1))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 translate = new Vector3(-delta.x * panSpeed * Time.deltaTime, -delta.y * panSpeed * Time.deltaTime, 0);
            translate = translate * (mainCamera.orthographicSize / 5f); // Scale pan speed with zoom level
            mainCamera.transform.Translate(translate);
            lastMousePosition = Input.mousePosition;
        }

        if (EventSystem.current != null)
        {
            if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject())
                return;
        }

        if (clicksAllowed && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Camera cam = Camera.main;
            if (cam == null)
                return;

            Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;

            Ray ray = cam.ScreenPointToRay(mousePos);
            RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);

            if (hit2D.collider != null)
            {
                GameObject go = hit2D.collider.gameObject;
                if (selectedGO && selectedGO.CompareTag("Player") && go.CompareTag("Enemy"))
                {
                    selectedGO.GetComponent<moveScript>().setTarget(go);
                    GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(aPing);
                }
                else
                {
                    GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(cPing);
                }
                if (go == selectedGO)
                {
                    ClearselectedGO();
                }
                else
                {
                    ClearselectedGO();
                    selectedGO = go;
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<canvasScript>().go = selectedGO;                    
                }
            }
            else{
                if (selectedGO && selectedGO.CompareTag("Player")) { 
                    selectedGO.GetComponent<moveScript>().setTarget(worldPos);
                    GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(aPing);
                }
                ClearselectedGO();
            }

        }
    }

    private void ClearselectedGO()
    {
        if (selectedGO != null)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<canvasScript>().go = null;
            selectedGO = null;
        }
    }
}
