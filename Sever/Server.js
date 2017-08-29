
var Exec_Enter = 1000;
var Exec_Ready = 1001;
var PlayerID = 9527;

var net = require('net');
var server = net.createServer(function(socket)
{
  socket.on('data', function(data)
  {
    var msg = data.toString();
    var json = JSON.parse(msg);

    console.log(msg);
    var exc = parseInt(json["exec"]);
    switch (exc) {
      case Exec_Enter:
      {
        var msg =  "{\"exec\" : " + Exec_Enter.toString();
        msg += ", \"uid\" : " + PlayerID.toString() + "}";
        socket.write(msg);
        PlayerID++;
      }
        break;
      case Exec_Ready:
      {
        var msg =  "{\"exec\" : " + Exec_Ready.toString();
        var config = JSON.stringify(Terrain.getTerrainConfig());
        msg += ", \"config\" : " + config + "}";
        socket.write(msg);
      }
      break;
      default:
        break;
    }
  });
});

server.listen(8888,function()
{
    console.log(" opened server on address %j ", server.address());
});

var Terrain = require('./Terrain.js');
Terrain.searchPath();
