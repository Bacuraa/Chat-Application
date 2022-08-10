import React from 'react'
import './Chatscreen.css'
import ContactCard from './ContactCard';
import { useState, useEffect } from 'react'
import AddFriend from './AddFriend';
import CurrentChat from './CurrentChat';
import { useLocation } from 'react-router-dom';

function Chatscreen() {
    const { state } = useLocation();
    const { username } = state;
    const [loggedUser, setLoggedUser] = useState('');

    //getting the user from the database (with his contacts and his messages)
    useEffect(async () => {
        await fetch('http://localhost:5000/api/Users', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ Username: username })
        })
        const currentUser = await Response.JSON();
        setLoggedUser(currentUser);
    }, [state])
    
    const [contacts, setContacts] = useState(loggedUser.Contacts);
    const [currentContactChat, setCurrentContactChat] = useState("")

    var handleSendMessage = async () => {
        var newMessageText = document.getElementById("chatBar").value
        // if the message is blank
        if (newMessageText == "") { return }

        // adds the message to the server's DB
        const addMsg = async() =>{
            await fetch('http://localhost:5000/api/' + username + '/Contacts/' + currentContactChat + '/Messages', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({Content: newMessageText })
        })}
        await addMsg();

        // gets the messages from the server's DB
        const getCurrentContactMessages = async() =>{
            await fetch('http://localhost:5000/api/' + username + '/Contacts/' + currentContactChat, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        })
        const currentContactMessages = await Response.JSON();
        setCurrentContactChat(currentContactMessages);
    }
        await getCurrentContactMessages();
    }
    // scrolls the chat bar down whenever an activity happens
    const element = document.getElementById("chat-messages-list");
    useEffect(() => {
        if (element) {
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
                            <div><img id="myAvatar" src="https://www.gravatar.com/avatar/00000000000000000000000000000000?d=mp" /></div>
                            <div><span id="myNickname">{loggedUser.DisplayName}</span></div>
                        </div>
                        <AddFriend loggedUser={loggedUser} setContacts={setContacts} />
                    </div>
                    <ContactCard contacts={contacts} setCurrentContactChat={setCurrentContactChat} />
                </div>
                <div className="chat" id="rightSide">
                    <div className="chat-header" id="chat-header" >
                        <div id="chat-header-avatar-name">
                            <img src="https://www.gravatar.com/avatar/00000000000000000000000000000000?d=mp" id="chat-header-avatar" />
                            <div className="chat-about" id="chat-header-name">{currentContactChat.nickname}</div>
                        </div>
                    </div>
                    <CurrentChat currentContactChat={currentContactChat} />
                    <div className="input-group mb-3" id="chat-line">
                        <input type="text" className="form-control" id="chatBar" placeholder="New message here..."></input>
                        <button className="btn btn-secondary" id="chatBox" type="button" onClick={handleSendMessage}> Send</button>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Chatscreen