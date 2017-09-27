var ChatHub = React.createClass({
    getInitialState: function () {
        return { chats: [{ id: '123' }] };
    },

    startChat: function (id) {
        console.log(this.state.chats);
        var chats = this.state.chats;
        chats.push(id);
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
            <div>
                {
                    this.state.chats.map(chat => <Chat key={chat.id} />)
                }
            </div>
        );
    }
});

var Chat = React.createClass({
    getInitialState: function () {
        return { messages: [], id: "" };
    },

    componentWillMount: function () {
        var chatID = this.props.id;


    },

    render: function () {
        return (
            <div>
                <div>
                    <MessageList messages={this.state.messages}></MessageList>
                </div>
                <div>
                    <input type="text" value="" />
                </div>
                <div>
                    <input type="button" value="Send" />
                    <input type="button" value="Clear" />
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