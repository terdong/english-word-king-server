using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace EWK_Server.TeamGehem.DataModels
{
    public class LogEntity : ICloneable
    {
        public ObjectId Id { get; set; }
        public string tag { get; set; }
        public string log { get; set; }

        #region ICloneable 멤버
        public object Clone()
        {
            LogEntity clone = new LogEntity();
            clone.tag = this.tag;
            return clone;
            //throw new NotImplementedException();
        }
        #endregion
    }
}
