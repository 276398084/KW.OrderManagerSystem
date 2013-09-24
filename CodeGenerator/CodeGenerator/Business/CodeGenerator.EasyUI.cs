using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CodeGenerator.Business
{
    public partial class CodeGenerator
    {
        StringBuilder cjs = new StringBuilder();
        StringBuilder ejs = new StringBuilder();
        StringBuilder chtml = new StringBuilder();
        StringBuilder ehtml = new StringBuilder();
        StringBuilder dhtml = new StringBuilder();
        StringBuilder ihtml = new StringBuilder();
        StringBuilder detailhtml = new StringBuilder();

        StringBuilder easySearch = new StringBuilder();
        StringBuilder advancedSearch = new StringBuilder();



        public string GetEasySearch()
        {
            GetEasyCreateInit(this.tableName);
            return easySearch.ToString();
        }




        public string ReplaceEasy(string code)
        {
            GetEasyCreateInit(this.tableName);

            code = code.Replace("<#ControllerRootName#>", this.className);
            code = code.Replace("<#ViewDataTypeName#>", this.project + ".Domain." + this.className + "Type");
            code = code.Replace("<#cjs#>", cjs.ToString());
            code = code.Replace("<#ejs#>", ejs.ToString());
            code = code.Replace("<#chtml#>", chtml.ToString());
            code = code.Replace("<#ehtml#>", ehtml.ToString());
            code = code.Replace("<#dhtml#>", detailhtml.ToString());
            code = code.Replace("<#ihtml#>", ihtml.ToString());
            code = code.Replace("<#detailhtml#>", detailhtml.ToString());
            code = code.Replace("<#easySearch#>", easySearch.ToString());
            return code;
        }

        public void GetEasyCreateInit(string tableName)
        {
            int a = 0;
            int b = 0;
            XmlNode xmlNode = this.GetXmlNode(tableName);
            for (int i = 0; i < xmlNode.ChildNodes.Count; i++)
            {
                if (((XmlNode)xmlNode.ChildNodes[i]).LocalName.Equals("Columns"))
                {
                    for (int j = 0; j < xmlNode.ChildNodes[i].ChildNodes.Count; j++)
                    {
                        string field = string.Empty;
                        string fieldName = string.Empty;
                        string fieldDescription = string.Empty;
                        string fieldDataType = string.Empty;
                        string dataType = string.Empty;
                        string fieldDefaultValue = string.Empty;
                        for (int z = 0; z < xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes.Count; z++)
                        {
                            if (xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[z].LocalName.Equals("Name"))
                            {
                                fieldName = xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[z].InnerText;
                            }
                            if (xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[z].LocalName.Equals("Code"))
                            {
                                field = xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[z].InnerText;
                                //关键字转换
                                this.IsKeywords(ref field);
                            }
                            if (xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[z].LocalName.Equals("DataType"))
                            {
                                //字段类型大写
                                fieldDataType = xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[z].InnerText.ToUpper();
                            }
                            if (xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[z].LocalName.Equals("Comment"))
                            {
                                fieldDescription = xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[z].InnerText;
                            }
                            if (xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[z].LocalName.Equals("DefaultValue"))
                            {
                                fieldDefaultValue = xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[z].InnerText;
                            }
                        }
                        if (String.IsNullOrEmpty(fieldDescription))
                        {
                            fieldDescription = fieldName;
                        }
                        // 首字母进行强制大写改进
                        string fieldKey = field.Substring(0, 1).ToUpper() + field.Substring(1);
                        string defaultValue = fieldDefaultValue;
                        dataType = "";
                        dataType = GetDataType(fieldDataType, ref defaultValue);
                        if (dataType == "DateTime")
                        {
                            cjs.AppendLine("");
                            cjs.AppendLine(String.Format("        $('#c_{0}').datebox();", fieldKey));
                            cjs.AppendLine("");

                            ejs.AppendLine("");
                            ejs.AppendLine(String.Format("        $('#e_{0}').datebox();", fieldKey));
                            ejs.AppendLine("");
                        }
                        if (fieldKey.EndsWith("By"))
                        {
                            cjs.AppendLine(String.Format("        $('#c_{0}').combobox({{", fieldKey));
                            cjs.AppendLine("          url: 'User/UserSrc',");
                            cjs.AppendLine("          editable: true");
                            cjs.AppendLine("      });");
                            cjs.AppendLine("");

                            ejs.AppendLine(String.Format("        $('#e_{0}').combobox({{", fieldKey));
                            ejs.AppendLine("          url: 'User/UserSrc',");
                            ejs.AppendLine("          editable: true");
                            ejs.AppendLine("      });");
                            ejs.AppendLine("");
                        }

                        if (a == 0)
                        {
                            chtml.AppendLine("	<tr>");
                            ehtml.AppendLine("	<tr>");
                        }
                        chtml.AppendLine("		<td align=\"right\">");
                        chtml.AppendLine(string.Format("            {0}：", fieldName));
                        chtml.AppendLine("		</td>");
                        chtml.AppendLine("		<td align=\"left\">");
                        chtml.AppendLine(string.Format("            <input class='easyui-validatebox' type='text' name='{0}' id='{0}'  data-options=\"required:true\"></input>", fieldKey));
                        chtml.AppendLine("		</td>");
                        a++;
                        ehtml.AppendLine("		<td align=\"right\">");
                        ehtml.AppendLine(string.Format("            {0}：", fieldName));
                        ehtml.AppendLine("		</td>");
                        ehtml.AppendLine("		<td align=\"left\">");
                        ehtml.AppendLine(string.Format("            <input  type='text' name='{0}' id='{0}' value='@Model.{0}' ></input>", fieldKey));
                        ehtml.AppendLine("		</td>");
                        if (a == 2)
                        {
                            chtml.AppendLine("	</tr>");
                            ehtml.AppendLine("	</tr>");
                            a = 0;
                        }


                        ////以上添加和修改页面修改完成

                        //生成Grid字段
                        if (j == 0)
                        {
                            ihtml.AppendLine("[[");
                        }
                        ihtml.AppendLine("{ title: '"+fieldName+"', field: '"+fieldKey+"', width: 80, sortable: true },");
                           if (j == xmlNode.ChildNodes[i].ChildNodes.Count - 1)
                        {
                            ihtml = ihtml.Remove(ihtml.Length - 1, 0);
                            ihtml.AppendLine("]]");
                        }
                        if (b == 0)
                        {
                            easySearch.AppendLine("<tr>");
                        }
                        easySearch.AppendLine(string.Format(@"            <td><label>{1}:</label><input type='text' id='txt{0}' /></td>", fieldKey, fieldName));
                        b++;

                        if (b == 4)
                        {
                            if (j == xmlNode.ChildNodes[i].ChildNodes.Count - 1)
                            {
                                easySearch.AppendLine("<a href='#' class='easyui-linkbutton' iconcls='icon-search' onclick='doSearch();'>查询</a>");
                            }
                            easySearch.AppendLine("</tr>");
                            b = 0;
                        }

                    }
                    if (b != 0)
                    {
                        easySearch.AppendLine("<a href='#' class='easyui-linkbutton' iconcls='icon-search' onclick='doSearch();'>查询</a>");
                        easySearch.AppendLine("</tr>");
                    }

                    break;
                }
            }

        }

    }
}
