using System.Text;
using FSH.WebApi.Application.Common.Mailing;
using Microsoft.Extensions.Localization;

namespace FSH.WebApi.Infrastructure.Mailing;

public class EmailTemplateService : IEmailTemplateService
{
    private readonly IStringLocalizer<EmailTemplateService> _localizer;

    public EmailTemplateService(IStringLocalizer<EmailTemplateService> localizer)
    {
        _localizer = localizer;
    }

    public string GenerateEmailConfirmationMail(string userName, string email, string emailVerificationUri)
    {
        string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string tmplFolder = Path.Combine(baseDirectory, "Email Templates");
        string filePath = Path.Combine(tmplFolder, "email-confirmation.html");

        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var sr = new StreamReader(fs, Encoding.Default);
        string mailText = sr.ReadToEnd();
        sr.Close();

        if (string.IsNullOrEmpty(mailText))
        {
            return string.Format(_localizer["Please confirm your account by <a href='{0}'>clicking here</a>."], emailVerificationUri);
        }

        return mailText.Replace("[userName]", userName).Replace("[email]", email).Replace("[emailVerificationUri]", emailVerificationUri);
    }
}