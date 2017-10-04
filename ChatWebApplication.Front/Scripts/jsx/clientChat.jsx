var MessageList = React.createClass({
    render: function () {
        return (
            <div className="col-md-12">
                <ul className="chatMessageList">
                    {
                        this.props.messages.map(message => <Message key={message.id} text={message.text} username={message.username} isMe={message.isMe} />)
                    }
                </ul>
            </div>
        );
    }
});

var Message = React.createClass({
    render: function () {
        let liMessage = null;

        if (!this.props.isMe) {

            liMessage =

                < li className="left clearfix" >
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
                </li>;

        } else {

            liMessage =

                <li className="right clearfix">
                    <div className="chat-body clearfix">
                        <div className="header">
                            <small className=" text-muted"><span className="glyphicon glyphicon-time"></span>13 mins ago</small>
                            <strong className="pull-right primary-font">{this.props.username}</strong>
                        </div>
                        <p>
                            {this.props.text}
                        </p>
                    </div>
                </li>;
        }


        return (
            liMessage
        );
    }
});

var ChatSupport = React.createClass({
    getInitialState: function () {
        return { chatText : "", chatHub: $.connection.chatHub, agentHub: $.connection.agentHub, messages: []};
    },

    openChat: function (e) {
        var chatWindowIsOpen = $("#clientChatWindow").hasClass("chatWindowFooterOpen");

        if (chatWindowIsOpen) {
            $('#clientChatWindow').removeClass("chatWindowFooterOpen");
            //$('#chatHead').hide();
            $('#chatListWindow').hide();
            $('#chatFooterContainer').hide();
        }
        else {
            $('#clientChatWindow').addClass("chatWindowFooterOpen");
            //$('#chatHead').show();
            $('#chatListWindow').show();
            $('#chatFooterContainer').show();
        }
    },

    sendMessageToSupport: function (e) {
        var text = this.state.chatText;
        this.state.chatHub.server.sendMessageToSupport('Client', text);
    },

    onMessageTextChange: function (event) {
        var chatText = event.target.value;
        this.setState({ chatText: chatText });
    },

    addNewMessageToPage: function (username, message, isMe) {
        console.log(username, message, isMe);

        var message = { username: username, text: message, isMe: isMe };
        this.state.messages.push(message);

        this.setState({ messages: this.state.messages });
    },

    componentWillMount: function () {
        $.connection.hub.url = "http://localhost:8090/signalr";

        var agent = this.state.agentHub;
        var chat = this.state.chatHub;

        this.state.chatHub.client.addNewMessageToPage = this.addNewMessageToPage;

        $.connection.hub.start().done(function () {
            var data = { ChatQueueKey: chat.connection.id, Username: 'Client-01' };
            var json = JSON.stringify(data);

            $.ajax({
                type: "POST",
                url: "http://localhost:59431/api/Chatqueue/",
                data: json,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    alert('In Ajax');
                }
            });
        });
    },

    render: function () {
        return (
            <div id="clientChatWindow" className="row chatWindowFooterClosed">
                <div id="chatHead" onClick={this.openChat} className="chatHead row">
                    <div className="chatHead-left pull-left">
                        <img src="https://cdn4.iconfinder.com/data/icons/professionals/512/dispatcher-512.png" alt="iamgurdeeposahan" />
                        Customer Support
                    </div>
                    <div className="chatHead-right pull-right">
                    </div>
                </div>
                <div id="chatListWindow" className="row chatListWindow">
                    <MessageList messages={this.state.messages}></MessageList>
                </div>
                <div id="chatFooterContainer" className="row chatFooterContainer">
                    <input className="form-control input-sm chatText pull-left" value={this.state.chatText} onChange={this.onMessageTextChange} type="text" />
                    <input onClick={this.sendMessageToSupport} className="btn btn-sm btn-default chatSendButton pull-right" type="button" value="Send" />
                </div>
            </div>
        );
    }
});

ReactDOM.render(
    <ChatSupport />,
    document.getElementById('chat')
);