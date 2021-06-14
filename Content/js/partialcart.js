$(".btnDeleteFromCartPartial").on("click", async function () {
    var productId = $(this).val();
    $.ajax({
        type: 'Get',
        url: '/Cart/Remove/',
        dataType: 'Json',
        data: { "id": productId },
        success: function (control) {
            if (control == 1) {

                $.ajax({
                    type: 'Post',
                    url: '/Cart/UpdateCart/',
                    dataType: 'Json',
                    success: function (cartCount) {
                        $("#cartCount").text("Sepet (" + cartCount + ")");

                    }
                });

                $.ajax({
                    type: 'Get',
                    url: '/Cart/_PartialCart/',
                    success: function (data) {
                        if (data) {
                            $('#partialCart').html("");
                            $("#partialCart").html(data);
                        }

                    }
                });

                Swal.fire({
                    position: 'top',
                    icon: 'warning',
                    title: 'Ürün sepetten kaldırıldı.',
                    showConfirmButton: false,
                    timer: 1000
                });

            }

        }
    });
});

$(".cartItemCountPartial").bind('keyup mouseup', async function () {
    var unit = $(this).val();
    var productID = $(this).attr("name");
    var unitprice = $(this).siblings("input:hidden").val();
    var totalprice = document.getElementById(productID);
    if (unit > 0) {
        $.ajax({
            type: 'Post',
            url: '/Cart/AddToCart/',
            data: { "unit": unit, "productID": productID, "changed": true },
            dataType: 'Json',
            success: function (control) {
                if (control == 1) {

                    $.ajax({
                        type: 'Post',
                        url: '/Cart/UpdateCart/',
                        dataType: 'Json',
                        success: function (cartCount) {
                            $("#cartCount").text("Sepet (" + cartCount + ")");

                        }
                    });
                    var unitpricenumber = unitprice.replace(/,/g, ".");
                    var newprice = (unit * unitpricenumber);
                    totalprice.innerHTML = "$" + newprice.toFixed(2);

                    $.ajax({
                        type: 'Get',
                        url: '/Cart/GetNewTotalPrice/',
                        success: function (total) {
                            $("#totalpricePartial").text("$"+total);
                        }
                    });

                }
                else {
                    Swal.fire({
                        position: 'top',
                        icon: 'warning',
                        title: 'Stok sayısı aşıldı.',
                        showConfirmButton: false,
                        timer: 1000
                    })
                    $.ajax({
                        type: 'Get',
                        url: '/Cart/_PartialCart/',
                        success: function (data) {
                            if (data) {
                                $('#partialCart').html("");
                                $("#partialCart").html(data);
                            }

                        }
                    });

                }
            }
        });
    }
    else {
        $(this).val(1);
    }

});