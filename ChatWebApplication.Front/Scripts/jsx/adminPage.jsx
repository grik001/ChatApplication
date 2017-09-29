var ChatHub = React.createClass({
    getInitialState: function () {
        return { chats: [], currentchat: { id: '', username: '', messages: [] }, chatHub: $.connection.chatHub, agentHub: $.connection.agentHub };
    },

    addNewMessageToPage: function (targetClient, username, message, isMe) {
        var result = $.grep(this.state.chats, function (e) { return e.id == targetClient; });

        var message = { username: username, text: message, isMe: isMe };
        result[0].messages.push(message);

        var foundIndex = this.state.chats.findIndex(x => x.id == targetClient);
        this.state.chats[foundIndex] = result[0];

        this.setState({ chats: this.state.chats });
    },

    startChat: function (id, clientName) {
        var chats = this.state.chats;
        chats.push({ id: id, username: clientName, messages: [] });
        this.setState({ chats: chats });
    },

    activateChat: function (id) {
        var result = $.grep(this.state.chats, function (e) { return e.id == id; });
        this.setState({ currentchat: result[0] });
    },

    componentWillMount: function () {
        $.connection.hub.url = "http://localhost:8090/signalr";

        var agent = this.state.agentHub;
        var chat = this.state.chatHub;

        this.state.chatHub.client.startChat = this.startChat;
        this.state.chatHub.client.addNewMessageToPage = this.addNewMessageToPage;

        $.connection.hub.start().done(function () {
            agent.server.startAgent($('#displayname').val(), true);
        });
    },

    render: function () {
        return (
            <div className="row chatHub">
                <div className="col-md-4 chatListContainer">
                    {
                        this.state.chats.map(chat => <ChatListItem key={chat.id} chat={chat} activateChat={this.activateChat.bind(null, chat.id)} />)
                    }
                </div>
                <div className="col-md-8 noPadding chatWindowContainer">
                    <ChatWindow currentchat={this.state.currentchat} chatHub={this.state.chatHub} />
                </div>
            </div>
        );
    }
});

var ChatListItem = React.createClass({
    componentWillMount: function () {
    },

    render: function () {
        return (
            <div onClick={this.props.activateChat} className="row sidebar-body">
                <div className="col-sm-3 col-xs-3">
                    <div className="avatar-icon">
                        <img className="chatListItemImage" src="https://bootdey.com/img/Content/avatar/avatar1.png" />
                    </div>
                </div>
                <div className="col-sm-9 col-xs-9 chatListItemData">
                    <div className="row">
                        <div className="col-sm-8 col-xs-8 sideBar-name">
                            <span className="name-meta">{this.props.chat.username}</span>
                        </div>
                        <div className="col-sm-4 col-xs-4 pull-right sideBar-time">
                            <span className="time-meta pull-right">18:18</span>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
});


var ChatWindow = React.createClass({
    getInitialState: function () {
        return { chatText: '' };
    },

    componentWillMount: function () {
        var chatID = this.props.id;
    },

    sendMessage: function () {
        var id = this.props.currentchat.id;
        var text = this.state.chatText;

        this.props.chatHub.server.sendMessageToClient(id, text);
    },

    chatTextChange: function (event) {
        var chatText = event.target.value;
        this.setState({ chatText: chatText });
    },


    render: function () {
        return (
            <div className="panel panel-default chatWindowOuter">
                <div className="panel-heading">
                    User: {this.props.currentchat.username}
                </div>
                <div className="panel-body chatWindowBody">
                    <div className="chatWindow">
                        <MessageList messages={this.props.currentchat.messages}></MessageList>
                    </div>
                    <div className="chatWindowButtonSection panel-footer">
                        <div className="input-group">
                            <input type="text" className="form-control" value={this.state.chatText} onChange={this.chatTextChange} />
                            <span className="input-group-btn">
                                <button className="btn btn-default" onClick={this.sendMessage} type="button">Send</button>
                                <button className="btn btn-default" type="button">Clear</button>
                            </span>
                        </div>
                    </div>
                </div>

            </div>
        );
    }
});

var MessageList = React.createClass({
    render: function () {
        return (
            <div className="col-md-12">
                <ul class="chat">
                    {
                        this.props.messages.map(message => <Message key={message.id} text={message.text} username={message.username} />)
                    }
                </ul>
            </div>
        );
    }
});

var Message = React.createClass({
    render: function () {
        return (
                <li className="left clearfix">
                    <span className="chat-img pull-left">
                        <img src="http://placehold.it/50/55C1E7/fff&amp;text=U" alt="User Avatar" className="img-circle" />
                    </span>
                    <div className="chat-body clearfix">
                        <div className="header">
                            <strong className="primary-font">{this.props.username}</strong>
                            <small className="pull-right text-muted">
                                <span className="glyphicon glyphicon-time"></span>12 mins ago
                            </small>
                        </div>
                        <p>
                            {this.props.text}
                        </p>
                    </div>
                </li>
        );
    }
});

ReactDOM.render(
    <ChatHub />,
    document.getElementById('content')
);