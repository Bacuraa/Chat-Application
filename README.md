#**Demonstration of the application in Youtube: https://www.youtube.com/watch?v=Z4tj43wWC74**

How to run the "Chat-Application" files.

The git repository contains 3 files:
1. Chat_Server
2. Chat_Web
3. Chat_Android (hasnt been finished yet)

<h3>Chat-Server</h3>
This file is the server of the whole chat application. The server's database uses maridaDB, so before running the server, make sure to download mariaDB in your computer. In addition to downloading mariaDB, make sure to download Visual Studio. After you finished with the downloads, go inside the "Chat_Application" file, and double click the solution named "Chat_Server.sln".
<br>
<br>
I provided my code without any existing database, so that you you will be able to start the database from 0. So firstly, my database name is "chatDB", so before creating it from the project, we need to make sure a database with this name doesnt already exist in your PC. So get into the MySQL command line, and type "drop database chatDB". After that, type inside the "nuget package manager" (in the visual studio solution, if it is not shown in your solution go to View->Other Windows->Package Manager Console) the command "add-migration Init" and then type "update-database" to make the database be created locally with the name "chatDB". Database "chatDB", consists 3 tables: "usersDB", "contactsDB" and "messagesDB". So for example, to see if a user has been really added to the database, go to the MySQL command line and type "select * from chatDB.usersDB;" to see the full table of usersDB. note: contactsDB is a foreign key of usersDB, and messagesDB has a foreign key of contactsDB. Which means User class contains a list of Contact class, which contains a list of Message class.
 
note: make sure your local mariaDB password corresponds to the password displayed in the "UsersContext" class. In line 7 there is a connection string, and inside it you can the password as you like, the default password is "1".

The server default URL - "http://localhost:5000"
 
<h3>Chat-Web</h3>
The website works with React. Thus, in order to run the website we will need to install the related files first. Firstly, Download Node.JS in your computer in order to be able to run npm commands in the cmd. Secondly, we will run some npm command to fully install the required files. Enter cmd, and write the following command:

### `npx create-react-app project_name`

"project_name" is the file name of the file which will consist the files inside "Chat-Web" (you will download it from this git repository). This command will install some react realted files (node-modules). It is required to have react bootstrap files as well. please run the follwing command at the node_modules directory in cmd (which you have just downloaded by using the npx create-react-app command) and type the:

### `npm install react-bootstrap bootstrap`

After it has finished, navigate back to project_name directory run and following 3 commands:

### `npm install bootstrap`
### `npm install react-router-dom@6`
### `npm start`

Now open http://localhost:3000 to view the project in your browser. Now as long as the server is running, the website can interact with the server and have a chat with other people.

<h3>Chat-Android</h3>
The android version of the website has not been completed yet, so stay tuned :)
