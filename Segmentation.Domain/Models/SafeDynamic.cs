using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace Segmentation.Domain.Models
{
    public class SafeDynamic : DynamicObject
    {
        public SafeDynamic(Dictionary<string, object> data)
        {
            _members = data;
        }

        private readonly Dictionary<string, object> _members = new();

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (_members.TryGetValue(binder.Name, out result))
                return true;

            result = null; // fallback instead of exception
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _members[binder.Name] = value;
            return true;
        }
    }
}
