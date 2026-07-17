namespace HelpDesk.Services.Constants;

public static class Messages
{
    public static class Auth
    {
        // Login
        public const string InvalidCredentials = "Invalid email or password";
        public const string AccountInactive = "Your account is inactive.";
        public const string AccountDeleted = "User account has been deleted.";
        public const string LoginSuccess = "Login successful";
        public const string UserNotExists = "A user is not exists.";

        // SSO
        public const string SSOEmailRetrievalFailed = "Unable to retrieve email from Azure AD token.";
        public const string InternalUsersOnly = "This sign-in method is available only for internal users.";

        // Registration
        public const string EmailAlreadyExists = "A user with this email already exists.";
        public const string RegisterSuccess = "User registered successfully.";
    }

    public static class Validation
    {
        // First Name
        public const string NameRequired = "Name is required.";
        public const string NameLength = "Name must be between 2 and 100 characters.";

        // Email
        public const string EmailRequired = "Email is required.";
        public const string EmailInvalid = "Please enter a valid email address.";
        public const string EmailLength = "Email cannot exceed 255 characters.";

        // Password
        public const string PasswordRequired = "Password is required.";
        public const string PasswordLength = "Password must be at least 8 characters long.";
        public const string PasswordComplexity = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.";

        // Role
        public const string RoleRequired = "Role is required.";
    }

    public static class User
    {
        public const string CreateSuccess = "User created successfully.";
        public const string UpdateSuccess = "User updated successfully.";
        public const string DeleteSuccess = "User deleted successfully.";
        public const string StatusToggled = "User status updated successfully.";
        public const string PasswordRequired = "Password is required when creating a new user.";
        public const string InActiveUser = "User is not active.";
    }

    public static class Comment
    {
        public const string CommentRequired = "Comment is required.";
        public const string CommentLength = "Comment cannot exceed 2000 characters.";
        public const string AddSuccess = "Comment added successfully.";
    }

    public static class Ticket
    {   public const string TitleRequired = "Title is required.";
        public const string TitleLength = "Title must be between 3 and 250 characters.";
        public const string DescriptionRequired = "Description is required.";
        public const string PriorityRequired = "Priority is required.";
        public const string CategoryRequired = "Category is required.";
        public const string SubCategoryRequired = "Sub-category is required.";
        public const string CreateSuccess = "Ticket created successfully.";
        public const string UpdateSuccess = "Ticket updated successfully.";
        public const string DeleteSuccess = "Ticket deleted successfully.";
        public const string TicketClosed = "This ticket is closed.";
        public const string AssignedSuccess = "Ticket assigned successfully.";
        public const string StatusUpdateSuccess = "Ticket status updated successfully.";

    }

    public static class Profile
    {
        public const string UpdateSuccess = "Profile updated successfully.";
        public const string PasswordChangeSuccess = "Password changed successfully.";
        public const string PasswordMismatch = "New password and confirm password do not match.";
        public const string CurrentPasswordInvalid = "Current password is incorrect.";
    }

    public static class General
    {
        public const string Success = "Success";
        public const string NotFound = "Not Found";
        public const string NotAllowed = "You are not allowed to access this.";
    }
}
