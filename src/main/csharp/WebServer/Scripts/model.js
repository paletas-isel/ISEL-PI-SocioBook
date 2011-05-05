function Model()
{
	function Share(newUser) 
	{
		var user = newUser;
		
		this.GetUser = function() {
			return user;
		}
								
		this.GetContent = function() {
			return undefined;
		}

		this.Equals = function(that) {
			if(this != undefined && that != undefined)
				return this.GetUser() == that.GetUser() && this.GetContent() == that.GetContent();
		}
	}
	
	this.TextShare = function(newUser, newContent)
	{
		Share.call(this, newUser);
		var content = newContent;
							
		this.GetContent = function() {
			return content;
		}
	}
	
	this.AnchorShare = function(newUser, newURL)
	{
		Share.call(this, newUser);
		var URL = newURL;
							
		this.GetContent = function() {
			return URL;
		}
	}
	
	this.VideoShare = function(newUser, newVideo)
	{
		Share.call(this, newUser);
		var video = newVideo;
							
		this.GetContent = function() {
			return video;
		}
	}

	this.ShareContainer = function()
	{
		var shares = [];
		
		this.AddShare = function(share, callback) {
			shares.push(share);	
			return share;
		}

        this.RemoveShare = function (share, callback) {
			shares.forEach(function(oShare, shareIdx)
							{
								if(share.Equals(oShare))
									shares.splice(shareIdx, 1);
							});
			return share;
		}
	}
}