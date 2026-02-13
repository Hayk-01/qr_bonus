using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InnLine.BLL.Constants;
public static class ErrorConstants
{
    public const long DuplicateItem = 1;
    public const long CannotRemoveDataWithReference = 2; 
    public const long TheUsernameOrPasswordIsIncorrect = 3;
    public const long NotExist = 4;
    public const long AlreadyUsed = 5;
    public const long NotActive = 6;
    public const long TemporarilyUnavailable = 7;
    public const long NotValid = 8;
    public const long TryAgain = 9;
}
