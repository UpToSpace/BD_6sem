using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Net.Mail;
using System.Net;
using System.IO;

public partial class StoredProcedures
{
    [SqlFunction]
    public static SqlBoolean MailFunction (SqlString objectType, SqlString objectName, SqlString loginName)
    {
        MailAddress from = new MailAddress("antonina.reshetilova@gmail.com", "Antonina");
        MailAddress to = new MailAddress("antonina.reshetilova@gmail.com");
        MailMessage message = new MailMessage(from, to);

        message.Subject = "Изменение прав!";
        message.Body = $"objectType: {objectType}, objectName: {objectName}, loginName: {loginName}";

        SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);

        smtp.Credentials = new NetworkCredential("antonina.reshetilova@gmail.com", "cmbmxhtgptcjobzq");
        smtp.EnableSsl = true;
        smtp.Send(message);

        return SqlBoolean.Null;
    }
}
