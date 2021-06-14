$("#btnAddToCart").on("click", async function () {
    var unit = $("#unit").val();
    var productID = $("#productID").val();
    if (unit > 0) {
        $.ajax({
            type: 'Post',
            url: '/Cart/AddToCart/',
            data: { "unit": unit, "productID": productID },
            dataType: 'Json',
            success: function (control) {
                if (control == 1) {
                    post = true;
                    UpdateCart();

                    PartialCart();

                    Swal.fire({
                        position: 'top',
                        icon: 'success',
                        title: 'Ürün sepete eklendi.',
                        showConfirmButton: false,
                        timer: 1000
                    })
                }

                else if (control == 0) {
                    Swal.fire({
                        position: 'top',
                        icon: 'warning',
                        title: 'Stok sayısı aşıldı.',
                        showConfirmButton: false,
                        timer: 1000
                    })
                }

            }
        });
    }
    else if (unit == 0) {
        Swal.fire({
            position: 'top',
            icon: 'warning',
            title: 'Bu ürün tükendi.',
            showConfirmButton: false,
            timer: 1000
        })
    }
    else {
        Swal.fire({
            position: 'top',
            icon: 'error',
            title: 'Bir sorun oluştu.',
            showConfirmButton: false,
            timer: 1000
        })
    }
});

$(".cartItemCount").bind('keyup mouseup', async function () {
    var unit = $(this).val();
    var productID = $(this).attr("name");
    var unitprice = $(this).parents("td").siblings(".unitPrice").text().slice(1);
    var totalpriceItem = $(this).parents("td").siblings(".totalPrice");

    if (unit > 0) {
        $.ajax({
            type: 'Post',
            url: '/Cart/AddToCart/',
            data: { "unit": unit, "productID": productID, "changed": true },
            dataType: 'Json',
            success: function (control) {
                if (control == 1) {

                    UpdateCart();

                    var unitpricenumber = unitprice.replace(/,/g, ".");
                    var newprice = (unit * unitpricenumber);
                    totalpriceItem.text("$" + newprice.toFixed(2));

                    $.ajax({
                        type: 'Get',
                        url: '/Cart/GetNewTotalPrice/',
                        success: function (total) {
                            $("#totalCartAmount").text("Sepet Tutarı: $" + total)
                        }
                    });

                    PartialCart();

                }
                else {
                    Swal.fire({
                        position: 'top',
                        icon: 'warning',
                        title: 'Stok sayısı aşıldı.',
                        showConfirmButton: false,
                        timer: 1000
                    })
                    setTimeout(function () {
                        window.location.reload();
                    }, 1000);
                }
            }
        });
    }
    else {
        $(this).val(1);
    }

});

$(".btnDeleteFromCart").on("click", async function () {
    var productId = $(this).val();
    var tr = $(this).parent("td").parent("tr");
    var trlength = $("#tbody>tr").length;
    $.ajax({
        type: 'Get',
        url: '/Cart/Remove/',
        dataType: 'Json',
        data: { "id": productId },
        success: function (control) {
            if (control == 1) {

                tr.remove();

                UpdateCart();

                $.ajax({
                    type: 'Get',
                    url: '/Cart/GetNewTotalPrice/',
                    success: function (total) {
                        $("#totalCartAmount").text("Sepet Tutarı: $" + total);
                    }
                });

                PartialCart();

                Swal.fire({
                    position: 'top',
                    icon: 'warning',
                    title: 'Ürün sepetten kaldırıldı.',
                    showConfirmButton: false,
                    timer: 1000
                });

                var trlength = $("#tbody>tr").length;
                if (trlength < 1) {

                    setTimeout(function () {
                        window.location = "/Cart/Index/";
                    }, 1000);
                }

            }

        }
    });

});


$(".changePassword").on("click", async function () {
    var button = $(this);
    await Swal.fire({
        title: 'Şifre Değiştir',
        html:
            '</br>' +
            '<p>Mevcut Şifre <p>' +
            '<input id="oldpassword" class="swal2-input" type="password">' +
            '<p>Yeni Şifre <p>' +
            '<input id="newpassword1" class="swal2-input" type="password">' +
            '<p>Yeni Şifre (Tekrar)<p>' +
            '<input id="newpassword2" class="swal2-input" type="password">',
        focusConfirm: true,
        showCancelButton: true,
        confirmButtonText: "Kaydet",
        cancelButtonText: "Vazgeç",
        allowEscapeKey: true

    }).then((result) => {
        if (result.dismiss === Swal.DismissReason.cancel || result.dismiss == Swal.DismissReason.backdrop || result.dismiss == Swal.DismissReason.esc)
            return;

        var oldpassword = document.getElementById('oldpassword').value;
        var newpassword1 = document.getElementById('newpassword1').value;
        var newpassword2 = document.getElementById('newpassword2').value;

        $.ajax({
            type: 'Post',
            url: '/User/ChangePassword/',
            data: { "oldpassword": oldpassword, "newpassword1": newpassword1, "newpassword2": newpassword2 },
            dataType: 'Json',
            success: function (control) {
                if (control == 1) {
                    Swal.fire({
                        type: 'success',
                        icon: 'success',
                        title: 'Başarılı',
                        text: 'Şifreniz başarıyla değiştirildi.',
                        confirmButtonText: 'Tamam'

                    })
                }
                else if (control == 0) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Hata!',
                        text: 'Mevcut şifrenizi yanlış girdiniz.',
                        confirmButtonText: 'Tamam'

                    }).then(() => {
                        button.click();
                    })
                }
                else if (control == 2) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Hata!',
                        text: 'Girdiğiniz şifreler uyuşmuyor.',
                        confirmButtonText: 'Tamam'

                    }).then(() => {
                        button.click();
                    })
                }
            }

        })
    })
})

$(".orderCart").mouseenter(function () {
    $(this).css("background-color", "#f7f7f7")
});

$(".orderCart").mouseleave(function () {
    $(this).css("background-color", "white")
});

$(".adminOrderCart").mouseenter(function () {
    $(this).css("background-color", "#BEDBEE")
});

$(".adminOrderCart").mouseleave(function () {
    $(this).css("background-color", "white")
});

$(".carttr").mouseenter(function () {
    $(this).css("background-color", "#9FD6D6")
});

$(".carttr").mouseleave(function () {
    $(this).css("background-color", "#f7f7f7")
});

$(".deleteOrder").on("click", async function () {
    var orderID = $(this).val();
    $.ajax({
        type: 'Post',
        url: '/User/DeleteOrder/',
        dataType: 'Json',
        data: { "orderID": orderID },
        success: function (control) {
            if (control == 1) {
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'Sipariş silindi.',
                    showConfirmButton: false,
                    timer: 1000
                });
                setTimeout(function () {
                    window.location = "/User/Orders/";
                }, 1000);

            }
            else
                Swal.fire({
                    icon: 'error',
                    title: 'Hata!',
                    text: 'Bir sorun oluştu',
                    confirmButtonText: 'Tamam'

                })
        }
    })
})

$(".cancelOrder").on("click", async function () {
    var orderID = $(this).val();
    $.ajax({
        type: 'Post',
        url: '/User/CancelOrder/',
        dataType: 'Json',
        data: { "orderID": orderID },
        success: function (control) {
            if (control == 1) {
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'Sipariş İptal Edildi.',
                    showConfirmButton: false,
                    timer: 1000
                });
                setTimeout(function () {
                    window.location = "/User/Orders/";
                }, 1000);

            }
            else
                Swal.fire({
                    icon: 'error',
                    title: 'Hata!',
                    text: 'Bir sorun oluştu',
                    confirmButtonText: 'Tamam'

                })
        }
    })
});

$(".cart-right").on("click", async function () {
    var productID = $(this).children("input").val();
    $.ajax({
        type: 'Post',
        url: '/Cart/AddToCart/',
        data: { "unit": 1, "productID": productID },
        dataType: 'Json',
        success: function (control) {
            if (control == 1) {
                post = true;
                UpdateCart();

                PartialCart();

                Swal.fire({
                    position: 'top',
                    icon: 'success',
                    title: 'Ürün sepete eklendi.',
                    showConfirmButton: false,
                    timer: 1000
                })
            }
            else if (control == 0) {
                Swal.fire({
                    position: 'top',
                    icon: 'warning',
                    title: 'Bu ürün tükendi.',
                    showConfirmButton: false,
                    timer: 1000
                })
            }
        }
    });
})


var total = $("#totalAmountComplete").text().slice(17).replace(/,/g, ".");
$("#selectShipper").change(async function () {
    if (total >= 100)
        return;
    var shipperID = $(this).val();
    $.ajax({
        type: 'Get',
        url: '/Cart/GetTotalWithShipping/',
        data: { "shipID": shipperID },
        dataType: 'Json',
        success: function (shipPrice) {

            var newTotal = (Number(total) + Number(shipPrice)).toFixed(2);
            $("#totalAmountComplete").text("Ödenecek Tutar: $" + newTotal);
        }
    });

});

function PartialCart() {

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

function UpdateCart() {

    $.ajax({
        type: 'Post',
        url: '/Cart/UpdateCart/',
        dataType: 'Json',
        success: function (cartCount) {
            $("#cartCount").text("Sepet (" + cartCount + ")");

        }

    });
}

$(".changePasswordAdmin").on("click", async function () {
    var button = $(this);
    await Swal.fire({
        title: 'Şifre Değiştir',
        html:
            '</br>' +
            '<p>Mevcut Şifre <p>' +
            '<input id="oldpassword" class="swal2-input" type="password">' +
            '<p>Yeni Şifre <p>' +
            '<input id="newpassword1" class="swal2-input" type="password">' +
            '<p>Yeni Şifre (Tekrar)<p>' +
            '<input id="newpassword2" class="swal2-input" type="password">',
        focusConfirm: true,
        showCancelButton: true,
        confirmButtonText: "Kaydet",
        cancelButtonText: "Vazgeç",
        allowEscapeKey: false

    }).then((result) => {
        if (result.dismiss === Swal.DismissReason.cancel || result.dismiss == Swal.DismissReason.backdrop)
            return;

        var oldpassword = document.getElementById('oldpassword').value;
        var newpassword1 = document.getElementById('newpassword1').value;
        var newpassword2 = document.getElementById('newpassword2').value;

        $.ajax({
            type: 'Post',
            url: '/Admin/ChangePassword/',
            data: { "oldpassword": oldpassword, "newpassword1": newpassword1, "newpassword2": newpassword2 },
            dataType: 'Json',
            success: function (control) {
                if (control == 1) {
                    Swal.fire({
                        type: 'success',
                        icon: 'success',
                        title: 'Başarılı',
                        text: 'Şifreniz başarıyla değiştirildi.',
                        confirmButtonText: 'Tamam'

                    })
                }
                else if (control == 0) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Hata!',
                        text: 'Mevcut şifrenizi yanlış girdiniz.',
                        confirmButtonText: 'Tamam'

                    }).then(() => {
                        button.click();
                    })
                }
                else if (control == 2) {
                    Swal.fire({
                        icon: 'error',
                        title: 'Hata!',
                        text: 'Girdiğiniz şifreler uyuşmuyor.',
                        confirmButtonText: 'Tamam'

                    }).then(() => {
                        button.click();
                    })
                }
            }

        })
    })
})

$("#btnhide").on("click", function () {

    $("#adminmenu").toggle();

    if ($('#adminmenu').is(':hidden')) {

        $(this).text("Menüyü Göster");
    }
    else {
        $(this).text("Menüyü Gizle");

    }

});

$(".deleteOrderAdmin").on("click", async function () {
    var orderID = $(this).val();
    $.ajax({
        type: 'Post',
        url: '/AdminOrder/DeleteOrder/',
        dataType: 'Json',
        data: { "orderID": orderID },
        success: function (control) {
            if (control == 1) {
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'Sipariş silindi.',
                    showConfirmButton: false,
                    timer: 1000
                });
                setTimeout(function () {
                    window.location.reload();
                }, 1000);

            }
            else
                Swal.fire({
                    icon: 'error',
                    title: 'Hata!',
                    text: 'Bir sorun oluştu',
                    confirmButtonText: 'Tamam'

                })
        }
    })
})

$(".cancelOrderAdmin").on("click", async function () {
    var orderID = $(this).val();
    $.ajax({
        type: 'Post',
        url: '/AdminOrder/CancelOrder/',
        dataType: 'Json',
        data: { "orderID": orderID },
        success: function (control) {
            if (control == 1) {
                Swal.fire({
                    position: 'center',
                    icon: 'warning',
                    title: 'Sipariş Geri Çevirildi.',
                    showConfirmButton: false,
                    timer: 1000
                });
                setTimeout(function () {
                    window.location.reload();
                }, 1000);

            }
            else
                Swal.fire({
                    icon: 'error',
                    title: 'Hata!',
                    text: 'Bir sorun oluştu',
                    confirmButtonText: 'Tamam'

                })
        }
    })
});

$(".acceptOrderAdmin").on("click", async function () {
    var orderID = $(this).val();
    $.ajax({
        type: 'Post',
        url: '/AdminOrder/GetOrder/',
        dataType: 'Json',
        data: { "orderID": orderID },
        success: function (data) {

            Swal.fire({
                title: 'Siparişi Gönder',
                html:
                    '</br><p><strong>Gönderim Bilgileri</strong></p><div class="card"></br>' +
                    '<p>Kargo Firması: <strong>' + data[0] + '</strong> </p>' +
                    '<p>Alıcı: <strong>' + data[1] + '</strong> </p>' +
                    '<p>Şirket Adı: <strong>' + data[2] + '</strong> </p>' +
                    '<p>Adres: <strong>' + data[3] + '</strong> </p>' +
                    '<p>Şehir: <strong>' + data[4] + '</strong> </p>' +
                    '<p>Ülke: <strong>' + data[5] + '</strong> </p>' +
                    '<p>Posta Kodu: <strong>' + data[6] + '</strong> </p></div>',
                focusConfirm: true,
                showCancelButton: true,
                confirmButtonText: "Siparişi Gönder",
                confirmButtonColor: "#198754",
                cancelButtonText: "İptal",
                allowEscapeKey: false

            }).then((result) => {
                if (result.dismiss === Swal.DismissReason.cancel || result.dismiss == Swal.DismissReason.backdrop)
                    return;

                $.ajax({
                    type: 'Post',
                    url: '/AdminOrder/AcceptOrder/',
                    dataType: 'Json',
                    data: { "orderID": orderID },
                    success: function (control) {
                        if (control == 1) {
                            Swal.fire({
                                position: 'center',
                                icon: 'success',
                                title: 'Sipariş gönderildi.',
                                showConfirmButton: false,
                                timer: 1000
                            });
                            setTimeout(function () {
                                window.location.reload();
                            }, 1000);

                        }
                        else
                            Swal.fire({
                                icon: 'error',
                                title: 'Hata!',
                                text: 'Bir sorun oluştu',
                                confirmButtonText: 'Tamam'

                            })
                    }
                })
            })
        }
    })
});


