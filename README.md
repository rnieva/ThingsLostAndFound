# ThingsLostAndFound
:construction: 🔍 A MVC project

Things Lost and Found is a self-taught project to learn and improvements the knowledge about MVC architecture and different programming languages. It try developer a project the most real possible using these tools

**Technical Features:**

- Architectural pattern: Model–View–Controller (MVC).<p>
- Software Framework:.NET Framework 4.5.2.<p>
- Programming Languages: C# (C Sharp), Java Script, HTML.<p>
- Database: MSQlExpress 10.5, Entity Framework 6.1.3.<p>
- Other Tools: Google API, GoogleMaps LocationServices, jQuery, CSS, Bootstrap.<p>
- Security: Authentication and Authorization. Use of Cookies. Crypto using Hash and Salt.<p>

---
----
---

Using project in local:<p>
- Add file PrivateSettings.config in c:\ path with private data, example in this folder.<p>

Using project in hosting:<p>
- Put PrivateSettings.config in the root server.
- Change the "connectionStrings" in We.config.

---
<h2>Things Lost And Found Manual</h2>

Menu:
ThingsLostAndFound --> Link to Home.
<p>Search Object --> Search a Lost or Found Object in the DB.


---
<h3>How work User Login?</h3><p>
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
