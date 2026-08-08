<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="WebApplication.register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />

    <title>Register</title>

    <style>
        body {
            background: #eef4f8;
            color: #243746;
        }

        .registration-shell {
            max-width: 760px;
        }

        .registration-title {
            color: #1261a0;
            font-size: 2rem;
            font-weight: 700;
            letter-spacing: -0.02em;
        }

        .question-card {
            border-radius: 1rem;
        }

        .question-label {
            display: block;
            color: #243746;
            font-size: 1.1rem;
            line-height: 1.45;
        }

        .question-card input[type="text"],
        .question-card input[type="email"],
        .question-card input[type="number"],
        .question-card input[type="date"],
        .question-card input[type="tel"],
        .question-card select,
        .question-card textarea {
            display: block;
            width: 100%;
            min-height: 3rem;
            margin-top: 0.9rem;
            padding: 0.65rem 0.85rem;
            border: 1px solid #b8c7d1;
            border-radius: 0.65rem;
            background-color: #ffffff;
            color: #243746;
            font: inherit;
        }

        .question-card textarea {
            min-height: 7rem;
        }

        .question-card input[type="text"]:focus,
        .question-card input[type="email"]:focus,
        .question-card input[type="number"]:focus,
        .question-card input[type="date"]:focus,
        .question-card input[type="tel"]:focus,
        .question-card select:focus,
        .question-card textarea:focus {
            border-color: #1687a7;
            outline: 0;
            box-shadow: 0 0 0 0.25rem rgba(22, 135, 167, 0.16);
        }

        .question-card table {
            width: 100%;
            margin-top: 0.5rem;
            border-collapse: separate;
            border-spacing: 0 0.65rem;
        }

        .question-card td {
            position: relative;
            padding: 0;
        }

        .question-card input[type="radio"],
        .question-card input[type="checkbox"] {
            width: 1.1rem;
            height: 1.1rem;
            margin-right: 0.6rem;
            accent-color: #1261a0;
            cursor: pointer;
        }

        .question-card input[type="radio"] + label,
        .question-card input[type="checkbox"] + label {
            cursor: pointer;
        }

        .navigation-buttons {
            display: flex;
            flex-wrap: wrap;
            justify-content: space-between;
            gap: 0.75rem;
            margin-top: 2rem;
        }

        .btn {
            min-width: 8rem;
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

        @media (max-width: 575.98px) {
            .navigation-buttons .btn {
                flex: 1 1 auto;
            }
        }
    </style>
</head>
<body class="min-vh-100 py-4 py-md-5">
    <form id="form1" runat="server" class="container registration-shell">
        <div class="mb-4">
            <asp:Label ID="Label1" runat="server" Text="Register" CssClass="registration-title"></asp:Label>
        </div>

            <asp:Repeater ID="rptQuestions" runat="server" OnItemDataBound="rptQuestions_db">
                <ItemTemplate>
                    <div class="card question-card border-0 shadow-sm mb-3">
                        <div class="card-body p-4">

                        <asp:HiddenField
                            ID="hiddenQuestionID"
                            runat="server"
                            Value='<%# Eval("questionID") %>' />

                        <asp:HiddenField
                            ID="hiddenFieldKey"
                            runat="server"
                            Value='<%# Eval("field_key") %>' />

                        <asp:Label
                            ID="lblQuestion"
                            runat="server"
                            CssClass="question-label form-label fw-bold mb-2"
                            Text='<%# Eval("question_text") %>' />

                        <asp:PlaceHolder
                            ID="phAnswerControl"
                            runat="server" />

            </div>
        </div>
                </ItemTemplate>
            </asp:Repeater>
        
            <div class="navigation-buttons">               
                <asp:Button ID="btn_start_survey" runat="server" class="btn btn-primary px-4 py-2 shadow-sm" onclick="Start_survey" Text="Start survey" />
                <asp:Button ID="btn_skip_registration" runat="server" class="btn btn-outline-secondary px-4 py-2" onclick="Skipped_registration" Text="Skip" />
            </div>
    </form>
</body>
</html>

