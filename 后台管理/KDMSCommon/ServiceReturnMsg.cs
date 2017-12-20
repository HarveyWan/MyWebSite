using System;

namespace KDMSCommon
{
    [Serializable]
    public class ServiceReturnMsg
    {
        private int _returnCode = 1; // 0:失败 1:成功 默认为1
        private string _returnMsg = "成功";//默认为"成功"
        private object _returnData = null;
        private int _returnTotalRecords = 0;
        private string _token = string.Empty;


        public int ReturnCode 
        {
            get {return _returnCode;}
            set {_returnCode=value;}
        }
        public string ReturnMsg
        {
            get {return _returnMsg;}
            set {_returnMsg=value;}
        }
        
        public object ReturnData
        {
            get { return _returnData; }
            set { _returnData = value; }
        }
        public int ReturnTotalRecords
        {
            get { return _returnTotalRecords; }
            set { _returnTotalRecords = value; }
        }
    }
}
