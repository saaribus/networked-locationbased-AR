using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// This class is only for debuggin purposes. No functionality is drawn from it.
/// this class identifies on what platform the application is running. We use this to instantiate
/// platform specific player objects in the NetworkManagerScript
/// </summary>
public class PlatformIdentifier : MonoBehaviour
{
    [SerializeField] string whatPlatform;
    [SerializeField] TextMeshProUGUI platformDebug;
    // Start is called before the first frame update
    void Start()
    {
        whatPlatform =  Application.platform.ToString();
        platformDebug.text = "Plattform: " + whatPlatform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
