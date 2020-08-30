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
    private static Thread readThread;
    private static bool readThreadStarted = false;

    // For socket write.
    private static Socket socket;
    private static bool socketConnected = false;

    // Send to login screen message status.
    public static bool forcedDisconnection = false;
    public static bool unexpectedDisconnection = false;

    private void OnApplicationQuit()
    {
        DisconnectFromServer();
    }

    public static void DisconnectFromServer()
    {
        if (socket != null && socket.Connected)
        {
            socket.Close();
        }
        socketConnected = false;
        readThreadStarted = false;

        // Clear stored variables.
        MainManager.Instance.accountName = null;
        MainManager.Instance.characterList = null;
        MainManager.Instance.selectedCharacterData = null;
    }

    // Best to call this only once per login attempt.
    public static bool ConnectToServer()
    {
        if (socketConnected = false || socket == null || !socket.Connected)
        {
            ConnectSocket();
        }
        return SocketConnected();
    }

    private static void ConnectSocket()
    {
        try
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IAsyncResult result = socket.BeginConnect(NetworkConfigurations.SERVER_IP, NetworkConfigurations.SERVER_PORT, null, null);
            bool success = result.AsyncWaitHandle.WaitOne(NetworkConfigurations.TIMEOUT_DELAY, true);

            if (!success)
            {
                socketConnected = false;
                socket.Close();
            }
            else
            {
                if (socket.Connected)
                {
                    socketConnected = true;
                    // Start Receive thread.
                    readThreadStarted = true;
                    readThread = new Thread(new ThreadStart(ChannelRead));
                    readThread.Start();
                }
                else
                {
                    socketConnected = false;
                    socket.Close();
                }
            }
        }
        catch (SocketException)
        {
            socketConnected = false;
            readThreadStarted = false;
        }
    }

    private static void ChannelRead()
    {
        byte[] bufferLength = new byte[2]; // We use 2 bytes for short value.
        byte[] bufferData;
        short length; // Since we use short value, max length should be 32767.

        while (readThreadStarted)
        {
            if (socket.Receive(bufferLength) > 0)
            {
                // Get packet data length.
                length = BitConverter.ToInt16(bufferLength, 0);
                bufferData = new byte[length];

                // Get packet data.
                socket.Receive(bufferData);

                // Handle packet.
                RecievablePacketHandler.Handle(new ReceivablePacket(Encryption.Process(bufferData))); // Decrypt
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
                socket.SendAsync(args);
            }
            catch (Exception)
            {
            }
        }
        else // Connection closed.
        {
            unexpectedDisconnection = true;
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
        return socketConnected && socket != null && socket.Connected;
    }

    // Dummy method to prevent console warning from UMA.
    internal void StartHost()
    {
        throw new NotImplementedException();
    }
}