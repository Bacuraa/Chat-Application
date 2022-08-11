import { Button, Modal } from 'react-bootstrap/'
import { useState } from 'react'
import './AddFriend.css'

function AddFriend(props) {
    const [show, setShow] = useState(false);
    const handleClose = () => setShow(false);
    const handleShow = () => setShow(true);
    const handleAdd = async () => {
        let loggedUsername = props.loggedUser.Username
        let contactUsername = document.getElementById("ContactUserName").value
        let contactDisplayname = document.getElementById("ContactDisplayName").value
        if (contactUsername == ""){
            alert("Enter username")
            return;
        }
        if (contactDisplayname){
            alert("Enter nickname")
            return;
        }

        if (loggedUsername == contactUsername) {
            alert("User can't add himself as a contact");
            return;
        }

        //checking with the server if the contact username exists in the server
        var checkContactUsername = await fetch('http://localhost:5000/api/Users', {
            method: 'HEAD',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ Username: contactUsername })
        })
        await checkContactUsername();

        if (checkContactUsername.status == 200) {
            alert("Username doesnt exist")
            return;
        }

        //checking with the server if the contact already exists in the user's contacts
        var checkContact = await fetch('http://localhost:5000/api/' + loggedUsername + '/Contacts' + contactUsername, {
            method: 'HEAD',
            headers: {
                'Content-Type': 'application/json'
            }
        })
        if (checkContact.status != 404) {
            alert("The contact is already in your contacts list");
            return;
        }

        // adds the contact to the server's DB
        const addContact = async () => {
            await fetch('http://localhost:5000/api/' + loggedUsername + '/Contacts/', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ Username: contactUsername, DisplayName: contactDisplayname })
            })
        }
        await addContact();

        // gets the new contact list from the server's DB
        const getCurrentContacts = async () => {
            await fetch('http://localhost:5000/api/' + loggedUsername + '/Contacts/', {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
            const currentContacts = await Response.JSON();
            props.setContacts(currentContacts);
        }
        await getCurrentContacts();
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