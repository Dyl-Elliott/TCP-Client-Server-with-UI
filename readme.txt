Introduction:
During this assignment a process of step has been followed and implemented on a weekly occurrence to develop a running server and client which are able communicate messages between each other using ‘localhost’ and ‘port # 43’. 
Initially the foundations of creating a client and database were provided then leaving the main logic and structuring of the relevant source code to complete the assignment to myself. 
The client can also be used to communicate with alternative servers depending on which host IP and port information has been provided by a user.

Implementation:
A combination of 4 protocols have been implemented on both sides of the project, each conforming to a specified output/input format to which should be received correctly by the server to 
allow saving of client data into the database and passing of a complying response (resulting in saving and loading of this data if specified). 

Further to the development of the source code a user interface has been created for both the client and server which allows the user to interact with the source code via extended development implemented 
in the later stages of the project using WPF application as part of Visual Studios. 

Both the client and server UI’s implement all available (-) flag inputs which simulates commands which may be entered into the command prompt when interacting with location or locationserver solutions.

Additional flags have been inserted which allow further assignment of messages passing between client and server in a more debugging message environment. 
Further to this the timeout can be changed to suit a certified response wait time between message being passed.

Unique to the server it is able to save logging information – which is formatted to simulate a timestamp and message push from the client. 
The server can also save specifically the name and location of the client message in its own text file. 
The text file can then be loaded upon running of the server, which will be reflective in the behaviour of the client’s submissions and response they receive.

At the point of submission, to my knowledge, all parts of the ACW have been adhered to for both client and server. 
This can be supported by access to test scripts which have allowed my source code to be checked in accordance with the specification throughout the development phase of the project.

I have also implemented the capabilities of threading, and is able to update the server with persistent messages, but I am unsure as to the reliability of this feature.


Arguments Implement:
Client:
‘-h’ – Ability to define at certain host IP to connect to.
‘-p’ – Allow the user to select a specified port number to connect to.
‘-d’ – If selected allow the user to see additional debugging information which is occurring within the source code. 
‘-t’ – Ability to specifying a timeout between receiving and sending messages.
‘-h9’ – Defines running of the HTTPS09 protocol and the format of messaging it passes.
‘-h0’ - Defines running of the HTTPS10 protocol and the format of messaging it passes.
‘-h1’ - Defines running of the HTTPS11 protocol and the format of messaging it passes.
Server:
‘-w’ – Flag to specify whether the server is launched using the UI or not.
‘-l’ – Informs the server that a log file is to be collated.
‘-f’ - Informs the server that a save file is to be collated. If flag is discovered, at the beginning of the is a check to load any save data text file from previous client connections.
 ‘-d’ - If selected allow the user to see additional debugging information which is occurring within the source code.
 ‘-t’ - Ability to specifying a timeout between receiving and sending messages.


Tests Completed: 
lab1test
15/03/2022 12:49:32.41 All tests of location completed: 28 passed, 0 advisory, 0 failed (of 28 total). 
Lab1Test can be signed off for DYLAN ELLIOTT (624474) at 15/03/2022 12:49:33.49 on BJL-1F-38 

lab2test
15/03/2022 13:00:07.09 All tests of G:\50081\UiFinal\locationserver\locationserver\bin\Debug completed: 16 passed, 0 advisory, 0 failed (of 16 total). 
Lab2Test can be signed off for DYLAN ELLIOTT (624474) at 15/03/2022 13:00:09.26 on BJL-1F-38 

lab3test
14/03/2022 12:12:15.97 All tests of location completed: 109 passed, 0 advisory, 0 failed (of 111 total). 
Lab3Test can be signed off for DYLAN ELLIOTT (624474) at 14/03/2022 12:12:18.10 on VH-CSE-11 

lab4test
15/03/2022 13:12:15.59 All tests of G:\50081\UiFinal\locationserver\locationserver\bin\Debug completed: 166 passed, 0 advisory, 0 failed (of 168 total). 
Lab4Test can be signed off for DYLAN ELLIOTT (624474) at 15/03/2022 13:12:17.68 on BJL-1F-38 

lab5test
15/03/2022 13:28:04.83 All tests of G:\50081\UiFinal\locationserver\locationserver\bin\Debug completed: 3 passed, 0 advisory, 0 failed. 
Lab5Test can be signed off for DYLAN ELLIOTT (624474) at 15/03/2022 13:28:06.82 on BJL-1F-38 
passed = 100 threads

lab6test
15/03/2022 13:30:50.89 All tests completed: 9 passed, 0 advisory, 0 failed. 
Lab6Test can be signed off for DYLAN ELLIOTT (624474) at 15/03/2022 13:30:52.22 on BJL-1F-38 
