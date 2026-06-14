using MySql.Data.MySqlClient; 
using System;
using System.Data;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FraudAlertSystem
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("🚨 FinTech System: Scanning Live Database...\n");

            //  Note for Reviewers: Please insert your local MySQL password below to test the connection
            string connectionString = "Server=localhost;Database=yourDatabaseName;Uid=root;Pwd=yourPassword;";

            using MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                await connection.OpenAsync();

                
                string query = "SELECT type, amount, oldbalanceOrg, newbalanceOrig FROM transactions WHERE type = 'TRANSFER' AND newbalanceOrig = 0 LIMIT 5"; // u can change the limit on your self

                using MySqlCommand cmd = new MySqlCommand(query, connection);
                using System.Data.Common.DbDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync()) 
                {
                   
                    string dbType = reader.GetString("type");
                    decimal dbAmount = reader.GetDecimal("amount");
                    decimal dbOldBalance = reader.GetDecimal("oldbalanceOrg");
                    decimal dbNewBalance = reader.GetDecimal("newbalanceOrig");

                    string dynamicTransaction = $"Type: {dbType}, Amount: ${dbAmount}, Old Balance: ${dbOldBalance}, New Balance: ${dbNewBalance}";

                    Console.WriteLine($" Suspicious Activity Found in DB: {dynamicTransaction}");
                    Console.WriteLine(" Sending dynamic data to AI for security check...\n");

                    
                    await SendToOllamaAI(dynamicTransaction);
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n❌ Database Connection Error: " + ex.Message);
            }

            Console.ReadLine();
        }

       
        static async Task SendToOllamaAI(string transactionData)
        {
            string url = "http://localhost:11434/api/generate";
            string promptText = $"You are a FinTech Security AI. Analyze this transaction: {transactionData}. Is this likely a fraud? Start your answer with [FRAUD DETECTED] or [SAFE] and give a short reason.";

            var requestData = new { model = "llama3", prompt = promptText, stream = false };
            string jsonPayload = JsonSerializer.Serialize(requestData);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            using HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string responseString = await response.Content.ReadAsStringAsync();
                using JsonDocument doc = JsonDocument.Parse(responseString);
                string aiMessage = doc.RootElement.GetProperty("response").GetString();

                Console.WriteLine("=========================================");
                Console.WriteLine("🤖 AI INSPECTOR RESULT:");
                Console.WriteLine(aiMessage);
                Console.WriteLine("=========================================");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n❌ Error connecting to AI: " + ex.Message);
            }
        }
    }
}