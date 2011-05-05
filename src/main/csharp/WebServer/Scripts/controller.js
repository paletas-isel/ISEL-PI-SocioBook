function Controller(view, model)
{
	var ShareContainer = new model.ShareContainer();
	var ShareView = new view.Share();
		
	this.SubmitTextShare_OnClick = function()
	{
		var share = new model.TextShare("Dummy", document.getElementById("content-share-textarea").value);
        share = ShareContainer.AddShare(share, function() { ShareView.ShareText(share) });		
			
	};
	
	this.SubmitLinkShare_OnClick = function()
	{
		var share = new model.AnchorShare("Dummy", document.getElementById("content-share-textarea").value);
        share = ShareContainer.AddShare(share, function() { ShareView.ShareAnchor(share) });	
	};
	
	this.SubmitVideoShare_OnClick = function()
	{
	    var share = new model.VideoShare("Dummy", document.getElementById("content-share-textarea").value);
	    share = ShareContainer.AddShare(share, function () { ShareView.ShareVideo(share) });		
	};

	this.RemoveShare = function(shareHTML, shareModel)
	{
		ShareView.RemoveShare(shareHTML);
		ShareContainer.RemoveShare(shareModel);
	}
	
	view.SetController(this);
	
	$(function() {	
		view.Initialize();
	});
}
