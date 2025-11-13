using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class canvasScript : MonoBehaviour
{
    public List<GameObject> units = new List<GameObject>();
    [SerializeField] private AudioClip ping1;
    [SerializeField] private AudioClip ping2;
    [SerializeField] private AudioClip back;
    public GameObject baseB;
    public GameObject unitTitle;
    public GameObject buildingTitle;
    public GameObject enemyTitle;
    public GameObject hud;
    public GameObject extraHud;

    public GameObject minimap;
    public GameObject minimapButtonUP;
    public GameObject go;

    void Start()
    {
        units = GameObject.FindGameObjectsWithTag("Player").ToList();
        baseB = GameObject.FindGameObjectWithTag("Respawn");
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
        updateMainHUD();
        if (units.Count() == 0)
        {
            unitTitle.SetActive(false);
        }

    }

    public void openDetails()
    {
        if (unitTitle.transform.GetChild(1).gameObject.activeSelf)
        {
            unitTitle.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            unitTitle.transform.GetChild(1).gameObject.SetActive(true);
        }
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
        hud.transform.GetChild(3).GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = $"{units.Count} / 5";
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
    }

    public void setConstructing(int unit_)
    {
        baseB.GetComponent<baseScript>().constructing = baseB.GetComponent<baseScript>().unitsToBuild[unit_-1];
        GameObject.FindGameObjectWithTag("soundManager").GetComponent<soundScript>().playClip(ping2);
        hideBuildMenu();
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
                        unitTitle.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = $"target:\n{selectedGO.GetComponent<moveScript>().goal.gameObject.name}";
                    }
                    else
                    {
                        unitTitle.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = "target:\nexplore";
                    }
                }
                else
                {
                    unitTitle.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = "target:\nnone";
                }
                unitTitle.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = $"lvl: {selectedGO.GetComponent<moveScript>().lvl}\nex: {selectedGO.GetComponent<moveScript>().exp}/{selectedGO.GetComponent<moveScript>().nextExp}";
                break;
            case "Respawn":
                unitTitle.SetActive(false);
                buildingTitle.SetActive(true);
                enemyTitle.SetActive(false);                buildingTitle.transform.GetChild(0).GetComponent<Text>().text = selectedGO.name;
                buildingTitle.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = $"hp: {selectedGO.GetComponent<baseScript>().health} / {selectedGO.GetComponent<baseScript>().maxHealth}";
                var mask2 = buildingTitle.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<RectMask2D>();
                Vector4 p2 = mask2.padding;
                p2.z = Mathf.Lerp(0f, 384f, 1f - (float)selectedGO.GetComponent<baseScript>().health / selectedGO.GetComponent<baseScript>().maxHealth);
                mask2.padding = p2;
                if (baseB.GetComponent<baseScript>().constructing)
                {
                    buildingTitle.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = $"building:\n{baseB.GetComponent<baseScript>().constructing.name}";
                }
                else
                {
                    buildingTitle.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(0).GetComponent<Text>().text = "building:\nnothing";
                }
                buildingTitle.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = $"lvl: {selectedGO.GetComponent<baseScript>().level}\nex: {selectedGO.GetComponent<baseScript>().exp} / {selectedGO.GetComponent<baseScript>().nextExp}";
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
}
