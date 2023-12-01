namespace BeautyAllure.Forms
{
    public static class UserContext
    {
        public static int UserId { get; private set; }
        public static string UserEmail { get; private set; }

        public static void SetCurrentUser(int userId, string userEmail)
        {
            UserId = userId;
            UserEmail = userEmail;
        }

        public static void Clear()
        {
            UserId = 0;
            UserEmail = null;
        }
    }
}
