namespace Play.Operation.Api.DTO;

public class OperationStatusDto
{
    public Guid CorrelationId { get; set; }
    public string Status { get; set; }
    public string Reason { get; set; }
}