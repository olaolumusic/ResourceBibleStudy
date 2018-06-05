namespace ResourceBibleStudy.Models
{
    public class UserDetail
    {
        public string ConnectionId { get; set; }
        public string UserName { get; set; }

        public string UserFullName
        {
            get { return UserName; }
        }
        public string UserImageUrl { get; set; }
    }
}