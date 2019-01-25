var intVal = function (i) {
    return typeof i === 'string' ?
        i.replace(/[\$,.]/g, '') * 1 :
        typeof i === 'number' ?
        i : 0;
};

function rentalServicesDatatablesInit() {
    table = $("#js-table-service-rental").DataTable({
        bInfo: false,
        paging: false,
        searching: false,
        ordering: false,
        rowId: "serviceId",
        columns: [
            { data: "name" },
            { data: "quantity" },
            { data: "sumPrice" },
            {}
        ],
        'columnDefs': [
            //{
            //    "render": function (data, type, row) {
            //        var result = parseInt(row["quantity"]) * parseInt(row["price"]);
            //        return result;
            //    },
            //    "targets": 2
            //},
            {
                "render": function (data, type, row) {
                    var result = "<a style='color:white' class='btn btn-secondary js-service-edit' data-edit-value='1' data-service-price='" + row["price"]
                                 + "' data-service-id='" + row["serviceId"] + "'>+</a>" +
                                 "<a style='color:white' class='btn btn-secondary js-service-edit' data-edit-value='-1' data-service-price='" + row["price"]
                                 + "' data-service-id='" + row["serviceId"] + "'>-</a>";
                    return result;
                },
                "targets": 3
            },
            {
                //set id for cell to call later
                'targets': 1,
                'createdCell': function (td, cellData, rowData, row, col) {
                    $(td).attr('id', 'text-sum-quantity-' + rowData.serviceId);
                }
            },
            {
                //set id for cell to call later
                'targets': 2,
                'createdCell': function (td, cellData, rowData, row, col) {
                    $(td).attr('id', 'text-sum-price' + rowData.serviceId);
                }
            }
        ],
        //load footer
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;
            var txtTotal = $("#js-text-total");
            var rentalFee = parseInt(txtTotal.attr("data-rental-fee"));


            //sum price on this page
            var sumPrice = api
                .column('.sum-price', { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return a + b;
                }, 0);

            // sumQuantity on this page
            var sumQuantity = api
                .column('.sum-quantity', { page: 'current' })
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            // Update footer
            var footerSumQuantity = $(api.column('.sum-quantity').footer());
            var footerSumPrice = $(api.column('.sum-price').footer());

            if (sumQuantity > 0) {
                footerSumQuantity.attr("id", "footer-sum-quantity")
                footerSumQuantity.html($.number(sumQuantity), 0, '.', ',');

                footerSumPrice.attr("id", "footer-sum-price")
                footerSumPrice.html($.number(sumPrice), 0, '.', ',');

                //var total = rentalFee + sumPrice;
                //txtTotal.html(
                //    "Total: " + ($.number(total)) + "VNĐ"
                //);
            }
            else {
                footerSumQuantity.html(
                    ""
                );

                footerSumPrice.html(
                    ""
                );
            }
        }
    });
    return table;
}

//const getServiceRentalsByRentalId = async function (rentalId) {
//    return Q($.ajax({
//        url: "/API/ServiceRentals/" + rentalId,
//        method: "GET",
//        dataType: "json",
//        dataSrc: "serviceRentals"
//    }));
//}

function loadCheckOutModal(rentalId, psId, psName, mode) {
    var textCheckinTime = $("#js-text-checkin-time");
    var textCheckoutTime = $("#js-text-checkout-time");
    var textTotal = $("#js-text-total");
    var textCheckout = $("#js-text-checkout");
    var hideActionCol = (mode == "view") ? "display:none;" : "";

    textCheckout.html("Checkout - " + psName);
    textCheckout.attr("data-rental-id", rentalId);

    $.ajax({
        url: "/API/Rentals/" + rentalId,
        method: "GET",
        datatype: "Json"
    }).done(function (data) {
        textCheckinTime.html("From: " + data.from);
        textCheckoutTime.html("To: " + data.to);
        var totalMoney = (parseInt(data.serviceFee) + parseInt(data.rentalFee)).toString();

        textTotal.attr("data-rental-fee", data.rentalFee);
        textTotal.html("Total: " + $.number(totalMoney, 0, '.', ','));

        //clear old rows
        $("#js-table-service-rental tbody").empty();
        $("#js-table-service-rental tfoot").empty();

        if (data.serviceFee > 0) {
            $.ajax({
                url: "/API/ServiceRentals/" + rentalId,
                method: "GET",
                dataType: "json"
            }).done(function (data) {
                var sumQuantity = 0;
                var sumPrice = 0;
                for (var i = 0; i < data.serviceRentals.length; i++) {
                    sumQuantity += data.serviceRentals[i].quantity;
                    sumPrice += data.serviceRentals[i].sumPrice;
                    $("#js-table-service-rental tbody").append(
                        "<tr id='" + data.serviceRentals[i].serviceId + "'><td>" + data.serviceRentals[i].name + "</td>" +
                        "<td id='text-sum-quantity-" + data.serviceRentals[i].serviceId + "'>" + data.serviceRentals[i].quantity + "</td>" +
                        "<td id='text-sum-price-" + data.serviceRentals[i].serviceId + "'>" + $.number(data.serviceRentals[i].sumPrice, 0, '.', ',') + "</td>" +
                        "<td style='" + hideActionCol + "'>" +
                        "<a style='color:white;' class='btn btn-secondary js-service-edit' data-edit-value='1' data-service-price='" + data.serviceRentals[i].price +
                        "' data-service-id='" + data.serviceRentals[i].serviceId + "'>+</a>" +
                        "<a style='color:white;' class='btn btn-secondary js-service-edit' data-edit-value='-1' data-service-price='" + data.serviceRentals[i].price +
                        "' data-service-id='" + data.serviceRentals[i].serviceId + "'>-</a>" +
                        "</td></tr>"
                    );
                    if (i == (data.serviceRentals.length - 1)) {
                        $("#js-table-service-rental tfoot").append(
                            "<tr>" +
                            "<th></th>" +
                            "<th id='footer-sum-quantity'>" + sumQuantity + "</th>" +
                            "<th id='footer-sum-price'>" + $.number(sumPrice, 0, '.', ',') + "</th>" +
                            "<th style='" + hideActionCol + "'></th>" +
                            "</tr>"
                        );
                    }
                }

            })
                .fail(function (response) {
                    toastr.error(response.responseJSON.message)
                });
        }
        else {
            $("#js-table-service-rental tbody").append("<td colspan='4'>No service</td>");
        }
    })
        .fail(function () {
            toastr.error("Something wrong, please contact the one who made this");
        });

    $("#checkout-modal").modal({ backdrop: 'static', animation: 'false'});
    $("#checkout-modal").modal("show");
}

$(document).ready(function () {

    //////////////////////// CHECKOUT MODAL /////////////////////////////////////
    var rentalId = 0;
    var psId = 0;
    //show checkoutModal
    $("#bs-overwrite").on("click", ".btn-checkout", function () {
        $("#btn-save").removeAttr("disabled")
        $("#btn-checkout").removeAttr("disabled");
        rentalId = parseInt($(this).attr("data-rental-id"));
        psId = parseInt($(this).attr("data-ps-id"));
        psName = $(this).attr("data-ps-name")
        loadCheckOutModal(rentalId, psId, psName, "checkout");
    });

    $("#checkout-modal").on("hidden.bs.modal", function () {
        $("#collapse-split-time").collapse("hide");
        $(".split-time-bar").empty();
    });

    // Checkout service edit click event

    $("#js-table-service-rental").on("click", ".js-service-edit", function () {
        var button = $(this);
        var serviceId = button.attr("data-service-id");
        var trServiceQuantity = button.parents("tr").children("#text-sum-quantity-" + serviceId);
        var trServiceSum = button.parents("tr").children("#text-sum-price-" + serviceId);
        var trFooterSumQuantity = $("#footer-sum-quantity");
        var trFooterSumPrice = $("#footer-sum-price");
        var quantity = parseInt(trServiceQuantity.html());
        var price = parseInt(button.attr("data-service-price"));
        var value = button.attr("data-edit-value");
        var txtTotal = $("#js-text-total");
        var rentalFee = parseInt(txtTotal.attr("data-rental-fee"));

        // add click
        if (value > 0) {
            trServiceQuantity.html((quantity + 1));
            trServiceSum.html(price * (quantity + 1));
            trFooterSumQuantity.html(
                (parseInt(trFooterSumQuantity.html()) + 1)
            );
            trFooterSumPrice.html(
                $.number(intVal(trFooterSumPrice.html(), 0, '.', ',') + price)
            );
        }
            // minus click
        else {
            if (quantity > 0) {
                trServiceQuantity.html((quantity - 1));
                trServiceSum.html(price * (quantity - 1));

                trFooterSumQuantity.html(
                    (parseInt(trFooterSumQuantity.html()) - 1)
                );
                trFooterSumPrice.html(
                    $.number(intVal(trFooterSumPrice.html(), 0, '.', ',') - price)
                );
            }
            else {
                return false;
            }
        }

        txtTotal.html(
            "Total: " + $.number(rentalFee + intVal(trFooterSumPrice.html()), 0, '.', ',') + " VNĐ"
        );

        //Request to change serviceRental quantity (obsolete)

        //$.ajax({
        //    url: "/API/ServiceRental/EditServiceRental/" + serviceId + "/" + rentalId + "/" + value,
        //    method: "POST",
        //    datatype: "Json"
        //})
        //.done(function (xhr, status, error) {

        //    //toastr.success(xhr);
        //})
        //.fail(function (response) {
        //    toastr.error(response.responseJSON.message);
        //});
    });

    
    function checkOut(rentalId) {
        var psBox = $("div[data-rental-id='" + rentalId + "']");
        var psImg = psBox.children(".ps-img");
        $.ajax({
            url: "/API/Rentals/" + rentalId,
            method: "PUT"
        }).done(function (xhr, status, error) {
            psBox.attr("class", "ps-box");
            psImg.attr("class", "ps-img");
            $("#checkout-modal").modal("hide");
        })
        .fail(function (response) {
            toastr.error(response.responseJSON.message);
        });
    }

    //SAVE SERVICERENTALS
    function saveServiceRental(rentalId) {
        var serviceRentalDto = { serviceRentals: [] };
        serviceRentalDto.rentalId = rentalId;

        //select every tr element where parent is a tbody elem where parent is #js-table-service-rental elem
        $("#js-table-service-rental > tbody > tr").each(function () {
            var serviceId = parseInt($(this).attr("id"));
            var quantity = parseInt($(this).children("#text-sum-quantity-" + serviceId).html());

            serviceRentalDto.serviceRentals.push({ "serviceId": serviceId, "quantity": quantity });
        });

        if (serviceRentalDto.serviceRentals.length > 0) {
            $.ajax({
                url: "/api/ServiceRentals",
                method: "PUT",
                data: serviceRentalDto
            })
            .done(function (xhr, status, error) {
                toastr.success(xhr);
            })
            .fail(function (response) {
                toastr.error(response.responseJSON.message);
            });
        }
        else {
            toastr.success("Saved");
        }
        $("#checkout-modal").modal("hide");
    }

    $("#btn-save").click(function () {
        $("#btn-save").attr("disabled", "disabled");
        saveServiceRental(rentalId);
    });

    $("#btn-checkout").click(function () {
        $("#btn-checkout").attr("disabled", "disabled");
        bootbox.confirm("Are you sure want to checkout this rental ?", function (result) {
            if (result) {
                saveServiceRental(rentalId);
                checkOut(rentalId);
            }
            else {
                $("#btn-checkout").removeAttr("disabled");
            }
        });
    });

    //////////////////////// SERVICE MODAL /////////////////////////////////////

    //show addServiceModal
    $("#bs-overwrite").on("click", ".btn-service", function () {
        $("#btnSaveService").removeAttr("disabled");
        $("#serviceTitle").html("Add service - PS " + $(this).attr("data-ps-id"));
        $("#serviceSelect").attr("data-rental-id", $(this).attr("data-rental-id"))
        $("#service-modal").modal({ backdrop: 'static' });
        $("#service-modal").modal("show");
    });

    //serviceSelect multiselect initial
    $("#serviceSelect").multiselect({
        buttonWidth: '100%',
        dropRight: true,
        onChange: function (option, checked, select) {
            var quantityInputGroup = "<div class='input-group group" + option.val() + "'>" +
                                        "<div class='input-group-prepend'><label class='input-group-text'>" + option.text() + "</label></div>" +
                                        "<input readonly class='form-control svQuantity' id='quantityInput" + option.val() + "' data-service-id='" + option.val() + "' value='1' />" +
                                        "<div class='input-group-append'>" +
                                            "<button class='btn btn-outline-secondary' id='btnAdd" + option.val() + "' type='button'>+</button>" +
                                            "<button class='btn btn-outline-secondary' id='btnMinus" + option.val() + "' type='button'>-</button>" +
                                        "</div>"
            "</div>";
            if (checked) {
                $(".service-form-group").append(quantityInputGroup);
            }
            else {
                $(".group" + option.val()).remove();
            }
        }
    });

    //btnAdd and btnMinus click event
    $(".service-form-group").on("click", ".btn-outline-secondary", function () {
        var btnId = $(this).attr("id");
        var quantityInput = $("#quantityInput" + btnId.match(/\d+/));

        if (btnId.indexOf("btnAdd") != -1) {
            quantityInput.attr("value", parseInt(quantityInput.val()) + 1);
        }
        else {
            if (parseInt(quantityInput.val()) > 1) {
                quantityInput.attr("value", parseInt(quantityInput.val()) - 1);
            }
        }
    });

    //close serviceModal event
    $('#service-modal').on('hidden.bs.modal', function () {
        $('#serviceSelect').multiselect('deselectAll', false);
        $('#serviceSelect').multiselect('updateButtonText');
        $(".service-form-group").empty();
        // do something…
    })

    //SAVE serviceRental action
    $("#btnSaveService").click(function () {
        $("#btnSaveService").attr("disabled", "disabled");
        var serviceRentalDto = {
            serviceRentals: []
        };

        var numberOfService = $(".svQuantity").length;
        var rentalId = $("#serviceSelect").attr("data-rental-id");
        serviceRentalDto.rentalId = rentalId;

        if (numberOfService < 1) {
            toastr.error("Pick some service to add");
            return false;
        }

        //each iterates over the DOM elements with same name or tag then push value to the array
        $("#service-modal .svQuantity").each(function () {
            var serviceId = $(this).attr("data-service-id");
            var quantity = $(this).attr("value");
            serviceRentalDto.serviceRentals.push({ "serviceId": serviceId, "quantity": quantity })
        });

        console.log(serviceRentalDto);

        $.ajax({
            url: "/API/ServiceRentals/AddServiceRental",
            method: "POST",
            contentType: 'application/json',
            dataType: "json", //return type must be json or it will go to fail
            data: JSON.stringify(serviceRentalDto)
        })
        .done(function (xhr, status, error) {
            toastr.success(xhr);
            $("#service-modal").modal("hide");
        })
        .fail(function (response) {
            toastr.error(response.responseJSON.message);
            $("#service-modal").modal("hide");
        });
    });

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////////////////////////////Switch ps modal//////////////////////////////////////////////////
    function switchPsStatus(psId, rentalId, isOn) {
        var psBox = $("div[data-ps-id='" + psId + "']");

        if (isOn) {
            psBox.attr("class", "ps-box ps-rented");
            psBox.attr("data-rental-id", rentalId);
            psBox.removeAttr("data-content");

            psBox.children("img").attr("class", "ps-img ps-rented");
        }
        else {
            psBox.attr("class", "ps-box");
            psBox.removeAttr("data-rental-id");
            psBox.removeAttr("data-content");

            psBox.children("img").attr("class", "ps-img");
        }
    }

    $("#bs-overwrite").on("click", ".btn-switch", function () {
        var btnSwitch = $(this);
        var selectSwitchPs = $("#select-switch-ps");
        $("#title-switch-ps-modal").attr("data-ps-id", btnSwitch.attr("data-ps-id"));
        $("#title-switch-ps-modal").html("Switch PS " + btnSwitch.attr("data-ps-id"));
        selectSwitchPs.children("option").css("display", "inline-block");
        $("option[value='" + btnSwitch.attr("data-ps-id") + "']").css("display", "none");

        $("#switch-ps-modal").modal({backdrop: "static"});
        $("#switch-ps-modal").modal("show");
    });

    $("#btn-save-switch-ps").on("click", function () {
        var selectSwitchPs = $("#select-switch-ps");
        var oldPsId = $("#title-switch-ps-modal").attr("data-ps-id")
        var newPsId = selectSwitchPs.find(":selected").val();
        var oldPsBox = $("div[data-ps-id='" + oldPsId + "']");
        var newPsBox = $("div[data-ps-id='" + newPsId + "']");
        var oldRentalId = oldPsBox.attr("data-rental-id");
        var newRentalId = newPsBox.attr("data-rental-id");


        var psSwitchDto = { "oldPsId": oldPsId, "NewPsId": newPsId };

        //new psBox is not rented
        if (newPsBox.attr("class").indexOf("ps-rented") == -1) {
            //turn off old ps
            switchPsStatus(oldPsId, oldRentalId, false);
            //turn on new ps
            switchPsStatus(newPsId, oldRentalId, true);

            //new psBox is not rented
            psSwitchDto.case = "1";
        }
            //new psBox is rented
        else {
            //swap rentalID and keep both ps on
            switchPsStatus(oldPsId, newRentalId, true);
            switchPsStatus(newPsId, oldRentalId, true);

            //new psBox is rented
            psSwitchDto.case = "2";
        }

        $.ajax({
            url: "/API/Rentals/SwitchPs/1",
            method: "PUT",
            data: psSwitchDto
        })
        .done(function (xhr, status, error) {
            toastr.success("Switch ps successfully");
            $("#switch-ps-modal").modal("hide");
        })
        .fail(function () {
            toastr.error("Can't switch ps, try again later or contact the one who make this");
            $("#switch-ps-modal").modal("hide");
        });
    });
});
