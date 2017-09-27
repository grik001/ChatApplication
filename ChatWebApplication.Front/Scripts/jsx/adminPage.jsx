var ChatHub = React.createClass({
    getInitialState: function () {
        return { chats: [] };
    },

    render: function () {
        return (
            this.state.Chats.map(game => <Chat key="" />)
        );
    }
});

var Chat = React.createClass({
    getInitialState: function () {
        return { messages: [], id: "" };
    },

    render: function () {
        return (
            <div>
                <div>
                    <MessageList></MessageList>
                </div>
                <div>
                    <input />
                    <input />
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
                    this.state.Messages.map(game => <Message key="" />)
                }
            </div>
        );
    }
});

var Message = React.createClass({
    render: function () {
        return (
            <div>
                <label>{this.props.Username}</label>
                <label>{this.props.Text}</label>
            </div>
        );
    }
});


ReactDOM.render(
    <ChatHub />,
    document.getElementById('content')
);