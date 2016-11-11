using System.Collections.Generic;

namespace projektseminar_test
{
    class BusinessObject
    {
        public BusinessObject()
        {
            Tags = new List<Tag>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
        public List<Tag> Tags { get; set; }

        public void AddTag(Tag tag)
        {
            Tags.Add(tag);
        }
    }
}
