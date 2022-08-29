using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Services.DTO;

namespace Microsoft.eShopWeb.ApplicationCore.Services;
public class OrderProcessorService : IOrderProcessorService
{
    private readonly HttpClient _httpClient;
    //TODO: move things below to config
    private const string _uri = "https://cloudx-function.azurewebsites.net/api/DeliveryOrderProcessor?code=DATA-REMOVED";
    private const string ServiceBusConnectionString = "Endpoint=sb://cloudx-msg.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=DATA-REMOVED";
    private const string QueueName = "orderitemsqueue";

    public OrderProcessorService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task ReserveOrderItems(IEnumerable<ItemDto> items)
    {
        var data = JsonSerializer.Serialize(items);

        await using var client = new ServiceBusClient(ServiceBusConnectionString);
        var message = new ServiceBusMessage(data);
        var sender = client.CreateSender(QueueName);

        await sender.SendMessageAsync(message);
    }

    public async Task ProcessOrderDelivery(OrderDto order)
    {
        var json = JsonSerializer.Serialize(order);

        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var request = new HttpRequestMessage(HttpMethod.Post, _uri);
        request.Content = content;
        
        await _httpClient.SendAsync(request);
    }
}
