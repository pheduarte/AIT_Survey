<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label id="label_title_login" runat="server" Text="Login" />
            <br />
            <br />
            <br />
            <asp:Label ID="Label_email" runat="server" Text="Email"></asp:Label>
            <asp:TextBox ID="TextBox_email" runat="server"></asp:TextBox>
             <br />
             <br />

            <asp:Label ID="Label_password" runat="server" Text="Password"></asp:Label>
            <asp:TextBox ID="TextBox_password" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="login_btn" runat="server" Text="Login" onClick="check_credentials"/>
            <br />
            <br />

           
        </div>
    </form>
</body>
</html>
