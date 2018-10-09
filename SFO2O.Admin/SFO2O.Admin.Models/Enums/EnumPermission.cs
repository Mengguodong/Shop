using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Admin.Models
{
    /// <summary>
    /// 平台權限
    /// </summary>
    public enum EnumPermission
    {
        None = 0,

        /// <summary>
        /// 會員管理
        /// </summary>
        Member = 100,
        /// <summary>
        /// 會員查詢
        /// </summary>
        Member_List = 110,
        /// <summary>
        /// 導出
        /// </summary>
        Member_List_Export = 111,

        /// <summary>
        /// 商家管理
        /// </summary>
        Supplier = 200,
        /// <summary>
        /// 管理商家
        /// </summary>
        Supplier_Manage = 210,
        /// <summary>
        /// 導出
        /// </summary>
        Supplier_Manage_Export = 211,
        /// <summary>
        /// 添加商家
        /// </summary>
        Supplier_Manage_Add = 212,
        /// <summary>
        /// 編輯
        /// </summary>
        Supplier_Manage_Edit = 213,
        /// <summary>
        /// 凍結／解除
        /// </summary>
        Supplier_Manage_Froze = 214,

        /// <summary>
        /// 商品管理
        /// </summary>
        Product = 300,
        /// <summary>
        /// 商品審核
        /// </summary>
        Product_AuditList = 310,
        /// <summary>
        /// 導出
        /// </summary>
        Product_AuditList_Export = 311,
        /// <summary>
        /// 審核
        /// </summary>
        Product_AuditList_Audit = 312,
        /// <summary>
        /// 管理商品
        /// </summary>
        Product_Manage = 320,
        /// <summary>
        /// 導出
        /// </summary>
        Product_Manage_Export = 321,
        /// <summary>
        /// 下架／允許上架
        /// </summary>
        Product_Manage_ChangeStatus = 322,
        /// <summary>
        /// 查看库存
        /// </summary>
        Product_InventoryList = 330,
        /// <summary>
        /// 導出
        /// </summary>
        Product_InventoryList_Export = 331,

        /// <summary>
        /// 訂單管理
        /// </summary>
        Order = 400,
        /// <summary>
        /// 訂單列表
        /// </summary>
        Order_List = 410,
        /// <summary>
        /// 導出
        /// </summary>
        Order_List_Export = 411,
        /// <summary>
        /// 退款單列表
        /// </summary>
        Order_RefundList = 420,
        /// <summary>
        /// 導出
        /// </summary>
        Order_RefundList_Export = 421,
        /// <summary>
        /// 審核
        /// </summary>
        Order_RefundList_Audit = 422,
        /// <summary>
        /// 退款單管理
        /// </summary>
        Order_SettlementManage = 430,

        /// <summary>
        /// 財務管理
        /// </summary>
        Finance = 500,
        /// <summary>
        /// 退款單管理
        /// </summary>
        Finance_RefundManage = 510,
        /// <summary>
        /// 導出
        /// </summary>
        Finance_RefundManage_Export = 511,
        /// <summary>
        /// 審核
        /// </summary>
        Finance_RefundManage__Audit = 512,
        /// <summary>
        /// 結算單管理
        /// </summary>
        Finance_SettlementManage = 520,
        /// <summary>
        /// 導出
        /// </summary>
        Finance_SettlementManage_Export = 521,
        /// <summary>
        /// 付款
        /// </summary>
        Finance_SettlementManage_Pay = 522,

        /// <summary>
        /// 賬號管理
        /// </summary>
        Account = 600,
        /// <summary>
        /// 後台用戶
        /// </summary>
        Account_UserManage = 610,
        /// <summary>
        /// 添加賬號
        /// </summary>
        Account_UserManage_Add = 611,
        /// <summary>
        /// 編輯
        /// </summary>
        Account_UserManage_Edit = 612,
        /// <summary>
        /// 禁用/啟用
        /// </summary>
        Account_UserManage_ChangeStatus = 613,
        /// <summary>
        /// 用户分組
        /// </summary>
        Account_RoleManage = 620,
        /// <summary>
        /// 添加分組
        /// </summary>
        Account_RoleManage_Add = 621,
        /// <summary>
        /// 查看對應賬號
        /// </summary>
        Account_RoleManage_ViewUser = 622,
        /// <summary>
        /// 修改
        /// </summary>
        Account_RoleManage_Edit = 623,
        /// <summary>
        /// 刪除
        /// </summary>
        Account_RoleManage_Delete = 624,
    }
}
