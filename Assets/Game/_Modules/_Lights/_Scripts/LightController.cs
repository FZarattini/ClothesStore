using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightController : MonoBehaviour
{
    [Title("References")]
    [SerializeField] Light2D _globalLight;
    [SerializeField] Light2D _wardrobeSpotlight;
    [SerializeField, ReadOnly] Transform _playerTransform;

    [Title("Lighting Data")]
    [SerializeField] float lowLightingIntensity;
    [SerializeField] float normalLightingIntensity;

    private void OnEnable()
    {
        WardrobeTrigger.OnWardrobeInteraction += EnableWardrobeLighting;
        InventoryManager.OnInventoryClose += DisableWardrobeLighting;
    }

    private void OnDisable()
    {
        WardrobeTrigger.OnWardrobeInteraction -= EnableWardrobeLighting;
        InventoryManager.OnInventoryClose -= DisableWardrobeLighting;
    }

    private void Awake()
    {
        _playerTransform = FindObjectOfType<PlayerController>().transform;
    }

    void EnableWardrobeLighting()
    {
        _wardrobeSpotlight.gameObject.SetActive(true);
        _wardrobeSpotlight.transform.position = _playerTransform.position;

        _globalLight.intensity = lowLightingIntensity;
    }

    void DisableWardrobeLighting()
    {
        _wardrobeSpotlight.gameObject.SetActive(false);
        _globalLight.intensity = normalLightingIntensity;
    }

}
