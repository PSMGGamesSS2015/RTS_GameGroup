﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// The ImpManager is a subcomponent of the GameManager and manages the
/// logic behind the player-controlled imps in a level. For example,
/// it spawns imps and gets notified when an imp is selected by the player.
/// </summary>

public class ImpManager : MonoBehaviour, ImpController.ImpControllerListener {

    private Level lvl;

    private List<ImpController> imps;
   
    private float spawnCounter;
    private int currentImps;

    private ImpController impSelected;

    public GameObject impPrefab;

    private void Awake()
    {
        imps = new List<ImpController>();
    }

    public void SetLvl(Level lvl) {
        this.lvl = lvl;
        Debug.Log(lvl.Config.Name);
        Debug.Log(lvl.Config.MaxProfessions);
    }

    private void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown)
        {
            switch (e.keyCode)
            {
                case KeyCode.Alpha1:
                    SelectProfession(ImpType.Spearman);
                    break;
                case KeyCode.Alpha2:
                    SelectProfession(ImpType.Coward);
                    break;
                case KeyCode.Alpha3:
                    SelectProfession(ImpType.Pest);
                    break;
                case KeyCode.Alpha4:
                    SelectProfession(ImpType.LadderCarrier);
                    break;
                case KeyCode.Alpha5:
                    SelectProfession(ImpType.Blaster);
                    break;
                case KeyCode.Alpha6:
                    SelectProfession(ImpType.Firebug);
                    break;
                case KeyCode.Alpha7:
                    SelectProfession(ImpType.Minnesinger);
                    break;
                case KeyCode.Alpha8:
                    SelectProfession(ImpType.Botcher);
                    break;
                case KeyCode.Alpha9:
                    SelectProfession(ImpType.Schwarzenegger);
                    break;
                case KeyCode.Alpha0:
                    SelectProfession(ImpType.Unemployed);
                    break;
            }
        }
    }

    private void SelectProfession(ImpType profession)
    {
        if (impSelected != null)
        {
            impSelected.Train(profession);
        }
    }

    public void SpawnImps()
    {
        if (currentImps == 0)
        {
            SpawnImp();
        }
        else if (IsMaxImpsReached() && IsSpawnTimeCooledDown())
        {
            SpawnImp();
        }
        else
        {
            spawnCounter += Time.deltaTime;
        }
    }

    private bool IsMaxImpsReached()
    {
        return currentImps < lvl.Config.MaxImps;
    }

    private bool IsSpawnTimeCooledDown()
    {
        return spawnCounter >= lvl.Config.SpawnInterval;
    }

    private void SpawnImp()
    {
        Vector3 spawnPosition = lvl.SpawnPosition;
        GameObject imp = (GameObject)Instantiate(impPrefab, spawnPosition, Quaternion.identity);
        ImpController impController = imp.GetComponent<ImpController>();
        impController.RegisterListener(this);
        currentImps++;
        imps.Add(impController);
        spawnCounter = 0f; 
    }

    void ImpController.ImpControllerListener.OnImpSelected(ImpController impController)
    {
        impSelected = impController;
    }

}