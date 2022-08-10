import { Link, useNavigate } from "react-router-dom";
import './Loginform.css'

function LoginForm() {
    const loginClick = async () => {
        let password = document.getElementById("password").value;
        let username = document.getElementById("username").value;
        // checking if the username inserted is valid
        var response1 = await fetch('http://localhost:5000/api/Users', {
            method: 'HEAD',
            headers: {
                'Content-Type' : 'application/json'},
            body: JSON.stringify({Username: username})
        })
        // if status == 404, user doesnt exist
        if (response1.status == 404) {
            alert("Username doesnt exist")
            return;
        }

        var response2 = await fetch('http://localhost:5000/api/Users/' + username, {
            method: 'HEAD',
            headers: {
                'Content-Type' : 'application/json'},
            body: JSON.stringify({Password: password})
        })
        // if status == 404, wrong password
        if (response2.status == 404) {
            alert("Wrong password")
            return;
        }

        const navigate = useNavigate();
        navigate("/chat", {state : {username: username}});
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
                            <label htmlFor="Username" className="col-sm-3 col-form-label col-form-label-sm">Username</label>
                            <div className="col-sm-7">
                                <input type="username" className="form-control form-control-sm" id="username" placeholder="Enter Username" required />
                            </div>
                        </div>

                        <div className="row mb-3">
                            <label htmlFor="password" className="col-sm-3 col-form-label-sm">Password</label>
                            <div className="col-sm-7">
                                <input type="password" className="form-control form-control-sm" id="password" placeholder="Enter password" required />
                            </div>
                        </div>
                        <div className="row-sm">
                            <button type="button" className="btn btn-secondary" onClick={loginClick}>Login</button>
                            <label className="m-1">Not registered? click <Link to="/register">here</Link> to register</label>
                        </div>
                    </div>
                </span>
            </form>
        </div>
    );
}

export default LoginForm