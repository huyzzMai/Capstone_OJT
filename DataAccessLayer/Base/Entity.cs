using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Base
{
    public abstract class Entity
    {
        public Guid Id { get; private set; }
        public DateTimeOffset CreatedDate { get; private set; }
        public Guid CreateBy { get; protected set; }
        public DateTimeOffset UpdatedDate { get; protected set; }
        public Guid UpdateBy { get; protected set; }
        public bool IsDeleted { get; protected set; }

        protected Entity()
        {
            Id = Guid.NewGuid();
            CreatedDate = UpdatedDate = DateTimeOffset.Now;
            IsDeleted = false;
        }
    }
}
