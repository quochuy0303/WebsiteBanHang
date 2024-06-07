<<<<<<< HEAD
﻿namespace WebsiteBanHang.ViewModels;

public class VnPaymentResponseModel
{
    public bool Success { get; set; }
    public string PaymentMethod { get; set; }
    public string OrderDescription { get; set; }
    public string OrderId { get; set; }
    public string PaymentId { get; set; }
    public string TransactionId { get; set; }
    public string Token { get; set; }
    public string VnPayResponseCode { get; set; }
}

public class VnPaymentRequestModel
{
    public int OrderId { get; set; }
    public string FullName { get; set; }
    public string Description { get; set; }
    public double Amount { get; set; }
    public DateTime CreatedDate { get; set; }
=======
﻿namespace WebsiteBanHang.ViewModels
{
    public class VnPaymentResponseModel
    {
        public bool Success { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderDescription { get; set; }
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string TransactionId { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }
    }
    public class VnPaymentRequestModel
    {
        public int OrderId { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedDate { get; set; }
    }
>>>>>>> 3a1e4bbd70492c8f59afe5690fb62d3770da0b90
}
