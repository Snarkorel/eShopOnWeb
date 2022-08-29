using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.eShopWeb.ApplicationCore.Services.DTO;

namespace Microsoft.eShopWeb.ApplicationCore.Interfaces;
public interface IOrderProcessorService
{
    Task ReserveOrderItems(IEnumerable<ItemDto> items);

    Task ProcessOrderDelivery(OrderDto order);
}
