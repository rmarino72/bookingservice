namespace bookingservice.bo;

public class OrderRequest
{
    public required int UserId { set; get; }
    public required List<OrderRequestDetail> Details { set; get; }
}