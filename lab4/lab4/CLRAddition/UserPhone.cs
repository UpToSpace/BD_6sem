using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.IO;
using Microsoft.SqlServer.Server;


[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.UserDefined, MaxByteSize = 8000)]
public struct UserPhone: INullable, IBinarySerialize
{
    private bool _null;
    String code;
    String number;
    String op;
    public override string ToString()
    {
        return $"{code} {number} {op}";
    }
    
    public bool IsNull
    {
        get
        {
            return _null;
        }
    }
    
    public static UserPhone Null
    {
        get
        {
            UserPhone h = new UserPhone();
            h._null = true;
            return h;
        }
    }
    
    public static UserPhone Parse(SqlString s)
    {
        string[] b = s.Value.Split(' ');
        if (s.IsNull)
            return Null;

        UserPhone u = new UserPhone
        {
            code = b[0],
            number = b[1],
            op = b[2]
        };
        return u;
    }
    
    public void Read(BinaryReader r)
    {
        op = r.ReadString();
    }
    
    public void Write(BinaryWriter w)
    {
        w.Write($"+375 ({code}) {number} ({op})");
    }

    public string Method1()
    {
        return string.Empty;
    }

    public static SqlString Method2()
    {
        return new SqlString("");
    }

    public string _var1;
}