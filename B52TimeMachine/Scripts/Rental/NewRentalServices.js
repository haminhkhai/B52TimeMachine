$(document).ready(function () {

    //////////////////////// INITIAL THINGS /////////////////////////////////////

    //add attributes to psBox on click
    $("#psPanel").on("focus", ".ps-box", function () {
        //$(".ps-box").click(function() {
        var psBox = $(this);
        var psName = psBox.children(".ps-title").html();

        if (psBox.attr("class").indexOf("ps-rented") == -1) {
            psBox.popover('hide');
            return false;
        }
        else {
            psBox.attr("data-content", "<a data-ps-name='" + psName + "' data-rental-id='" + psBox.attr("data-rental-id") + "' data-ps-id='" + psBox.attr("data-ps-id") + "' class='btn-checkout'>Check out</a>" +
                                   "<a data-rental-id='" + psBox.attr("data-rental-id") + "'  data-ps-id='" + psBox.attr("data-ps-id") + "' class='btn-service'>Services</a>" +
                                   "<a data-rental-id='" + psBox.attr("data-rental-id") + "'  data-ps-id='" + psBox.attr("data-ps-id") + "' class='btn-split'>Split time</a>" +
                                   "<a data-rental-id='" + psBox.attr("data-rental-id") + "'  data-ps-id='" + psBox.attr("data-ps-id") + "' class='btn-switch'>Switch</a>");
            psBox.popover('show');
        }
    });

    //popover initial
    $('[data-toggle="popover"]').popover({
        html: true,
        animation: true,
        trigger: 'focus'
    });

    //////////////////////// NEW RENTAL MODAL /////////////////////////////////////

    //show newRental modal
    var clickedPsBox;
    $("#psPanel").on("click", ".ps-box", function () {
        clickedPsBox = $(this);

        if (clickedPsBox.attr("class") == "ps-box ps-rented") {
            return false;
        }

        $("#btnSaveNewRental").removeAttr("disabled");
        $("#newRentalTitle").html("New rental - PS " + clickedPsBox.attr("data-ps-id"));
        $("#btnSaveNewRental").attr("data-ps-id", clickedPsBox.attr("data-ps-id"));
        $("#new-rental-modal").modal("show");
    });

    //save newRental action
    $("#btnSaveNewRental").click(function () {
        var psId = $(this).attr("data-ps-id");
        $("#btnSaveNewRental").attr("disabled", "disabled");
        $("#new-rental-modal").modal('hide');
        $.ajax({
            url: "/API/Rentals/" + psId,
            method: "POST"
        })
        .done(function (response) {
            clickedPsBox.attr("data-rental-id", response);
            clickedPsBox.attr("class", "ps-box ps-rented");
            clickedPsBox.children(".ps-img").attr("class", "ps-img ps-rented");
            
        })
        .fail(function (response) {
            toastr.error(response.responseJSON.message);
            clickedPsBox.attr("class", "ps-box ps-rented");
            clickedPsBox.children(".ps-img").attr("class", "ps-img ps-rented");
            
        });
    });
});