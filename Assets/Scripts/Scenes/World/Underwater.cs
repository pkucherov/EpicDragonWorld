using UnityEngine;

/**
 * Author: Ilias Vlachos
 * Date: January 3rd 2019
 */
public class Underwater : MonoBehaviour
{
    public float _waterHeight = 65f;

    private bool _isUnderwater;
    private Color _normalColor;
    private Color _underwaterColor;

    private void Start()
    {
        _normalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        _underwaterColor = new Color(0.3047348f, 0.5396f, 0.6037f, 0.5019f);

        // Set both modes to avoid later latency when switch and initializing.
        SetUnderwater();
        SetNormal();
    }

    private void Update()
    {
        if ((transform.position.y < _waterHeight) != _isUnderwater)
        {
            _isUnderwater = transform.position.y < _waterHeight;
            if (_isUnderwater)
			{
				SetUnderwater();
			}
            else
			{
				SetNormal();
			}
        }
    }

    private void SetNormal()
    {
        RenderSettings.fogColor = _normalColor;
        RenderSettings.fogDensity = 0.01f;
        RenderSettings.fogMode = FogMode.Linear;
    }

    private void SetUnderwater()
    {
        RenderSettings.fogColor = _underwaterColor;
        RenderSettings.fogDensity = 0.1f;
        RenderSettings.fogMode = FogMode.Exponential;
    }
}