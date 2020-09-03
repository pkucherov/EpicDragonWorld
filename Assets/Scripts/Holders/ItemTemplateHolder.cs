using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: May 5th 2019
 */
public class ItemTemplateHolder
{
    private readonly int _itemId;
    private readonly ItemSlot _itemSlot;
    private readonly ItemType _itemType;
    private readonly string _name;
    private readonly string _description;
    private readonly Sprite _icon;
    private readonly string _recipeMale;
    private readonly string _recipeFemale;
    private readonly int _prefabId;
    private readonly Vector3 _positionMale;
    private readonly Vector3 _positionFemale;
    private readonly Quaternion _rotationMale;
    private readonly Quaternion _rotationFemale;
    private readonly Vector3 _scaleMale;
    private readonly Vector3 _scaleFemale;
    private readonly bool _stackable;
    private readonly bool _tradable;
    private readonly int _stamina;
    private readonly int _strength;
    private readonly int _dexterity;
    private readonly int _intelect;

    /// <summary>
    /// itemId: the item id.
    /// itemSlot: the ItemSlot enum value.
    /// itemType: the ItemType enum value.
    /// name: UI name information.
    /// description: UI description information.
    /// icon: Sprite used for item icon.
    /// recipeMale: the recipe for male avatars.
    /// recipeFemale: the recipe for female avatars.
    /// prefabId: prefab for mounted item.
    /// positionMale: mounted item position for male avatars.
    /// positionFemale: mounted item position for female avatars.
    /// rotationMale: mounted item rotation for male avatars.
    /// rotationFemale: mounted item rotation for female avatars.
    /// scaleMale: mounted item scale for male avatars.
    /// scaleFemale: mounted item scale for female avatars.
    /// stackable: UI stackable information.
    /// tradable: UI tradable information.
    /// stamina: UI stamina information.
    /// strength: UI strength information.
    /// dexterity: UI dexterity information.
    /// intelect: UI intelect information.
    /// </summary>
    public ItemTemplateHolder(int itemId, ItemSlot itemSlot, ItemType itemType, string name, string description, Sprite icon, string recipeMale, string recipeFemale, int prefabId, Vector3 positionMale, Vector3 positionFemale, Quaternion rotationMale, Quaternion rotationFemale, Vector3 scaleMale, Vector3 scaleFemale, bool stackable, bool tradable, int stamina, int strength, int dexterity, int intelect)
    {
        _itemId = itemId;
        _itemSlot = itemSlot;
        _itemType = itemType;
        _name = name;
        _description = description;
        _icon = icon;
        _recipeMale = recipeMale;
        _recipeFemale = recipeFemale;
        _prefabId = prefabId;
        _positionMale = positionMale;
        _positionFemale = positionFemale;
        _rotationMale = rotationMale;
        _rotationFemale = rotationFemale;
        _scaleMale = scaleMale;
        _scaleFemale = scaleFemale;
        _stackable = stackable;
        _tradable = tradable;
        _stamina = stamina;
        _strength = strength;
        _dexterity = dexterity;
        _intelect = intelect;
    }

    public int GetItemId()
    {
        return _itemId;
    }

    public ItemSlot GetItemSlot()
    {
        return _itemSlot;
    }

    public ItemType GetItemType()
    {
        return _itemType;
    }

    public string GetName()
    {
        return _name;
    }

    public string GetDescription()
    {
        return _description;
    }

    public Sprite GetIcon()
    {
        return _icon;
    }

    public string GetRecipeMale()
    {
        return _recipeMale;
    }

    public string GetRecipeFemale()
    {
        return _recipeFemale;
    }

    public int GetPrefabId()
    {
        return _prefabId;
    }

    public Vector3 GetPositionMale()
    {
        return _positionMale;
    }

    public Vector3 GetPositionFemale()
    {
        return _positionFemale;
    }

    public Quaternion GetRotationMale()
    {
        return _rotationMale;
    }

    public Quaternion GetRotationFemale()
    {
        return _rotationFemale;
    }

    public Vector3 GetScaleMale()
    {
        return _scaleMale;
    }

    public Vector3 GetScaleFemale()
    {
        return _scaleFemale;
    }

    public bool IsStackable()
    {
        return _stackable;
    }

    public bool IsTradable()
    {
        return _tradable;
    }

    public int GetSTA()
    {
        return _stamina;
    }

    public int GetSTR()
    {
        return _strength;
    }

    public int GetDEX()
    {
        return _dexterity;
    }

    public int GetINT()
    {
        return _intelect;
    }
}
