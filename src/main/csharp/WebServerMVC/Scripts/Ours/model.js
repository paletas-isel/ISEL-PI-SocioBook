function Model()
{
	function Share(newUser, nType, nContent) 
	{
		var user = newUser;
		var type = nType;
		var stamp;
        var content = nContent; 

		this.GetUser = function() {
			return user;
		}
								
		this.GetContent = function() {
			return content;
		}

		this.GetType = function () {
		    return type;
		}

		this.SetStamp = function (nstamp) {
		    stamp = nstamp;
		}

        this.GetStamp = function(){
            return stamp;
        }

		this.Equals = function(that) {
			if(this != undefined && that != undefined)
				return this.GetUser() == that.GetUser() && this.GetContent() == that.GetContent();
		}
	}
	
	this.TextShare = function(newUser, newContent)
	{
		Share.call(this, newUser, "text", newContent);
	}
	
	this.AnchorShare = function(newUser, newURL)
	{
	    Share.call(this, newUser, "anchor", newURL);
	}
	
	this.VideoShare = function(newUser, newVideo)
	{
		Share.call(this, newUser, "video", newVideo);
	}

	this.ShareContainer = function () {
	    var shares = [];

	    this.AddShare = function (share, callback) {
	        $.post("/Shares/Add", { user: share.GetUser(), type: share.GetType(), content: share.GetContent() }, function (data) { share.SetStamp(Number(data)); shares.push(share); callback(); });

	        return share;
	    };

	    function Remove(stamp, callback) {
	        shares.forEach(function (oShare, shareIdx) {
	            if (oShare.GetStamp() == stamp)
	                shares.splice(shareIdx, 1);
	        });

	        callback();
	    }

	    this.RemoveShare = function (user, stamp, callback) {
	        $.post("/Shares/Remove", { user: user, stamp: stamp }, function () { Remove(stamp, callback); });
	    }

	    this.GetShares = function (user, callback, newestStamp, oldestStamp) {
	        var params;
	        if (newestStamp == undefined) {
	            params = { user: user };
	        }
	        else {
	            params = { user: user, newestStamp: newestStamp };
	        }

	        $.post("/Shares/Get", params, function (data) {
	            var shares = [];

	            for (var i = 0; i < data.length; ++i) {
	                var obj = data[i];
	                shares[i] = new Share(obj.User, obj.Type, obj.Content);
	                shares[i].SetStamp(obj.Stamp);
	            }

	            callback(shares, []);
	        });
	    };

	}
}