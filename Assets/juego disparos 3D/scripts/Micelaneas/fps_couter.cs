using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fps_couter : MonoBehaviour{
    
    
    [Header("Configuración de FPS")]
    [SerializeField] private int targetFPS = 60;
    public Text fpsText;
    
    Vector3 initialLineRenderPosition;
    float fps;
    float msec;
    string text;
    float deltaTime = 0.0f;
    List<Vector3> fpsTimes = new List<Vector3>();
    void Update(){
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        GuiUpdate();
        }
    void GuiUpdate(){
        msec = deltaTime * 1000.0f;
        fps = 1.0f / deltaTime;
        //fpsTimes.Add(Camera.main.ScreenToWorldPoint(Camera.main.pixelWidth, Camera.main.));
        fpsTimes.Add(new Vector2((fpsTimes.Count + 1000)/ 1000f, (fps - 200) / 500f));
        text = string.Format(" {1:0.} fps ({0:0.0} ms)", msec, fps);
        fpsText.text = text;
    }

    private void Awake()
    {
        // 1. Desactivar VSync para permitir el límite manual
        QualitySettings.vSyncCount = 0;

        // 2. Establecer el límite de cuadros por segundo
        Application.targetFrameRate = targetFPS;
        Debug.Log("Limite de FPS: "+targetFPS);
    }
}
