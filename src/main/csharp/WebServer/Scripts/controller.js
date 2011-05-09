function Controller(view, model)
{
	var ShareContainer = new model.ShareContainer();
	var ShareView = new view.Share();
	var User;
	var MostRecentStamp;
	var OldestStamp;

	function UpdateState(share) {
	    if (share.stamp > MostRecentStamp) MostRecentStamp = share.stamp;

	    if (share.stamp < OldestStamp) OldestStamp = share.stamp;
	};

	this.AddTextShare = function (content) {
	    var share = new model.TextShare(User, content);
	    share = ShareContainer.AddShare(share, function () { ShareView.ShareText(share) });
	    UpdateState(share);
	};

    this.AddAnchorShare = function (content)
	{
	    var share = new model.AnchorShare(User, content);
	    share = ShareContainer.AddShare(share, function () { ShareView.ShareAnchor(share) });
	    UpdateState(share);
	};

    this.AddVideoShare = function (content)
	{
	    var share = new model.VideoShare(User, content);
	    share = ShareContainer.AddShare(share, function () { ShareView.ShareVideo(share) });
	    UpdateState(share);		
	};

	this.RemoveShare = function (shareHTML, shareModel) {
	    ShareView.RemoveShare(shareHTML);
	    ShareContainer.RemoveShare(shareModel);
	};

	this.UpdateShares = function () {
        ShareContainer.   
	};

	this.SetUser = function (nUser) {
	    User = nUser;
	};
	
	view.SetController(this);
	
	$(function() {	
		view.Initialize();
	});
}
