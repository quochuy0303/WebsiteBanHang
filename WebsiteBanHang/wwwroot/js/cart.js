// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var cart = {
    init: function () {
        cart.regEvents();
    },
    regEvents: function () {
        $('#btnContinue').off('click').on('click', function () {
            window.location.href = "/san-pham";
        });
        $(document).ready(function () {
            $('#btnPayment').click(function () {
                $.ajax({
                    url: '/Cart/Payment',
                    type: 'POST',
                    dataType: 'json',
                    success: function (response) {
                        if (response.status) {
                            // Thanh toán thành công, hiển thị thông báo và xóa giỏ hàng
                            alert(response.message);
                            // Redirect hoặc thực hiện các hành động khác sau khi thanh toán thành công
                            window.location.href = '/'; // Redirect đến trang chủ
                        } else {
                            // Hiển thị thông báo lỗi nếu có lỗi xảy ra
                            alert('Error: ' + response.message);
                        }
                    },
                    error: function (xhr, status, error) {
                        // Xử lý lỗi khi gửi yêu cầu Ajax
                        alert('Error sending payment request.');
                    }
                });
            });
        });

        $(document).ready(function () {
            $('#btnVnPayPayment').click(function () {
                // Gọi action "VnPayPayment" trên controller khi click vào nút "Thanh toán VnPay"
                $.ajax({
                    url: '/Cart/VnPayPayment',
                    type: 'POST',
                    dataType: 'json',
                    success: function (response) {
                        if (response.status) {
                            // Nếu lấy được URL thanh toán từ server, chuyển hướng tới trang thanh toán VnPay
                            window.location.href = response.paymentUrl;
                        } else {
                            // Hiển thị thông báo lỗi nếu có lỗi xảy ra
                            alert('Lỗi: ' + response.message);
                        }
                    },
                    error: function (xhr, status, error) {
                        // Xử lý lỗi khi gửi yêu cầu Ajax
                        alert('Lỗi khi gửi yêu cầu thanh toán VnPay.');
                    }
                });
            });
        });

        $('#btnDeleteAll').off('click').on('click', function () {
            $.ajax({
                url: '/Cart/DeleteAll',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            });
        });

        $('.btn-delete').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = $(this).data('id');
            $.ajax({
                url: '/Cart/Delete',
                data: { id: productId },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status == true) {
                        window.location.href = "/gio-hang";
                    }
                }
            });
        });

    }
}
cart.init();
