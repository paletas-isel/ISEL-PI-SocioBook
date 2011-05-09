function Wall(controller) {
    $(function () {
        $(".share > .delete-share").click(function (e) {
            var share = $(e.target).parent()[0];
            controller.RemoveShare(share.id);
        });
    });
};