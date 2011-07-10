function ChatView() {
    var view = this;

    this.SetStatusOnline = function (isonline) {
        var canvas = $("#chat-status-icon");
        var context = canvas.get()[0].getContext('2d');
        canvas.empty();

        if (isonline) {
            canvas.append("Online");
            context.strokeStyle = '#33FF00';
            context.fillStyle = '#33FF00';
        }
        else {
            canvas.append("Offline");
            context.strokeStyle = '#FF0000';
            context.fillStyle = '#FF0000';

            EmptyUserList();
        }

        context.arc(5, 5, 5, 0, Math.PI * 2, true);
        context.fill();
    };    

    this.AddUser = function(username) {
        var userlist = $("#chat-users-list");
        var div = $("<div id=" + username + ">" + username + "</div>");
        userlist.append(div);
        div.click(function() {
            var chatmess = $("#chat-message-text");
            if(chatmess.val() == "")
                $("#chat-message-text").attr("value", '@'+username+' ');
            else
                $("#chat-message-text").attr("value", '@'+username+' '+chatmess.val());
        });        
    }

    this.RemoveUser = function(username) {
        $("#" + username).remove();
    }

    this.ProcessUserList = function(message) {
        var usernames = message.split(';');
        for (var i = 0; i < usernames.length; ++i) {
            view.AddUser(usernames[i]);
        }
    };

    this.ProcessMessage = function (message, klass) {
        if (klass == undefined)
            klass = "chat-message";
        var messages = $("#chat-messages");
        var splitMessage = message.split(':', 2);
        if (splitMessage.length == 2) {
            var user = splitMessage[0];
            var m = splitMessage[1];

            messages.append($("<div class='" + klass + "'><span class='chat-message-username'>" + user + "</span> said <span class='char-message-m'>" + m + "</span>.</div>"));
        }
    }

    this.ProcessPrivateMessage = function (message) {
        this.ProcessMessage(message, "chat-private-message");    
    }

    this.SendMessage;
    function EmptyUserList() {
        $("#chat-users-list").empty();
    }

    this.ShowUnavailable = function (message) {
        $("#chat-status").append($("<p>"+message+"</p>"));
    }

    $(function () {
        view.SetStatusOnline(false);

        $("#chat-message-sender").click(function () {
            var message = $("#chat-message-text").val();

            if (message != "") {
                $("#chat-message-text").attr("value", "");
                view.SendMessage(message);
            }
        });
    });
}

function ChatModel() {
    var model = this;
    var status;

    if (window.WebSocket != undefined) {
        var socket;
        ConnectSocket();
        setInterval(function () {
            if (!status) {
                ConnectSocket();
            }
        }, 5000);

        this.SendMessage = function (message) {
            if(socket != undefined)
                socket.send(message);
        };
    } else {
        model.NoSupport("Chat is not supported in this browser!");
    }

    function ConnectSocket() {
        socket = new WebSocket("ws://localhost:4500/chat");

        socket.onerror = function () {
            model.NoSupport("There was an error while connecting! Server might be down..");
        };

        socket.onopen = function () {
            model.StatusChanged(status = true);
        };

        socket.onclose = function () {
            model.StatusChanged(status = false);
        };

        socket.onmessage = function (event) {
            var message = event.data.substr(1);
            var message_type = event.data.charAt(0);

            if (message_type == 'L') {
                model.ReceivedUserList(message);
            }
            else if (message_type == 'M') {
                model.ReceivedMessage(message);
            }
            else if (message_type == 'U') {
                model.ReceivedUserJoin(message);
            }
            else if (message_type == 'R') {
                model.ReceivedUserLeft(message);
            }
            else if (message_type == 'P') {
                model.ReceivedPrivateMessage(message);
            }
        };
    }

    this.NoSupport;
    this.ReceivedUserList;
    this.ReceivedUserJoin;
    this.ReceivedUserLeft;
    this.ReceivedMessage;
    this.ReceivedPrivateMessage;
    this.StatusChanged;
}

function ChatController(view, model) {
    view.SendMessage = function (message) {
        model.SendMessage(message);

        var ix = message.indexOf('@');
        if(ix != -1) {
            message = message.substr(message.indexOf(' '));
        }

        view.ProcessMessage("You:" + message);
    };

    model.NoSupport = function (message) {
        view.ShowUnavailable(message);
    };

    model.ReceivedUserList = function (list) {
        view.ProcessUserList(list);
    };

    model.ReceivedUserJoin = function(user) {
        view.AddUser(user);
    };

    model.ReceivedUserLeft = function(user) {
        view.RemoveUser(user);
    };

    model.ReceivedMessage = function(message) {
        view.ProcessMessage(message);  
    };

    model.StatusChanged = function(online) {
        view.SetStatusOnline(online);
    };

    model.ReceivedPrivateMessage = function (message) {
        view.ProcessPrivateMessage(message);
    }
}

new ChatController(new ChatView(), new ChatModel());