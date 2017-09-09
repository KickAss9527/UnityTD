//path ----------
//12w x 10h
var defaultTerrain = [
 "XXXXXXOOOOOO", //0
 "XXXXXXOOOOXO", //1
 "XXXXXXOOOOXX", //2
 "XXXXXXOOOOXX", //3
 "XXXXXOOXXXXX", //4
 "XXXXOOXXXXXX", //5
 "XXXOOXXXXXXX", //6
 "XXOOXXXXXXXX", //7
 "XOOXXXXXXXXX", //8
 "OOXXXXXXXXXX"];//9
 var TerrainConfig;
 this.init = function()
 {
   TerrainConfig = [9];
   for (var i = 0; i < defaultTerrain.length; i++) {
     TerrainConfig[i] = defaultTerrain[i];
   }
 }
 this.init();
// 0123456789
var TerrainWidth = TerrainConfig[0].length;
var TerrainHeight = TerrainConfig.length;
var PointA = new TerrainTile(new TerrainTilePoint(11, 1));
var PointB = new TerrainTile(new TerrainTilePoint(0, 9));
this.getStartPointTag = function(){return PointA.objPoint.tId;}
this.getEndPointTag = function(){return PointB.objPoint.tId;}

this.updateConfigTileDisable = function(idx)
{
  var x = idx%TerrainWidth;
  var y = parseInt(idx/TerrainWidth);
  if(y >= TerrainHeight)
  {
    console.log("Invalid building tile idx");
    return;
}
  console.log("x", x);
  console.log("y", y);
  var str = TerrainConfig[y];
  str = str.substr(0, x) + "X" + str.substr(x+1, str.length);
  TerrainConfig[y] = str;
}
this.getTerrainConfig = function()
{
  return TerrainConfig;
}
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
var defaultPath = null;
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
    var res = [PointB.objPoint.tId];
    var cur = PointB;
    while (cur.objParent)
    {
        res.push(cur.objParent.objPoint.tId);
        cur = cur.objParent;
    }
    res.reverse();
    if (!defaultPath) defaultPath = res;
    return res;
  }
  else//堵死了
  {
    console.log("no available path..");
    return defaultPath;
  }
}
console.log('module terrain..');
this.searchPath = function()
{
  return findPath(TerrainConfig);
};
