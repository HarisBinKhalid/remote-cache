using Amazon.Runtime;
using System.Collections;
using System.Configuration;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using TableData;

namespace ClientSocket
{
    public class Client : ICache
    {
        TcpClient client = null;
        HashTableData dataObject = null;
        NetworkStream stream;


        public void Connect(String server, int port)
        {
            try
            {
                client = new TcpClient(server, port);
            }
            catch (SocketException e)
            {
                throw new Exception(e.Message);
            }
        }

        //public void SendData(String message)
        //{
        //    try
        //    {
        //        Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
        //        NetworkStream stream = client.GetStream();
        //        stream.Write(data, 0, data.Length);

        //        Console.WriteLine("Sent: {0}", message);
        //        data = new Byte[256];
        //        String responseData = String.Empty;
        //        Int32 bytes = stream.Read(data, 0, data.Length);
        //        responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

        //        Console.WriteLine("Received: {0}", responseData);

        //    }
        //    catch (ArgumentNullException e)
        //    {
        //        Console.WriteLine("ArgumentNullException: {0}", e);
        //    }
        //    catch (SocketException e)
        //    {
        //        Console.WriteLine("SocketException: {0}", e);
        //    }

        //    Console.WriteLine("\n Press Enter to continue...");
        //    Console.Read();
        //}

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

        public void Initialize()
        {
            throw new NotImplementedException();
        }
        public void Add(string key, object value)
        {
            dataObject = new HashTableData(1, key, value);
            Byte[] send = ObjectToByteArray(dataObject);
            stream = client.GetStream();
            stream.Write(send, 0, send.Length);
        }
        public void Remove(string key)
        {
            dataObject = new HashTableData(2, key, null);
            Byte[] send = ObjectToByteArray(dataObject);
            stream = client.GetStream();
            stream.Write(send, 0, send.Length);
        }
        public object Get(string key)
        {
            dataObject = new HashTableData(3, key, null);
            Byte[] send = ObjectToByteArray(dataObject);
            stream = client.GetStream();
            stream.Write(send, 0, send.Length);

            int i;
            Object val = "";
            Byte[] bytes = new Byte[256];
            object objectValue = null;
            try
            {
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    objectValue = ByteArrayToObject(bytes);
                    return objectValue;
                }
            }
            catch (Exception e)
            {
                return e;
            }
            return 0;
        }
        public void Clear()
        {
            dataObject = new HashTableData(4, null, null);
            Byte[] send = ObjectToByteArray(dataObject);
            stream = client.GetStream();
            stream.Write(send, 0, send.Length);
        }

        public void Dispose()
        {
            dataObject = new HashTableData(5, null, null);
            Byte[] send = ObjectToByteArray(dataObject);
            stream = client.GetStream();
            stream.Write(send, 0, send.Length);
        }
    }
}
