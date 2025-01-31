﻿using System;
using System.IO;
using System.Text;

/**
 * Author: Pantelis Andrianakis
 * Date: December 23rd 2017
 */
public class SendablePacket
{
    private readonly MemoryStream _memoryStream;

    public SendablePacket()
    {
        _memoryStream = new MemoryStream();
    }

    public void WriteBoolean(bool value)
    {
        _memoryStream.WriteByte((byte)(value ? 1 : 0));
    }

    public void WriteString(string value)
    {
        if (value != null)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(value);
            // Since we use short value maximum byte size for strings is 32767.
            // Take care that maximum packet size data is 32767 bytes as well.
            // Sending a 32767 byte string would require all the available packet size.
            WriteShort(byteArray.Length);
            WriteBytes(byteArray);
        }
        else
        {
            _memoryStream.WriteByte(0);
        }
    }

    public void WriteBytes(byte[] byteArray)
    {
        for (int i = 0; i < byteArray.Length; i++)
        {
            _memoryStream.WriteByte(byteArray[i]);
        }
    }

    public void WriteByte(int value)
    {
        _memoryStream.WriteByte((byte)value);
    }

    public void WriteShort(int value)
    {
        _memoryStream.WriteByte((byte)value);
        _memoryStream.WriteByte((byte)(value >> 8));
    }

    public void WriteInt(int value)
    {
        _memoryStream.WriteByte((byte)value);
        _memoryStream.WriteByte((byte)(value >> 8));
        _memoryStream.WriteByte((byte)(value >> 16));
        _memoryStream.WriteByte((byte)(value >> 24));
    }

    public void WriteLong(long value)
    {
        _memoryStream.WriteByte((byte)value);
        _memoryStream.WriteByte((byte)(value >> 8));
        _memoryStream.WriteByte((byte)(value >> 16));
        _memoryStream.WriteByte((byte)(value >> 24));
        _memoryStream.WriteByte((byte)(value >> 32));
        _memoryStream.WriteByte((byte)(value >> 40));
        _memoryStream.WriteByte((byte)(value >> 48));
        _memoryStream.WriteByte((byte)(value >> 56));
    }

    // TODO: Until BitConverter SingleToInt32Bits .Net support for Unity is added.
    private unsafe int SingleToInt32Bits(float fvalue)
    {
        return *(int*)(&fvalue);
    }

    public void WriteFloat(float value)
    {
        WriteInt(SingleToInt32Bits(value));
    }

    public void WriteDouble(double value)
    {
        WriteLong(BitConverter.DoubleToInt64Bits(value));
    }

    public byte[] GetSendableBytes()
    {
        // Get array of bytes.
        byte[] byteArray = _memoryStream.ToArray();
        int size = byteArray.Length;

        // Create two bytes for length (short - max length 32767).
        byte[] lengthBytes = new byte[2];
        lengthBytes[0] = (byte)(size & 0xff);
        lengthBytes[1] = (byte)((size >> 8) & 0xff);

        // Join bytes.
        byte[] result = new byte[size + 2];
        Array.Copy(lengthBytes, 0, result, 0, 2);
        Array.Copy(byteArray, 0, result, 2, size);

        // Return the data.
        return result;
    }
}
