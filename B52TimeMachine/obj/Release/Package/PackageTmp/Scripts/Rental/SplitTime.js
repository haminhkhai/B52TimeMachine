

//////////////////////////////////////////////////
/////////////////////Split Time///////////////////
function kFormatter(num) {
    return num > 999 ? (num / 1000).toFixed() + 'k' : num
}

function randomChosenColors() { //function name
    var color = '#'; // hexadecimal starting symbol
    var letters = ['a2f0d6', 'a2cff0', 'a2f0d0', 'bcf0a2', 'ebf0a2', 'f0a2e9', '7af0f6', 'd3a2f0']; //Set your colors here
    color += letters[Math.floor(Math.random() * letters.length)];
    return color;
}

$(document).ready(function () {
    $("#bs-overwrite").on("click", ".btn-split", function () {
        var button = $(this);
        var rentalId = button.attr("data-rental-id");

        $.ajax({
            url: "/Api/SplitTimes/CreateTimeSplit/" + rentalId,
            method: "POST"
        })
        .done(function (response) {
            toastr.success("Saved");
        })
        .fail(function (response) {
            toastr.error(response.responseJSON.message);
        });
    });

    $("#collapse-split-time").on("shown.bs.collapse", function () {
        var rentalId = $("#js-text-checkout").attr("data-rental-id");
        var splitTimeBar = $(".split-time-bar");

        if (splitTimeBar.children().length > 0) {
            return false;
        }

        $.ajax({
            url: "/Api/SplitTimes/" + rentalId,
            method: "GET"
        })
        .done(function (data) {
            for (var i = 0; i < data.length; i++) {
                splitTimeBar.append(
                    "<div class='split-time-item' style='background-color:" + randomChosenColors() +
                    ";width:" + data[i].splitSpanRatio + "%'><span>" + data[i].splitSpan + "-" + kFormatter(data[i].splitFee) + "</span></div>"
                );
            }
        })
        .fail(function (response) {
            toastr.error(response.responseJSON.message);
        });
    });
});

