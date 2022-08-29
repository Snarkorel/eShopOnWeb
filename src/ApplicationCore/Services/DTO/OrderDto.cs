using System.Collections.Generic;

namespace Microsoft.eShopWeb.ApplicationCore.Services.DTO;
public class OrderDto
{
    public int Id { get; set; }
    public string ShippingAddress { get; set; }
    public List<ItemDto> Items { get; set; }
    public decimal FinalPrice { get; set; }
}
