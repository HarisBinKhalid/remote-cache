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
        NetworkStream stream = null;
        Byte[] bytes = new Byte[256];
        String data = null;
        Hashtable cache = null;
        HashTableData dataObject = null;
        static Server serverObj = new Server();
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
            catch
            {

            }
        }

        public void HandleDevice (object obj)
        {
            TcpClient client = (TcpClient)obj;
            serverObj.Initialize();
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
                            try
                            {
                                dataObject = ByteArrayToObject(bytes) as HashTableData;
                                functionCall = dataObject.getFuntion();
                                key = dataObject.getKey();
                                value = dataObject.getvalue();

                                Console.WriteLine("Val: " + functionCall + key + value);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                            }
                            switch (functionCall)
                            {
                                case 1:
                                    {
                                        if (!cache.Contains(key))
                                        {
                                            serverObj.Add(key, value);
                                        }
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
                    catch (Exception e)
                    {

                    }
            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
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
