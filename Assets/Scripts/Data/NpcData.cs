using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

/**
 * Author: Pantelis Andrianakis
 * Date: November 28th 2019
 */
public class NpcData : MonoBehaviour
{
    public static NpcData Instance { get; private set; }

    private static readonly Dictionary<int, CharacterDataHolder> NPCS = new Dictionary<int, CharacterDataHolder>();

    private void Start()
    {
        Instance = this;

        TextAsset data = Resources.Load("data/NpcData") as TextAsset;
        string[] lines = Regex.Split(data.text, "\r\n|\n\r|\n|\r");
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (line.StartsWith("#"))
            {
                continue;
            }
            string[] values = line.Split(';');
            if (values.Length < 18)
            {
                continue;
            }

            CharacterDataHolder holder = new CharacterDataHolder();
            holder.SetName(values[1]);
            holder.SetRace(byte.Parse(values[2]));
            holder.SetHeight(float.Parse(values[3], CultureInfo.InvariantCulture));
            holder.SetBelly(float.Parse(values[4], CultureInfo.InvariantCulture));
            holder.SetHairType(int.Parse(values[5]));
            holder.SetHairColor(int.Parse(values[6]));
            holder.SetSkinColor(int.Parse(values[7]));
            holder.SetEyeColor(int.Parse(values[8]));
            holder.SetHeadItem(int.Parse(values[9]));
            holder.SetChestItem(int.Parse(values[10]));
            holder.SetHandsItem(int.Parse(values[11]));
            holder.SetLegsItem(int.Parse(values[12]));
            holder.SetFeetItem(int.Parse(values[13]));
            holder.SetRightHandItem(int.Parse(values[14]));
            holder.SetLeftHandItem(int.Parse(values[15]));
            holder.SetMaxHp(int.Parse(values[16]));
            holder.SetTargetable(bool.Parse(values[17]));

            NPCS.Add(int.Parse(values[0]), holder);
        }
    }

    public static CharacterDataHolder GetNpc(int id)
    {
        if (!NPCS.ContainsKey(id))
        {
            return null;
        }
        return NPCS[id];
    }
}
