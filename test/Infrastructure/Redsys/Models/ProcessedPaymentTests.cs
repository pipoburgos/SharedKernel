using SharedKernel.Infrastructure.Redsys;

namespace SharedKernel.Integration.Tests.Redsys.Models;

public class ProcessedPaymentTests
{
    [Fact]
    public void ProcessedPayment_ShouldNotHave_AnInvalidState()
    {
        var processedPayment = new ProcessedPayment(Build.PaymentResponse(true), false);
        processedPayment.IsPaymentPerformed.Should().BeFalse();
    }

    [Fact]
    public void ProcessedPayment_ShouldBePerformed_IfSignatureIsValidAndOrderHasBeenPaid()
    {
        var processedPayment = new ProcessedPayment(Build.PaymentResponse(true), true);
        processedPayment.IsPaymentPerformed.Should().BeTrue();
    }

    [Fact]
    public void ProcessedPayment_ShouldNotBePerformed_IfSignatureIsValidAndOrderHasNotBeenPaid()
    {
        var processedPayment = new ProcessedPayment(Build.PaymentResponse(false), true);
        processedPayment.IsPaymentPerformed.Should().BeFalse();
    }
}