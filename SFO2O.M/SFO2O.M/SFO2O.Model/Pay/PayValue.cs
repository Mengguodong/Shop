using System;
using System.Collections.Generic;
using System.Text;

namespace SFO2O.Model.Pay
{
    [Serializable]
    public class PayValue
    {
        private bool mHasError;
        private int mReturnCode;
        private string mMessage;
        private string mReturnString;
        private object mReturnObject;

        /// <summary>
        /// 
        /// </summary>
        public bool HasError
        {
            get
            {
                return this.mHasError;
            }
            set
            {
                this.mHasError = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ReturnCode
        {
            get
            {
                return this.mReturnCode;
            }
            set
            {
                this.mReturnCode = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Message
        {
            get
            {
                return this.mMessage;
            }
            set
            {
                this.mMessage = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public object ReturnObject
        {
            get
            {
                return this.mReturnObject;
            }
            set
            {
                this.mReturnObject = value;
            }
        }

        public string ReturnString
        {
            get
            {
                return this.mReturnString;
            }
            set
            {
                this.mReturnString = value;
            }
        }
    }
}
