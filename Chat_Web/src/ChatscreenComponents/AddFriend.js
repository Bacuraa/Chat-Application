import { Button, Modal } from 'react-bootstrap/'
import { useState } from 'react'
import './AddFriend.css'

function AddFriend(props) {
    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const handleAdd = async () => {
        let loggedUsername = props.loggedUser.username
        let contactUsername = document.getElementById("ContactUserName").value
        let contactDisplayname = document.getElementById("ContactDisplayName").value
        if (contactUsername == "") {
            alert("Enter username")
            return;
        }
        if (contactDisplayname == "") {
            alert("Enter nickname")
            return;
        }

        if (loggedUsername == contactUsername) {
            alert("User can't add himself as a contact");
            return;
        }

        //checking with the server if the contact username exists in the server
        var checkContactUsername = await fetch('http://localhost:5000/api/Users/' + contactUsername, {
            method: 'HEAD',
            headers: {
                'Content-Type': 'application/json'
            }
        })
        await checkContactUsername.status;

        if (checkContactUsername.status == 404) {
            alert("Username doesnt exist")
            return;
        }

        //checking with the server if the contact already exists in the user's contacts
        var checkContact = await fetch('http://localhost:5000/api/' + loggedUsername + '/Contacts/' + contactUsername, {
            method: 'HEAD',
            headers: {
                'Content-Type': 'application/json'
            }
        })
        await checkContact.status;

        if (checkContact.status == 200) {
            alert("The contact is already in your contacts list");
            return;
        }

        // adds the contact to the server's DB (the user that receives the friend request)
        await fetch(`http://localhost:5000/api/Invitations`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ From: loggedUsername, To: contactUsername })
        })

        // adds the contact to the server's DB (current loggedUser)
        await fetch('http://localhost:5000/api/' + loggedUsername + '/Contacts', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ Username: contactUsername, DisplayName: contactDisplayname })
        })

        // gets the new contact list from the server's DB
        var currentContacts = await fetch('http://localhost:5000/api/' + loggedUsername + '/Contacts', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        })
        currentContacts = await currentContacts.json();
        props.setContacts(currentContacts);

        // invoke signalR for the receiving friend request user
        props.signalRConnection.invoke("SendMessage", contactUsername, loggedUsername);
        handleClose();
    }

    return (
        <div>
            <Button id="addFriendButton" onClick={handleShow}>Add friend</Button>
            <Modal show={show} onHide={handleClose}>
                <Modal.Header closeButton>
                    <Modal.Title id="modal-title">Add new contact</Modal.Title>
                </Modal.Header>
                <Modal.Body>
                    <div id="contact-username">
                        <div></div>
                        <div id="Username">Username*</div>
                        <input placeholder="Enter contact's username" id="ContactUserName"></input>
                    </div>
                    <div id="contact-nickname">
                        <div></div>
                        <div id="Nickname">Nickname*</div>
                        <input placeholder="Enter contact's nickname" id="ContactDisplayName"></input>
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