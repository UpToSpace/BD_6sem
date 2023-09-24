using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.SqlServer.Server;

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.UserDefined, ValidationMethodName = "ValidatePassport", MaxByteSize = 8000)]
public struct SqlUserDefinedType1: INullable, IBinarySerialize
{
    string passport;

    public override string ToString()
    {
        return $"{Passport}";
    }
    
    public bool IsNull
    {
        get
        {
            return _null;
        }
    }
    
    public static SqlUserDefinedType1 Null
    {
        get
        {
            SqlUserDefinedType1 h = new SqlUserDefinedType1();
            h._null = true;
            return h;
        }
    }

    public string Passport
    {
        get => passport;
        set
        {
            string temp = passport;
            passport = value;
            if (!ValidatePassport())
            {
                passport = temp;
                throw new ArgumentException("Введите правильный паспорт");
            }
        }
    }

    public static SqlUserDefinedType1 Parse(SqlString s)
    {
        if (s.IsNull)
            return Null;
        SqlUserDefinedType1 u = new SqlUserDefinedType1
        {
            Passport = s.Value
        };
        return u;
    }
    
    // Это метод-заполнитель
    public string Method1()
    {
        return string.Empty;
    }
    
    // Это статический метод заполнителя
    public static SqlString Method2()
    {
        return new SqlString("");
    }

    private bool ValidatePassport()
    {
        string region = passport.Substring(0, 2).ToUpper();
        string number = passport.Substring(2, passport.Length - 2);
        string[] regions = { "АВ", "ВМ", "НВ", "КН", "МР", "МС", "КВ" }; // на русском
        bool regionFlag = false;
        for (int i = 0; i < regions.Length; i++)
        {
            if (regions[i] == region)
            {
                regionFlag = true;
                break;
            }
        } 
        if (regionFlag && Regex.IsMatch(number, "^[0-9]{6}$"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Read(BinaryReader r)
    {
        Passport = r.ReadString();
    }

    public void Write(BinaryWriter w)
    {
        w.Write($"{Passport}");
    }

    // Это поле элемента-заполнителя
    public int _var1;
 
    // Закрытый член
    private bool _null;
}