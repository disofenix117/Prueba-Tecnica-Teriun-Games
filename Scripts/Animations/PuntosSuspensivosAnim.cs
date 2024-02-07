using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(TextMeshProUGUI))]
public class PuntosSuspensivosAnim : MonoBehaviour
{

    TextMeshProUGUI Texto;
    [SerializeField] string texto;

    private void OnEnable()
    {
        Texto = GetComponent<TextMeshProUGUI>();

        StartCoroutine(anim());
    }
    IEnumerator anim()
    {
        string main = texto;
        while (true)
        {
            yield return new WaitForSecondsRealtime(0.5f);
            Texto.text = main + ".";

            yield return new WaitForSecondsRealtime(0.5f);
            Texto.text = main + "..";

            yield return new WaitForSecondsRealtime(0.5f);
            Texto.text = main + "...";

            yield return new WaitForSecondsRealtime(0.5f);
            Texto.text = main;

        }
    }

}