import React from 'react'
import './Chatscreen.css'
import ContactCard from './ContactCard';
import { useState, useEffect } from 'react'
import AddFriend from './AddFriend';
import usersList from '../usersDB';
import CurrentChat from './CurrentChat';
import Message from '../Message';
import AudioMsg from '../AudioMsg';

function Chatscreen(props) {
    var loggedPersonUsername = localStorage.getItem("currentUser")
    var loggingUser = usersList.find(x => x.username == loggedPersonUsername)
    const [friends, setFriends] = useState(loggingUser.friends);
    const [friendChat, setFriendChat] = useState("")

    const [userMessages, setMessage] = useState(loggingUser.chats)
    var handleSendMessage = () => {
        var newMessageText = document.getElementById("chatBar").value
        // blank message
        if (newMessageText == "") { return }
        var time = new Date().getTime()
        var newMessage = new Message(newMessageText, time, "text", loggingUser.nickname, friendChat.nickname)
        if(loggingUser.nickname>=friendChat.nickname){
            loggingUser.lastMessages.set(loggingUser.nickname + friendChat.nickname, newMessageText + "*" + time)
            friendChat.lastMessages.set(loggingUser.nickname + friendChat.nickname, newMessageText + "*" + time)
        } else {
            loggingUser.lastMessages.set(friendChat.nickname + loggingUser.nickname, newMessageText + "*" + time)
            friendChat.lastMessages.set(friendChat.nickname + loggingUser.nickname, newMessageText + "*" + time)
        }
        loggingUser.chats.push(newMessage)
        // temporary line, thats the work of the server
        friendChat.chats.push(newMessage)
        document.getElementById("chatBar").value = "";
        setMessage((messages) => {
            let newUserMessage = [...messages]
            newUserMessage.push(newMessage)
            return newUserMessage
        })
    }

    var clickImageInput = () => {
        document.getElementById("imageInput").click();
    }

    var clickVideoInput = () => {
        document.getElementById("videoInput").click();
    }
    const element = document.getElementById("chat-messages-list");        
    useEffect(() => {
        if (element){
            element.scrollTop = element.scrollHeight
        }
    })
    return (
        <div>
            <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" />
            <div className="clearfix card chat-app" id="chat-window">
                <div className="people-list" id="people-list">
                    <div className="chat-header" id="profileAndButton">
                        <div id="myProfile">
                        <div><img id="myAvatar" src={loggingUser.avatar} /></div>
                        <div><span id="myNickname">{loggingUser.nickname}</span></div>
                        </div>
                        <AddFriend loggingUser={loggingUser} setFriends={setFriends} />
                    </div>
                    <ContactCard loggingUser={loggingUser} userFriends={friends} setFriendChat={setFriendChat} />
                </div>
                <div className="chat" id="rightSide">
                    <div className="chat-header" id="chat-header" >
                        <div id="chat-header-avatar-name">
                            <img src={friendChat.avatar} id="chat-header-avatar" />
                            <div className="chat-about" id="chat-header-name">{friendChat.nickname}</div>
                        </div>
                    </div>
                    <CurrentChat loggingUser={loggingUser} hisFriend={friendChat} />
                    <div className="input-group mb-3" id="chat-line">
                        <div className="input-group-prepend">
                            <input id="imageInput" type="file" onChange={handleImageMsg} accept="image/*" hidden></input>

                            <button className="iconBoxes bi bi-image" id="imageUpload" onClick={clickImageInput}><i> </i></button>

                            <input id="videoInput" type="file" onChange={handleVideoMsg} accept="video/*"  hidden></input>

                            <button className="iconBoxes bi bi-camera-reels" id="videoUpload" onClick={clickVideoInput}><i ></i></button>


                            <button type="button" className="iconBoxes bi bi-mic" data-bs-toggle="modal" data-bs-target="#exampleModal" id="recordingUpload"></button>

                            <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <h5 class="modal-title" id="exampleModalLabel">Audio Recording</h5>
                                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                        </div>
                                        <div class="modal-body">
                                            <AudioMsg setFriendChat={setFriendChat} loggingUser={loggingUser} userMessages={userMessages} setMessage={setMessage}></AudioMsg>
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <input type="text" className="form-control" id="chatBar" placeholder="New message here..."></input>
                        <button className="btn btn-secondary" id="chatBox" type="button" onClick={handleSendMessage}> Send</button>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Chatscreen