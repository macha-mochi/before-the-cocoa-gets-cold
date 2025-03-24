using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private PostProcessVolume volume;
    private Image fill;
    [SerializeField] DrainWarmth d;

    Vignette vignette;
    ChromaticAberration ca;
    ColorGrading cg;
    private void Start()
    {
        fill = GameObject.Find("fill").GetComponent<Image>();
        GameObject gameObject = GameObject.Find("Coloured Cubes");
    }

    private void Update()
    {
        fill.fillAmount = ((float)health.health/ (float)health.maxHealth);
        volume.profile.TryGetSettings(out vignette);
        volume.profile.TryGetSettings(out ca);
        volume.profile.TryGetSettings(out cg);
        if(fill.fillAmount <= 0.5) vignette.opacity.value = (0.5f - fill.fillAmount)*2;
        else
        {
            if(fill.fillAmount > 0) vignette.opacity.value -= Time.deltaTime;
        }
        if (d.inFire)
        {
            if(ca.intensity.value < 0.5f) ca.intensity.value += Time.deltaTime;
            if (cg.temperature < 100) cg.temperature.value += (Time.deltaTime * 100);
        }
        else
        {
            if (ca.intensity.value > 0) ca.intensity.value -= Time.deltaTime;
            if(cg.temperature > 0) cg.temperature.value -= (Time.deltaTime * 100);
        }
    }
}
