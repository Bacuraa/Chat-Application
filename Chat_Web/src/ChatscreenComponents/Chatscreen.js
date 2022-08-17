import './Chatscreen.css'
import ContactCard from './ContactCard';
import { useState, useEffect, React } from 'react'
import AddFriend from './AddFriend';
import CurrentChat from './CurrentChat';
import { useLocation } from 'react-router-dom';
import { HubConnectionBuilder } from '@microsoft/signalr';

function Chatscreen() {
    const { state } = useLocation();
    const loggedUser = state.loggedUser;
    const [signalRConnection, setSignalRConnection] = useState(null);
    const [contacts, setContacts] = useState(loggedUser.contacts);
    const [currentContactChat, setCurrentContactChat] = useState({ displayName: "" })

    // scrolls the chat bar down whenever an activity happens
    const element = document.getElementById("chat-messages-list");
    useEffect(() => {
        if (element) {
            element.scrollTop = element.scrollHeight
        }
    })
    // connection builder for signalR
    useEffect(() => {
        const newSignalRConnection = new HubConnectionBuilder()
            .withUrl('http://localhost:5000/Hubs/Chat')
            .withAutomaticReconnect()
            .build();

        setSignalRConnection(newSignalRConnection);
    }, []);

    // opens an active connection with the server
    useEffect(() => {
        if (signalRConnection) {
            signalRConnection.start()
                .then(result => {
                    signalRConnection.invoke("AddConnection", loggedUser.username)
                    signalRConnection.on('ReceiveMessage', async (loggedUsername, contactUsername) => {

                        // gets the new contact with his messages from the server's DB (signalR invoke)
                        var invokedContactChat = await fetch('http://localhost:5000/api/'
                            + loggedUsername + '/Contacts/' + contactUsername, {
                            method: 'GET',
                            headers: {
                                'Content-Type': 'application/json'
                            }
                        })

                        invokedContactChat = await invokedContactChat.json();
                        setCurrentContactChat(prevState => {
                            if (prevState.contactUsername == contactUsername)
                                return invokedContactChat
                            return prevState;
                        });

                        // gets the new contact list from the server's DB (signalR invoke)
                        var invokedContacts = await fetch('http://localhost:5000/api/' + loggedUsername + '/Contacts', {
                            method: 'GET',
                            headers: {
                                'Content-Type': 'application/json'
                            }
                        })
                        invokedContacts = await invokedContacts.json();
                        setContacts(invokedContacts);
                    });
                })
        }
    }, [signalRConnection]);

    // sends a message when 'Enter' key is pressed
    var onKeyPressEnter = async (e) => {
        if (e.key === 'Enter')
            await handleSendMessage(e);
    }

    var handleSendMessage = async (e) => {
        e.target.blur()
        var newMessageText = document.getElementById("chatBar").value
        // if the message is blank
        if (newMessageText == "") { return }

        // adds the message to the server's DB (author)
        await fetch('http://localhost:5000/api/'
            + loggedUser.username + '/Contacts/' + currentContactChat.contactUsername + '/Messages', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ Content: newMessageText })
        })
        // adds the message to the server's DB (receiver)
        await fetch(`http://localhost:5000/api/Transfer`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ From: loggedUser.username, To: currentContactChat.contactUsername, Content: newMessageText })
        })

        // gets the new updated list of contacts
        var newContacts = await fetch('http://localhost:5000/api/' + loggedUser.username + '/Contacts', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        })
        newContacts = await newContacts.json();
        setContacts(newContacts);

        // gets the new contact with his messages from the server's DB
        var contactChat = await fetch('http://localhost:5000/api/'
            + loggedUser.username + '/Contacts/' + currentContactChat.contactUsername, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        })
        contactChat = await contactChat.json();
        setCurrentContactChat(contactChat);
        document.getElementById("chatBar").value = "";
        document.getElementById("chatBar").focus();
        // invoke signalR to the user that receives the message
        await signalRConnection.invoke("SendMessage", currentContactChat.contactUsername, loggedUser.username);
    }

    return (
        <div>
            <link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" />
            <div className="clearfix card chat-app" id="chat-window">
                <div className="people-list" id="people-list">
                    <div className="chat-header" id="profileAndButton">
                        <div id="myProfile">
                            <div><img id="myAvatar" src="https://www.gravatar.com/avatar/00000000000000000000000000000000?d=mp" /></div>
                            <div><span id="myNickname">{loggedUser.displayName}</span></div>
                        </div>
                        <AddFriend loggedUser={loggedUser} setContacts={setContacts} signalRConnection={signalRConnection} currentContactChat={currentContactChat} />
                    </div>
                    <ContactCard contacts={contacts} setCurrentContactChat={setCurrentContactChat} />
                </div>
                <div className="chat" id="rightSide">
                    <div className="chat-header" id="chat-header" >
                        <div id="chat-header-avatar-name">
                            <img src="https://www.gravatar.com/avatar/00000000000000000000000000000000?d=mp" id="chat-header-avatar" />
                            <div className="chat-about" id="chat-header-name">{currentContactChat.displayName}</div>
                        </div>
                    </div>
                    <CurrentChat currentContactChat={currentContactChat} />
                    <div className="input-group mb-3" id="chat-line">
                        <input type="text" className="form-control" id="chatBar" onKeyPress={onKeyPressEnter} placeholder="New message here..."></input>
                        <button className="btn btn-secondary" id="chatBox" type="button" onClick={handleSendMessage}> Send</button>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default Chatscreen