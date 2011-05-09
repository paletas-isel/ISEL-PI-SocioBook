function View()
{	
	var Controller;
	
	this.SetController = function(controller)
	{
		Controller = controller;
	}
	
	this.Initialize = function() {
						$("#status_anchor").click(StatusAnchorsStatus_OnClick); StatusAnchorsStatus_OnClick();
						$("#photo_anchor").click(StatusAnchorsPhoto_OnClick);
						$("#link_anchor").click(StatusAnchorsLink_OnClick);
						$("#video_anchor").click(StatusAnchorsVideo_OnClick);

						$("#content-share-textarea").keyup(function() {
							Grow();
						});

                        $("#shares").hide();

                        $("#user-login > input[type='submit']").click(
                            function() { 
                                $("#user-login").hide(); 
                                $("#shares").show(); 
                                Controller.SetUser($("#user-login > input[type='text']")[0].value);
                            });

	                    SetOption(status);
					}
	
    function AddShare(addFunction) {    
        var content = $("#content-share-textarea")[0].value;
		addFunction(content);
        $("#content-share-textarea")[0].value = "";
    };

	function StatusAnchorsStatus_OnClick()
	{
        $("#content-share-submit").unbind("click").click(function() { AddShare(Controller.AddTextShare); });
		SetOption('status');
	};
	
	function StatusAnchorsPhoto_OnClick()
	{
		//document.getElementById("content-share-submit").onclick = Controller.SubmitTextShare_OnClick;
		SetOption('photo');
		alert('Not implemented!');
	};
	
	function StatusAnchorsLink_OnClick()
	{
		$("#content-share-submit").unbind("click").click(function() { AddShare(Controller.AddAnchorShare); });
		SetOption('link');
	};
	
	function StatusAnchorsVideo_OnClick()
	{
		$("#content-share-submit").unbind("click").click(function() { AddShare(Controller.AddVideoShare); });
		SetOption('video');
	};
	
	function IsValidUrl(text)
	{
		var pattern = /(\b(https?):\/\/[-A-Z0-9+&@#\/%?=~_|!:,.;]*[-A-Z0-9+&@#\/%=~_|])/ig;
		
		if (! text || ! pattern.test(text)) {
			return false;
		}
		
		return true;
		
	}
		
	function TransformIntoUrl(text)
	{
		if (! text)
			return;
			
		var pattern = /(\b(https?|ftp|file):\/\/[-A-Z0-9+&@#\/%?=~_|!:,.;]*[-A-Z0-9+&@#\/%=~_|])/ig;
		
		return text.replace(pattern, "<a href='$1'>$1</a>");
	};
	
	function GetIdFromYoutubeVideo(text)
	{
		var pattern = /^.*((v\/)|(embed\/)|(watch\?))\??v?=?([^\&\?]*).*/;
		
		if (! text || ! pattern.test(text))
			return null;
		
			return text.replace(pattern, "$5");
	};

	function GetThumbnail(url)
	{
		if (! IsValidUrl(url))
			return null;

		var t1 = "http://open.thumbshots.org/image.pxf?url=" + escape(url);
		return "<img src=\"" + t1 + "\" />";
	};
	

	var options = {
			status: {
				def: '',
				title: 'What\'s on your mind?'
			},
			
			photo: {
				def: '',
				title: 'Try some pictures. Valid only for girls!',
			},
			
			link: {
				def: 'http://',
				title: 'Post something from worldwide web...',
			},
			
			video: {
				def: '',
				title: 'Start by making a movie ; )',
			}
		};

	function SetOption(status) 
	{
		if (status && options[status]) {
			$("#content-share-textarea").value = options[status].def;
			$("#content-share-title")[0].innerHTML = options[status].title;
		}
	};
	
	function Grow() 
	{
				var textarea = document.getElementById('content-share-textarea');
				var currentHeight = textarea.clientHeight;
				var newHeight = textarea.scrollHeight;

				if (newHeight > currentHeight) {
					textarea.style.height = (newHeight + 1) + 'px';
		  		}
	};
	
	this.Share = function()
	{	
		var being_used = false;
		
		this.RegisterEvents = function()
		{
			//document.getElementById("write_share").onclick = share_input.ShareInput_OnClick;
			//document.getElementById("write_share").onkeypress = share_input.ShareInput_OnKeyPressed;
			$("#content-share-submit").click(share_input.ShareInput_Submit);			
		};
		
		this.ShareInput_OnClick = function()
		{		
			if(!being_used)
				document.getElementById("content-share-textarea").value = ""; //So se nada estiver escrito, falta verificar
			being_used = true;
		};
		
		function AddShare(share)
		{						
			$("#content-share-textarea").value = "";		
			$("#content-shared").prepend(share);
		}
		
		this.RemoveShare = function(share)
		{
			//$("#content-shared").remove(share);
            share.remove();
		}

		function Share(user, deleteF)
		{
			var textShare = $('<div></div>').addClass("share");

			var deleteShare = $('<div class="delete-share">X</div>').click(deleteF); 
			
			var userShare = $('<span class="user-share"></span>').append(user);
			
			var pShare = $('<p></p>');			
			
			textShare.append(deleteShare);
			textShare.append(userShare);
			textShare.append(pShare);					
			
			AddShare(textShare);
			
			return pShare;
		}		
		
		this.ShareText = function(share)
		{			
			var pShare = Share(share.GetUser(), function() {
													Controller.RemoveShare(pShare.parent(), share);
												});
			pShare.append(TransformIntoUrl(share.GetContent()));
		};
		
		this.ShareAnchor = function(share)
		{						
			var thumb = GetThumbnail(share.GetContent());
			    var pShare = Share(share.GetUser(), function() {
													    Controller.RemoveShare(pShare.parent(), share);
												    });
			
			// Temporary version (bad code!)
			if (thumb != null) {
				var aShare = $("<a></a>").attr("href", share.GetContent()).addClass("thumbnail-share").append($(thumb));
				
				pShare.append(aShare);
			}			
			else {
				pShare.append("Invalid URL!");
			}
		};
		
		this.ShareVideo = function(share)
		{			
			var id = GetIdFromYoutubeVideo(share.GetContent());
			
			if (id == null) {
				alert("The link does not contain a valid Youtube ID!");
				return;
			}

			var url = "http://www.youtube.com/embed/" + id;

			var iframeShare = $("<iframe></iframe>").addClass("video-share");
			iframeShare.attr("src", url);
			
			var pShare = Share(share.GetUser(), function() {
													Controller.RemoveShare(pShare.parent(), share);
												});
			pShare.append(iframeShare);
		};
	};
};