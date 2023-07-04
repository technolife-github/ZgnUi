using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Dynamic;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Net.Sockets;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using ZgnWebApi.Core.Utilities.Results;
using ZgnWebApi.Core.Utilities.Security;
using static ZgnWebApi.Integrations.BlueBotics.Models.BlueBotics.Responses;
using System.IO;
using System.Threading;

namespace ZgnWebApi.BackgroundWorkers;
public class ZgnWebSocketUser
{
    public string ConnectionType { get; set; }
    public string SerialNumber { get; set; }
    public WebSocket WebSocket { get; set; }

}
public class TcpWorkerService : BackgroundService
{
    private static readonly ConcurrentDictionary<string, ZgnWebSocketUser> _sockets = new();
    private readonly List<string> DeviceSerials;
    private readonly IConfiguration Configuration;
    private readonly ITokenHelper _tokenHelper;
    public TcpWorkerService(IConfiguration configuration, ITokenHelper tokenHelper)
    {
        Configuration = configuration;
        DeviceSerials = Configuration.GetSection("DeviceSerialNumbers").Get<List<string>>();
        _tokenHelper = tokenHelper;
    }
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            string ipAddress = Configuration.GetValue<string>("TcpWorker:IpAddress") ?? "127.0.0.1";
            int port = Configuration.GetValue<int>("TcpWorker:Port");

            // Create a TCP listener
            TcpListener listener = new TcpListener(IPAddress.Any, port);

            // Start listening for incoming connections
            listener.Start();

            // Accept a client connection
            while (true)
            {
                try
                {
                    TcpClient client = listener.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(StartRead, client);
                }
                catch (Exception) { }
            }
        });
    }
    public void StartRead(object clientObj)
    {
        var client = (TcpClient)clientObj;

        while (true)
        {
            try
            {
                // Get the client stream for reading and writing
                NetworkStream stream = client.GetStream();
                string response = "";
                response = Process(stream);
                // Convert the response to bytes*/
                byte[] responseData = Encoding.ASCII.GetBytes(response);
                // Send the response back to the client
                stream.Write(responseData, 0, responseData.Length);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Exit") break;
            }

        }
        client.Close();
    }
    public string Process(NetworkStream stream)
    {
        string response = "";
        try
        {


            // Buffer for reading data
            byte[] buffer = new byte[1024];

            // Read the incoming data
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            // Convert the data to a string
            string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Received: "+dataReceived);
            dynamic someData = JsonConvert.DeserializeObject(dataReceived);
            if (someData.Type == "GetDate")
            {
                someData.Value = DateTime.Now.ToString("yyyy-MM-dd");
                someData.Status = true;
                someData.Message = "Current date is getted";
                response = JsonConvert.SerializeObject(someData);
            }
            else if (someData.Type == "Login")
            {
                if (DeviceSerials.Contains(someData.DeviceSerial.ToString()) && DateTime.Now.ToString("yyyy-MM-dd") == someData.Password.ToString())
                {
                    var socketUser = new ZgnWebSocketUser();
                    socketUser.SerialNumber = someData.DeviceSerial.ToString();
                    var token = _tokenHelper.CreateWebSocketToken(socketUser);
                    dynamic result = new ExpandoObject();
                    result.Type = "Login";
                    result.Token = token.Token;
                    result.Status = true;
                    result.Message = "Login Successfuly";
                    response = JsonConvert.SerializeObject(result);

                }
            }
            else if (someData.Type == "ReadBarcode")
            {
                //todo: tokenvalidate
                dynamic result = new ExpandoObject();
                result.Type = "ReadBarcode";
                result.Status = true;
                result.Message = "Barcode readed.";
                response = JsonConvert.SerializeObject(result);
            }
            if (someData.Type == "Exit")
            {
                throw new Exception("Exit");
            }

            // Close the client connection
        }
        catch (Exception ex)
        {
            dynamic result = new ExpandoObject();
            result.Type = "Exception";
            result.Status = false;
            result.Message = "Error: " + ex.Message;
            response = JsonConvert.SerializeObject(result);
        }
        Console.WriteLine("Response: " + response);
        return response;
    }
}