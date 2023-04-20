namespace SpectruMineAPI.Controllers.PaymentDTO;

public class PaymentReciveQuery
{
    public string invoice_id { get; set; } = null!;
    public string status { get; set; } = null!;
    public string amount { get; set; } = null!;
    public string currency { get; set; } = null!;
    public string order_id { get; set; } = null!;
    public CustomFields custom_fields { get; set; } = null!;
    public int type { get; set; }
    public string pay_time { get; set; } = null!;
    public string pay_service { get; set; } = null!;
    public string payer_details { get; set; } = null!;
    public int code { get; set; }
    public string credited { get; set; } = null!;
}
public class CustomFields
{
    public string userId { get; set; } = null!;
    public string productId { get; set; } = null!;
}
