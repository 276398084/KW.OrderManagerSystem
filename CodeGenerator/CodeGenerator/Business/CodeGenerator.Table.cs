//-----------------------------------------------------------------
// All Rights Reserved , Copyright (C) 2012 , Hairihan TECH, Ltd. 
//-----------------------------------------------------------------

using System;
using System.Xml;

namespace CodeGenerator.Business
{
    /// <summary>
    ///	CodeGenerator
    /// 主键生成器
    /// 
    /// 修改纪录
    /// 
    ///		2011.10.13 版本：1.0    吉日嘎拉 整理。
    ///	
    /// 版本：1.0
    ///
    /// <author>
    ///		<name>JiRiGaLa</name>
    ///		<date>2011.10.13</date>
    /// </author> 
    /// </summary>
    public partial class CodeGenerator
    {
        string sss = string.Empty;
        public bool BuilderTable(string outputDirectory, bool overwrite, bool twoTier = true, string ssss = "")
        {
            sss = ssss;
            this.postfix = "TypeMap";
            string fileName = outputDirectory + "\\" + this.project + ".Domain\\" + "\\Mappings\\" + this.className + "TypeMap.cs";
            string code = this.BuilderTable(sss);
            return WriteCode(fileName, overwrite, code);
        }

        public string BuilderTable(string ssss)
        {
            sss = ssss;
            this.GetCodeCopyright();
            this.GetCodeUsing(false);
            this.GetCodeNamespace("Domain", true);
            this.GetCodeRemark();
            this.GetCodeClassName();
            this.GetCodeTableColumn(this.tableName, sss);
            this.GetCodeEnd();
            return this.CodeText.ToString();
        }

        private void GetCodeTableColumn(string tableName, string sss)
        {

            this.CodeText.AppendLine("        public " + this.className + "TypeMap()");
            this.CodeText.AppendLine("        {");
            this.CodeText.AppendLine("            Table(\"" + sss + "\");");
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
                        string Length = string.Empty;
                        for (int z = 0; z < xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes.Count; z++)
                        {
                            if (xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[z].LocalName.Equals("Name"))
                            {
                                fieldName = xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[z].InnerText;
                            }
                            if (xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[z].LocalName.Equals("Code"))
                            {
                                field = xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[z].InnerText;
                                // 关键字转换
                                this.IsKeywords(ref field);
                            }
                            if (xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[z].LocalName.Equals("Length"))
                            {
                                Length = xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[z].InnerText;
                            }
                            if (xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[z].LocalName.Equals("Comment"))
                            {
                                fieldDescription = xmlNode.ChildNodes[i].ChildNodes[j].ChildNodes[z].InnerText;
                            }
                        }
                        if (String.IsNullOrEmpty(fieldDescription))
                        {
                            fieldDescription = fieldName;
                        }
                        // 首字母进行强制大写改进

                        string fieldKey = field.Substring(0, 1).ToUpper() + field.Substring(1);

                        if (fieldKey == "Id")
                        {
                            this.CodeText.AppendLine("            Id(x => x.Id);");

                        }
                        else
                        {
                            //.Length(50);
                            if (string.IsNullOrEmpty(Length))
                            {
                                this.CodeText.AppendLine("            Map(x => x." + fieldKey + ");");
                            }
                            else
                            {
                                this.CodeText.AppendLine("            Map(x => x." + fieldKey + ").Length(" + Length + ");");
                            }
                        }
                    }
                    break;
                }
            }
            this.CodeText.AppendLine("        }");
        }
    }
}