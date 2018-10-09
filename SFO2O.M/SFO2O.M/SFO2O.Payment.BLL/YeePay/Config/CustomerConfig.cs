/// <summary>
/// 商户信息配置文件
/// </summary>
public class CustomerConfig
{
    /// <summary>
    /// 商户编号   
    /// 商户密钥       
    /// 文件存储地址
    /// </summary>
    static CustomerConfig()
    {
        //商户编号
        //merchantAccount = "10000418926";
        merchantAccount = "10014981503";
        //商户私钥
        //merchantPrivateKey = "MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBALD0Tou2w7EHbP3q5wi5PG5xrvC0CBawXxSI1PlZAGo2iFYhaBK6SsB5UiYT64fSR3YemQGS2vSqQii5vYdOfrffvvDprrr7Vo7BziS6sJQ9B0/DzwN2zY7jJBCz55CLMBsZCtuqDNVxTcsOcZnrgSSMqnhk+usuR4hPoV9qABeHAgMBAAECgYAfnth2UOdxN/F7AkHcpjUtSzVGn/UeENA8vCLKl+PiFvKP6ZJOXmnDMSrD0SVydNn+OoN+634i4FXIL0C18Anmh4IlQM9hj+rFTg1bMSUHvSPKoZpoEfjR0R+3TQF8PycBbaIWgLV/5NA8dMld0DvF5d8bbqpgH6FzEXZPvF8OgQJBANwHRhCu+o/JoCoH0coVhNFuobVYZU0pQRlfDaE4ph0+daiJ4HlT630JrBFb728Ga7E81dsfGMSi1N6QSipJMEECQQDN4kb+O/ecDNQrEsjA0LqDXkaKsRP6iU/HVNyr4Z/7ojHws0F5Vypj1euCII+V6U7StMKRbSaB1GI8Bs34llXHAkEAnIc0KiRBLk+S+LOtZGVgoplgwyEKmBUUMdd0W9BwJHfNvkOwBMBV1BMwbP0JXeOkc2dDAGqj9Sed5mOhz2lXwQJAVeA0TIcm2Ohg9zZ2ljZ6FaGVOvRxqObtZ+91vBv4ZzVYL1YV0U8SV2I7QaPjQFx4jFrpbU9h6HV2JCOSdkX+sQJBAJ+PfNA0b25HuY9n4cTk/hLc2TCWVDsPnONuhNpuRpXqxu9L0p2aHX5JLf1kTUoYxqmlEjx6IYcObcB9Snw0Tf0=";
        merchantPrivateKey = "MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBAIQntnKmEYsyiSo4AD1pyD/tDTQjmLufY5zCMq3Gn1pSW1iQaFwy7n/x6nOzNkPtghuHxO0jS2URAenNJgGoOGO7WBgwDQBr4rDo3CwN8dqe9/UBtHV/XGhPGMsGb3COugsUwJS8IM3nIi/GmAdVYw96l5JFccX6drfuLRzEdlj3AgMBAAECgYB6SMz39VbSeKaXXE5MhW94R38qOa4AHpJr4P5H8jxNtVs07hrDBZXO6ijJepDB9imoE89SppADKuOcVenPKrtRqDmmVsIerv2BjYMFPygUKtjClHHPGX+8FVk9uMUj6GhgyO8zUrk87+0824P3cnxcYCznqizmVYgPIqdITm0vAQJBAMUUQMOC30XSx9Fgi77+5dhHVASTOaQqy5OnoeaIEZEmmYSn4RfQQLAjYObXiJSlBvYM+ihurGLAD1HjnmijVjcCQQCrqmeT8xQIMs/CKf8DQIQvVkHqZKctk7aQLK72gRB/eF7OMdxasqTMtKzhHcCbOPOAOLtKbLHAHJAQTxZVc7NBAkEAiMF+E21sS8JxQBxzvKyaiBMu/SHAnOfJboOjeBoxvnx/iSsJqoGrcc6K/oTP1P4TL4hfytDJtJi7yMJfeRLmzwJAG9q4l88XbwUfpPe/gz2StUOfynKulbykINy/PwxOKwDTEU4R3T2jc/vVGWoEeKtTB3ktGrRsWynHTn4mt5LggQJAStiEDwpTI+eYCmZChYzh0M8p4fLCzpY1iGAOvsu7rCo4P+K0Fqw9NpODnssp9ieWBoDUZGAtbpYERwxtLmEJ3A==";

        ////商户公钥
        //merchantPublickey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCw9E6LtsOxB2z96ucIuTxuca7wtAgWsF8UiNT5WQBqNohWIWgSukrAeVImE+uH0kd2HpkBktr0qkIoub2HTn63377w6a66+1aOwc4kurCUPQdPw88Dds2O4yQQs+eQizAbGQrbqgzVcU3LDnGZ64EkjKp4ZPrrLkeIT6FfagAXhwIDAQAB";
       merchantPublickey="MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCEJ7ZyphGLMokqOAA9acg/7Q00I5i7n2OcwjKtxp9aUltYkGhcMu5/8epzszZD7YIbh8TtI0tlEQHpzSYBqDhju1gYMA0Aa+Kw6NwsDfHanvf1AbR1f1xoTxjLBm9wjroLFMCUvCDN5yIvxpgHVWMPepeSRXHF+na37i0cxHZY9wIDAQAB";
        ////易宝公钥
        //yeepayPublicKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCKcSa7wS6OMUL4oTzJLCBsE5KTkPz9OTSiOU6356BsR6gzQ9kf/xa+Wi1ZANTeNuTYFyhlCI7ZCLW7QNzwAYSFStKzP3UlUzsfrV7zge8gTgJSwC/avsZPCWMDrniC3HiZ70l1mMBK5pL0H6NbBFJ6XgDIw160aO9AxFZa5pfCcwIDAQAB";
       yeepayPublicKey = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCDuOtGy2YZCEkxYGk3R1Lm3qB4u0t5+lx4ji/qHMyKPKDFqxI53X/umN1etSnGJuqfS0JhZv4/29xOIeS7D1q5SZ7835/WOEK8+J9BX5Vtp90cSeHyXrZojNsyYEdBnnPeoNfuo7wwebnm1FkN5/opAombyS+Pa+g4wZzdRbtSxQIDAQAB";
    }
    /// <summary>
    /// 商户编号
    /// </summary>
    public static string merchantAccount { get; private set; }
    /// <summary>
    /// 商户私钥
    /// </summary>
    public static string merchantPrivateKey { get; private set; }
    /// <summary>
    /// 商户公钥
    /// </summary>
    public static string merchantPublickey { get; private set; }
    /// <summary>
    /// 易宝公钥
    /// </summary>
    public static string yeepayPublicKey { get; private set; }
}