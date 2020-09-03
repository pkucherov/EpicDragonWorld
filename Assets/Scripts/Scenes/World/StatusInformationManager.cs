using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * Author: Pantelis Andrianakis
 * Date: November 30th 2019
 */
public class StatusInformationManager : MonoBehaviour
{
    public static StatusInformationManager Instance { get; private set; }

    public TextMeshProUGUI _playerInformation;
    public Slider _playerHpBar;
    public TextMeshProUGUI _playerHpPercent;

    public TextMeshProUGUI _targetInformation;
    public Slider _targetHpBar;
    public TextMeshProUGUI _targetHpPercent;

    private void Start()
    {
        if (Instance != null)
        {
            return;
        }
        Instance = this;

        HideTargetInformation();
    }

    private void HideTargetInformation()
    {
        _targetInformation.text = "";
        _targetHpBar.value = 1;
        _targetHpPercent.text = "";
        _targetInformation.gameObject.SetActive(false);
        _targetHpBar.gameObject.SetActive(false);
    }

    public void UpdateTargetInformation(WorldObject obj)
    {
        // Hide when object is null.
        if (obj == null)
        {
            HideTargetInformation();
            return;
        }
        // Show if hidden.
        if (!_targetInformation.IsActive())
        {
            _targetInformation.gameObject.SetActive(true);
            _targetHpBar.gameObject.SetActive(true);
        }
        // Update information.
        CharacterDataHolder data = obj.GetCharacterData();
        if (data != null)
        {
            _targetInformation.text = data.GetName();
            float progress = Mathf.Clamp01(data.GetCurrentHp() / data.GetMaxHp());
            _targetHpBar.value = progress;
            _targetHpPercent.text = (int)(progress * 100f) + "%";
        }
    }

    public void UpdatePlayerInformation()
    {
        CharacterDataHolder data = MainManager.Instance.GetSelectedCharacterData();
        _playerInformation.text = data.GetName();
        float progress = Mathf.Clamp01(data.GetCurrentHp() / data.GetMaxHp());
        _playerHpBar.value = progress;
        _playerHpPercent.text = (int)(progress * 100f) + "%";
    }
}
