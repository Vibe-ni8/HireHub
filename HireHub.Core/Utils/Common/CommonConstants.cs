namespace HireHub.Core.Utils.Common;

public static class LogMessage
{
    public const string StartMethod = "Start - {Method}";
    public const string EndMethod = "End - {Method}";
    public const string EndMethodException = "End - {Method} - {Exception}";
    public const string UserNotFoundOnLogin = "Login failed: user not found: {Username}";
    public const string InvalidPassword = "Login failed: invalid password for {EmpId}";
}

public static class ExceptionMessage
{
    public const string JwtNotConfigured = "JWT settings not configured";
    public const string ConnectionStringNotConfigured = "ConnectionString not configured";
    public const string AzureLogicAppNotConfigured = "Azure Logic App not configured";
    public const string UserIdNotSetOnMiddleware = "Middlerware failed: User Id not set";
    public const string UserRoleNotSetOnMiddleware = "Middlerware failed: User Role not set";
}

public static class ResponseMessage
{
    public const string UserNotFound = "User not found";
    public const string PasswordSetRequire = "Password require to be set";
    public const string PasswordReSetRequire = "Password require to be re-set";
    public const string NoSlotsProvided = "No slots provided";
    public const string InvalidSlots = "Some slots are invalid";
    public const string PastSlotsInvalid = "Past slots can not be set";
    public const string SlotsAlreadySet = "Some slots are already registered";
    public const string InvalidCandidate = "Invalid Candidate";
    public const string InvalidPanel = "Invalid Panel";
    public const string CandidateNotAssignedToPanel = "Candidate not assigned to this Panel";
    public const string CanNotSetFeedbackForPastCandidate = "Can not set Feedback for Past Interviewed Candidate";
    public const string UserDifferFromLoggedUser = "Provided user is differ from logged in user";
    public const string CanNotChangeOrSetAttendanceForPastCandidate = "Can not change or set Attendance for Past Interviewed Candidate";
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

public static class Options
{
    public static string[] Recommendations => ["Strong Hire", "Hire", "May Be", "No Hire"];
}