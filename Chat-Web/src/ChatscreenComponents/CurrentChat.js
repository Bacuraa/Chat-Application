import React from 'react'
import './Chatscreen.css'
import './CurrentChat.css'

const CurrentChat = (props) => {
    // we will get the total messages from the loggedUser
    var totalMessages = props.loggedUser.chats
    // now we will filter the array to get messages only between the loggedUser and his friend
    var loggedUserMessages = totalMessages.filter(message =>
    ((message.author == props.loggedUser.nickname && message.receiver == props.currentContactChat.nickname) ||
        (message.author == props.currentContactChat.nickname && message.receiver == props.loggedUser.nickname)))


    return (
        <ul className="chat-history overflow-auto h-100" id="chat-messages-list">
            {
                loggedUserMessages.map((message) => {
                    if (message.type == "text") {
                        return (
                            message.author == props.loggedUser.nickname ?
                                (<li className="clearfix chat-messages">
                                    <div className="message-data">
                                    </div>
                                    <div className="message my-message">{message.data}</div>
                                </li>)
                                :
                                (<li className="clearfix chat-messages">
                                    <div className="message-data text-right">
                                    </div>
                                    <div className="message other-message float-right">{message.data} </div>
                                </li>)
                        )
                    }
                })
            }
        </ul>
    );
}

export default CurrentChat