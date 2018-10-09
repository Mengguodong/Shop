using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFO2O.Supplier.Models
{
    /// <summary>
    /// 平台權限
    /// </summary>
    public enum EnumPermission
    {
        None = 0,
        /// <summary>
        /// 商品管理
        /// </summary>
        Product = 100,
        /// <summary>
        /// 商品上傳
        /// </summary>
        Product_Upload = 110,
        /// <summary>
        /// 管理商品
        /// </summary>
        Product_Manage = 120,
        /// <summary>
        /// 下架／允許上架
        /// </summary>
        Product_Manage_ChangeStatus = 121,
        /// <summary>
        /// 編輯
        /// </summary>
        Product_Manage_Edit = 122,
        /// <summary>
        /// 放棄編輯
        /// </summary>
        Product_Manage_DiscardChanges = 123,
        /// <summary>
        /// 查看庫存
        /// </summary>
        Product_InventoryList = 130,
        /// <summary>
        /// 導出
        /// </summary>
        Product_InventoryList_Export = 131,

        /// <summary>
        /// 訂單管理
        /// </summary>
        Order = 200,
        /// <summary>
        /// 订单列表
        /// </summary>
        Order_List = 210,
        /// <summary>
        /// 導出
        /// </summary>
        Order_List_Export = 211,
        /// <summary>
        /// 退款單列表
        /// </summary>
        Order_RefundList = 220,
        /// <summary>
        /// 導出
        /// </summary>
        Order_RefundList_Export = 221,

        /// <summary>
        /// 財務管理
        /// </summary>
        Finance = 300,
        /// <summary>
        /// 退款單管理
        /// </summary>
        Finance_RefundManage = 310,
        /// <summary>
        /// 導出
        /// </summary>
        Finance_RefundManage_Export = 311,
        /// <summary>
        /// 結算單管理
        /// </summary>
        Finance_SettlementManage = 320,
        /// <summary>
        /// 導出
        /// </summary>
        Finance_SettlementManage_Export = 321,

        /// <summary>
        /// 賬號管理
        /// </summary>
        Account = 400,
        /// <summary>
        /// 後台用戶
        /// </summary>
        Account_UserManage = 410,
        /// <summary>
        /// 添加賬號
        /// </summary>
        Account_UserManage_Add = 411,
        /// <summary>
        /// 編輯
        /// </summary>
        Account_UserManage_Edit = 412,
        /// <summary>
        /// 禁用／啟用
        /// </summary>
        Account_UserManage_ChangeStatus = 413,
        /// <summary>
        /// 用户分組
        /// </summary>
        Account_RoleManage = 420,
        /// <summary>
        /// 添加分組
        /// </summary>
        Account_RoleManage_Add = 421,
        /// <summary>
        /// 查看對應賬號
        /// </summary>
        Account_RoleManage_ViewUser = 422,
        /// <summary>
        /// 修改
        /// </summary>
        Account_RoleManage_Edit = 423,
        /// <summary>
        /// 刪除
        /// </summary>
        Account_RoleManage_Delete = 424,

        /// <summary>
        /// 品牌管理
        /// </summary>
        Brand = 500,
        /// <summary>
        /// 品牌列表
        /// </summary>
        Brand_List = 510,
        /// <summary>
        /// 添加品牌
        /// </summary>
        Brand_List_Add = 511,
        /// <summary>
        /// 编辑
        /// </summary>
        Brand_List_Edit = 512,
        /// <summary>
        /// 下架
        /// </summary>
        Brand_List_OffShelf = 513,
        /// <summary>
        /// 查看详情
        /// </summary>
        Brand_List_View = 514,
    }
}
