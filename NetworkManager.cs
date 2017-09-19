using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Starts and closes the sockets and treats different processes to process the transmitted commands
/// </summary>
public class NetworkManager : MonoBehaviour
{
    private Queue<string> _cmdq;
    private bool _running;
    private Thread _thread;
    private TcpListener _tcpListener;
    private TcpListener _sensorRequestListener;
    private UdpClient _udpListener;
    private UdpClient _udpBroadcastListener;

    private Thread _udpBroadcastThread;

    private readonly int _port = 13000;
    private readonly int _broadcastPort = 15000;

    private IPAddress _localAddr;

    private String _commandString;

    public SensorManager SensorManager;

    private void Awake()
    {
        SceneManager.activeSceneChanged += HandleSceneChange;
    }

    // Use this for initialization
    private void Start()
    {
        _commandString = "";

        _localAddr = GetIPAddress();
        _cmdq = new Queue<string>();
        _running = true;
        ThreadStart ts = RunServer;
        _thread = new Thread(ts);
        _thread.Start();

        ThreadStart tsRequest = RunSensorRequestServer;

        Thread thread2 = new Thread(tsRequest);
        thread2.Start();

        RunUDPBroadcast();

        RunUDPServer();
        
    }


    private void RunUDPBroadcast()
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, _broadcastPort);
        _udpBroadcastListener = new UdpClient(endPoint);

        try
        {
            Debug.Log("Receiving UDP Broadcast Packets...");

            _udpBroadcastListener.BeginReceive(ReceiveBroadcast, _udpBroadcastListener);

        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
    
    // Invoked if a broadcast is received
    private void ReceiveBroadcast(IAsyncResult result)
    {
        const String broadcastRequestMessage = "DISCOVER_SIMULATOR_REQUEST";
        const String broadcastResponseMessage = "DISCOVER_SIMULATOR_RESPONSE";

        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, _broadcastPort);

        UdpClient listener = (UdpClient)result.AsyncState;

        Debug.Log("Ist bereit Broadcast zu receiven");

        byte[] receivedPacket = listener.EndReceive(result, ref RemoteIpEndPoint);

        Debug.Log("Irgend nen Broadcast erhalten");

        string message = Encoding.ASCII.GetString(receivedPacket);

        if (message.Equals(broadcastRequestMessage))
        {
            try
            {
                Socket sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                byte[] respondMessage = Encoding.ASCII.GetBytes(broadcastResponseMessage);

                sendSocket.SendTo(respondMessage, RemoteIpEndPoint);

                sendSocket.Close();

                Debug.Log("Serverinformationen an Broadcaster verschickt.");

                // Neue Iteration; es wird wieder auf einen neuen Broadcast gewartet
                _udpBroadcastListener.BeginReceive(ReceiveBroadcast, _udpBroadcastListener);
            }
            catch (Exception e)
            {
                Debug.Log("Could not respond to broadcast, Error while trying to respond: "
                          + e.ToString());
            }
        }
        else
        {
            // Neue Iteration; es wird wieder auf einen neuen Broadcast gewartet
            _udpBroadcastListener.BeginReceive(ReceiveBroadcast, _udpBroadcastListener);
        }

        
    }

    

    //
    public String GetCommandString()
    {
        return _commandString;
    }

    // Runs the UDP Server/Listener
    private void RunUDPServer()
    {

        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, _port);
        _udpListener = new UdpClient(endPoint);

        try
        {
            _udpListener.BeginReceive(OnReceived, _udpListener);
            

            Debug.Log("Receiving UDP Packets...");
           
        }
        catch (Exception e)
        {
            Debug.Log("Fehler beim Empfangen der UDP Nachricht: " + e.Message);
        }
    }

    // Async
    private void OnReceived(IAsyncResult result)
    {

        IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, _port);

        UdpClient listener = (UdpClient) result.AsyncState;

        byte[] receivedPacket = listener.EndReceive(result, ref RemoteIpEndPoint);


        _udpListener.BeginReceive(OnReceived, _udpListener);

        String receivedData = Encoding.ASCII.GetString(receivedPacket);

        _commandString = receivedData;


    }

// Runs the TCP Server/Listener
    private void RunServer()
    {
        _tcpListener = null;

        try
        {
            // Set the TcpListener on _port 13000.
            var port = 13000;
            var localAddr = GetIPAddress();

            // TcpListener server = new TcpListener(_port);
            _tcpListener = new TcpListener(localAddr, port);

            // Start listening for client requests.
            _tcpListener.Start();

            // Buffer for reading data
            var bytes = new byte[256];
            string data = null;

            // Enter the listening loop.
            while (_running)
            {
                Debug.Log("TCP Socket initialized... ");

                 // Perform ablocking call to accept requests.
                // You could also user server.AcceptSocket() here.
                var client = _tcpListener.AcceptTcpClient();


                data = null;

                // Get a stream object for reading and writing
                var stream = client.GetStream();

                int i;

            // Loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
            // Translate data bytes to an ASCII string.
                    data = Encoding.ASCII.GetString(bytes, 0, i);
            // Debug.Log("Received: " + data);

            // Process the data sent by the client.
                    data.ToUpper();

                    var msg = Encoding.ASCII.GetBytes(data);

                    // Enqueues the new command


                    lock (_cmdq)
                    {
                        _cmdq.Enqueue(data);
                    }

                                }
                // Flush Stream
                stream.Flush();

                // Shutdown and end connection
                client.Close();
            }
        }
        catch (SocketException e)
        {
            Debug.Log("Socket Exception: " + e.Message);
        }
        finally
        {
            // Stop listening for new clients.
            _running = false;

            if (_tcpListener != null)
                _tcpListener.Stop();
        }

        Debug.Log("Verbindung geschlossen");
    }


    private void RunSensorRequestServer()
    {
        _sensorRequestListener = null;

        try
        {
            var port = 13001;
            var localAddr = GetIPAddress();

            _sensorRequestListener = new TcpListener(localAddr, port);

            _sensorRequestListener.Start();

           

            while (_running)
            {
                Debug.Log("Warte auf Sensorrequest Anfrage ...");
                var client = _sensorRequestListener.AcceptTcpClient();
                Debug.Log("SensorRequest erhalten!");

                SensorRequestHandler srHandler = new SensorRequestHandler();

                srHandler.HandleClient(client, SensorManager);
            }

         
            
        }
        catch (SocketException e)
        {
            Debug.Log("Socket Exception: " + e.Message);
        }
        finally
        {
            // Stop listening for new clients.
            _running = false;

            if (_sensorRequestListener != null)
                _sensorRequestListener.Stop();
        }
    }

    private class SensorRequestHandler
    {
        private TcpClient _client;
        private SensorManager _sensorManager;

        public void HandleClient(TcpClient client, SensorManager sensorManager)
        {
            _client = client;
            _sensorManager = sensorManager;
            Thread thread = new Thread(StartHandle);
            thread.Start();

            
        }

        private void StartHandle()
        {

            NetworkStream stream = _client.GetStream();

            int i;
            Byte[] bytes = new byte[256];
            String data = null;
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = Encoding.ASCII.GetString(bytes, 0, i);
                data = data.ToUpper();

                Debug.Log("SensorRequest-Code: " + data);

                byte[] responseMsg = Encoding.ASCII.GetBytes(_sensorManager.GetSensorData(data) + "\n");

                stream.Write(responseMsg, 0, responseMsg.Length);
                Debug.Log("Antwort: " + _sensorManager.GetSensorData(data) + " wurde versandt.");
            }
        }
    }

    public IPAddress GetIPAddress()
    {
        if (!NetworkInterface.GetIsNetworkAvailable())
            return null;

        var host = Dns.GetHostEntry(Dns.GetHostName());

        return host
            .AddressList
            .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
    }

    private void OnApplicationQuit()
    {
        // stop listening thread
        
        _udpListener.Close();
        _udpBroadcastListener.Close();
        _tcpListener.Stop();
        _sensorRequestListener.Stop();

        StopListening();

        // wait for listening thread to terminate (max. 500ms)
        _thread.Join(500);
    }

    // Benutzt um die Verbindungen im Falle eines Szenewechsels zu schließen
    private void HandleSceneChange(Scene previousScene, Scene newScene)
    {
        if (newScene.buildIndex == 0)
        {
            Debug.Log("Connection Close Wird aufgerufen");
            _udpListener.Close();
            _udpBroadcastListener.Close();
            _tcpListener.Stop();
            _sensorRequestListener.Stop();
        }

    }

    public void StopListening()
    {
        _running = false;
    }

    public void AddToCommandQueue(string cmd)
    {
        _cmdq.Enqueue(cmd);
    }

    public void RemoveFromCommandQueue()
    {
        _cmdq.Dequeue();
    }

    public Queue<string> GetCommandQueue()
    {
        return _cmdq;
    }

    public int getPort()
    {
        return _port;
    }
}
