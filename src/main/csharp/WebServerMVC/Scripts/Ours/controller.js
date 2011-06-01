function Controller(view, model)
{
	var ShareContainer = new model.ShareContainer();
	var ShareView = new view.Share();
	var User = document.location.search.slice(document.location.search.indexOf("user=") + 5);
	this.MostRecentStamp = -1;
	this.OldestStamp = -1;
	Controller.TimeOut = 3000;
    
	function UpdateState(share) {
	    if (SocioBookController.MostRecentStamp == undefined || share.GetStamp() > SocioBookController.MostRecentStamp) SocioBookController.MostRecentStamp = share.GetStamp();

	    if (SocioBookController.OldestStamp == undefined || share.GetStamp() < SocioBookController.OldestStamp) SocioBookController.OldestStamp = share.GetStamp();
	};

	this.AddTextShare = function (content) {
	    var share = new model.TextShare(User, content);
	    share = ShareContainer.AddShare(share, function () {
	        ShareView.ShareText(share);
	        UpdateState(share);
	    });
	};

    this.AddAnchorShare = function (content)
	{
	    var share = new model.AnchorShare(User, content);
	    share = ShareContainer.AddShare(share, function () {
	        ShareView.ShareAnchor(share);
	        UpdateState(share);
	    });
	};

	this.AddVideoShare = function (content) {
	    var share = new model.VideoShare(User, content);
	    share = ShareContainer.AddShare(share, function () {
	        ShareView.ShareVideo(share);
	        UpdateState(share);
	    });
	};

	function AddShare(share) {
	    if (share.GetType() == "text") {
	        SocioBookController.AddTextShare(share.GetContent());
	    }
	    else if (share.GetType() == "anchor") {
	        SocioBookController.AddAnchorShare(share.GetContent());
	    }
	    else if (share.GetType() == "video") {
	        SocioBookController.AddVideoShare(share.GetContent());
	    }
	    UpdateState(share);
	}

	this.RemoveShare = function (shareStamp) {            
	    ShareContainer.RemoveShare(User, shareStamp, function () { ShareView.RemoveShare(shareStamp); });
	};

	function Synchronize(toAdd, toDelete) {
	    for (var i = 0; i < toAdd.length; ++i) {
	        if (toAdd[i].GetType() == "text") {
	            ShareView.ShareText(toAdd[i]);
	        }
	        else if (toAdd[i].GetType() == "anchor") {
	            ShareView.ShareAnchor(toAdd[i]);
	        }
	        else if (toAdd[i].GetType() == "video") {
	            ShareView.ShareVideo(toAdd[i]);
	        }
	        UpdateState(toAdd[i]);
	    }

	    for (var i = 0; i < toDelete.length; ++i) {
	        RemoveShareByStamp(toDelete[i].GetStamp());
	    }
	};

	this.UpdateShares = function() {
	    ShareContainer.GetShares(User, Synchronize, SocioBookController.MostRecentStamp, SocioBookController.OldestStamp);
	};

	this.SetUser = function (nUser) {
	    User = nUser;
	};
	
	view.SetController(this);
	
	$(function() {	
		view.Initialize();
    });
}
