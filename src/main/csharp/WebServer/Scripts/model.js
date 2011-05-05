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
		
		this.AddShare = function(share) {
			shares.push(share);	
			return share;
		}

		this.RemoveShare = function(share) {
			shares.forEach(function(oShare, shareIdx)
							{
								if(share.Equals(oShare))
									shares.splice(shareIdx, 1);
							});
			return share;
		}
    }

/****************************************** Parte com AJAX ********************************************/
	/*this.ShareContainer = function () {

	    this.AddShare = function (share) {
	        sendShare(createXHR(), share, "A");
	        return share;
	    }

	    this.RemoveShare = function (share) {
	        sendShare(createXHR(), share, "R");
	        return share;
	    }
	}

	function createXHR() {
	    var xhr;
	    if (window.XMLHttpRequest) {
	        // If IE7, Mozilla, Safari, etc.
	        xhr = new XMLHttpRequest()
	    }
	    else {
	        if (window.ActiveXObject) {

	            // ...otherwise, use the ActiveX control for IE5.x and IE6
	            xhr = new ActiveXObject("Microsoft.XMLHTTP");
	        }
	    }
	    return xhr;
	}

	function sendShare(xhr, share, action) {
	    xhr.open("POST", "../Handlers/SharesHandler.ashx", true);

	    xhr.onreadystatechange = function () {
	        if (xhr.readyState == 4 && xhr.status == 200) {
	            alert(xhr.responseText);
	        }
	    }

        xhr.setRequestHeader("Content-Type", "text/plain;charset=UTF-8");
	    xhr.send(  action + "\n"
                 + share.GetUser() + "\n"
                 + share.GetContent()
                );
	}*/
}