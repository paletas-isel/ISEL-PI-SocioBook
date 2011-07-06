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
        userlist.append($("<div id=" + username + ">" + username + "</div>"));
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

    this.ProcessMessage = function(message) {
        var messages = $("#chat-messages");
        var splitMessage = message.split(':', 2);
        if (splitMessage.length == 2) {
            var user = splitMessage[0];
            var m = splitMessage[1];

            messages.append($("<div class='char-message'><span class='chat-message-username'>" + user + "</span> said <span class='char-message-m'>" + m + "</span>.</div>"));
        }
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
            var message = event.data;

            if (message.charAt(0) == 'L') {
                model.ReceivedUserList(message.substr(1));
            }
            else if (message.charAt(0) == 'M') {
                model.ReceivedMessage(message.substr(1));
            }
            else if (message.charAt(0) == 'U') {
                model.ReceivedUserJoin(message.substr(1));
            }
            else if (message.charAt(0) == 'R') {
                model.ReceivedUserLeft(message.substr(1));
            }
        };
    }

    this.NoSupport;
    this.ReceivedUserList;
    this.ReceivedUserJoin;
    this.ReceivedUserLeft;
    this.ReceivedMessage;
    this.StatusChanged;
}

function ChatController(view, model) {
    view.SendMessage = function (message) {
        model.SendMessage(message);
        view.ProcessMessage("You:"+message);
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
}

new ChatController(new ChatView(), new ChatModel());