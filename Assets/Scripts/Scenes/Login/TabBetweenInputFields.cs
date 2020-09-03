using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/**
 * Author: Pantelis Andrianakis
 * Date: December 24th 2018
 */
public class TabBetweenInputFields : MonoBehaviour
{
    public Selectable[] _selectables;
    public int _startIndex = 0;

    private void Start()
    {
        ApplyEnterSelect(_selectables[_startIndex]);
    }

    private void Update()
    {
        if (InputManager.TAB_DOWN)
        {
            _startIndex++;

            if (_startIndex >= _selectables.Length)
            {
                _startIndex = 0;
            }

            if (_selectables[_startIndex] != null)
            {
                _selectables[_startIndex].Select();
            }
        }

        if (InputManager.RETURN_DOWN)
        {
            _startIndex = 2; // Login button.
            ApplyEnterSelect(_selectables[_startIndex]);
        }
    }

    private void ApplyEnterSelect(Selectable selectable)
    {
        if (selectable != null)
        {
            if (selectable.GetComponent<TMP_InputField>() != null)
            {
                selectable.Select();
            }
            else
            {
                Button selectedButton = selectable.GetComponent<Button>();
                if (selectedButton != null)
                {
                    selectable.Select();
                    selectedButton.OnPointerClick(new PointerEventData(EventSystem.current));
                }
            }
        }
    }
}
