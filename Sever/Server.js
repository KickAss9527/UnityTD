
var SerExec_Enter = 1000;
var SerExec_Start = 1001;

var net = require('net');
var server = net.createServer(function(socket)
{
  socket.on('data', function(data)
  {
    console.log("rec data : ", data.toString());
    socket.write('sf u');
  });
});

server.listen(8888,function()
{
    console.log(" opened server on address %j ", server.address());
});

var Terrain = require('./Terrain.js');
Terrain.searchPath();
