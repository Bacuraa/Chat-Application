import usersList from "../usersDB"
import './ContactCard.css'

const ContactCard = (props) => {
    //user's friend nicknames
    const userFriends = props.userFriends
    //user's friends objects
    var friendsObjects = []

    function creatingFriendsArr(user) {

        if (userFriends.includes(user.nickname)) {
            friendsObjects.push(user)
        }
    }
    // creating the user's friends list out of our whole user Database
    usersList.forEach(creatingFriendsArr)

    function timeago(friend) {
        if(props.loggingUser.nickname>=friend.nickname){
            var combinedString = props.loggingUser.lastMessages.get(props.loggingUser.nickname + friend.nickname)
        } else {
            var combinedString = props.loggingUser.lastMessages.get(friend.nickname + props.loggingUser.nickname)
        }
        if (typeof combinedString === 'string'){
        let splitString = combinedString.split('*')
        var date = parseInt(splitString[1])

        var seconds = Math.floor(((new Date().getTime() / 1000) - date / 1000));

        var interval = Math.floor(seconds / 31536000);
        if (interval >= 1) return interval + "y ago";

        interval = Math.floor(seconds / 2592000);
        if (interval >= 1) return interval + "m ago";

        interval = Math.floor(seconds / 86400);
        if (interval >= 1) return interval + "d ago";

        interval = Math.floor(seconds / 3600);
        if (interval >= 1) return interval + "h ago";

        interval = Math.floor(seconds / 60);
        if (interval >= 1) return interval + "m ago";

        return Math.floor(seconds) + "s ago";
        }
        else return ""
    }

    function lastMessages(friend){
        if(props.loggingUser.nickname>=friend.nickname){
            var combinedString = props.loggingUser.lastMessages.get(props.loggingUser.nickname + friend.nickname)
        } else {
            var combinedString = props.loggingUser.lastMessages.get(friend.nickname + props.loggingUser.nickname)
        }
        if (typeof combinedString === 'string'){
            let splitString = combinedString.split('*');
            return splitString[0]
        }
        else return ""
        }

    return (
        <ul className="list-unstyled chat-list overflow-auto h-100">
            {
                friendsObjects.map((friend) => (
                    <div onClick={() => { props.setFriendChat(friend) }} id="clicker">
                        <li id="wrapper">
                            <img src={friend.avatar} />
                            <div id="#wrapper-2">
                                <div id="wrapper-3">
                                    <span className="name">{friend.nickname}</span>
                                    <span id="time"> {timeago(friend)} </span>
                                </div>
                                <div id="latestComment" > {lastMessages(friend)} </div>
                            </div>
                        </li>
                    </div>
                ))
            }
        </ul>
    );
}

export default ContactCard