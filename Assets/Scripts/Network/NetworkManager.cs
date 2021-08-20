using System;
using System.Net.Sockets;
using System.Threading;

/**
 * Author: Pantelis Andrianakis
 * Date: December 25th 2017
 */
public class NetworkManager
{
    // For socket read.
    private static Thread _readThread;
    private static bool _readThreadStarted = false;

    // For socket write.
    private static Socket _socket;
    private static bool _socketConnected = false;

    // Send to login screen message status.
    private static bool _forcedDisconnection = false;
    private static bool _unexpectedDisconnection = false;

    // Best to call this only once per login attempt.
    public static bool ConnectToServer()
    {
        if (!_socketConnected || _socket == null || !_socket.Connected)
        {
            ConnectSocket();
        }
        return SocketConnected();
    }

    private static void ConnectSocket()
    {
        try
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IAsyncResult result = _socket.BeginConnect(NetworkConfigurations.SERVER_IP, NetworkConfigurations.SERVER_PORT, null, null);
            bool success = result.AsyncWaitHandle.WaitOne(NetworkConfigurations.TIMEOUT_DELAY, true);

            if (!success)
            {
                _socketConnected = false;
                _socket.Close();
            }
            else
            {
                if (_socket.Connected)
                {
                    _socketConnected = true;
                    // Start Receive thread.
                    _readThreadStarted = true;
                    _readThread = new Thread(new ThreadStart(ChannelRead));
                    _readThread.Start();
                }
                else
                {
                    _socketConnected = false;
                    _socket.Close();
                }
            }
        }
        catch (SocketException)
        {
            _socketConnected = false;
            _readThreadStarted = false;
        }
    }

    private static void ChannelRead()
    {
        byte[] bufferLength = new byte[2]; // We use 2 bytes for short value.
        byte[] bufferData;
        short length; // Since we use short value, max length should be 32767.

        while (_readThreadStarted)
        {
            if (_socket.Receive(bufferLength) > 0)
            {
                // Get packet data length.
                length = BitConverter.ToInt16(bufferLength, 0);
                bufferData = new byte[length];

                // Get packet data.
                _socket.Receive(bufferData);

                // Handle packet.
                RecievablePacketHandler.Handle(new ReceivablePacket(bufferData));
            }
        }
    }

    public static void ChannelSend(SendablePacket packet)
    {
        if (SocketConnected())
        {
            // socket.Send(packet.GetSendableBytes());

            byte[] buffer = packet.GetSendableBytes();
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.SetBuffer(buffer, 0, buffer.Length);

            try
            {
                _socket.SendAsync(args);
            }
            catch (Exception)
            {
            }
        }
        else // Connection closed.
        {
            _unexpectedDisconnection = true;
            DisconnectFromServer();
            // Clear world instance values.
            if (WorldManager.Instance != null)
            {
                WorldManager.Instance.ExitWorld();
            }
            // Go to login screen.
            MainManager.Instance.LoadScene(MainManager.LOGIN_SCENE);
        }
    }

    private static bool SocketConnected()
    {
        // return !(socket.Poll(1000, SelectMode.SelectRead) && socket.Available == 0);
        return _socketConnected && _socket != null && _socket.Connected;
    }

    public static void DisconnectFromServer()
    {
        if (_socket != null && _socket.Connected)
        {
            _socket.Close();
        }
        _socketConnected = false;
        _readThreadStarted = false;

        // Clear stored variables.
        MainManager.Instance.SetAccountName(null);
        MainManager.Instance.SetCharacterList(null);
        MainManager.Instance.SetSelectedCharacterData(null);
    }

    private void OnApplicationQuit()
    {
        DisconnectFromServer();
    }

    public static bool IsForcedDisconnection()
    {
        return _forcedDisconnection;
    }

    public static void SetForcedDisconnection(bool value)
    {
        _forcedDisconnection = value;
    }

    public static bool IsUnexpectedDisconnection()
    {
        return _unexpectedDisconnection;
    }

    public static void SetUnexpectedDisconnection(bool value)
    {
        _unexpectedDisconnection = value;
    }

    // Dummy method to prevent console warning from UMA.
    internal void StartHost()
    {
        throw new NotImplementedException();
    }
}