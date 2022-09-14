using System.Configuration;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TableData;


namespace ServerSocket
{
    class Server : ICache
    {
        [ThreadStatic]
        private static Hashtable cache = null;
        private static Server serverObj = new Server();
        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);
                return obj;
            }
        }
        public static void Main()
        {
            while (true)
            {
                TcpListener server = null;
                try
                {
                    Int32 port = int.Parse(ConfigurationManager.AppSettings["port"]);
                    IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                    server = new TcpListener(localAddr, port);
                    server.Start();

                    while (true)
                    {
                        Console.Write("Waiting for a connection... ");
                        TcpClient client = server.AcceptTcpClient();
                        Console.WriteLine("Connected!");

                        Thread t = new Thread(new ParameterizedThreadStart(serverObj.HandleDevice));
                        t.Start(client);
                    }
                }
                catch{}
            }
        }
        public void HandleDevice(object obj)
        {
            NetworkStream stream = null;
            Byte[] bytes = new Byte[256];
            String data = null;
            HashTableData dataObject = null;
            TcpClient client = (TcpClient)obj;
            int functionCall = 0;
            String key = "0";
            object value = 0;
            String send = "";

            int i;
            try
            {
                stream = client.GetStream();
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    if (cache == null)
                    {
                        serverObj.Initialize();
                    }
                    try
                    {
                        dataObject = ByteArrayToObject(bytes) as HashTableData;
                        functionCall = dataObject.getFuntion();
                        key = dataObject.getKey();
                        value = dataObject.getvalue();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                    switch (functionCall)
                    {
                        case 1:
                            {
                                byte[] sendback = null;
                                if (!cache.Contains(key))
                                {
                                    serverObj.Add(key, value);
                                    object returnObj = "Pass";
                                    sendback = ObjectToByteArray(returnObj);
                                }
                                else
                                {
                                    object returnObj = "Exists";
                                    sendback = ObjectToByteArray(returnObj);
                                }
                                stream.Write(sendback, 0, sendback.Length);
                                break;
                            }
                        case 2:
                            {
                                serverObj.Remove(key);
                                break;
                            }
                        case 3:
                            {
                                byte[] sendback = null;
                                if (cache.Contains(key))
                                {
                                    sendback = ObjectToByteArray(serverObj.Get(key));
                                }
                                else
                                {
                                    object returnObj = "Not Found";
                                    sendback = ObjectToByteArray(returnObj);
                                }
                                stream.Write(sendback, 0, sendback.Length);
                                break;
                            }
                        case 4:
                            {
                                serverObj.Clear();
                                break;
                            }
                        case 5:
                            {
                                serverObj.Dispose();
                                break;
                            }
                    }
                }
            }
            catch
            {
                Main();
            }
        }
        public void Initialize()
        {
            cache = new Hashtable();
        }
        public void Add(string key, object value)
        {
            cache.Add(key, value);
        }
        public void Remove(string key)
        {
            cache.Remove(key);
        }
        public object Get(string key)
        {
            return cache[key];
        }
        public void Clear()
        {
            cache.Clear();
        }
        public void Dispose()
        {
            cache = null;
            GC.Collect();
        }
    }
}
