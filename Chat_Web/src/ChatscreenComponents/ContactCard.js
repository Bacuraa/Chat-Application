import './ContactCard.css'

const ContactCard = (props) => {
    return (
        <ul className="list-unstyled chat-list overflow-auto h-100">
            {
                props.contacts.map((contact) => (
                    <div onClick={() => {props.setCurrentContactChat(contact) }} id="clicker">
                        <li id="wrapper">
                            <img src="https://www.gravatar.com/avatar/00000000000000000000000000000000?d=mp" />
                            <div id="#wrapper-2">
                                <div id="wrapper-3">
                                    <span className="name">{contact.displayName}</span>
                                    <span id="time"> {contact.timeAgo} </span>
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