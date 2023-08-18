public static class CredentialsValidator
{
    private static int minimumEmailLength = 10;
    private static int minimumPasswordLength = 6;
    
    public static bool VerifyEmail(string _email)
    {
        if (string.IsNullOrEmpty(_email))
        {
            UIManager.Instance.OkDialog.Setup("Please enter email");
            return false;
        }

        if (_email.Length < minimumEmailLength)
        {
            UIManager.Instance.OkDialog.Setup($"Email must contain at least {minimumEmailLength} characters");
            return false;
        }

        if (!_email.Contains("@"))
        {
            UIManager.Instance.OkDialog.Setup("Please enter valid email");
            return false;
        }

        return true;
    }

    public static bool VerifyPassword(string _password)
    {
        if (string.IsNullOrEmpty(_password))
        {
            UIManager.Instance.OkDialog.Setup("Please enter password");
            return false;
        }
        if (_password.Length < minimumPasswordLength)
        {
            UIManager.Instance.OkDialog.Setup($"Password must contain atleast {minimumPasswordLength} characters");
            return false;
        }

        return true;
    }
}
