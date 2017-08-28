//path ----------
//12w x 10h

var TerrainConfig = [
  "XXXXXXXOOOOO", //0
  "XXXXXXXOOOOX", //1
  "XXXXXXXOOOOX", //2
  "XXXXXXXXOXXX", //3
  "XXXOOOOOOXXX", //4
  "XXXOXXXXXXXX", //5
  "XOOOXXXXXXXX", //6
  "XOOOXXXXXXXX", //7
  "XOOOXXXXXXXX", //8
  "OOOOXXXXXXXX"];//9
// 0123456789
var TerrainWidth = TerrainConfig[0].length;
var TerrainHeight = TerrainConfig.length;

function convertXYToId(x, y){return x + y*TerrainWidth;}
function TerrainTilePoint(x, y)
{
  this.iX = x;
  this.iY = y;
  this.tId = convertXYToId(x, y);
  this.getDistance = function(obj)
  {
    return Math.pow(obj.iX - this.iX, 2) + Math.pow(obj.iY - this.iY, 2);
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

var PointA = new TerrainTile(new TerrainTilePoint(11, 0));
var PointB = new TerrainTile(new TerrainTilePoint(0, 9));

function findPath(terrain)
{
  PointB.objParent = null; //清空上次记录
  var arrOpen = [PointA];
  var arrClose = [];

  function isInCloseList(tId)
  {
    for (var i = 0; i < arrClose.length; i++)
    {
       var tile = arrClose[i];
       if (tile.objPoint.tId == tId) return true;
    }
    return false;
  }

  function findInOpenList(tId)
  {
    for (var i = 0; i < arrOpen.length; i++) {
       var tile = arrOpen[i];
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
  while (arrOpen.length > 0 && !PointB.objParent)
  {
      var cur = arrOpen.pop();
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
        var tmpTid = convertXYToId(tmpX, tmpY);

        //available path found
        if (tmpTid == PointB.objPoint.tId)
        {
          PointB.objParent = cur;
          break;
        }

        //不要
        if (isInCloseList(tmpTid)) continue;

        var existTile = findInOpenList(tmpTid);
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
          var point = new TerrainTilePoint(tmpX, tmpY);
          var obj = new TerrainTile(point);
          obj.objParent = cur;
          obj.iFromValue = point.getDistance(PointA.objPoint);
          obj.iToValue = point.getDistance(PointB.objPoint);;
          obj.iFValue = obj.iFromValue + obj.iToValue;
          arrOpen.push(obj);
        }
      }
  }

  if (PointB.objParent)//找到了
  {
    var res = [PointB];
    var cur = PointB;
    while (cur.objParent)
    {
        res.push(cur.objParent);
        cur = cur.objParent;
    }

    res.reverse();
    for (var i=0; i<res.length; i++)
    {
      var tile = res[i];
      var info = 'x: ' + tile.objPoint.iX;
      info += ', y: ' + tile.objPoint.iY;
      console.log(info);
    }
    console.log("path info : ", res.length);

    return res;
  }
  else//堵死了
  {
    console.log("no available path..");
    return null;
  }
}
console.log('module terrain..');
this.searchPath = function(){findPath(TerrainConfig);};
