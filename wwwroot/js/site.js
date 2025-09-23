$(document).ready(function () {
  $("form[action='/Cart/AddToCart']").submit(function (e) {
    e.preventDefault();
    $.ajax({
      url: $(this).attr("action"),
      type: "POST",
      data: $(this).serialize(),
      success: function (response) {
        if (response.success) {
          toastr.success(response.message);
          $(".cart-badge").text(response.itemCount); // Cập nhật số lượng trên biểu tượng
        } else {
          toastr.error(response.message);
        }
      },
      error: function () {
        toastr.error("Đã xảy ra lỗi!");
      },
    });
  });
});
