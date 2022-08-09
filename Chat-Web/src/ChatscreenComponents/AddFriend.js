import { Button, Modal } from 'react-bootstrap/'
import { useState } from 'react'
import React from 'react'
import usersList from '../usersDB';
import './AddFriend.css'

function AddFriend(props) {
    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const handleAdd = async () => {
        let contactUsername = document.getElementById("ContactUserName").value
        let contactDisplayname = document.getElementById("ContactDisplayName").value

        if (friendUser) {
            //User can't add himself as a contact
            if (props.loggedUser.Username == contactUsername) {
                alert("User can't add himself as a contact");
                return;
            }
            //User can't add a friend that already in his contact list
            if (props.loggedUser.Contacts.find(x => x == contactUsername)) {
                alert("The contact is already in your contacts list");
                return;
            }

            var addContact = async () => {
                var newMessageText = document.getElementById("chatBar").value
                // blank message
                if (newMessageText == "") { return }
                var newMessage = new Message(newMessageText, loggedUser.DisplayName, currentContactChat.nickname)
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
        
                const getCurrentContactChat = async() =>{
                    await fetch('http://localhost:5000/api/' + username + '/Contacts/' + currentContactChat, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                const currentContactChat = await Response.JSON();
                setCurrentContactChat(currentContactChat);
            }
                await getCurrentContactChat();
            }

            props.setContacts((currentContacts) => {
                let newContacts = [...currentContacts];
                props.loggedUser.friends.push(contactDisplayName)
                newFriends.push(contactDisplayName)
                handleClose();
                return newContacts;
            })
        }
        //No such friend in database
        else {
            alert("Friend doesn't exist")
        }
    }

    return (
        <div>
            <Button id="addFriendButton" onClick={handleShow}>Add friend</Button>
            <Modal show={show} onHide={handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title>Add friend to chat</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div>
                        <input placeholder="Enter friend's username" id="ContactUserName"></input>
                    </div>
                    <div>
                        <input placeholder="Enter friend's nickname" id="ContactDisplayName"></input>
                    </div>
                </Modal.Body>
                <Modal.Footer>
                    <Button variant="secondary" onClick={handleClose}>
                        Close
                    </Button>
                    <Button variant="primary" onClick={handleAdd}>
                        Save Changes
                    </Button>
                </Modal.Footer>
            </Modal>
        </div>
    );
}
export default AddFriend;