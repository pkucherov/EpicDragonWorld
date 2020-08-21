using System;
using System.IO;
using System.Text;

/**
 * Author: Pantelis Andrianakis
 * Date: April 23rd 2019
 */
public class ConfigWriter
{
    private readonly string fileName;

    public ConfigWriter(string fileName)
    {
        this.fileName = fileName;

        try
        {
            // Create file if it does not exist.
            if (!File.Exists(fileName))
            {
                File.Create(fileName).Dispose();
            }
        }
        catch (Exception)
        {
        }
    }

    public void SetString(string config, string value)
    {
        try
        {
            // Check for existing config.
            bool found = false;
            StringBuilder contents = new StringBuilder();
            string[] lines = File.ReadAllLines(fileName);
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                if (line.StartsWith(config + "=") || line.StartsWith(config + " ="))
                {
                    // Existing config found.
                    found = true;
                    contents.Append(config);
                    contents.Append(" = ");
                    contents.Append(value);
                    contents.Append(Environment.NewLine);
                }
                else
                {
                    contents.Append(line);
                    contents.Append(Environment.NewLine);
                }
            }

            // If not found create a new entry.
            if (!found)
            {
                contents.Append(config);
                contents.Append(" = ");
                contents.Append(value);
                contents.Append(Environment.NewLine);
            }

            // Write new file contents.
            File.WriteAllText(fileName, contents.ToString());
        }
        catch (Exception)
        {
        }
    }

    public void SetBool(string config, bool value)
    {
        SetString(config, value.ToString());
    }

    public void SetInt(string config, int value)
    {
        SetString(config, value.ToString());
    }

    public void SetLong(string config, long value)
    {
        SetString(config, value.ToString());
    }

    public void SetFloat(string config, float value)
    {
        SetString(config, value.ToString());
    }

    public void SetDouble(string config, double value)
    {
        SetString(config, value.ToString());
    }
}
