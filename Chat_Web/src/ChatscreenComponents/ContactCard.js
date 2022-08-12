import './ContactCard.css'

const ContactCard = (props) => {

    function timeago(lastMessageTime) {
        if (lastMessageTime == 0) return "";
        var seconds = (new Date().getTime()/1000) - lastMessageTime;

        var interval = Math.floor(seconds / 31536000);
        if (interval >= 1) return interval + "y ago";

        interval = Math.floor(seconds / 2592000);
        if (interval >= 1) return interval + "m ago";

        interval = Math.floor(seconds / 86400);
        if (interval >= 1) return interval + "d ago";

        interval = Math.floor(seconds / 3600);
        if (interval >= 1) return interval + "h ago";

        interval = Math.floor(seconds / 60);
        if (interval >= 1) return interval + "m ago";

        return seconds + "s ago";
    }

    return (
        <ul className="list-unstyled chat-list overflow-auto h-100">
            {
                props.contacts.map((contact) => (
                    <div onClick={() => { props.setCurrentContactChat(contact) }} id="clicker">
                        <li id="wrapper">
                            <img src="https://www.gravatar.com/avatar/00000000000000000000000000000000?d=mp" />
                            <div id="#wrapper-2">
                                <div id="wrapper-3">
                                    <span className="name">{contact.displayName}</span>
                                    <span id="time"> {timeago(Number(contact.lastDate))} </span>
                                </div>
                                <div id="latestComment" > {contact.lastMessage} </div>
                            </div>
                        </li>
                    </div>
                ))
            }
        </ul>
    );
}

export default ContactCard