//由三原色值合成颜色整数值
function ColorFromRGB(red, green, blue)
{
    return red + green*256 + blue*256*256;
}

//创建一个MSXML DOM 对象
function CreateXMLDOM() 
{
    var arrSignatures = ["MSXML2.DOMDocument.5.0", "MSXML2.DOMDocument.4.0",
                         "MSXML2.DOMDocument.3.0", "MSXML2.DOMDocument",
                         "Microsoft.XmlDom"];
    for (var i=0; i < arrSignatures.length; i++) 
    {
        try 
        {
            var oXmlDom = new ActiveXObject(arrSignatures[i]);
            return oXmlDom;
        
        } 
        catch (oError) 
        {
            //ignore
        }
    }              

    throw new Error("你的系统没有安装MSXML");           
}     
