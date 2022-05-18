"use strict";
//
//var connection = new signalR.HubConnectionBuilder().withUrl("chatHub").build();

//var _connectionId = '';
//connection.on("RecieveMessage", function (data) {
//    console.log("this is recieved data - ", data);
//    console.log(data.senderId);
//    var message = document.createElement("div")
//    message.classList.add('row')


//    var senderId = document.createElement("p")
//    senderId.appendChild(document.createTextNode(data.SenderId))
//    var msg = document.createElement("p")
//    msg.appendChild(document.createTextNode(data.Message))
//    var time = document.createElement("p")
//    time.appendChild(document.createTextNode(data.CurrentTime))

//    message.appendChild(senderId);
//    message.appendChild(msg);
//    message.appendChild(time);


//    var body = document.querySelector('.chat')
//    body.append(message);
//    debugger;
//});

//function sendMessage(event) {
    
//    var msg = $("#message").val();
//    const form = document.getElementById('messageForm');
//    event.preventDefault();
//    $.ajax({
//        type: "POST",
//        url: "../Chat/Message",
//        data: { "Message": msg },
//        success: function (data) {
//            console.log('msg send' , data)
//        },
//        error: function () {
//            console.log("not send")
//        }
//    });

//    form.reset();
//}



//connection.start().then(function () {
//    connection.invoke('getConnectionId')
//        .then(function (connectionId) {
//            _connectionId = connectionId
//            connection.invoke('JoinRoom', 'user1');
//            console.log("connection established")
//        })
//})
//    .catch(function (err) {
//        console.log(err)
//    })