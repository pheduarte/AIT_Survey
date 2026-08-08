<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="home.aspx.cs" Inherits="WebApplication.home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />

    <title>Home</title>

    <style>
        body {
            background: #eef4f8;
            color: #243746;
        }

        .portal-shell {
            max-width: 980px;
        }

        .portal-title {
            color: #1261a0;
            font-weight: 700;
            letter-spacing: -0.03em;
        }

        .portal-card {
            border-radius: 1rem;
            transition: transform 0.2s ease, box-shadow 0.2s ease;
        }

        .portal-card:hover {
            transform: translateY(-2px);
            box-shadow: 0 1rem 2.5rem rgba(36, 55, 70, 0.14) !important;
        }

        .survey-card {
            background: linear-gradient(145deg, #1261a0, #1687a7);
            color: #ffffff;
        }

        .survey-card #startSurvey {
            display: block;
            color: rgba(255, 255, 255, 0.88);
            font-size: 1.1rem;
            line-height: 1.7;
        }

        .login-title {
            color: #1261a0;
            font-weight: 700;
        }

        .field-label {
            display: inline-block;
            margin-bottom: 0.45rem;
            color: #435766;
            font-size: 0.9rem;
            font-weight: 600;
        }

        .form-control {
            min-height: 3rem;
            border-color: #b8c7d1;
            border-radius: 0.65rem;
        }

        .form-control:focus {
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

        .btn-light {
            color: #1261a0;
        }

        .wrong_credentials {
            display: block;
            color: #b42318;
            font-size: 0.9rem;
        }
    </style>
</head>
<body class="min-vh-100 d-flex align-items-center py-5">
    <form id="form1" runat="server" class="container portal-shell">
        <div class="text-center mb-5">
            <h1 class="portal-title display-5 mb-0">iHospital - Portal</h1>
        </div>

        <div class="row g-4 align-items-stretch justify-content-center">
            <div class="col-md-6">
                <div class="card portal-card survey-card h-100 border-0 shadow-lg">
                    <div class="card-body d-flex flex-column p-4 p-lg-5">
                        <asp:Label ID="startSurvey" runat="server" Text="Please click the button bellow to take the survey."></asp:Label>
                        <asp:Button ID="BtnTakeSurvey" runat="server" Text="Start" onClick="Btn_register_Click" CssClass="btn btn-light px-4 py-2 mt-auto align-self-start shadow-sm"/>
                    </div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="card portal-card h-100 border-0 shadow-lg">
                    <div class="card-body d-flex flex-column p-4 p-lg-5">
                        <h3 class="login-title text-center mb-4">Staff Login</h3>

                        <div class="mb-3 textBox">
                            <asp:Label ID="Label_email" runat="server" Text="Email" CssClass="field-label"></asp:Label>
                            <asp:TextBox ID="TextBox_email" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="mb-3 textBox">
                            <asp:Label ID="Label_password" runat="server" Text="Password" CssClass="field-label"></asp:Label>
                            <asp:TextBox ID="TextBox_password" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                        </div>

                        <asp:Label ID="label_title_login" runat="server" Text="" CssClass="wrong_credentials mb-3"></asp:Label>
                        <asp:Button ID="login_btn" runat="server" Text="Login" onClick="check_credentials" CssClass="btn btn-primary w-100 py-2 mt-auto shadow-sm"/>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
