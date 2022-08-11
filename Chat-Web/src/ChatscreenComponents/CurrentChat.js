import React from 'react'
import './Chatscreen.css'
import './CurrentChat.css'

const CurrentChat = (props) => {
    var messages;
    if (props.currentContactChat.displayName === "")
        messages = []
    else messages = props.currentContactChat.messages

    return (
        <ul className="chat-history overflow-auto h-100" id="chat-messages-list">
            {
                messages.map((message) => {
                    if (message.type == "text") {
                        return (
                            message.Created == true ?
                                (<li className="clearfix chat-messages">
                                    <div className="message-data">
                                    </div>
                                    <div className="message my-message">{message.Content}</div>
                                </li>)
                                :
                                (<li className="clearfix chat-messages">
                                    <div className="message-data text-right">
                                    </div>
                                    <div className="message other-message float-right">{message.Content} </div>
                                </li>)
                        )
                    }
                })
            }
        </ul>
    );
}

export default CurrentChat