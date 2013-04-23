//由三原色值合成颜色整数值
function ColorFromRGB(red, green, blue)
{
    return red + green*256 + blue*256*256;
}

//获取颜色中的红色值，传入参数为整数表示的RGB值
function ColorGetR(intColor)
{
    return intColor & 255;
}

//获取颜色中的绿色值，传入参数为整数表示的RGB值
function ColorGetG(intColor)
{
    //return intColor & 255*256;
    return (intColor>>8) & 255;
}

//获取颜色中的蓝色值，传入参数为整数表示的RGB值
function ColorGetB(intColor)
{
    //return intColor & 255*256*256;
    return (intColor>>16) & 255;
}