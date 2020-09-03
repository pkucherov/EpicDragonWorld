using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

/**
 * Author: Pantelis Andrianakis
 * Date: November 7th 2018
 */
public class ConfigReader
{
    private readonly Dictionary<string, string> _configs = new Dictionary<string, string>();

    public ConfigReader(string fileName)
    {
        try
        {
            string[] lines = File.ReadAllLines(fileName);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (!line.StartsWith("#") && line.Trim().Length > 0)
                {
                    string[] lineSplit = line.Split('=');
                    if (lineSplit.Length > 1)
                    {
                        _configs.Add(lineSplit[0].Trim(), string.Join("=", lineSplit.Skip(1).ToArray()).Trim());
                    }
                }
            }
        }
        catch (Exception)
        {
        }
    }

    public string GetString(string config, string defaultValue)
    {
        if (!_configs.ContainsKey(config))
        {
            return defaultValue;
        }
        return _configs[config];
    }

    public bool GetBool(string config, bool defaultValue)
    {
        if (!_configs.ContainsKey(config))
        {
            return defaultValue;
        }

        try
        {
            return bool.Parse(_configs[config]);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    public int GetInt(string config, int defaultValue)
    {
        if (!_configs.ContainsKey(config))
        {
            return defaultValue;
        }

        try
        {
            return int.Parse(_configs[config]);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    public long GetLong(string config, long defaultValue)
    {
        if (!_configs.ContainsKey(config))
        {
            return defaultValue;
        }

        try
        {
            return long.Parse(_configs[config]);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    public float GetFloat(string config, float defaultValue)
    {
        if (!_configs.ContainsKey(config))
        {
            return defaultValue;
        }

        try
        {
            return float.Parse(_configs[config], CultureInfo.InvariantCulture);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    public double GetDouble(string config, double defaultValue)
    {
        if (!_configs.ContainsKey(config))
        {
            return defaultValue;
        }

        try
        {
            return double.Parse(_configs[config], CultureInfo.InvariantCulture);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }
}
