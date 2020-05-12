using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: May 5th 2019
 */
public class ItemTemplateHolder
{
    private readonly int itemId;
    private readonly ItemSlot itemSlot;
    private readonly ItemType itemType;
    private readonly string name;
    private readonly string description;
    private readonly Sprite icon;
    private readonly string recipeMale;
    private readonly string recipeFemale;
    private readonly int prefabId;
    private readonly Vector3 positionMale;
    private readonly Vector3 positionFemale;
    private readonly Quaternion rotationMale;
    private readonly Quaternion rotationFemale;
    private readonly Vector3 scaleMale;
    private readonly Vector3 scaleFemale;
    private readonly bool stackable;
    private readonly bool tradable;
    private readonly int stamina;
    private readonly int strength;
    private readonly int dexterity;
    private readonly int intelect;

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
        this.itemId = itemId;
        this.itemSlot = itemSlot;
        this.itemType = itemType;
        this.name = name;
        this.description = description;
        this.icon = icon;
        this.recipeMale = recipeMale;
        this.recipeFemale = recipeFemale;
        this.prefabId = prefabId;
        this.positionMale = positionMale;
        this.positionFemale = positionFemale;
        this.rotationMale = rotationMale;
        this.rotationFemale = rotationFemale;
        this.scaleMale = scaleMale;
        this.scaleFemale = scaleFemale;
        this.stackable = stackable;
        this.tradable = tradable;
        this.stamina = stamina;
        this.strength = strength;
        this.dexterity = dexterity;
        this.intelect = intelect;
    }

    public int GetItemId()
    {
        return itemId;
    }

    public ItemSlot GetItemSlot()
    {
        return itemSlot;
    }

    public ItemType GetItemType()
    {
        return itemType;
    }

    public string GetName()
    {
        return name;
    }

    public string GetDescription()
    {
        return description;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public string GetRecipeMale()
    {
        return recipeMale;
    }

    public string GetRecipeFemale()
    {
        return recipeFemale;
    }

    public int GetPrefabId()
    {
        return prefabId;
    }

    public Vector3 GetPositionMale()
    {
        return positionMale;
    }

    public Vector3 GetPositionFemale()
    {
        return positionFemale;
    }

    public Quaternion GetRotationMale()
    {
        return rotationMale;
    }

    public Quaternion GetRotationFemale()
    {
        return rotationFemale;
    }

    public Vector3 GetScaleMale()
    {
        return scaleMale;
    }

    public Vector3 GetScaleFemale()
    {
        return scaleFemale;
    }

    public bool IsStackable()
    {
        return stackable;
    }

    public bool IsTradable()
    {
        return tradable;
    }

    public int GetSTA()
    {
        return stamina;
    }

    public int GetSTR()
    {
        return strength;
    }

    public int GetDEX()
    {
        return dexterity;
    }

    public int GetINT()
    {
        return intelect;
    }
}
