
//warehouse scripts

//////////////////////////////////// Restock /////////////////////////////////////
//////////////////////////////////// Restock /////////////////////////////////////

function getrestock(month) {
    $("#js-table-restock").children("tbody").empty();
    $.ajax({
        url: "/Api/Warehouse/GetRestock/" + month,
        method: "GET"
    })
        .done(function (response) {
            for (var i = 0; i < response.length; i++) {
                $("#js-table-restock").children("tbody").append(
                    "<tr>" +
                    "<td>" + response[i].restockDate + "</td>" +
                    "<td>" + response[i].total + "</td>" +
                    "<td>" +
                    "<a href='#responsive' class='btn-detail-restock' data-restock-id='" + response[i].restockId + "'>Detail</a>" +
                    "<a href='#responsive' class='btn-delete-restock' data-restock-id='" + response[i].restockId + "'>Del</a>" +
                    "</td>" +
                    "</tr>"
                );
            }
        })
        .fail(function () {
            $("#js-table-restock").children("tbody").append(
                "<tr><td colspan='4'>No restock in this month</td></tr>"
            );
        });
}

//reset datetime and quantity value
function resetRestockModal() {
    
    today = new Date();
    var day = today.getDate() + "-" + (today.getMonth() + 1) + "-" + (today.getFullYear());
    $("#new-restock-title").html("Restock " + day);

    $("#js-table-restock-services > tbody > tr").each(function () {
        $(this).find(".service-quantity").val('0');
        $(this).find(".service-total").val('0');
    });
}

function loadRestockModal() {
    var table = $("#js-table-restock-services");

    //get service list
    $.ajax({
        url: "/Api/Services",
        method: "GET"
    })
        .done(function (response) {
            for (var i = 0; i < response.length; i++) {
                table.children("tbody").append(
                    "<tr data-service-id='" + response[i].serviceId + "'>" +
                    "<td>" + response[i].name + "</td>" +
                    "<td><input value='0' name='quantity" + i + "' id='quantity" + i + "' class='form-control service-quantity' /></td>" +
                    "<td><input value='0' name='total" + i + "' id='total" + i + "' class='form-control service-total' /></td>" +
                    "</tr>"
                );
                //add validate rules
                $("#quantity" + i).rules("add", {
                    required: true,
                    digits: true
                });
                $("#total" + i).rules("add", {
                    required: true,
                });
            }
            //add mask
            $('.service-total').mask("#,##0", { reverse: true });
        })
        .fail(function (reponse) {
            toastr.error(reponse.responseJSON.message);
        });
}

function loadRestockServices(restockId) {
    $.ajax({
        url: "/Api/Warehouse/" + restockId,
        method: "GET"
    })
        .done(function (response) {
            $("#new-restock-title").html("Restock " + response[0].restockDate);
            for (var i = 0; i < response.length; i++) {
                var tr = $("tr[data-service-id='" + response[i].serviceId + "']");
                tr.find(".service-quantity").val(response[i].quantity);
                tr.find(".service-total").val(response[i].total);
            }
        })
        .fail(function (response) {
            toastr.error(response.responseJSON.message);
        });
}

$(document).ready(function () {

    //validate and post when data valid on form submit. restockServiceDtos array is built in save-btn click event
    //save restock and restock service
    var validator = $("#form-new-restock").validate({
        submitHandler: function () {
            $.ajax({
                url: "/Api/Warehouse/",
                method: "POST",
                contentType: 'application/json',
                dataType: "json",
                data: JSON.stringify(restockServiceDtos)
            })
                .done(function (response) {
                    toastr.success("Saved successfully");

                    //reset form and close modal
                    resetRestockModal();
                    restockServiceDtos = [];
                    validator.resetForm();
                    $("#new-restock-modal").modal("hide");
                    if ($("#js-table-restock > tbody > tr > td").length <= 1) {
                        $("#js-table-restock").children("tbody").empty();
                    }

                    //apend new restock to the view
                    $("#js-table-restock").children("tbody").append(
                        "<tr><td>" + response.RestockDate + "</td>" +
                        "<td>" + response.Total + "</td>" +
                        "<td>" +
                        "<a href='#' class='btn-detail-restock' data-restock-id='" + response.RestockId + "'>Detail</a>" +
                        "<a href='#' class='btn-delete-restock' data-restock-id='" + response.RestockId + "'>Del</a>" +
                        "</td>" +
                        "</tr>"
                    );

                })
                .fail(function () {
                    toastr.error("Something magnificent happened");
                });
        }
    });

    var selectMonth = $("#select-month");
    selectMonth.val(new Date().getMonth() + 1);

    //load restock
    getrestock(selectMonth.val());
    //load restock modal (base with services)
    loadRestockModal();



    $("#btn-get-restock").on("click", function () {
        getrestock(selectMonth.val());
    });

    $("#btn-new-restock").on("click", function () {
        //reset da form first
        resetRestockModal();
        //reset validator
        validator.resetForm();
        $("#new-restock-modal").modal({ backdrop: 'static' });
        $("#btn-save-new-restock").css("display", "inline-block");
    })

    var restockServiceDtos = [];
    $("#btn-save-new-restock").on("click", function () {
        restockServiceDtos = [];
        var totalQuantity = 0;

        $("#js-table-restock-services > tbody > tr").each(function () {

            var serviceId = $(this).attr("data-service-id");
            var quantity = $(this).find(".service-quantity").val();

            //cleanVal() get unmasked typed value
            var total = $(this).find(".service-total").cleanVal();
            restockServiceDtos.push({ "restockServiceId": "0", "serviceId": serviceId, "quantity": quantity, "total": total });
            totalQuantity += quantity;
        });
        if (totalQuantity < 1) {
            toastr.error("You have to enter quantity for at least one service");
            return false;
        }
    })

    $("#js-table-restock").on("click", ".btn-detail-restock", function () {
        restockId = $(this).attr("data-restock-id");
        resetRestockModal();
        //load restock services
        loadRestockServices(restockId);

        //reset validator
        validator.resetForm();
        //hide save btn
        $("#btn-save-new-restock").css("display", "none");
        $("#new-restock-modal").modal("show");
    })

    $("#js-table-restock").on("click", ".btn-delete-restock", function () {
        var button = $(this);
        restockId = button.attr("data-restock-id");

        bootbox.confirm("Do you want to delete this restock form", function (result) {
            if (result) {
                $.ajax({
                    url: "/Api/Warehouse/" + restockId,
                    method: "Delete"
                })
                    .done(function () {
                        button.parents("tr").remove();
                        toastr.success("Restock form deleted");
                    })
                    .fail(function (response) {
                        toastr.error(response.responseJSON.message);
                    });
            }
        });
    })
});