using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFO2O.EntLib.DataExtensions;
using SFO2O.Model.Common;
using SFO2O.Utility.Uitl;
using System.Data;

namespace SFO2O.DAL.Message
{
    public class MessageDal:BaseDal
    {
        /// <summary>
        /// 增加一条记录
        /// </summary>
        public int Insert(MessageInfo messageInfo)
        {
            const string INSERT_SQL = @"

            insert into Message(Title,Content,SendWay,SendType,MessageType,MessageGroup,Timing,MesasgeTypeContent,CreateUserType,Status,CreateTime,CreateBy,Sender,ContentType,MsgType,ObjectId)

            values(@Title,@Content,@SendWay,@SendType,@MessageType,@MessageGroup,@Timing,@MesasgeTypeContent,@CreateUserType,@Status,@CreateTime,@CreateBy,@Sender,@ContentType,@MsgType,@ObjectId)

            select @@IDENTITY";

            var dbParameters = DbSFO2ORead.CreateParameterCollection();


            dbParameters.Append("@Title", messageInfo.Title);

            dbParameters.Append("@Content", messageInfo.Content);

            dbParameters.Append("@SendWay", messageInfo.SendWay);

            dbParameters.Append("@SendType", messageInfo.SendType);

            dbParameters.Append("@MessageType", messageInfo.MessageType);

            dbParameters.Append("@MessageGroup", messageInfo.MessageGroup);

            dbParameters.Append("@Timing", messageInfo.Timing);

            dbParameters.Append("@MesasgeTypeContent", messageInfo.MesasgeTypeContent);

            dbParameters.Append("@CreateUserType", messageInfo.CreateUserType);

            dbParameters.Append("@Status", messageInfo.Status);

            dbParameters.Append("@CreateTime", messageInfo.CreateTime);

            dbParameters.Append("@CreateBy", messageInfo.CreateBy);
            dbParameters.Append("@Sender", messageInfo.Sender);
            dbParameters.Append("@ContentType", messageInfo.ContentType);
            dbParameters.Append("@MsgType", messageInfo.PushType);
            dbParameters.Append("@ObjectId", messageInfo.ObjectId);

            object result = DbSFO2OMain.ExecuteScalar(CommandType.Text,INSERT_SQL, dbParameters);
            return result == null ? 0 : int.Parse(result.ToString());
        }
        /// <summary>
        /// 更新消息状态
        /// </summary>
        public bool UpdateMessageStatus(int messageID, int status)
        {
            return UpdateMessageStatus(new MessageInfo
            {
                ID = messageID,
                Status = status
            });
        }
        public bool UpdateMessageStatus(MessageInfo messageInfo)
        {
            if (messageInfo == null)
                return false;

            string UPDATE_SQL = @" update  Message set status = @status where  ID=@ID ";
            var  dbParameters = DbSFO2OMain.CreateParameterCollection();
            dbParameters.Append("@ID", messageInfo.ID);
            dbParameters.Append("@status", messageInfo.Status);

            if (!string.IsNullOrEmpty(messageInfo.UpdateBy))
            {
                UPDATE_SQL = @" update  Message set status = @status 
                                ,UpdateBy=@UpdateBy,UpdateTime=@UpdateTime 
                                where  ID=@ID ";
                dbParameters.Append("@UpdateBy", messageInfo.UpdateBy);
                dbParameters.Append("@UpdateTime", messageInfo.UpdateTime);
            }

            int effectCount = 0;
            try
            {
                effectCount = DbSFO2OMain.ExecuteNonQuery(CommandType.Text, UPDATE_SQL, dbParameters);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            return effectCount > 0;
        }
    }
}
