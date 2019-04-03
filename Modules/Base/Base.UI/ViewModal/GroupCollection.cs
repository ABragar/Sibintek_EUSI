using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Base.UI.ViewModal
{
    public class GroupCollection : IEnumerable<Group>
    {
        public bool ShowFooter { get; set; }
        public bool Groupable { get; set; }
        public IList<Group> Groups { get; set; }
        public GroupCollection()
        {
            this.Groups = new List<Group>();
        }

        public Group this[string field]
        {
            get { return this.Groups.FirstOrDefault(x => x.Field == field); }
        }

        public IEnumerator<Group> GetEnumerator()
        {
            return this.Groups.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public GroupCollection Copy()
        {
            return new GroupCollection()
            {
                Groupable = this.Groupable,
                Groups = this.Groups.Select(x => x.Copy()).ToList()
            };
        }
    }
}