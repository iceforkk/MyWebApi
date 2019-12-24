＃自用的WebAPI
基础架构：
1：EF + AutoFac +webapi+oAuts2
2：增加接口报错自动日志，权限拦截，跨域请求处理
AutoFac支持自动注入



一、均匀生成两个数值之间的随机数的探索

1、首先考虑要生成的随机数为10个，假设当要生成0~9这10个数，必然要做的是利用Math.random()方法并与10相乘，接着取整，而取整一般有以下方法：

~~两次否运算，舍弃小数部分
1.23^0  异或运算符，舍弃小数部分
1.23<<0 左移运算符 ，舍弃小数部分
1.23>>0 右移运算符，舍弃小数部分
1.23>>>0带符号的右移运算符，只对正数有用
parseInt()舍弃小数部分
Math.round()四舍五入取整
Math.floor()向下取整 
除了Math.round()、Math.ceil()、带符号的右移运算方法外，其他的都是一样的（而右移在处理整数时，也可以归为此）。

利用Math.round()

Math.round(Math.random()*10)
1
我试验5000次，得到如下直方图（直方图的原理是，每次获得的随机数，都让其对应的方块的高度加1）：

Math.round()5000次后得到的直方图

显然此时0出现的概率是其他数字的一半。
利用Math.floor()

Math.floor(Math.random()*10)
1
依旧试验5000次，得到如下直方图：

Math.floor()5000次后得到的直方图

显然此时0~9这10个数出现的概率是相等的。
利用Math.ceil(),显然如果此时只是简单的乘以10,那么0这个数字将永远不会出现，而且随机数中会多出现一个10，为此，应该再减1,

Math.ceil(Math.random()*10-1)
1
依旧试验5000次，得到如下直方图：

Math.ceil()5000次后得到的直方图

显然此时0~9这10个数出现的概率是相等的。
接下来讨论一下如何生成两个数之间的随机数
假设我们现在要生成23~28之间的随机数，利用以下代码：

Math.floor(Math.random()*(28-22)+23)
1
这次试验2000000次（这次我让随机数对应的方块的高度每次加0.001），得到了以下直方图：

试验2000000次

均匀得无可挑剔：
重复几次试验，可以总结出均匀获得两个数字之间的随机数的公式：

function randomNum(max,min){
        return Math.floor(Math.random()*(max-min+1)+min)
    }
1
2
3
如果是利用Math.round的话就是以下公式：

function randomNum(max,min){
        return Math.round(Math.random()*(max-min+1)+min-0.5)
    }
1
2
3
如果是利用Math.ceil的话就是以下公式：

function randomNum(max,min){
        return Math.ceil(Math.random()*(max-min+1)+min-1)
    }
1
2
3
接下来利用概率相等的函数生成一个随机颜色的函数，之前一篇文章我也有写到
点击这里进入
1、十六位进制颜色：

function randomColor(){
    var color="#";
    var colorArr=["0","1","2","3","4","5","6","7","8","9","a","b","c","d","e","f"];
    for(i=0;i<6;i++){
        var cur=randomNum(15,0);
        color+=colorArr[cur];
    }
    function randomNum(max,min){
        return Math.floor(Math.random()*(max-min+1)+min)
    }
    return color;
} 
2、rgb颜色：

function randomColor(){
    var color="rgb(";
    for(i=0;i<3;i++){
        var cur=randomNum(255,0);
        if(i>1){
            color+=cur+")"
        }else{
            color+=cur+","
        }
    }
    function randomNum(max,min){
        return Math.floor(Math.random()*(max-min+1)+min)
    }
    return color;
} 
二、生成概率不等的随机数

我们先考虑0-1之间的随机数：
我们知道一个事实，小于1的数相乘将会更加小，因此利用两个随机数相乘即可得到一个偏向0的随机数，如下：

循环100000次后的结果

我采用生成随机数的代码是：

(Math.random()*Math.random()-0.05).toFixed(1)
1
显然0出现的概率最大，而且逐渐递减（如果更多的随机数相乘将会使0出现的概率更加大）。
在上面我们知道了使0概率增大的情况，目前这个方法的运用的话，可以用于一个个射击的游戏中，更改随机数的个数可以实现玩游戏的人射中靶心的概率增加（类似于升级吧），但是概率不太好控制，现在我们考虑一个实际问题假设有一个抽奖系统，一等奖的概率显然不能和二等奖或是不中奖的概率一样，并且还需要对概率进行精准的控制。
常见的场景一：

不同等分的转盘

我们经常会看到这样的解决方案，将圆进行不同等分，当然我们这个可以利用js实现，这个方案也会相对容易实现，这时Math.round()等那些方法可能就会派不上用场了，而且这容易让人看出那些好奖品的概率更加低，商家应该不愿意吧，哈哈哈（瞎bb两句）。
常见场景二：

等分的转盘

那么这个时候要对概率可控，可能就需要我们用代码来实现了。
显然可以把0-1进行分配实现，如：

var randomNum=Math.random();
    if(randomNum<0.01){
        /*奖品一coding*/
    }
    else if(randomNum<0.04){
        /*奖品二coding*/
    }
    else if(randomNum<0.1){
        /*奖品三coding*/
    }
    else if(randomNum<1){
        /*奖品四coding*/
    } 
另外看到过开发者KaiFaX的一种实现方法，这种方法是上面讨论过的升级版。
如下：

var pri=["一等奖","二等奖","三等奖","参与奖"];
    function updateRandom(){
        var ranNum=Math.random();
        var n=Math.random()/4;
        if(ranNum<0.01){
            return n;
        }
        else if(ranNum<0.04){
            return n+0.25;
        }
        else if(ranNum<0.1){
            return n+0.5;
        }
        else if(ranNum<1){
            return n+0.75;
        }
    }
    var randomIn=Math.floor(4*updateRandom());
    alert(pri[randomIn]); 
参考资料：math.random()二三事

本人的案例：

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Document</title>
</head>
<body>

    <script>
        var pri=["一等奖","二等奖","三等奖","参与奖"];
    function updateRandom(){
        var ranNum=Math.random();//生成一个0-1的随机数
        var n=Math.random()/4;//0-0.25随机数
        if(ranNum<0.01){
            return n;
        }
        else if(ranNum<0.04){
            return n+0.25;//小于0.04+0.25=0.29
        }
        else if(ranNum<0.1){
            return n+0.5;//小于0.1+0.5=0.6
        }
        else if(ranNum<1){
            return n+0.75;//小于1+0.75=1.75
        }
    }
    var randomIn=Math.floor(4*updateRandom());
    alert(pri[randomIn]);

    </script>
</body>
</html> 
