namespace HireHub.Core.Utils.Common;

public static class LogMessage
{
    public const string StartMethod = "Start - {Method}";
    public const string EndMethod = "End - {Method}";
    public const string EndMethodException = "End - {Method} - {Exception}";
    public const string UserNotFoundOnLogin = "Login failed: user not found: {Username}";
    public const string InvalidPassword = "Login failed: invalid password for {EmpId}";
    //public const string UserNotFoundOnReset = "Reset failed: user not found: {Email}";
    //public const string ResetTokenNotGeneratedOrInvalid = "Reset failed: Reset token not generated or invalid";
    //public const string ResetTokenExpired = "Reset failed: Reset token expired";
    //public const string ResetTokenLog = "Generated Reset token {Token}";
    //public const string UserNotFound = "User not found";
}

public static class ExceptionMessage
{
    public const string JwtNotConfigured = "JWT settings not configured";
    public const string ConnectionStringNotConfigured = "ConnectionString not configured";
    //public const string EmailNotConfigured = "Email config not configured";
    public const string AzureLogicAppNotConfigured = "Azure Logic App not configured";
    public const string UserIdNotSetOnMiddleware = "Middlerware failed: User Id not set";
    public const string UserRoleNotSetOnMiddleware = "Middlerware failed: User Role not set";
}

public static class ResponseMessage
{
    //public const string InvalidCredentials = "Invalid credentials";
    //public const string LoginCredentialsRequired = "Username and password are required";
    //public const string EmailRequired = "Email is required";
    //public const string EmailNotRegistered = "Email not registered";
    //public const string OtpSendToEmail = "Otp send to the email:{0}";
    //public const string PasswordRequired = "Password is Required";
    //public const string PasswordResetSuccess = "Password reset successfully";
    //public const string ResetTokenSent = "Token for reset password sent to {0}";
    //public const string ResetTokenRequired = "Reset token is required";
    //public const string InvalidUser = "Invalid user";
    //public const string InvalidResetToken = "Invalid reset token";
    public const string UserNotFound = "User not found";
    public const string PasswordSetRequire = "Password require to be set";
    public const string PasswordReSetRequire = "Password require to be re-set";
    public const string NoSlotsProvided = "No slots provided";
    public const string InvalidSlots = "Some slots are invalid";
    public const string SlotsAlreadySet = "Some slots are already registered";
}

public static class FieldName
{
    //public const string Password = "Password";
    //public const string Username = "Username";
    //public const string Email = "Email";
    //public const string Role = "Role";
}

public static class PropertyName
{
    public const string Exception = "Exception";
    public const string Main = "Main";
}

public static class AppSettingKey
{
    public const string DefaultConnection = "DefaultConnection";
    public const string JwtSettings = "JwtSettings";
    //public const string PasswordSettings = "PasswordSettings";
    //public const string DarkwolfEmailConfig = "DarkwolfEmailConfig";
    public const string AzureLogicApp = "AzureLogicApp";
}

public static class EmailAddress
{
    //public const string DisplayName = "Darkwolf Org";
    //public const string NoReplyDarkwolf = "noreply@darkwolf.com";
}

public static class Key
{
    public const string UserId = "UserId";
    public const string Role = "Role";
}

public static class Role
{
    public const string Mentor = "Mentor";
    public const string Hr = "HR";
    public const string Panel = "Panel";
    public const string Admin = "Admin";
}