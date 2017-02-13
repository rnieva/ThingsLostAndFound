# ThingsLostAndFound
:construction: A MVC project

A self-taught project to learn more about Model–View–Controller (MVC),C#, Maps and more design tools.


---
Add file PrivateSettings.config in c:\ path with private data, example in this folder.

---
Things Lost And Found Manual

Menu:
ThingsLostAndFound --> Link to Home.



---
How work User Login?
Create a new User.
1- The user into data to create a new user.
2- If the NameUser is not using, the process go on.
3- It create a new Salt using "getSalt" method in "Crypto.cs".
4- It create a new crypto password using the "Hash" method in "Crypto.cs" with tha Salt and password.
5- It strore data in DB. 

User Login:
1- The user into UserName and Password in Login View.
2- The controller "LoginController.cs" get these data, retrive the Salt for that UserName and use the method Hash in "Crypto.cs" to check the password.
3- If it´s right it go to Home Page.
---
