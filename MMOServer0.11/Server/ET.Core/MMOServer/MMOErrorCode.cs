namespace ETModel
{
    public static partial class MMOErrorCode
    {
        public const int ERR_SignError = 10000;
        public const int ERR_Disconnect = 210000;
        public const int ERR_LoginError = 210005;

        //自定义错误
        public const int ERR_AccountAlreadyRegisted = 300001;
        public const int ERR_UserNotOnline = 300003;
        public const int ERR_CreateNewCharacter = 300007;
        public const int ERR_CannotCreateMoreCharacter = 300008;
    }
}