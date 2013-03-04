using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeWeiOMS.NhibernateHelper
{
    /// <summary>
    /// 可以持久到数据库的业务类都要继承的基类
    /// </summary>
    public abstract class Entity : BaseObject
    {
        public Entity()
        {
            Id = Guid.NewGuid().ToString();
            CreateTime = DateTime.Now;
            IsDelete = false;
        }
        public virtual string Id { get; protected set; }

        public virtual DateTime CreateTime { get; protected set; }

        public virtual bool IsDelete { get; set; }

        public virtual Int32 Version { get; protected set; }

    }
}
