<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaffPanel.aspx.cs" Inherits="WebApplication.AdminPanel" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Staff Panel</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label id="label_title" runat="server" Text="Staff Panel" />
        </div>

        

        <div class="card shadow p-4 mb-4">
    <h3>Search Respondents</h3>

    <h5>Personal Details</h5>
    <div class="row">
        <div class="col-md-3">
            <asp:DropDownList ID="ddlTitle" runat="server" CssClass="form-select" />
        </div>
        <div class="col-md-3">
            <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" Placeholder="First name" />
        </div>
        <div class="col-md-3">
            <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" Placeholder="Last name" />
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select" />
        </div>
    </div>

    <h5 class="mt-4">Hospital Details</h5>
    <div class="row">
        <div class="col-md-4">
            <asp:Label ID="Label2" runat="server" Text="Service: "></asp:Label>
            <asp:DropDownList ID="ddlTypeOfService" runat="server" CssClass="form-select" />
        </div>
        <div class="col-md-4">
            <asp:Label ID="Label3" runat="server" Text="Room Type: "></asp:Label>
            <asp:DropDownList ID="ddlRoomType" runat="server" CssClass="form-select" />
        </div>
        <div class="col-md-4">
            <asp:Label ID="lblInsuranceList" runat="server" Text="Insurance Provider: "></asp:Label>
            <asp:DropDownList ID="ddlInsurance" runat="server" CssClass="form-select" />
        </div>
    </div>

    <div class="mt-4">
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" />
        <asp:Button ID="btnClear" runat="server" Text="Clear Filters" CssClass="btn btn-outline-secondary ms-2" />
    </div>
</div>

        <div class="mt-4">
    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    <asp:GridView ID="gvAtendents" runat="server" CssClass="table table-striped table-hover"></asp:GridView>
</div>

<asp:GridView ID="GridViewRespondents" runat="server"
    CssClass="table table-striped table-hover"
    >
</asp:GridView>
    </form>
</body>
</html>
