<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>

    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Login.css" rel="stylesheet" />

</head>
<body class="d-flex justify-content-center align-items-center vh-100 bg-light">
    <form id="form1" runat="server">
        <div class="card shadow p-4 border-0" style="width: 400px";>
            
            <h3 class="text-center mb-4">Staff Login</h3>
           
            
            <div class="mb-3 textBox">
                <asp:Label ID="Label_email" runat="server" Text="Email"></asp:Label>
                <asp:TextBox ID="TextBox_email" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
             
            <div class="mb-3 textBox">
                <asp:Label ID="Label_password" runat="server" Text="Password"></asp:Label>
                <asp:TextBox ID="TextBox_password" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
            </div>
           
            <asp:Label ID="label_title_login" runat="server" Text="" CssClass="wrong_credentials"></asp:Label>
            <br />
            <br />
            <asp:Button ID="login_btn" runat="server" Text="Login" onClick="check_credentials" CssClass="btn btn-primary w-100"/>

           
        </div>
    </form>
</body>
</html>
