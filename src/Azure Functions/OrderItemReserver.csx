#r "Newtonsoft.Json"

using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

private static string logicAppUri = "DATA-REMOVED";
private static HttpClient httpClient = new HttpClient();

public class ItemDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
}

public static void Run(string orderQueueItem, out string outputBlob, ILogger log)
{
    log.LogInformation($"C# ServiceBus queue trigger function processed message: {orderQueueItem}");

    //Checking that we got valid data before adding order to a blob storage
    try {
        var orderItems = JsonConvert.DeserializeObject<IEnumerable<ItemDto>>(orderQueueItem);
        log.LogInformation($"Received {orderItems.Count()} order items");
    }
    catch (Exception)
    {
        log.LogInformation("Failed to deserialize JSON, triggering logic app that will email the details");
        httpClient.PostAsync(logicAppUri, new StringContent(orderQueueItem, Encoding.UTF8, "application/json"));
    }
    finally
    {
        outputBlob = orderQueueItem;
    }
        
}
