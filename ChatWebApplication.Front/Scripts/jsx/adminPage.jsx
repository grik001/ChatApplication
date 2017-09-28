var ChatHub = React.createClass({
    getInitialState: function () {
        return { chats: [], currentchat: { id: '', username:'', messages: [] } };
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

        var agent = $.connection.agentHub;
        var chat = $.connection.chatHub;

        chat.client.startChat = this.startChat;

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
                <div className="col-md-8 chatWindowContainer">
                    <ChatWindow currentchat={this.state.currentchat}  />
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
    componentWillMount: function () {
        var chatID = this.props.id;
    },

    sendMessage: function () {
        var id = this.props.currentchat.id;
        var text = this.state.textValue;
    },

    render: function () {
        return (
            <div className="panel panel-default">
                <div className="panel-heading">
                    User: {this.props.currentchat.username}
                </div>
                <div className="panel-body">
                    <div className="container chatWindow">
                        <MessageList messages={this.props.currentchat.messages}></MessageList>
                    </div>
                    <div className="chatWindowButtonSection panel-footer">
                        <div className="input-group">
                            <input type="text" className="form-control" />
                            <span className="input-group-btn">
                                <button className="btn btn-default" type="button">Send</button>
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
                {
                    this.props.messages.map(message => <Message key={message.id} text={message.text} username={message.username} />)
                }
            </div>
        );
    }
});

var Message = React.createClass({
    render: function () {
        return (
            <div>
                <label>{this.props.username}</label>
                <label>{this.props.text}</label>
            </div>
        );
    }
});


ReactDOM.render(
    <ChatHub />,
    document.getElementById('content')
);