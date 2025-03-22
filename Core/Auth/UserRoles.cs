namespace Bakalauras.Core.Auth;

public static class UserRoles
{
    public const string Admin = "Admin";
    public const string Mechanic = "Mechanic";
    public const string Customer = "Customer";

    public static readonly string[] All = { Admin, Mechanic, Customer };
} 