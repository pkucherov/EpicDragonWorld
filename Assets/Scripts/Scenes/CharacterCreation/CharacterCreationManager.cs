using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

/**
 * Author: Ilias Vlachos, Pantelis Andrianakis
 * Date: December 28th 2018
 */
public class CharacterCreationManager : MonoBehaviour
{
    public static CharacterCreationManager Instance { get; private set; }

    public Slider _heightSlider;
    public Slider _bellySlider;
    public Button _zoomIn;
    public Button _zoomOut;
    public Button _createButton;
    public Button _backButton;
    public Text _textMessage;
    public TMP_InputField _charNameField;

    private int _creationResult;
    private bool _waitingServer;
    private GameObject _avatar;
    private CharacterDataHolder _dataHolder;
    private CharacterDataHolder _dataHolderMale;
    private CharacterDataHolder _dataHolderFemale;
    private int _currentHairMale = 0;
    private int _currentHairFemale = 0;

    private void Start()
    {
        // Return if account name is empty.
        if (MainManager.Instance == null || MainManager.Instance.GetAccountName() == null)
        {
            return;
        }

        // Set instance.
        if (Instance != null)
        {
            return;
        }
        Instance = this;

        // Schedule exit to login screen.
        StartCoroutine(ExitToCharacterSelection());

        // Add button listeners.
        _zoomIn.onClick.AddListener(CameraZoomIn);
        _zoomOut.onClick.AddListener(CameraZoomOut);
        _createButton.onClick.AddListener(OnClickCreateButton);
        _backButton.onClick.AddListener(OnClickBackButton);

        // Initialize character data holders.
        _dataHolderMale = new CharacterDataHolder();
        _dataHolderMale.SetRace(0);
        _dataHolderFemale = new CharacterDataHolder();
        _dataHolderFemale.SetRace(3);
        _dataHolder = _dataHolderMale;

        // Initial values.
        _avatar = CharacterManager.Instance.CreateCharacter(_dataHolderMale, 8.28f, 0.1035156f, 20.222f, 180);
        _heightSlider.onValueChanged.AddListener(HeightChange);
        _bellySlider.onValueChanged.AddListener(BellyChange);

        // Camera position.
        Camera.main.transform.position = new Vector3(8.29f, 1.29f, 17.7f);
    }

    private void Update()
    {
        if (InputManager.RETURN_DOWN)
        {
            OnClickCreateButton();
        }
    }

    public void SwitchGender(bool male)
    {
        if (male)
        {
            // Check if current race is female.
            if (_dataHolder.GetRace() >= 3 && _dataHolder.GetRace() <= 5)
            {
                Destroy(_avatar.gameObject);
                _dataHolder = _dataHolderMale;
                _avatar = CharacterManager.Instance.CreateCharacter(_dataHolder, 8.28f, 0.1035156f, 20.222f, 180);
            }
        }
        else
        {
            // Check if current race is male.
            if (_dataHolder.GetRace() >= 0 && _dataHolder.GetRace() <= 2)
            {
                Destroy(_avatar.gameObject);
                _dataHolder = _dataHolderFemale;
                _avatar = CharacterManager.Instance.CreateCharacter(_dataHolder, 8.28f, 0.1035156f, 20.222f, 180);
            }
        }
    }

    public void HeightChange(float val)
    {
        //_dna["height"].Set(val);
        //_avatar.BuildCharacter();
        _dataHolder.SetHeight(val);
    }

    public void BellyChange(float val)
    {
        //_dna["belly"].Set(val);
        //_avatar.BuildCharacter();
        _dataHolder.SetBelly(val);
    }

    public void ChangeSkinColor(Color color)
    {
        //_avatar.SetColor("Skin", color);
        //_avatar.UpdateColors(true);
        _dataHolder.SetSkinColor(Util.ColorToInt(color));
    }

    public void ChangeHairColor(Color color)
    {
        //_avatar.SetColor("Hair", color);
        //_avatar.UpdateColors(true);
        _dataHolder.SetHairColor(Util.ColorToInt(color));
    }

    public void ChangeEyesColor(Color color)
    {
        //_avatar.SetColor("Eyes", color);
        //_avatar.UpdateColors(true);
        _dataHolder.SetEyeColor(Util.ColorToInt(color));
    }

    public void ChangeHair(bool plus)
    {
        //if (_avatar.activeRace.name == "HumanMaleDCS")
        //{
        //    if (plus)
        //    {
        //        _currentHairMale++;
        //    }
        //    else
        //    {
        //        _currentHairMale--;
        //    }

        //    _currentHairMale = Mathf.Clamp(_currentHairMale, 0, CharacterManager.Instance.GetHairModelsMale().Count - 1);

        //    if (CharacterManager.Instance.GetHairModelsMale()[_currentHairMale] == "None")
        //    {
        //        _avatar.ClearSlot("Hair");
        //    }
        //    else
        //    {
        //        _avatar.SetSlot("Hair", CharacterManager.Instance.GetHairModelsMale()[_currentHairMale]);
        //    }

        //    _dataHolder.SetHairType(_currentHairMale);
        //}

        //if (_avatar.activeRace.name == "HumanFemaleDCS")
        //{
        //    if (plus)
        //    {
        //        _currentHairFemale++;
        //    }
        //    else
        //    {
        //        _currentHairFemale--;
        //    }

        //    _currentHairFemale = Mathf.Clamp(_currentHairFemale, 0, CharacterManager.Instance.GetHairModelsFemale().Count - 1);

        //    if (CharacterManager.Instance.GetHairModelsFemale()[_currentHairFemale] == "None")
        //    {
        //        _avatar.ClearSlot("Hair");
        //    }
        //    else
        //    {
        //        _avatar.SetSlot("Hair", CharacterManager.Instance.GetHairModelsFemale()[_currentHairFemale]);
        //    }

        //    _dataHolder.SetHairType(_currentHairFemale);
        //}

        //_avatar.BuildCharacter();
    }

    public void CameraZoomIn()
    {
        //if (_avatar.activeRace.name == "HumanMaleDCS")
        //{
        //    StartCoroutine(LerpFromTo(Camera.main.transform.position, new Vector3(8.3f, 1.568f, 19.491f), 1f));
        //}
        //if (_avatar.activeRace.name == "HumanFemaleDCS")
        //{
        //    StartCoroutine(LerpFromTo(Camera.main.transform.position, new Vector3(8.3f, 1.472f, 19.48f), 1f));
        //}
    }

    public void CameraZoomOut()
    {
        StartCoroutine(LerpFromTo(Camera.main.transform.position, new Vector3(8.29f, 1.29f, 17.7f), 1f));
    }

    private IEnumerator LerpFromTo(Vector3 pos1, Vector3 pos2, float duration)
    {
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            Camera.main.transform.position = Vector3.Lerp(pos1, pos2, t / duration);
            yield return 0;
        }
        Camera.main.transform.position = pos2;
    }

    private void OnClickBackButton()
    {
        if (_avatar != null)
        {
            Destroy(_avatar.gameObject);
        }
        MainManager.Instance.LoadScene(MainManager.CHARACTER_SELECTION_SCENE);
    }

    private IEnumerator ExitToCharacterSelection()
    {
        yield return new WaitForSeconds(1800); // Wait 30 minutes.
        OnClickBackButton();
    }

    private void OnClickCreateButton()
    {
        // Disable buttons.
        DisableButtons();

        // Store creation information.
        string name = _charNameField.text;

        // No name entered.
        if (name == "")
        {
            _textMessage.text = "Please enter a name.";
            EnableButtons();
            return;
        }

        // Set name
        _dataHolder.SetName(name);

        // Request character creation.
        NetworkManager.ChannelSend(new CharacterCreationRequest(_dataHolder));

        // Wait until server sends creation result.
        _waitingServer = true;
        _creationResult = -1;
        while (_waitingServer)
        {
            switch (_creationResult)
            {
                case 0:
                    _textMessage.text = "Invalid name.";
                    _waitingServer = false;
                    break;

                case 1:
                    _textMessage.text = "Name is too short.";
                    _waitingServer = false;
                    break;

                case 2:
                    _textMessage.text = "Name already exists.";
                    _waitingServer = false;
                    break;

                case 3:
                    _textMessage.text = "Cannot create additional characters.";
                    _waitingServer = false;
                    break;

                case 4:
                    _textMessage.text = "Invalid creation parameters.";
                    _waitingServer = false;
                    break;

                case 100:
                    _textMessage.text = "Creation success!";
                    _waitingServer = false;
                    break;
            }
        }

        // Go to player selection screen.
        if (_creationResult == 100)
        {
            OnClickBackButton();
        }
        else // Enable buttons.
        {
            EnableButtons();
        }
    }

    private void DisableButtons()
    {
        _textMessage.text = "Waiting for server..."; // Clean any old messages.
        _createButton.enabled = false;
        _backButton.enabled = false;
        _charNameField.enabled = false;
    }

    private void EnableButtons()
    {
        _createButton.enabled = true;
        _backButton.enabled = true;
        _charNameField.enabled = true;
    }

    public void SetCreationResult(int value)
    {
        _creationResult = value;
    }
}
