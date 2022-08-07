import React from 'react'
import './Chatscreen.css'
import './CurrentChat.css'

const CurrentChat = (props) => {
    // we will get the total messages from the loggingUser
    var totalMessages = props.loggingUser.chats
    // now we will filter the array to get messages only between the loggingUser and his friend
    var loggingUserMessages = totalMessages.filter(message =>
    ((message.author == props.loggingUser.nickname && message.receiver == props.hisFriend.nickname) ||
        (message.author == props.hisFriend.nickname && message.receiver == props.loggingUser.nickname)))


    return (
        <ul className="chat-history overflow-auto h-100" id="chat-messages-list">
            {
                loggingUserMessages.map((message) => {
                    if (message.type == "text") {
                        return (
                            message.author == props.loggingUser.nickname ?
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