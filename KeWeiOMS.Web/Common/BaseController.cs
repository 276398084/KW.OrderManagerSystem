using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KeWeiOMS.Domain;
using System.Web.Mvc;
using System.Text;
using System.EnterpriseServices;
using System.Configuration;
using KeWeiOMS.NhibernateHelper;
using NHibernate;
using KeWeiOMS.Domain;
using System.Web.UI;

namespace KeWeiOMS.Web.Controllers
{
    [SupportFilter]//此处如果去掉注释，则全部继承BaseController的Controller，都将执行SupportFilter过滤
    public class BaseController : Controller
    {
        public ISession NSession = NHibernateHelper.CreateSession();

        private UserType currentUser;

        public UserType CurrentUser
        {
            get
            {
                if (currentUser == null)
                {
                    UserType account = GetCurrentAccount();
                    return account;
                }
                return currentUser;
            }
        }

        /// <summary>
        /// 获取当前登陆人的账户信息
        /// </summary>
        /// <returns>账户信息</returns>
        public UserType GetCurrentAccount()
        {
            if (Session["account"] != null)
            {
                UserType account = (UserType)Session["account"];
                return account;
            }
            return null;
            //return new UserType { Id = 0, Realname = "邵锡栋" };
        }

        public BaseController()
        {

        }

        public void GetPM()
        {


            List<PermissionScopeType> listByModules = new List<Domain.PermissionScopeType>();
            List<PermissionScopeType> listByPermissions = new List<Domain.PermissionScopeType>();

            foreach (var item in GetUserScope())
            {
                if (item.TargetCategory == TargetCategoryEnum.Module.ToString())
                    listByModules.Add(item);
                else
                    listByPermissions.Add(item);
            }
            foreach (var item in GetRoleScope())
            {
                if (item.TargetCategory == TargetCategoryEnum.Module.ToString())
                    listByModules.Add(item);
                else
                    listByPermissions.Add(item);
            }
            foreach (var item in GetDepartmentScope())
            {
                if (item.TargetCategory == TargetCategoryEnum.Module.ToString())
                    listByModules.Add(item);
                else
                    listByPermissions.Add(item);
            }
            string mids = "";
            string pids = "";

            foreach (var item in listByModules)
            {
                mids += item.Id + ",";
            }
            foreach (var item in listByPermissions)
            {
                pids += item.Id + ",";
            }
            mids = mids.Trim(',');
            pids = pids.Trim(',');

            List<ModuleType> Modules = NSession.CreateQuery("from ModuleType where Id=in(;p)").SetString("p1", mids).List<ModuleType>().ToList<ModuleType>();
            List<PermissionItemType> Permissions = NSession.CreateQuery("from PermissionItemType where Id=in(;p)").SetString("p1", mids).List<PermissionItemType>().ToList<PermissionItemType>();

            CurrentUser.Modules = Modules;
            currentUser.Permissions = Permissions;
            Session["account"] = CurrentUser;
        }


        public List<PermissionScopeType> GetUserScope()
        {
            List<PermissionScopeType> list = NSession.CreateQuery("from PermissionScopeType where ResourceCategory=:p1 and ResourceId=:p2").SetString("p1", ResourceCategoryEnum.User.ToString()).SetInt32("p2", CurrentUser.Id).List<PermissionScopeType>().ToList<PermissionScopeType>();
            return list;
        }

        public List<PermissionScopeType> GetRoleScope()
        {
            List<PermissionScopeType> list = NSession.CreateQuery("from PermissionScopeType where ResourceCategory=:p1 and ResourceId=:p2").SetString("p1", ResourceCategoryEnum.Role.ToString()).SetInt32("p2", CurrentUser.RoleId).List<PermissionScopeType>().ToList<PermissionScopeType>();
            return list;
        }

        public List<PermissionScopeType> GetDepartmentScope()
        {
            List<PermissionScopeType> list = NSession.CreateQuery("from PermissionScopeType where ResourceCategory=:p1 and ResourceId=:p2").SetString("p1", ResourceCategoryEnum.User.ToString()).SetInt32("p2", CurrentUser.DId).List<PermissionScopeType>().ToList<PermissionScopeType>();
            return list;
        }

        /// <summary>
        /// 获取组织机构权限域数据
        /// </summary>
        //public OrganizeType GetOrganizeScope(string permissionItemScopeCode, bool isInnerOrganize)
        //{
        //    // 获取部门数据，不启用权限域
        //    List<OrganizeType> list =new List<Domain.OrganizeType>();

        //    if ((CurrentUser.IsVisible) || (String.IsNullOrEmpty(permissionItemScopeCode)))
        //    {
        //        dataTable = ClientCache.Instance.GetOrganizeDT(UserInfo).Copy();
        //        if (isInnerOrganize)
        //        {
        //            BaseBusinessLogic.SetFilter(dataTable, BaseOrganizeEntity.FieldIsInnerOrganize, "1");
        //            BaseInterfaceLogic.CheckTreeParentId(dataTable, BaseOrganizeEntity.FieldId, BaseOrganizeEntity.FieldParentId);
        //        }
        //        dataTable.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
        //    }
        //    else
        //    {
        //        DotNetService dotNetService = new DotNetService();
        //        dataTable = dotNetService.PermissionService.GetOrganizeDTByPermissionScope(UserInfo, UserInfo.Id, permissionItemScopeCode);
        //        if (dotNetService.PermissionService is ICommunicationObject)
        //        {
        //            ((ICommunicationObject)dotNetService.PermissionService).Close();
        //        }
        //        if (isInnerOrganize)
        //        {
        //            BaseBusinessLogic.SetFilter(dataTable, BaseOrganizeEntity.FieldIsInnerOrganize, "1");
        //            BaseInterfaceLogic.CheckTreeParentId(dataTable, BaseOrganizeEntity.FieldId, BaseOrganizeEntity.FieldParentId);
        //        }
        //        dataTable.DefaultView.Sort = BaseOrganizeEntity.FieldSortCode;
        //    }
        //    return dataTable;
        //}

        ///// <summary>
        ///// 获取用户权限域数据
        ///// </summary>
        //public DataTable GetUserScope(string permissionItemScopeCode)
        //{
        //    // 是否有用户管理权限，若有用户管理权限就有所有的用户类表，这个应该是内置的操作权限
        //    bool userAdmin = false;
        //    userAdmin = this.IsAuthorized("UserAdmin");
        //    DataTable returnValue = new DataTable(BaseUserEntity.TableName);
        //    // 获取用户数据
        //    DotNetService dotNetService = new DotNetService();
        //    if (userAdmin)
        //    {
        //        if (this.UserInfo.IsAdministrator || (String.IsNullOrEmpty(permissionItemScopeCode)))
        //        {
        //            returnValue = dotNetService.UserService.GetDataTable(UserInfo);
        //            if (dotNetService.UserService is ICommunicationObject)
        //            {
        //                ((ICommunicationObject)dotNetService.UserService).Close();
        //            }
        //        }
        //        else
        //        {
        //            returnValue = dotNetService.PermissionService.GetUserDTByPermissionScope(UserInfo, UserInfo.Id, permissionItemScopeCode);
        //            if (dotNetService.PermissionService is ICommunicationObject)
        //            {
        //                ((ICommunicationObject)dotNetService.PermissionService).Close();
        //            }
        //        }

        //    }
        //    return returnValue;
        //}

        ///// <summary>
        ///// 获取角色权限域数据
        ///// </summary>
        //public DataTable GetRoleScope(string permissionItemScopeCode)
        //{
        //    // 获取部门数据
        //    DataTable returnValue = new DataTable(BaseOrganizeEntity.TableName);
        //    DotNetService dotNetService = new DotNetService();
        //    if ((UserInfo.IsAdministrator) || (String.IsNullOrEmpty(permissionItemScopeCode)))
        //    {
        //        returnValue = dotNetService.RoleService.GetDataTable(UserInfo);
        //        if (dotNetService.RoleService is ICommunicationObject)
        //        {
        //            ((ICommunicationObject)dotNetService.RoleService).Close();
        //        }
        //    }
        //    else
        //    {
        //        returnValue = dotNetService.PermissionService.GetRoleDTByPermissionScope(UserInfo, UserInfo.Id, permissionItemScopeCode);
        //        if (dotNetService.PermissionService is ICommunicationObject)
        //        {
        //            ((ICommunicationObject)dotNetService.PermissionService).Close();
        //        }
        //    }
        //    return returnValue;
        //}

        ///// <summary>
        ///// 获取模块菜单限域数据
        ///// </summary>
        //public  GetModuleScope(string permissionItemScopeCode)
        //{
        //    DotNetService dotNetService = new DotNetService();
        //    // 获取部门数据
        //    if ((UserInfo.IsAdministrator) || (String.IsNullOrEmpty(permissionItemScopeCode)))
        //    {
        //        DataTable dtModule = dotNetService.ModuleService.GetDataTable(UserInfo);
        //        if (dotNetService.ModuleService is ICommunicationObject)
        //        {
        //            ((ICommunicationObject)dotNetService.ModuleService).Close();
        //        }
        //        // 这里需要只把有效的模块显示出来
        //        // BaseBusinessLogic.SetFilter(dtModule, BaseModuleEntity.FieldEnabled, "1");
        //        // 未被打上删除标标志的
        //        // BaseBusinessLogic.SetFilter(dtModule, BaseModuleEntity.FieldDeletionStateCode, "0");
        //        return dtModule;
        //    }
        //    else
        //    {
        //        DataTable dataTable = dotNetService.PermissionService.GetModuleDTByPermissionScope(UserInfo, UserInfo.Id, permissionItemScopeCode);
        //        if (dotNetService.PermissionService is ICommunicationObject)
        //        {
        //            ((ICommunicationObject)dotNetService.PermissionService).Close();
        //        }
        //        BaseInterfaceLogic.CheckTreeParentId(dataTable, BaseModuleEntity.FieldId, BaseModuleEntity.FieldParentId);
        //        return dataTable;
        //    }
        //}

        ///// <summary>
        ///// 获取授权范围数据 （按道理，应该是在某个数据区域上起作用）
        ///// </summary>
        //public DataTable GetPermissionItemScop(string permissionItemScopeCode)
        //{
        //    // 获取部门数据
        //    DataTable dtPermissionItem = new DataTable(BasePermissionItemEntity.TableName);
        //    DotNetService dotNetService = new DotNetService();
        //    if (UserInfo.IsAdministrator)
        //    {
        //        dtPermissionItem = dotNetService.PermissionItemService.GetDataTable(UserInfo);
        //        if (dotNetService.PermissionItemService is ICommunicationObject)
        //        {
        //            ((ICommunicationObject)dotNetService.PermissionItemService).Close();
        //        }
        //        // 这里需要只把有效的模块显示出来
        //        // BaseBusinessLogic.SetFilter(dtPermissionItem, BasePermissionItemEntity.FieldEnabled, "1");
        //        // 未被打上删除标标志的
        //        // BaseBusinessLogic.SetFilter(dtPermissionItem, BasePermissionItemEntity.FieldDeletionStateCode, "0");

        //    }
        //    else
        //    {
        //        dtPermissionItem = dotNetService.PermissionService.GetPermissionItemDTByPermissionScope(UserInfo, UserInfo.Id, permissionItemScopeCode);
        //        if (dotNetService.PermissionService is ICommunicationObject)
        //        {
        //            ((ICommunicationObject)dotNetService.PermissionService).Close();
        //        }
        //        BaseInterfaceLogic.CheckTreeParentId(dtPermissionItem, BasePermissionItemEntity.FieldId, BasePermissionItemEntity.FieldParentId);
        //    }
        //    return dtPermissionItem;
        //}

        //#region public bool ModuleIsVisible(string moduleCode) 模块是否可见
        ///// <summary>
        ///// 模块是否可见
        ///// </summary>
        ///// <param name="moduleCode">模块编号</param>
        ///// <returns>有权限</returns>
        //public bool ModuleIsVisible(string moduleCode)
        //{
        //    bool returnValue = false;
        //    foreach (DataRow dataRow in ClientCache.Instance.DTMoule.Rows)
        //    {
        //        if (dataRow[BaseModuleEntity.FieldCode].ToString().Equals(moduleCode))
        //        {
        //            returnValue = dataRow[BaseModuleEntity.FieldEnabled].ToString().Equals("1");
        //            break;
        //        }
        //    }
        //    // 模块是否可见;
        //    return returnValue;
        //}
        //#endregion

        //#region public bool IsModuleAuthorized(string moduleCode) 模块是否有权限访问
        ///// <summary>
        ///// 模块是否有权限访问
        ///// </summary>
        ///// <param name="moduleCode">模块编号</param>
        ///// <returns>有权限</returns>
        //public bool IsModuleAuthorized(string moduleCode)
        //{
        //    bool returnValue = false;
        //    // 1：是否超级管理员？若是超级管理员，什么模块都是能访问的，这是为了提高判断程序的执行效率
        //    if (this.UserInfo.IsAdministrator)
        //    {
        //        returnValue = true;
        //        return returnValue;
        //    }
        //    // 2：是否已经设置为公开？公开的模块谁都可以访问的
        //    foreach (DataRow dataRow in ClientCache.Instance.DTMoule.Rows)
        //    {
        //        if (dataRow[BaseModuleEntity.FieldCode].ToString().Equals(moduleCode))
        //        {
        //            returnValue = dataRow[BaseModuleEntity.FieldIsPublic].ToString().Equals("1");
        //            break;
        //        }
        //    }
        //    // 3：当前用户是否有模块访问权限？（已包含用户的、角色的模块访问权限）
        //    if (!returnValue)
        //    {
        //        foreach (DataRow dataRow in ClientCache.Instance.DTUserMoule.Rows)
        //        {
        //            if (dataRow[BaseModuleEntity.FieldCode].ToString().Equals(moduleCode))
        //            {
        //                returnValue = true;
        //                break;
        //            }
        //        }
        //    }
        //    return returnValue;
        //}
        //#endregion

    }
}
