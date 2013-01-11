using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.IO;
using System.Text;
using Langben.IBLL;
using Langben.DAL;
using Langben.BLL;
using Common;

namespace  Models
{
    /// <summary>
    /// 上传控件的后台处理
    /// </summary>
    public class FileUploaderHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";
            var filde = context.Request["queueId"];
            HttpPostedFile file = context.Request.Files["Filedata"];
            string uploadPath = HttpContext.Current.Server.MapPath(@context.Request["folder"]) + "\\";

            if (file != null)
            {
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                 FileUploader entity = new  FileUploader();
                entity.Id = Common.Result.GetNewId();
                //存储到服务器
                file.SaveAs(uploadPath + entity.Id + file.FileName);

                #region 记录到数据库

                entity.CreatePerson = AccountModel.GetCurrentPerson();
                entity.CreateTime = DateTime.Now;
                entity.Id = Common.Result.GetNewId();
                string returnValue = string.Empty;
                 IFileUploaderBLL m_BLL = new  FileUploaderBLL();
                Common.ValidationErrors validationErrors = new Common.ValidationErrors();
                entity.Size = file.ContentLength;
                entity.Suffix = file.FileName.Substring(file.FileName.IndexOf('.') + 1);
                entity.Path = uploadPath;
                entity.FullPath = uploadPath + file.FileName;
                entity.Name = file.FileName;

                if (m_BLL.Create(ref validationErrors, entity))
                {//下面这句代码缺少的话，上传成功后上传队列的显示不会自动消失
                    context.Response.Write(entity.Id);
                    LogClassModels.WriteServiceLog(Suggestion.InsertSucceed + "，附件的信息的Id为" + entity.Id, "附件"
                        );//写入日志 
                }
                else
                {//下面这句代码缺少的话，上传成功后上传队列的显示不会自动消失
                    context.Response.Write("0");
                    if (validationErrors != null && validationErrors.Count > 0)
                    {
                        validationErrors.All(a =>
                        {
                            returnValue += a.ErrorMessage;
                            return true;
                        });
                    }
                    LogClassModels.WriteServiceLog(Suggestion.InsertFail + "，附件的信息，" + returnValue, "附件"
                        );//写入日志    

                }
                #endregion
            }
            else
            {
                context.Response.Write("0");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
