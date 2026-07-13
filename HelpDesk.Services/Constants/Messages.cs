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
        public const string FirstNameRequired = "First name is required.";
        public const string FirstNameLength = "First name must be between 2 and 100 characters.";

        // Last Name
        public const string LastNameRequired = "Last name is required.";
        public const string LastNameLength = "Last name must be between 2 and 100 characters.";

        // Email
        public const string EmailRequired = "Email is required.";
        public const string EmailInvalid = "Please enter a valid email address.";
        public const string EmailLength = "Email cannot exceed 255 characters.";

        // Password
        public const string PasswordRequired = "Password is required.";
        public const string PasswordLength = "Password must be at least 8 characters long.";
        public const string PasswordComplexity = "Password must contain at least one uppercase letter, one lowercase letter, one number, and one special character.";
    }

    public static class General
    {
        public const string Success = "Success";
    }
}
