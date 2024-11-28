using FastEndpoints;

namespace Payment.Api.Endpoints;
public class ProcessPaymentEndpoint : Endpoint<ProcessPaymentRequest, ProcessPaymentResponse>
{
  public override void Configure()
  {
    Post("/ProcessPayment");
  }

  public override async Task HandleAsync(ProcessPaymentRequest request, CancellationToken cancellationToken)
  {
    var response = await ProcessPaymentAsync();
    await SendAsync(response);
  }

  //placeholder for actual implementation
  private Task<ProcessPaymentResponse> ProcessPaymentAsync()
  {
    return Task.FromResult(new ProcessPaymentResponse { PaymentId = "1" });
  }
}



public class ProcessPaymentRequest
{
  public string PaymentMethod { get; set; }
  public decimal Amount { get; set; }
  public string Currency { get; set; }
  public string CardNumber { get; set; }
  public string ExpiryDate { get; set; }
  public string CVV { get; set; }
}

public class ProcessPaymentResponse
{
  public string PaymentId { get; set; }
}
