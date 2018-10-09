using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



/// <summary>
/// RSA加密和验证签名
/// </summary>
public class EncryptUtil
{
    /// <summary>
    /// 将SortedDictionary中的键值拼接成一个大字符串，然后使用RSA生成签名
    /// </summary>
    /// <returns></returns>
    public static string handleRSA(SortedDictionary<string, object> sd, string privatekey,string type)
    {
        StringBuilder sbuffer = new StringBuilder();
        foreach (KeyValuePair<string, object> item in sd)
        {
            sbuffer.Append(item.Value);
        }
        string sign = "";
        //生成签名
        sign = RSAFromPkcs8.sign(sbuffer.ToString(), privatekey, type);
        return sign;
    }
    /// <summary>
    /// 字符串RSA公钥加密
    /// </summary>
    /// <param name="data"></param>
    /// <param name="privatekey"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string handleRSA(string data, string publicKey,string type)
    {
        string sign = "";
        //生成签名
        sign = RSAFromPkcs8.encryptData(data.ToString(), publicKey, type);
        return sign;
    }



    /// <summary>
    /// 对一键支付返回的业务数据进行验签
    /// </summary>
    /// <param name="data"></param>
    /// <param name="encrypt_key"></param>
    /// <param name="yibaoPublickKey"></param>
    /// <param name="merchantPrivateKey"></param>
    /// <returns></returns>
    public static bool checkDecryptAndSign(string data, string encrypt_key,string yibaoPublickKey, string merchantPrivateKey)
    {
        /** 1.使用YBprivatekey解开aesEncrypt。 */
        string AESKey = "";
        try
        {
            AESKey = RSAFromPkcs8.decryptData(encrypt_key, merchantPrivateKey, "UTF-8");
        }
        catch (Exception e)
        {
            /** AES密钥解密失败 */
            return false;
        }

        /** 2.用aeskey解开data。取得data明文 */
        string realData = AES.Decrypt(data, AESKey);


        SortedDictionary<string, object> sd = Newtonsoft.Json.JsonConvert.DeserializeObject<SortedDictionary<string, object>>(realData);


        /** 3.取得data明文sign。 */
        string sign = (string)sd["sign"];

        /** 4.对map中的值进行验证 */
        StringBuilder signData = new StringBuilder();
        foreach (var item in sd)
        {
            /** 把sign参数隔过去 */
            if (item.Key == "sign")
            {
                continue;
            }
            signData.Append(item.Value);
        }

        signData = signData.Replace("\r", "");
        signData = signData.Replace("\n", "");
        signData = signData.Replace("    ", "");
        signData = signData.Replace("  ", "");
        signData = signData.Replace("\": \"", "\":\"");
        signData = signData.Replace("\": ", "\":");

        /**5. result为true时表明验签通过 */
        bool result = RSAFromPkcs8.checkSign(Convert.ToString(signData), sign, yibaoPublickKey, "UTF-8");

        return result;
    }

    /// <summary>
    /// 一键支付回调信息解密
    /// </summary>
    /// <param name="data">返回的data</param>
    /// <param name="encryptkey">返回的encryptkey</param>
    /// <param name="yibaoPublickey">易宝公钥</param>
    /// <param name="merchantPrivatekey">商户私钥</param>
    /// <param name="type">编码格式</param>
    /// <returns></returns>
    public static string checkYbCallbackResult(string data, string encryptkey,string yibaoPublickey,string merchantPrivatekey,string type)
    {
        string yb_encryptkey = encryptkey;
        string yb_data = data;
        //将易宝返回的结果进行验签名
        bool passSign = EncryptUtil.checkDecryptAndSign(yb_data, yb_encryptkey, yibaoPublickey, merchantPrivatekey);
        if (passSign)
        {
            string yb_aeskey = RSAFromPkcs8.decryptData(yb_encryptkey, merchantPrivatekey, type);

            string payresult_view = AES.Decrypt(yb_data, yb_aeskey);
            //返回易宝支付回调的业务数据明文
            return payresult_view;
        }
        else
        {
            return "验签未通过";
        }

    }

}

