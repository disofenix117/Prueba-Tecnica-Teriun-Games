using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class loading : MonoBehaviour
{

    [SerializeField] float progreso;
    [SerializeField] TextMeshProUGUI porcentaje;
    [SerializeField] Image BarraProgreso;
    bool isDone = false;
    // Start is called before the first frame update
    void Start()
    {
        string leveltoLoad = LevelLoader.nextLevel;
        StartCoroutine(load(leveltoLoad));
        
    }
    IEnumerator load(string level)
    {
       // yield return new WaitForSeconds(1f);
        AsyncOperation operation= SceneManager.LoadSceneAsync(level);
        while(operation.isDone==false)
        {
            progreso = operation.progress;
            porcentaje.text = progreso * 100+ "%";
            //porcentaje.text = (progreso * 100f).ToString("f0") + "%";
            BarraProgreso.fillAmount = operation.progress;

            yield return null;

        }
    }
}
