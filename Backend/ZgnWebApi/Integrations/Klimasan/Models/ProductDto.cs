namespace ZgnWebApi.Integrations.Klimasan.Models
{
    public class GroupDto
    {
        public string? GroupCode { get; set; }
    }
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public string? GroupCode { get; set; }
    }
    public class InventoryDto
    {
        public int ProductId { get; set; }
        public string? SerialNumber { get; set; }
        public string? UnitQuantity { get; set; }
        public string? LocationName { get; set; }
    }
}
