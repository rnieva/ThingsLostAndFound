# ThingsLostAndFound
:construction: A MVC project

A self-taught project to learn more about Model–View–Controller (MVC),C#, Maps and more design tools.


---
Add file PrivateSettings.config in c:\ path with private data, example in this folder.

---
<h3>Things Lost And Found Manual</h3>

Menu:
ThingsLostAndFound --> Link to Home.
<p>Search Object --> Search a Lost or Found Object in the DB.


---
How work User Login?<p>
Create a new User.<p>
1- The user into data to create a new user.<p>
2- If the NameUser is not using, the process go on.<p>
3- It create a new Salt using "getSalt" method in "Crypto.cs".<p>
4- It create a new crypto password using the "Hash" method in "Crypto.cs" with tha Salt and password.<p>
5- It strore data in DB.<p> 

User Login:<p>
1- The user into UserName and Password in Login View.<p>
2- The controller "LoginController.cs" get these data, retrive the Salt for that UserName and use the method Hash in "Crypto.cs" to check the password.<p>
3- If it´s right it go to Home Page.<p>

---
