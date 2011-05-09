function Controller(view, model, mostRecent, oldest)
{
	var ShareContainer = new model.ShareContainer();
	var ShareView = new view.Share();
	var User = document.location.search.slice(document.location.search.indexOf("user=") + 5);
	var MostRecentStamp = mostRecent;
	var OldestStamp = oldest;
	Controller.TimeOut = 1000;
    
	function UpdateState(share) {
	    if (MostRecentStamp == undefined || share.stamp > MostRecentStamp) MostRecentStamp = share.stamp;

	    if (OldestStamp == undefined || share.stamp < OldestStamp) OldestStamp = share.stamp;
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

	this.AddVideoShare = function (content) {
	    var share = new model.VideoShare(User, content);
	    share = ShareContainer.AddShare(share, function () { ShareView.ShareVideo(share); });
	    UpdateState(share);
	};

	function AddShare(share) {
	    if (share.GetType() == "text") {
	        AddTextShare(share.GetContent());
	    }
	    else if (share.GetType() == "anchor") {
	        AddAnchorShare(share.GetContent());
	    }
	    else if (share.GetType() == "video") {
	        AddVideoShare(share.GetContent());
	    }
	}

	this.RemoveShare = function (shareStamp) {
	    ShareContainer.RemoveShare(User, shareStamp, function () { ShareView.RemoveShare(shareStamp); });
	};

	function Synchronize(toAdd, toDelete) {
	    for (var i = 0; i < toAdd.length; ++i) {
	        AddShare(toAdd[i]);
	    }

	    for (var i = 0; i < toDelete.length; ++i) {
	        RemoveShareByStamp(toDelete[i].GetStamp());
	    }
	};

	this.UpdateShares = function() {
	    ShareContainer.GetShares(User, Synchronize, MostRecentStamp, OldestStamp);
	};

	this.SetUser = function (nUser) {
	    User = nUser;
	};
	
	view.SetController(this);
	
	$(function() {	
		view.Initialize();
    });

    setInterval(this.UpdateShares, Controller.TimeOut);
}
