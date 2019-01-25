

///////////////////////////////// Check Quantity ///////////////////////////////////////////
///////////////////////////////// Check Quantity ///////////////////////////////////////////
function loadCheckQuantities(month) {
    $("#js-table-check-quantities").children("tbody").empty();
    $.ajax({
        url: "/Api/CheckQuantity/GetCheckQuantities/" + month,
        method: "GET"
    })
        .done(function (response) {
            for (var i = 0; i < response.length; i++) {
                $("#js-table-check-quantities").append(
                    "<tr>" +
                    "<td>" + response[i].checkDate + "</td>" +
                    "<td>" + response[i].margin + "</td>" +
                    "<td>" + response[i].totalQuantity + "</td>" +
                    "<td>" +
                    "<a href='#responsive' class='btn-datail-check-quantity' data-check-quantity-id='" + response[i].checkQuantityId + "'>Details" +
                    "<a href='#responsive' class='btn-delete-check-quantity' data-check-quantity-id='" + response[i].checkQuantityId + "'>Del" +
                    "</td>" + 
                    "</tr>"
                );
            }
        })
        .fail(function (response) {
            $("#js-table-check-quantities").append(
                "<tr>" +
                "<td colspan='4'>" + response.responseJSON.message + "</td>" + 
                "</tr>"
            );
        });
}

function loadCheckQuantityDetails(checkQuantityId) {
    $.ajax({
        url: "/Api/CheckQuantity/" + checkQuantityId,
        method: "GET"
    })
        .done(function (response) {
            $("#js-check-quantity-title").html("Check quantity");
            for (var i = 0; i < response.length; i++) {
                var tr = $("tr[data-service-id='" + response[i].serviceId + "']");
                tr.find(".old-quantity").html(response[i].oldQuantity);
                tr.find(".current-quantity").val(response[i].currentQuantity);
            }
        })
        .fail(function (response) {
            toastr.error(response.responseJSON.message);
        });
}

function resetCheckQuantityModal() {
    today = new Date();
    var day = today.getDate() + "-" + (today.getMonth() + 1) + "-" + (today.getFullYear());
    $("#js-check-quantity-title").html("Check Quantity " + day);

    $("#js-table-check-quantity > tbody > tr").each(function () {
        $(this).find(".current-quantity").val('0');
    });
}

function initCheckQuantityModal() {
    $("#js-table-check-quantity").children("tbody").empty();
    $.ajax({
        url: "/Api/Warehouse",
        method: "GET"
    })
    .done(function (response) {
        for (var i = 0; i < response.length; i++) {
            $("#js-table-check-quantity").children("tbody").append(
                "<tr data-service-id='" + response[i].service.serviceId + "'>" +
                "<td>" + response[i].service.name + "</td>" +
                "<td class='old-quantity'>" + response[i].quantity + "</td>" +
                "<td><input value='0' class='current-quantity form-control' name='current-quantity-" + i + "' id='current-quantity-" + i + "' </td>" +
                "</tr>"
            );
        }
        $(".current-quantity").mask("#,##0", { reverse: true });
    })
    .fail(function (response) {
        toastr.error(response.responseJSON.message);
    });
}

function saveCheckQuantity(checkQuantityDetailDtos) {
    $.ajax({
        url: "/Api/CheckQuantity",
        method: "POST",
        contentType: 'application/json',
        dataType: "json",
        data: JSON.stringify(checkQuantityDetailDtos)
    })
        .done(function (response) {
            toastr.success("Saved successfully");

            if ($("#js-table-check-quantities > tbody > tr > td").length <= 1) {
                $("#js-table-check-quantities").children("tbody").empty();
            }

            $("#js-table-check-quantities").children("tbody").append(
                "<tr>" +
                "<td>" + response.checkDate + "</td>" +
                "<td>" + response.margin + "</td>" +
                "<td>" + response.totalQuantity + "</td>" +
                "<td>" +
                "<a href='#' class='btn-datail-check-quantity' data-check-quantity-id='" + response.checkQuantityId + "'>Details" +
                "<a href='#' class='btn-delete-check-quantity' data-check-quantity-id='" + response.checkQuantityId + "'>Del" +
                "</td>" + 
                "</tr>"
            );
            $("#check-quantity-modal").modal("hide");
        })
        .fail(function (response) {
            toastr.error(response.responseJSON.message);
        });
}

$(document).ready(function () {

    var selectMonth = $("#select-month");
    selectMonth.val(new Date().getMonth() + 1);

    loadCheckQuantities(selectMonth.val());
    initCheckQuantityModal();

    $("#btn-new-check-quantity").on("click", function () {
        resetCheckQuantityModal();
        initCheckQuantityModal();
        
        $("#btn-save-check-quantity").css("display", "inline-block");
        $("#check-quantity-modal").modal("show");
    });

    $("#btn-get-check-quantity").on("click", function () {
        loadCheckQuantities(selectMonth.val());
    })

    var checkQuantityDetailDtos = [];
    $("#btn-save-check-quantity").on("click", function () {
        bootbox.confirm("Are you sure want to save this check quantity form, (these quantity will apply to the warehouse quantity)", function (result) {
            if (result) {
                $("#js-table-check-quantity > tbody > tr").each(function () {
                    var serviceId = $(this).attr("data-service-id");
                    var oldQuantity = $(this).find(".old-quantity").html();
                    var currentQuantity = $(this).find(".current-quantity").cleanVal();
                    checkQuantityDetailDtos.push({ "serviceId": serviceId, "oldQuantity": oldQuantity, "currentQuantity": currentQuantity });
                });
                saveCheckQuantity(checkQuantityDetailDtos);
            }
        });
    });

    $("#js-table-check-quantities").on("click", ".btn-datail-check-quantity", function () {
        var checkQuantityId = $(this).attr("data-check-quantity-id");
        loadCheckQuantityDetails(checkQuantityId);
        $("#check-quantity-modal").modal("show");

        $("#btn-save-check-quantity").css("display", "none");
    });

    $("#js-table-check-quantities").on("click", ".btn-delete-check-quantity", function () {
        var button = $(this);
        bootbox.confirm("Are you sure want to delete this check form", function (result) {
            if (result) {
                var checkQuantityId = button.attr("data-check-quantity-id");
                $.ajax({
                    url: "/Api/CheckQuantity/" + checkQuantityId,
                    method: "Delete"
                })
                    .done(function (response) {
                        button.parents("tr").remove();
                        toastr.success("Check form deleted");
                    })
                    .fail(function () {
                        toastr.success(response.responseJSON.message);
                    });
            }
        })
    });
});