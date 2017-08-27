// var express = require('express'),
//     app = express(),
//     server = require('http').createServer(app),
//     io = require('socket.io').listen(server); //引入socket.io模块并绑定到服务器
// // app.use('/', express.static(__dirname + '/www'));

// server.listen(8888);
// io = require('socket.io').listen(server); //引入socket.io模块并绑定到服务器
// var io = require('socket.io').listen(8888);
// console.log("td1 server start running...");
//
// io.sockets.on('event', (args) => {
//   console.log("sdfds");
// })
//
// io.sockets.on('connection', function(socket) {
//   console.log("sdf coming!!!!!");
//   socket.on('message', (args) => {
//
//   })
// });

var net = require('net');
var server = net.createServer(function(socket){


  socket.on('data', function(data)
  {
    console.log("rec data : ", data.toString());
    socket.write('fuck u');
  });
});
server.listen(8888,function()
{
        var address=server.address();
        console.log(" opened server on address %j ",address);
    });
