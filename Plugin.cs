using System;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using CookingGrenades.Config;
using CookingGrenades.Patches;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Linq;

namespace CookingGrenades
{
    [BepInPlugin("com.November75.CookingGrenades", "CookingGrenades", BuildInfo.Version)]
    [BepInDependency("com.SPT.core", "3.11.0")]  
    public class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource log;
        private void Awake()
        {
            log = Logger;
            ConfigManager.Init(Config);
            ConfigEventHandler.Init();
            Utils.FuseTimeTester.Init();

            new GrenadeInitPatch().Enable();
            new GrenadeHandsControllermethod_2Patch().Enable();
            new PlayerGrenadeHandsControllerHandleFireInputPatch().Enable();
            new PlayerGrenadeHandsControllerHandleAltFireInputPatch().Enable();
            new EftGamePlayerOwnerTranslateCommandPatch().Enable();
            new ThrowWeapItemClassGetExplDelayPatch().Enable();
            if (!ConfigManager.UserWarningConfirmed.Value)
            {
                new MenuScreenPatch().Enable();                
            }

        }
    }
}

/*
    The controller changes with each grenade.
    (I confirmed that the controller.GetInstanceID() changes, but I wasn’t able to verify whether the controller itself does. 
    This plugin was written under the assumption that it does change.)


    idle is 1158 hold grenade, if pull pin(low or high) then can go 1156 or 1157

    1156 remove the pull ring high throw
    1157 remove the pull ring low throw

    When throw
    low 1157 will call 1156.method_2
    so we only have to change 1156.method_2
*/
