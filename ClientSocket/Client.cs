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
                throw e;
            }
        }
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
        public object getreturningvalue()
        {
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

            object returnvalue = getreturningvalue();
            object checkvalue = "Exists";
            if (returnvalue.Equals(checkvalue))
            {
                throw new Exception((string?)returnvalue);
            }
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

            object returnvalue = getreturningvalue();
            return returnvalue;
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
