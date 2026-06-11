<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm.aspx.cs" Inherits="WebApplication.WebForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
           <asp:Label id="label_welcome" runat="server" Text="Welcome" />
            <br />
            

            <br />
                <asp:Label id="labelRoles" runat="server" Text="Roles: " />
                <asp:GridView ID="GridViewRoles" runat="server"></asp:GridView>

            <br />
                <asp:Label id="labelUsers" runat="server" Text="Users: " />
                <asp:GridView ID="GridViewUsers" runat="server"></asp:GridView>
        </div>
    </form>
</body>
</html>
