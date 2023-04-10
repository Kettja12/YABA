using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace ServicesShared;
public static class Messages
{
    public const string Fail = "Fail";
    public const string InvalidParameters = "Invalid parameters.";
    public const string NoData = "No data.";
    public const string NoPermissionToInsert = "No permission to insert.";
    public const string NoPermissionToModify = "No permission to modify.";
    public const string OK = "OK";
    public const string Operationfailed = "Operation failed.";
    public const string SaveSuccess = "Save Success";
    public const string Success = "Success";

    //Master
    public const string LoginExpired = "Login expired.";
    public const string InvalidOldPassword = "Invalid old password.";
    public const string PasswordSaveSuccess = "Password save success.";
    public const string PasswordSaveFail = "Password save fail.";

    //Account
    public const string LoginToUserDbFailed = "login to user db failed.";
    public const string UserNotFoundDB = "User not found.";
    public const string UsernameEmpty = "Username empty.";
    public const string UsernameAlreadyInUse = "Username already in use.";
    public const string UserSaveFailed = "User save failed.";
    public const string UserSaveSuccess = "User save success.";
    public const string NoUserToSave = "No user to save.";
 


}
