import { Link, useNavigate } from "react-router-dom";
import './Loginform.css'

function LoginForm() {
    const navigate = useNavigate();
    const loginClick = async () => {
        let password = document.getElementById("password").value;
        let username = document.getElementById("username").value;
        // checking if the username inserted is valid
        var response1 = await fetch('http://localhost:5000/api/Users/' + username, {
            method: 'HEAD',
            headers: {
                'Content-Type': 'application/json'
            }
        })
        // if status == 404, user doesnt exist
        if (response1.status == 404) {
            alert("Username doesnt exist")
            return;
        }
        //checking if the password is valid
        var response2 = await fetch('http://localhost:5000/api/Users/' + username + '/' + password, {
            method: 'HEAD',
            headers: {
                'Content-Type': 'application/json'
            }
        })
        // if status == 404, wrong password
        if (response2.status == 404) {
            alert("Wrong password")
            return;
        }
        // getting the new user from the server's DB
        var user = await fetch('http://localhost:5000/api/Users/' + username, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        })
        navigate("/chat", { state: { loggedUser: await user.json()}});
    }

    return (
        <div className="loginbox">
            <form action="">
                <span className="d-flex justify-content-center">
                    <div>
                        <div className="d-flex justify-content-center">
                            <h1>Login page</h1>
                        </div>
                        <br />
                        <div className="row mb-3">
                            <label className="col-sm-3 col-form-label">Username*</label>
                            <div className="col-sm-7">
                                <input type="username" className="form-control form-control" id="username" placeholder="Enter username" required />
                            </div>
                        </div>

                        <div className="row mb-3">
                            <label htmlFor="password" className="col-sm-3 col-form-label">Password*</label>
                            <div className="col-sm-7">
                                <input type="password" className="form-control" id="password" placeholder="Enter password" required />
                            </div>
                        </div>
                        <div className="row-sm">
                            <button type="button" className="btn btn-primary" onClick={loginClick}>Login</button>
                            <label className="m-1">Not registered? click <Link to="/register">here</Link> to register</label>
                        </div>
                    </div>
                </span>
            </form>
        </div>
    );
}

export default LoginForm