public class RespondJson
{
    /// <summary>
    ///加密的响应结果
    /// </summary>
    public string data;

    public string encryptkey;

}

public class ResponseData
{
    public string merchantaccount;
    public string yborderid;
    public string orderid;
    public string payurl;
    public string imghexstr;
    public string sign;
    public string error_code;
    public string error_msg;
 
  
}
public class YeeCallBackResponseModel 
{
    public string Sign { get; set; }

    public string MerchantAccount { get; set; }
    public string CardType { get; set;}
    public int Amount { get; set; }
    public int Status { get; set; }
    public string BankCode { get; set; }
    public string Bank { get; set; }
    public string OrderId { get; set; }
    public string YborderId { get; set; }
    public string Lastno { get; set; }
 
}