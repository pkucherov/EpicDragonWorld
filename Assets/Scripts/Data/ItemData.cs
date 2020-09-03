using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: June 22nd 2019
 */
public class ItemData : MonoBehaviour
{
    public static ItemData Instance { get; private set; }

    public GameObject[] _itemPrefabs;
    private static readonly Dictionary<int, ItemTemplateHolder> ITEMS = new Dictionary<int, ItemTemplateHolder>();

    private void Start()
    {
        Instance = this;

        TextAsset data = Resources.Load("data/ItemData") as TextAsset;
        string[] lines = Regex.Split(data.text, "\r\n|\n\r|\n|\r");
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (line.StartsWith("#"))
            {
                continue;
            }
            string[] values = line.Split(';');
            if (values.Length < 20)
            {
                continue;
            }

            int itemId = int.Parse(values[0]);
            ItemSlot itemSlot = (ItemSlot)Enum.Parse(typeof(ItemSlot), values[1]);
            ItemType itemType = (ItemType)Enum.Parse(typeof(ItemType), values[2]);
            string name = values[3];
            string description = values[4];
            string recipeMale = values[5];
            string recipeFemale = values[6];
            int prefabId = int.Parse(values[7]);
            string[] positionMaleSplit = values[8].Split(',');
            Vector3 positionMale = new Vector3(float.Parse(positionMaleSplit[0], CultureInfo.InvariantCulture), float.Parse(positionMaleSplit[1], CultureInfo.InvariantCulture), float.Parse(positionMaleSplit[2], CultureInfo.InvariantCulture));
            string[] positionFemaleSplit = values[9].Split(',');
            Vector3 positionFemale = new Vector3(float.Parse(positionFemaleSplit[0], CultureInfo.InvariantCulture), float.Parse(positionFemaleSplit[1], CultureInfo.InvariantCulture), float.Parse(positionFemaleSplit[2], CultureInfo.InvariantCulture));
            string[] rotationMaleSplit = values[10].Split(',');
            Quaternion rotationMale = Quaternion.Euler(float.Parse(rotationMaleSplit[0], CultureInfo.InvariantCulture), float.Parse(rotationMaleSplit[1], CultureInfo.InvariantCulture), float.Parse(rotationMaleSplit[2], CultureInfo.InvariantCulture));
            string[] rotationFemaleSplit = values[11].Split(',');
            Quaternion rotationFemale = Quaternion.Euler(float.Parse(rotationFemaleSplit[0], CultureInfo.InvariantCulture), float.Parse(rotationFemaleSplit[1], CultureInfo.InvariantCulture), float.Parse(rotationFemaleSplit[2], CultureInfo.InvariantCulture));
            string[] scaleMaleSplit = values[12].Split(',');
            Vector3 scaleMale = new Vector3(float.Parse(scaleMaleSplit[0], CultureInfo.InvariantCulture), float.Parse(scaleMaleSplit[1], CultureInfo.InvariantCulture), float.Parse(scaleMaleSplit[2], CultureInfo.InvariantCulture));
            string[] scaleFemaleSplit = values[13].Split(',');
            Vector3 scaleFemale = new Vector3(float.Parse(scaleFemaleSplit[0], CultureInfo.InvariantCulture), float.Parse(scaleFemaleSplit[1], CultureInfo.InvariantCulture), float.Parse(scaleFemaleSplit[2], CultureInfo.InvariantCulture));
            bool stackable = bool.Parse(values[14]);
            bool tradable = bool.Parse(values[15]);
            int stamina = int.Parse(values[16]);
            int strength = int.Parse(values[17]);
            int dexterity = int.Parse(values[18]);
            int intelect = int.Parse(values[19]);

            Sprite icon = null; // TODO: Resources.Load<Sprite>("ItemIcons/Equipment/" + recipeFemale.Replace("_Recipe", ""));

            ITEMS.Add(itemId, new ItemTemplateHolder(itemId, itemSlot, itemType, name, description, icon, recipeMale, recipeFemale, prefabId, positionMale, positionFemale, rotationMale, rotationFemale, scaleMale, scaleFemale, stackable, tradable, stamina, strength, dexterity, intelect));
        }
    }

    public static ItemTemplateHolder GetItemTemplate(int itemId)
    {
        if (!ITEMS.ContainsKey(itemId))
        {
            return null;
        }
        return ITEMS[itemId];
    }

    public GameObject GetItemPrefab(int prefabId)
    {
        return _itemPrefabs[prefabId];
    }
}
