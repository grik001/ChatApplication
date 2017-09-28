var ChatHub = React.createClass({
    getInitialState: function () {
        return { chats: [] };
    },

    startChat: function (id) {
        var chats = this.state.chats;
        chats.push({ id: id });
        this.setState({ chats: chats });
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
            <div className="row">
                <div className="col-md-3">
                    {
                        this.state.chats.map(chat => <ChatListItem key={chat.id} username={chat.id} />)
                    }
                </div>
                <div className="col-md-1"></div>
                <div className="col-md-8">
                    <ChatWindow />
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
            <div className="chatListItem row">
                <div className="col-md-2">
                    <img className="img-rounde chatListItemImage" src="https://camo.githubusercontent.com/6d2c494388b835ccabf99146799a47b4eaf70d19/68747470733a2f2f662e636c6f75642e6769746875622e636f6d2f6173736574732f353838333136362f313439383237322f63393038303566342d343832612d313165332d383734342d6162373266303764336263622e706e67" />
                </div>
                <div className="col-md-10">
                    <div className="">{this.props.username}</div>
                    <div className="">Last message ...</div>
                </div>
            </div>
        );
    }
});

var ChatWindow = React.createClass({
    getInitialState: function () {
        return { messages: [], id: "" };
    },

    componentWillMount: function () {
        var chatID = this.props.id;
    },

    render: function () {
        return (
            <div>
                <div className="row chatWindow">
                    <MessageList messages={this.state.messages}></MessageList>
                </div>
                <div className="row">
                    <div className="col-md-8">
                        <input className="col-md-12 form-control" type="text" value="" />
                    </div>
                    <div className="col-md-4">
                        <input className="col-md-5 btn btn-primary pull-right chatButton" type="button" value="Send" />
                        <input className="col-md-5 btn btn-secondary pull-right chatButton" type="button" value="Clear" />
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