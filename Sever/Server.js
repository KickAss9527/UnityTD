
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
        var address=server.address();
        console.log(" opened server on address %j ",address);
});


//path ----------
//12w x 10h

var TerrainConfig = [
  "XXXXXXXOOOOO",
  "XXXXXXXOOOOX",
  "XXXXXXXOOOOX",
  "XXXXXXXXOXXX",
  "XXXOOOOOOXXX",
  "XXXOXXXXXXXX",
  "XOOOXXXXXXXX",
  "XOOOXXXXXXXX",
  "XOOOXXXXXXXX",
  "OOOOXXXXXXXX"];
var TerrainWidth = TerrainConfig[0].length;
var TerrainHeight = TerrainConfig.length;

function TerrainTilePoint(x, y)
{
  this.iX = x;
  this.iY = y;
  this.TID = x*TerrainHeight + TerrainWidth;
  this.getDistance = function(obj)
  {
    return Math.pow(obj.iX - iX, 2) + Math.pow(obj.iY - iY, 2);
  }
}
function TerrainTile(tileObj)
{
    this.objPoint = tileObj;
    this.iFromValue = 0;//起点到self的距离
    this.iToValue = 0;//self到终点的距离
    this.iFValue = 0;//总距离
    this.objParent = null;
}
TerrainTile PointA = new TerrainTile(new TerrainTilePoint(0, 11));
TerrainTile PointB = new TerrainTile(new TerrainTilePoint(9, 0));

function findPath(terrain)
{
  PointB.objParent = null; //清空上次记录
  var arrOpen = [PointA];
  var arrClose = [];

  function isInCloseList(tId)
  {
    for (var i = 0; i < arrClose.length; i++) {
       TerrainTile tile = arrClose[i];
       if (tile.objPoint.tId == tId)
       {
          return false;
       }
    }
    return true;
  }

  function findInOpenList(tId)
  {
    for (var i = 0; i < arrOpen.length; i++) {
       TerrainTile tile = arrOpen[i];
       if (tile.objPoint.tId == tId)
       {
          return tile;
       }
    }
    return null;
  }

  function isTileEnable(x, y)
  {
    return TerrainConfig[y][x] != 'X';
  }

  var neighborOffset = [[-1, 0], [0, -1], [1, 0], [0, 1]];//左 上 右 下
  while (arrOpen.length > 0)
  {
      TerrainTile cur = arrOpen.pop();
      arrClose.push(cur);

      for (var i = 0; i < neighborOffset.length; i++)
      {
        var offset = neighborOffset[i];
        var tmpX = cur.objPoint.iX + offset[0];
        var tmpY = cur.objPoint.iY + offset[1];
        //超出范围 不要
        if (tmpX < 0 || tmpX >= TerrainWidth ||
            tmpY < 0 || tmpY >= TerrainHeight)
        {
          continue;
        }

        //被占用了 不要
        if (!isTileEnable(tmpX, tmpY)) continue;
        var tmpTid = getTerrainId(tmpX, tmpY);

        //available path found
        if (tmpTid == PointB.objPoint.tId)
        {
          PointB.objParent = cur;
          break;
        }

        //不要
        if (isInCloseList(tmpTid)) continue;

        TerrainTile existTile = findInOpenList(tmpTid);
        if (existTile)
        {
          //需要重新计算，看有没有更短
          //a->c or a -> cur -> c, update gValue, c's parent -> cur
          var dis_Cur_C = existTile.objPoint.getDistance(cur.objPoint);
          if (dis_Cur_C + cur.iFromValue < existTile.iFromValue)
          {
              existTile.iFromValue = dis_Cur_C + cur.iFromValue;
              existTile.iFValue = existTile.iFromValue + existTile.iToValue;
              existTile.objParent = cur;
          }
        }
        else //新加入open
        {
          TerrainTilePoint point = new TerrainTilePoint(tmpX, tmpY);
          TerrainTile obj = new TerrainTile(point);
          obj.objPoint = cur;
          obj.iFromValue = point.getDistance(PointA.objPoint);
          obj.iToValue = point.getDistance(PointB.objPoint);;
          obj.iFValue = obj.iFromValue + obj.iToValue;
          arrOpen.push(obj);
        }
      }
  }


  if (PointB.objParent)//找到了
  {

  }
  else//堵死了
  {

  }
}
