using ClientSocket;
using System.Configuration;
using System.Threading;
using TableData;

namespace Main
{
    class main
    {
        static void Main()
        {

            //int port = int.Parse(ConfigurationManager.AppSettings["port"]);
            //client.Connect("127.0.0.1", port);
            //client.Add("1", "Text");
            //client.SendData("1");
            Client client = new Client();
            Boolean check = true;
            int port = int.Parse(ConfigurationManager.AppSettings["port"]);
            client.Connect("127.0.0.1", port);

            while (true)
            {
                String menuChoice = "";
                String key = "";
                object value = "";

                Console.WriteLine("Menu:");
                Console.WriteLine("1. Add.");
                Console.WriteLine("2. Remove.");
                Console.WriteLine("3. Get.");
                Console.WriteLine("4. Clear.");
                Console.WriteLine("5. Dispose.");
                Console.Write("Selection: ");
                menuChoice = Console.ReadLine();

                switch(menuChoice)
                {
                    case "1":
                        {
                            Console.WriteLine("Add Menu:");
                            Console.Write("Enter key: ");
                            key = Console.ReadLine();
                            Console.Write("Enter value: ");
                            value = Console.ReadLine();
                            client.Add(key, value);
                            break;
                        }
                    case "2":
                        {
                            Console.WriteLine("Remove Menu:");
                            Console.Write("Enter key: ");
                            key = Console.ReadLine();
                            client.Remove(key);
                            Console.WriteLine("Item removed");
                            break;
                        }
                    case "3":
                        {
                            Console.WriteLine("Get Menu:");
                            Console.Write("Enter key: ");
                            key = Console.ReadLine();
                            try
                            {
                                value = client.Get(key);
                                Console.WriteLine("Value: " + value);
                            }
                            catch
                            {
                                Console.WriteLine("Item not found");
                            }
                            break;
                        }
                    case "4":
                        {
                            client.Clear();
                            Console.WriteLine("Items cleared");
                            break;
                        }
                    case "5":
                        {
                            client.Dispose();
                            Console.WriteLine("Items disposed");
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Wrong input");
                            break;
                        }
                            
                 }
                Console.WriteLine("");
                
            }
        }
    }
}
