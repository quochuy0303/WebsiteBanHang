using WebsiteBanHang.ViewModels;

namespace WebsiteBanHang.Services
{
    public interface IVnpayService
    {
        string CreatePaymentUrl(HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExcute(IQueryCollection collections);
    }
}
