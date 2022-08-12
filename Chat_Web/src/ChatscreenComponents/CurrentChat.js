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
                        return (
                            message.sent == true ?
                                (<li className="clearfix chat-messages">
                                    <div className="message-data">
                                    </div>
                                    <div className="message my-message">{message.content}</div>
                                </li>)
                                :
                                (<li className="clearfix chat-messages">
                                    <div className="message-data text-right">
                                    </div>
                                    <div className="message other-message float-right">{message.content} </div>
                                </li>)
                        )
                })
            }
        </ul>
    );
}

export default CurrentChat