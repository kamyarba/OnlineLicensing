using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using System.Text.Json.Nodes;
using System.Management;


namespace OnlineLicensing
{
    internal class Program
    {
        //static void Main(string[] args)
        //{
        //    Console.ReadLine();
        //}
        //static readonly HttpClient client2 = new HttpClient();

        static async Task Main()
        {
            HttpClient client = new HttpClient();


            string serialNumber = GetSerialNumber();
            string hardware_id = serialNumber;
            
            
            string license_key = "000"; // Read from loacal storage if exists

            Console.WriteLine("Choose a number: \n1: For activating the software using license code \n2: For checking a wrong serial number" +
                "\n3: For checking a correct serial number");
            string user_response = Console.ReadLine();

            if (user_response.ToLower() == "3")
            {
                license_key = "123456";
            }


            if (user_response.ToLower() == "2" || user_response.ToLower() == "3")
            {
                HttpResponseMessage response = await client.GetAsync($"https://ksoft.ir/api_test/?license_key={license_key}&hardware_id={hardware_id}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                // string responseBody = await client.GetStringAsync(uri);

                Console.WriteLine("Server response:\n"+responseBody);



                string json = responseBody;

                JsonNode jsonresponse = JsonNode.Parse(json);
                string Status = (string)jsonresponse["status"];
                if (Status == "inactive")
                {
                    Console.WriteLine("This app is not Active");
                }
                else
                {
                        Console.WriteLine("The app works!"); 
                }

            }


            if (user_response.ToLower() == "1")
            {
                Console.WriteLine("Hardware serial number:" + hardware_id);

                Console.WriteLine("Enter your license key[Correct serial is 123456]:");
                license_key = Console.ReadLine();
                Console.WriteLine($"Your liccense number:{license_key}");
                Console.WriteLine("Conecting to server....");

                var values = new Dictionary<string, string>
                {
                    { "license_key", license_key },
                    { "hardware_id", hardware_id }
                };
                // Encode the values as form parameters
                var content = new FormUrlEncodedContent(values);

                HttpResponseMessage response = await client.PostAsync("https://ksoft.ir/api_test/", content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                Console.WriteLine("Server response:\n" + responseBody);

                string json = responseBody;

                JsonNode jsonresponse = JsonNode.Parse(json);
                string valid = (string)jsonresponse["valid"];
                if (valid == "false")
                {
                    Console.WriteLine("The entered license code is not valid!!");
                }
                else
                {
                    Console.WriteLine("The app is registered.");
                    // Save action in local storage
                }

            }

            Console.WriteLine("\n\nPress any key to exit...");
            Console.ReadLine();
        }


        public static string GetSerialNumber()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                return queryObj["SerialNumber"].ToString();
            }

            return null;
        }

    }
}
