<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm.aspx.cs" Inherits="WebApplication.WebForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Admin Panel</title>

    <link href="Content/bootstrap.min.css" rel="stylesheet" />

    <style>
        body {
            background: #eef4f8;
            color: #243746;
        }

        .page-title {
            color: #1261a0;
            font-size: 2rem;
            font-weight: 700;
            letter-spacing: -0.02em;
        }

        .staff-card {
            position: relative;
            border-radius: 1rem;
        }


        .section-heading {
            color: #1261a0;
            font-weight: 700;
        }

        .filter-card .row > div > span {
            display: inline-block;
            margin-bottom: 0.45rem;
            color: #435766;
            font-size: 0.9rem;
            font-weight: 600;
        }

        .form-control,
        .form-select {
            min-height: 2.75rem;
            border-color: #b8c7d1;
            border-radius: 0.65rem;
        }

        .form-control:focus,
        .form-select:focus {
            border-color: #1687a7;
            box-shadow: 0 0 0 0.25rem rgba(22, 135, 167, 0.16);
        }

        .btn {
            border-radius: 0.6rem;
            font-weight: 600;
        }

        .btn-primary {
            border-color: #1261a0;
            background-color: #1261a0;
        }

        .btn-primary:hover,
        .btn-primary:focus {
            border-color: #0f5288;
            background-color: #0f5288;
        }

        .results-card {
            overflow: hidden;
            border-radius: 1rem;
        }

        .results-card > span {
            display: block;
            padding: 1rem 1.25rem;
            border-bottom: 1px solid #dce6ec;
            color: #435766;
            font-weight: 600;
        }

        .results-card .table {
            margin-bottom: 0;
            vertical-align: middle;
        }

        .results-card .table th {
            padding: 0.9rem 1rem;
            border-bottom-width: 1px;
            background: #1261a0;
            color: #ffffff;
            font-weight: 600;
            white-space: nowrap;
        }

        .results-card .table td {
            padding: 0.85rem 1rem;
            border-color: #e3ebf0;
        }

        .results-card .table-striped > tbody > tr:nth-of-type(odd) > * {
            --bs-table-accent-bg: #f5f9fb;
        }

        @media (max-width: 767.98px) {
            .page-title {
                font-size: 1.75rem;
            }
        }
    </style>


</head>
<body class="min-vh-100">
    <form id="form1" runat="server" class="container-xl py-4 py-md-5">
        <div class="mb-4">
            <asp:Label id="label_title" runat="server" Text="Staff Panel" CssClass="page-title" />
        </div>

        

        <div class="card staff-card filter-card border-0 shadow-lg p-4 p-md-5 mb-4 overflow-hidden">
            <h3 class="section-heading mb-4">Search Respondents</h3>

            <h5 class="fw-bold mb-3">Personal Details</h5>

        <div class="row g-3">
        
        <div class="col-md-3">
            <asp:Label ID="Label4" runat="server" Text="First name"></asp:Label>
            <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" Placeholder="First name" />
        </div>
        <div class="col-md-3">
            <asp:Label ID="Label5" runat="server" Text="Last name"></asp:Label>
            <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" Placeholder="Last name" />
        </div>
        <div class="col-md-3">
            <asp:Label ID="Label6" runat="server" Text="Email"></asp:Label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" Placeholder="Email" />
        </div>
        <div class="col-md-3">
            <asp:Label ID="Label7" runat="server" Text="Gender"></asp:Label>
            <asp:DropDownList ID="ddlGender" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                <asp:ListItem Text="Any" Value="" />
            </asp:DropDownList>
        </div>
    </div>

    <h5 class="fw-bold mt-4 mb-3">Hospital Details</h5>
    <div class="row g-3">
        
        <div class="col-md-4">
            <asp:Label ID="Label3" runat="server" Text="Room Type: "></asp:Label>
            <asp:DropDownList ID="ddlRoomType" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                <asp:ListItem Text="Any" Value="" />
            </asp:DropDownList>
        </div>
        <div class="col-md-4">
            <asp:Label ID="Label2" runat="server" Text="Rehab/Surgery: "></asp:Label>
            <asp:DropDownList ID="ddlSurgery" runat="server" CssClass="form-select" AppendDataBoundItems="true"> 
                <asp:ListItem Text="Any" Value="" />
            </asp:DropDownList>
        </div>
        <div class="col-md-4">
            <asp:Label ID="Label8" runat="server" Text="In-room Service: "></asp:Label>
            <asp:DropDownList ID="ddlInRoom" runat="server" CssClass="form-select" AppendDataBoundItems="true">
            <asp:ListItem Text="Any" Value="" />
        </asp:DropDownList>
        </div>
        <div class="col-md-4">
            <asp:Label ID="lblInsuranceList" runat="server" Text="Insurance Provider: "></asp:Label>
            <asp:DropDownList ID="ddlInsurance" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                <asp:ListItem Text="Any" Value="" />
            </asp:DropDownList>
        </div>
    </div>

    <div class="d-flex flex-wrap gap-2 mt-4 pt-2">
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary px-4 py-2 shadow-sm" onClick="Search_clicked"/>
        <asp:Button ID="btnClear" runat="server" Text="Clear Filters" CssClass="btn btn-outline-secondary px-4 py-2" onClick="Clear_clicked"/>
    </div>
    </div>

        <div class="results-card bg-white shadow-sm mt-4">
    <asp:Label ID="Label1" runat="server" Text="Result"></asp:Label>
    <div class="table-responsive">
        <asp:GridView ID="gvAtendents" runat="server" CssClass="table table-striped table-hover"></asp:GridView>
    </div>
    </div>


    </form>
</body>
</html>
