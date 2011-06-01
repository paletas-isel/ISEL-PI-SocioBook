var controller = SocioBookController;

$(function () {
    $(".share > .delete-share").click(function (e) {
        var share = $(e.target).parent()[0];
        controller.RemoveShare(share.id);
    });
});

$(function () {
    var shares = $(".share");
    if (shares.length > 0) {
        controller.MostRecentStamp = Number(shares[0].id);
        controller.OldestStamp = Number(shares[shares.length - 1].id);
    }
});

function TimeoutFunc() { controller.UpdateShares();setTimeout(TimeoutFunc, Controller.TimeOut);}

setTimeout(TimeoutFunc, Controller.TimeOut);