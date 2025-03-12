import { Link, useNavigate } from 'react-router-dom'
import './Registerform.css'
import { useState } from 'react'

function Registerform() {
    const navigate = useNavigate();
    const RegisterClick = async (e) => {
        e.preventDefault();
        // inserting the user's input into variables
        var username = document.getElementById("Username").value;
        var displayname = document.getElementById("Nickname").value;
        var password = document.getElementById("Password").value;
        var passwordVerification = document.getElementById("Password-verification").value;
        var paswd = /^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{5,}$/;
        //checks if username already exists in the server's database
        var response = await fetch('http://localhost:5000/api/Users/' + username, {
            method: 'HEAD',
            headers: {
                'Content-Type': 'application/json'
            }
        })
        // if status == 200, the user exists
        if (response.status == 200) {
            alert("Username already exists")
            return;
        }
        //no blank password
        if (!password) {
            alert("Enter a password")
            return;
        }
        if (password.length < 5) {
            alert("Password length should be at least 5 characters long")
            return;
        }

        if (!password.match(paswd)) {
            alert("Password must contain at least one numeric digit and a special character(!@#$%^&*)")
            return;
        }
        //checks if passwords are the same
        if (passwordVerification !== password) {
            alert("Password doesn't match");
            return;
        }
        //inserting the new user to the server's DB
        await fetch('http://localhost:5000/api/Users', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({ Username: username, Displayname: displayname, Password: password })
        })

        // getting the new user from the server's DB
        var user = await fetch('http://localhost:5000/api/Users/' + username, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        })
        navigate("/chat", { state: { loggedUser: await user.json() } });
    }
    return (
        <div className="registerbox">
            <span className="d-flex justify-content-center">
                <div>
                    <div className="d-flex justify-content-center">
                        <h1>Register page</h1>
                    </div>
                    <br />

                    <div className="row mb-3">
                        <label htmlFor="Username" className="col-sm-3 col-form-label registerfield">Username*</label>
                        <div className="col-sm-7">
                            <input type="text" className="form-control" id="Username" placeholder="Enter your userName" required />
                        </div>
                    </div>

                    <div className="row mb-3">
                        <label htmlFor="nickname" className="col-sm-3 col-form-label registerfield">Nickname*</label>
                        <div className="col-sm-7">
                            <input type="text" className="form-control" id="Nickname" placeholder="Enter your nickname" required />
                        </div>
                    </div>

                    <div className="row mb-3">
                        <label htmlFor="Password" className="col-sm-3 col-form-label registerfield">Password*</label>
                        <div className="col-sm-7">
                            <input type="password" className="form-control" id="Password" placeholder="Enter password" required />
                        </div>
                    </div>

                    <div className="row mb-3">
                        <label htmlFor="Password-verification" className="col-sm-3 col-form-label">Password verification*</label>
                        <div className="col-sm-7">
                            <input type="password" className="form-control" id="Password-verification" placeholder="Enter password again" required />
                        </div>
                    </div>

                    <div className="row-sm">
                        <button type="button" onClick={RegisterClick} className="btn btn-primary">Register</button>
                        <label className="m-1">Already registered? click <Link to="/">here</Link> to login</label>
                    </div>
                </div>
            </span>
        </div>
    );
}

export default Registerform