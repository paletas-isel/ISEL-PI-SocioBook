function Wall(controller) {
    $(function () {
        $(".share > .delete-share").click(function (e) {
            var share = $(e.target).parent()[0];
            controller.RemoveShareByStamp($(share), share.id);
        });
    });
};