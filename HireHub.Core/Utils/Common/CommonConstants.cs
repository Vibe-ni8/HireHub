namespace HireHub.Core.Utils.Common;

public static class LogMessage
{
    public const string StartMethod = "Start - {Method}";
    public const string EndMethod = "End - {Method}";
    public const string EndMethodException = "End - {Method} - {Exception}";
    public const string UserNotFoundOnLogin = "Login failed: user not found: {Username}";
    public const string InvalidPassword = "Login failed: invalid password for {EmpId}";
    public const string NotActiveUser = "Login failed: {EmpId} is not a Active User";
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
    public const string NotActiveUser = "You are not a Active User";
    public const string PasswordSetRequire = "Password require to be set";
    public const string PasswordReSetRequire = "Password require to be re-set";
    public const string InvalidExperienceLevel = "Provided Candidate Experience Level is Invalid";
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

public static class RoleName
{
    public const string Mentor = "Mentor";
    public const string Hr = "HR";
    public const string Panel = "Panel";
    public const string Admin = "Admin";
}

public static class UserAction
{
    public const string Drive = "Drive";
}

public static class ActionType
{
    public const string View = "View";
    public const string Add = "Add";
    public const string Update = "Update";
    public const string Delete = "Delete";
}