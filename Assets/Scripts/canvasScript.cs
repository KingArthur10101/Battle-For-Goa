using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class canvasScript : MonoBehaviour
{
    [SerializeField] private AudioClip ping1;
    [SerializeField] private AudioClip ping2;
    [SerializeField] private AudioClip sPing;
    [SerializeField] private AudioClip cPing;
    [SerializeField] private AudioClip back;
    public GameObject baseB;
    private GameObject unitTitle;
    private GameObject buildingTitle;
    private GameObject enemyTitle;
    private GameObject hud;
    private GameObject extraHud;

    private Camera mainCamera;
    public GameObject minimap;
    public GameObject minimapButtonUP;
    public GameObject go;


    public bool clicksAllowed;
    private GameObject selectedGO;
    public GameObject debugDot;
    [SerializeField] private float zoomSpeed;
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] private float panSpeed;
    [SerializeField] private float arrowsPanSpeed;
    private float inpX;
    private float inpY;
    private bool isDragging = false;
    private Vector3 lastMousePosition;

    void Start()
    {
        baseB = GameObject.FindGameObjectWithTag("Respawn");
        mainCamera = Camera.main;

    }

    void Update()
    {
        if (go)
        {
            updateUnitHUD(go);
        }
        else
        {
            unitTitle.SetActive(false);
            enemyTitle.SetActive(false);
            buildingTitle.SetActive(false);
        }
        if (baseB && baseB.GetComponent<baseScript>().alive)
        {
            updateMainHUD();
        }
        if (baseB && baseB.GetComponent<baseScript>().units.Count() == 0)
        {
            unitTitle.SetActive(false);
        }




        // CLICK LOGGER
        
        inpX = Input.GetAxisRaw("Horizontal");
        inpY = Input.GetAxisRaw("Vertical");

        mainCamera.GetComponent<BoxCollider2D>().size = new Vector2(mainCamera.orthographicSize * 2f * mainCamera.aspect, mainCamera.orthographicSize * 2f);
        mainCamera.transform.position += Time.deltaTime * arrowsPanSpeed * new Vector3(inpX, inpY, 0f);

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

        if (clicksAllowed && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;

            Vector3 worldPos = mainCamera.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;

            Ray ray = mainCamera.ScreenPointToRay(mousePos);
            RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);

            if (hit2D.collider != null)
            {
                GameObject go = hit2D.collider.gameObject;
                if (selectedGO && selectedGO.CompareTag("Player") && go.CompareTag("Enemy"))
                {
                    selectedGO.GetComponent<moveScript>().setTarget(go);
                    GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(ping2);
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
                    GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(ping2);
                }
                ClearselectedGO();
            }

        }
    }

    public void openDetails(int type)
    {
        switch (type)
        {
            case 0:
                if (unitTitle.transform.GetChild(1).gameObject.activeSelf)
                {
                    unitTitle.transform.GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    unitTitle.transform.GetChild(1).gameObject.SetActive(true);
                }
            break;

            case 1:
                if (buildingTitle.transform.GetChild(1).gameObject.activeSelf)
                {
                    buildingTitle.transform.GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    buildingTitle.transform.GetChild(1).gameObject.SetActive(true);
                }
            break;

            case 2:
                if (enemyTitle.transform.GetChild(1).gameObject.activeSelf)
                {
                    enemyTitle.transform.GetChild(1).gameObject.SetActive(false);
                }
                else
                {
                    enemyTitle.transform.GetChild(1).gameObject.SetActive(true);
                }
            break;
        }
        GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(sPing);
    }

    public void updateMainHUD()
    {
        if (GameObject.FindGameObjectWithTag("pause").GetComponent<pauseScript>().pause)
        {
            hud.transform.GetChild(3).GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>().text = "unpause";
        }
        else
        {
            hud.transform.GetChild(3).GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>().text = "pause";
        }
        var mask = hud.transform.GetChild(1).GetChild(0).GetComponent<RectMask2D>();
        Vector4 p = mask.padding;
        p.z = Mathf.Lerp(0f, 384f, 1f - (float)baseB.GetComponent<baseScript>().health / baseB.GetComponent<baseScript>().maxHealth);
        mask.padding = p;
        hud.transform.GetChild(2).GetComponent<Text>().text = $"lvl: {baseB.GetComponent<baseScript>().level}";
        hud.transform.GetChild(0).GetComponent<Text>().text = $"$ {baseB.GetComponent<baseScript>().money}";
        hud.transform.GetChild(3).GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = $"{baseB.GetComponent<baseScript>().units.Count} / {baseB.GetComponent<baseScript>().maxUnits}";
        hud.transform.GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>().text = $"{baseB.GetComponent<baseScript>().health} / {baseB.GetComponent<baseScript>().maxHealth}";
        hud.transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<Text>().text = $"${baseB.GetComponent<baseScript>().perSecondCash} / sec";
        if (baseB.GetComponent<baseScript>().constructing)
        {
            hud.transform.GetChild(3).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = $"{baseB.GetComponent<baseScript>().constructing.name}";
        }
        else
        {
            hud.transform.GetChild(3).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = "nothing";
        }

    }
    public void moveMinimap(int upDown)
    {
        if (upDown == 1)
        {
            minimap.SetActive(true);
            minimapButtonUP.SetActive(false);
        }
        else
        {
            minimap.SetActive(false);
            minimapButtonUP.SetActive(true);
        }
        GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(sPing);
    }

    public void hideExtraHud()
    {
        if (extraHud.activeSelf)
        {
            extraHud.SetActive(false);
        }
        else
        {
            extraHud.SetActive(true);
        }
        GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(sPing);
    }

    public void setConstructing(int unit_)
    {
        if (baseB.GetComponent<baseScript>().units.Count < baseB.GetComponent<baseScript>().maxUnits && baseB.GetComponent<baseScript>().money > baseB.GetComponent<baseScript>().unitsToBuild[unit_].GetComponent<moveScript>().costToBuild)
        {
            baseB.GetComponent<baseScript>().constructing = baseB.GetComponent<baseScript>().unitsToBuild[unit_];
            GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(ping2);
            hideBuildMenu();
        }
        else
        {
            GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(back);
        }
    }

    public void showBuildMenu()
    {
        buildingTitle.transform.GetChild(1).GetChild(1).GetChild(1).gameObject.SetActive(true);
        GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(ping1);
    }

    public void hideBuildMenu()
    {
        buildingTitle.transform.GetChild(1).GetChild(1).GetChild(1).gameObject.SetActive(false);
    }
    public void toggleBuildMenu()
    {
        if (buildingTitle.transform.GetChild(1).GetChild(1).GetChild(1).gameObject.activeSelf){
            GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(back);
            hideBuildMenu();
        }
        else
        {
            showBuildMenu();
        }
    }
    public void updateUnitHUD(GameObject selectedGO)
    {
        switch (selectedGO.gameObject.tag)
        {
            case "Player":
                unitTitle.SetActive(true);
                buildingTitle.SetActive(false);
                enemyTitle.SetActive(false);
                unitTitle.transform.GetChild(0).GetComponent<Text>().text = selectedGO.name;
                unitTitle.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = $"hp: {selectedGO.GetComponent<moveScript>().health} / {selectedGO.GetComponent<moveScript>().maxHealth}";
                var mask = unitTitle.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<RectMask2D>();
                Vector4 p = mask.padding;
                p.z = Mathf.Lerp(0f, 384f, 1f - (float)selectedGO.GetComponent<moveScript>().health / selectedGO.GetComponent<moveScript>().maxHealth);
                mask.padding = p;
                if (selectedGO.GetComponent<moveScript>().goal)
                {
                    if (!selectedGO.GetComponent<moveScript>().goal.gameObject.CompareTag("targPrefab"))
                    {
                        unitTitle.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text = $"target:\n{selectedGO.GetComponent<moveScript>().goal.gameObject.name}";
                    }
                    else
                    {
                        unitTitle.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text = "target:\nexplore";
                    }
                }
                else
                {
                    unitTitle.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text = "target:\nnone";
                }
                unitTitle.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetComponent<Text>().text = $"lvl: {selectedGO.GetComponent<moveScript>().lvl}\nex: {selectedGO.GetComponent<moveScript>().exp}/{selectedGO.GetComponent<moveScript>().nextExp}";
                break;
            case "Respawn":
                unitTitle.SetActive(false);
                buildingTitle.SetActive(true);
                enemyTitle.SetActive(false);
                buildingTitle.transform.GetChild(0).GetComponent<Text>().text = selectedGO.name;
                buildingTitle.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = $"hp: {selectedGO.GetComponent<baseScript>().health} / {selectedGO.GetComponent<baseScript>().maxHealth}";
                var mask2 = buildingTitle.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<RectMask2D>();
                Vector4 p2 = mask2.padding;
                p2.z = Mathf.Lerp(0f, 384f, 1f - (float)selectedGO.GetComponent<baseScript>().health / selectedGO.GetComponent<baseScript>().maxHealth);
                mask2.padding = p2;
                if (baseB.GetComponent<baseScript>().constructing)
                {
                    buildingTitle.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text = $"building:\n{baseB.GetComponent<baseScript>().constructing.name}";
                }
                else
                {
                    buildingTitle.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text = "building:\nnothing";
                }
                buildingTitle.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetComponent<Text>().text = $"lvl: {selectedGO.GetComponent<baseScript>().level}\nex: {selectedGO.GetComponent<baseScript>().exp} / {selectedGO.GetComponent<baseScript>().nextExp}";
                break;
            case "Enemy":
                unitTitle.SetActive(false);
                buildingTitle.SetActive(false);
                enemyTitle.SetActive(true);
                enemyTitle.transform.GetChild(0).GetComponent<Text>().text = selectedGO.name;
                var mask3 = enemyTitle.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<RectMask2D>();
                Vector4 p3 = mask3.padding;
                if (selectedGO.GetComponent<enemyScript>())
                {
                    p3.z = Mathf.Lerp(0f, 384f, 1f - (float)selectedGO.GetComponent<enemyScript>().health / selectedGO.GetComponent<enemyScript>().maxHealth);
                }
                else
                {
                    p3.z = Mathf.Lerp(0f, 384f, 1f - (float)selectedGO.GetComponent<spawnerScript>().health / selectedGO.GetComponent<spawnerScript>().maxHealth);                    
                }
                mask3.padding = p3;
                break;
        }
    }
    public void unitReturn()
    {
        go.GetComponent<moveScript>().setTarget(baseB);
    }
    private void ClearselectedGO()
    {
        if (selectedGO != null)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<canvasScript>().go = null;
            selectedGO = null;
        }
    }

    public void setSettings()
    {
        
    }

}
