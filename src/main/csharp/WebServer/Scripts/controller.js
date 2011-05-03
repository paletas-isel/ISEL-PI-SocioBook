function Controller(view, model)
{
	var ShareContainer = new model.ShareContainer();
	var ShareView = new view.Share();
		
	this.SubmitTextShare_OnClick = function()
	{
		var share = ShareContainer.AddShare(new model.TextShare("Dummy", document.getElementById("content-share-textarea").value));
		
		ShareView.ShareText(share);		
	};
	
	this.SubmitLinkShare_OnClick = function()
	{
		var share = ShareContainer.AddShare(new model.AnchorShare("Dummy", document.getElementById("content-share-textarea").value));
		
		ShareView.ShareAnchor(share);	
	};
	
	this.SubmitVideoShare_OnClick = function()
	{
		var share = ShareContainer.AddShare(new model.VideoShare("Dummy", document.getElementById("content-share-textarea").value));
		
		ShareView.ShareVideo(share);
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
