
$(".selectProduct").change(async function () {
    $.ajax({
        type: 'Get',
        url: '/AdminProduct/Products/',
        data: {
            "categoryID": $("#selectCategory option:selected").val(),
            "status": $("#selectStatus option:selected").val()
        },
        success: function (data) {
            if (data) {
                $('#divfortable').html("");
                $('#divfortable').html(data);

            }
            if ($("#tableProductsBody > tr").children().length < 1) {
                $('#divfortable').html('<h4 style="text-align:center;">Gösterilecek hiçbir ürün bulunamadı.</h3>');
            }
        }
    });
});

$(".selectCategory").change(async function () {
    $.ajax({
        type: 'Get',
        url: '/AdminCategory/Categories/',
        data: { "status": $("#selectcategoryStatus option:selected").val() },
        success: function (data) {
            if (data) {
                $('#divfortablecategories').html("");
                $('#divfortablecategories').html(data);

            }
            if ($("#tablecategoriesBody > tr").children().length < 1) {
                $('#divfortablecategories').html('<h4 style="text-align:center;">Gösterilecek hiçbir sonuç bulunamadı.</h3>');
            }
        }
    });
});

$(".selectShipper").change(async function () {
    $.ajax({
        type: 'Get',
        url: '/AdminShipper/Shippers/',
        data: { "status": $("#selectshipperStatus option:selected").val() },
        success: function (data) {
            if (data) {
                $('#divfortableshippers').html("");
                $('#divfortableshippers').html(data);

            }
            if ($("#tableshippersBody > tr").children().length < 1) {
                $('#divfortableshippers').html('<h4 style="text-align:center;">Gösterilecek hiçbir sonuç bulunamadı.</h3>');
            }
        }
    });
});

$(".selectCustomer").change(async function () {
    $.ajax({
        type: 'Get',
        url: '/AdminCustomer/Customers/',
        data: { "status": $("#selectcustomerStatus option:selected").val() },
        success: function (data) {
            if (data) {
                $('#divfortablecustomers').html("");
                $('#divfortablecustomers').html(data);

            }
            if ($("#tablecustomersBody > tr").children().length < 1) {
                $('#divfortablecustomers').html('<h4 style="text-align:center;">Gösterilecek hiçbir sonuç bulunamadı.</h3>');
            }
        }
    });
});

$("#tsearch").on("keyup", function () {
    var value = $(this).val().toLowerCase();
    $("#tableProductsBody tr").filter(function () {
        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
    });
});

$("#tsearchCategory").on("keyup", function () {
    var value = $(this).val().toLowerCase();
    $("#tablecategoriesBody tr").filter(function () {
        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
    });
});

$("#tsearchShipper").on("keyup", function () {
    var value = $(this).val().toLowerCase();
    $("#tableshippersBody tr").filter(function () {
        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
    });
});

$("#tsearchCustomer").on("keyup", function () {
    var value = $(this).val().toLowerCase();
    $("#tablecustomersBody tr").filter(function () {
        $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
    });
});

async function enableCategory(id) {
    var button = document.getElementById(id);
    $.ajax({
        type: 'Get',
        url: '/AdminCategory/Enable/',
        data: { "CategoryID": id },
        success: function (control) {
            if (control == 1) {
                Swal.fire({
                    position: 'top',
                    icon: 'success',
                    title: 'Kategori ve ilgili ürünler etkinleştirildi.',
                    showConfirmButton: false,
                    timer: 1000
                })
                button.innerHTML = '<i class="far fa-check-circle"></i>';
                button.className = 'btn btn-success';
                button.setAttribute("onclick", 'disableCategory(' + id + ')');

            }
        }
    })
}

async function disableCategory(id) {
    var button = document.getElementById(id);
    $.ajax({
        type: 'Get',
        url: '/AdminCategory/Disable/',
        data: { "CategoryID": id },
        success: function (control) {
            if (control == 1) {
                Swal.fire({
                    position: 'top',
                    icon: 'warning',
                    title: 'Kategori ve ilgili ürünler etkisizleştirildi.',
                    showConfirmButton: false,
                    timer: 1000
                })
                button.innerHTML = '<i class="far fa-times-circle"></i>';
                button.className = 'btn btn-danger';
                button.setAttribute("onclick", 'enableCategory(' + id + ')');

            }
        }
    })
}

async function enableCustomer(id) {
    var button = document.getElementById(id);
    $.ajax({
        type: 'Get',
        url: '/AdminCustomer/Enable/',
        data: { "CustomerID": id },
        success: function (control) {
            if (control == 1) {
                Swal.fire({
                    position: 'center',
                    icon: 'success',
                    title: 'Kullanıcı engeli kaldırıldı. Artık sisteme giriş yapabilir.',
                    showConfirmButton: true,
                    confirmButtonText: 'Tamam'
                })
                button.innerHTML = '<i class="far fa-check-circle"></i>';
                button.className = 'btn btn-success';
                button.setAttribute("onclick", 'disableCustomer(' + '"' + id + '"' + ')');

            }
        }
    })
}

async function disableCustomer(id) {
    var button = document.getElementById(id);
    $.ajax({
        type: 'Get',
        url: '/AdminCustomer/Disable/',
        data: { "CustomerID": id },
        success: function (control) {
            if (control == 1) {
                Swal.fire({
                    position: 'center',
                    icon: 'warning',
                    title: 'Kullanıcı engellendi. Artık sisteme giriş yapamayacak.',
                    showConfirmButton: true,
                    confirmButtonText: 'Tamam'
                })
                button.innerHTML = '<i class="far fa-times-circle"></i>';
                button.className = 'btn btn-danger';
                button.setAttribute("onclick", 'enableCustomer(' + '"' + id + '"' + ')');

            }
        }
    })
}

async function enableShipper(id) {
    var button = document.getElementById(id);
    $.ajax({
        type: 'Get',
        url: '/AdminShipper/Enable/',
        data: { "ShipperID": id },
        success: function (control) {
            if (control == 1) {
                Swal.fire({
                    position: 'top',
                    icon: 'success',
                    title: 'Firma başarıyla etkinleştirildi.',
                    showConfirmButton: false,
                    timer: 1000
                })
                button.innerHTML = '<i class="far fa-check-circle"></i>';
                button.className = 'btn btn-success';
                button.setAttribute("onclick", 'disableShipper(' + id + ')');

            }
        }
    })
}

async function disableShipper(id) {
    var button = document.getElementById(id);
    $.ajax({
        type: 'Get',
        url: '/AdminShipper/Disable/',
        data: { "ShipperID": id },
        success: function (control) {
            if (control == 1) {
                Swal.fire({
                    position: 'top',
                    icon: 'warning',
                    title: 'Firma etkisizleştirildi.',
                    showConfirmButton: false,
                    timer: 1000
                })
                button.innerHTML = '<i class="far fa-times-circle"></i>';
                button.className = 'btn btn-danger';
                button.setAttribute("onclick", 'enableShipper(' + id + ')');

            }
        }
    })
}


$("#addNewCategory").on("click", async function () {
    var button = $(this);
    Swal.fire({
        title: 'Kategori Ekle',
        html:
            '</br><label>Kategori Adı: <input id="CategoryName" class="swal2-input" type="text"></label>' +
            '</br><label>Açıklama: <input id="Description" class="swal2-input" type="text"></label>' +
            '</br><label><input id="Activated" class="form-check-input" type="checkbox"> Aktifleştir</label>',
        focusConfirm: true,
        showCancelButton: true,
        confirmButtonText: "Kaydet",
        cancelButtonText: "Vazgeç",
        allowEscapeKey: true,
        allowEnterKey: true

    }).then((result) => {
        if (result.dismiss === Swal.DismissReason.cancel || result.dismiss == Swal.DismissReason.backdrop || result.dismiss == Swal.DismissReason.esc)
            return;
        var category = {
            CategoryName: $("#CategoryName").val(),
            Description: $("#Description").val(),
            Activated: false
        }
        if ($("#Activated").prop("checked") == true) {
            category.Activated = true;
        }

        $.ajax({
            type: 'Post',
            url: '/AdminCategory/Add/',
            data: { "category": category },
            success: function (result) {
                if (result == 1) {
                    Swal.fire({
                        position: 'center',
                        icon: 'success',
                        title: 'Kategori başarıyla eklendi.',
                        showConfirmButton: false,
                        timer: 1000

                    })
                    setTimeout(function () {
                        window.location.reload();
                    }, 1000);

                }
                else if (result == 2) {
                    Swal.fire({
                        position: 'center',
                        icon: 'error',
                        title: 'Bu kategori zaten mevcut.',
                        showConfirmButton: true,
                        confirmButtonText: "Tamam",

                    }).then(() => {
                        button.click();
                    });
                }
                else {
                    Swal.fire({
                        position: 'center',
                        icon: 'error',
                        title: 'Lütfen tüm alanları eksiksiz doldurunuz.',
                        showConfirmButton: true,
                        confirmButtonText: "Tamam",

                    }).then(() => {
                        button.click();
                    });
                }
            }
        })
    })
})



$("#addNewShipper").on("click", async function () {
    var button = $(this);
    Swal.fire({
        title: 'Firma Ekle',
        html:
            '</br><label>Firma Adı: <input id="CompanyName" class="swal2-input" type="text"></label>' +
            '</br><label>Telefon: <input id="Phone" class="swal2-input" type="tel"></label>' +
            '<label>Kargo Ücreti: $  <input id="ShipPrice" min=0 class="swal2-input" type="number" step="0.01"></label>' +
            '</br><label><input id="Activated" class="form-check-input" type="checkbox"> Aktifleştir</label>',
        focusConfirm: true,
        showCancelButton: true,
        confirmButtonText: "Kaydet",
        cancelButtonText: "Vazgeç",
        allowEscapeKey: true,
        allowEnterKey: true

    }).then((result) => {
        if (result.dismiss === Swal.DismissReason.cancel || result.dismiss == Swal.DismissReason.backdrop || result.dismiss == Swal.DismissReason.esc)
            return;
        var shipper = {
            CompanyName: $("#CompanyName").val(),
            Phone: $("#Phone").val(),
            Activated: false
        }
        var price = parseFloat($("#ShipPrice").val()).toFixed(2);

        if ($("#Activated").prop("checked") == true) {
            shipper.Activated = true;
        }

        $.ajax({
            type: 'Post',
            url: '/AdminShipper/Add/',
            data: { "shipper": shipper, "price": price },
            success: function (result) {
                if (result == 1) {
                    Swal.fire({
                        position: 'center',
                        icon: 'success',
                        title: 'Firma başarıyla eklendi.',
                        showConfirmButton: false,
                        timer: 1000

                    })
                    setTimeout(function () {
                        window.location.reload();
                    }, 1000);

                }
                else if (result == 2) {
                    Swal.fire({
                        position: 'center',
                        icon: 'error',
                        title: 'Bu firma zaten mevcut.',
                        showConfirmButton: true,
                        confirmButtonText: "Tamam",

                    }).then(() => {
                        button.click();
                    });
                }
                else {
                    Swal.fire({
                        position: 'center',
                        icon: 'error',
                        title: 'Lütfen tüm alanları eksiksiz doldurunuz.',
                        showConfirmButton: true,
                        confirmButtonText: "Tamam",

                    }).then(() => {
                        button.click();
                    });
                }
            }
        })
    })
})



async function enableProduct(id) {
    var button = document.getElementById(id);
    $.ajax({
        type: 'Get',
        url: '/AdminProduct/Enable/',
        data: { "ProductID": id },
        success: function (control) {
            if (control == 1) {
                Swal.fire({
                    position: 'top',
                    icon: 'success',
                    title: 'Ürün satışa koyuldu.',
                    showConfirmButton: false,
                    timer: 1000
                })
                button.innerHTML = '<i class="far fa-check-circle"></i>';
                button.className = 'btn btn-success';
                button.setAttribute("onclick", 'disableProduct(' + id + ')');

            }
        }
    });
}

async function disableProduct(id) {
    var button = document.getElementById(id);
    $.ajax({
        type: 'Get',
        url: '/AdminProduct/Disable/',
        data: { "ProductID": id },
        success: function (control) {
            if (control == 1) {
                Swal.fire({
                    position: 'top',
                    icon: 'warning',
                    title: 'Ürün satıştan kaldırıldı.',
                    showConfirmButton: false,
                    timer: 1000
                })
                button.innerHTML = '<i class="far fa-times-circle"></i>';
                button.className = 'btn btn-danger';
                button.setAttribute("onclick", 'enableProduct(' + id + ')');

            }
        }
    });
}



async function editPrice(id) {
    $.ajax({
        type: 'Get',
        url: '/AdminProduct/EditPrice/',
        data: { "ProductID": id },
        success: function (product) {
            var price = Number(product[2].replace(/,/g, ".")).toFixed(2);
            Swal.fire({
                title: 'Fiyat/Stok Güncelle',
                html:
                    '</br><p>Ürün No: <strong>' + product[0] + '</strong> </br></br> Ürün Adı: <strong>' + product[1] + '</strong></p>' +
                    '<label>Birim Fiyat: $   <input id="UnitPrice" min=0 class="swal2-input" type="number" step="0.01"  value=' + price + '></label>' +
                    '<label>Stok Adedi:   <input id="UnitsInStock" min=0 class="swal2-input" type="number" value=' + product[3] + '></label>',
                focusConfirm: true,
                showCancelButton: true,
                confirmButtonText: "Kaydet",
                cancelButtonText: "Vazgeç",
                allowEscapeKey: true,
                allowEnterKey: true

            }).then((result) => {
                if (result.dismiss === Swal.DismissReason.cancel || result.dismiss == Swal.DismissReason.backdrop || result.dismiss == Swal.DismissReason.esc)
                    return;
                var unitprice = $("#UnitPrice").val();
                var unitsinstock = Number($("#UnitsInStock").val());

                $.ajax({
                    type: 'Post',
                    url: '/AdminProduct/EditPrice/',
                    data: { "productID": id, "UnitPrice": unitprice, "UnitsInStock": unitsinstock },

                }).then((result) => {
                    if (result == 1) {
                        Swal.fire({
                            position: 'center',
                            icon: 'success',
                            title: 'Fiyat/Stok başarıyla güncellendi.',

                            showConfirmButton: true,
                            confirmButtonText: "Tamam",

                        })

                        $.ajax({
                            type: 'Get',
                            url: '/AdminProduct/Products/',
                            data: {
                                "categoryID": $("#selectCategory option:selected").val(),
                                "status": $("#selectStatus option:selected").val()
                            },
                            success: function (data) {
                                if (data) {
                                    $('#divfortable').html("");
                                    $('#divfortable').html(data);

                                }
                                if ($("#tableProductsBody > tr").children().length < 1) {
                                    $('#divfortable').html('<h4 style="text-align:center;">Gösterilecek hiçbir ürün bulunamadı.</h3>');
                                }
                            }
                        });

                    }
                    else {
                        Swal.fire({
                            position: 'center',
                            icon: 'error',
                            title: 'Bir sorun oluştu.',
                            showConfirmButton: true,
                            confirmButtonText: "Tamam"
                        })

                    }
                })
            })
        }
    })
}

$("#inputUnitPrice").on("change", function () {
    var value = parseFloat($(this).val()).toFixed(2);
    $(this).val(value);
})



