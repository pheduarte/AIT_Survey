<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="survey.aspx.cs" Inherits="WebApplication.survey" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>iHospital Sydney Survey</title>

    <link href="Content/bootstrap.min.css" rel="stylesheet" />

    <style>
        body {
            background: #f5f5f5;
        }

        .d-none {
            display: none !important;
        }

        .survey-container {
            max-width: 680px;
            margin: 0 auto;
            padding: 40px 15px;
        }

        .survey-card {
            padding: 30px;
            background: #ffffff;
            border: 1px solid #dddddd;
            border-radius: 6px;
            box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
        }

        .survey-title {
            margin-top: 0;
            color: #337ab7;
        }

        .progress-details,
        .navigation-buttons {
            display: flex;
            justify-content: space-between;
        }

        .progress-details {
            margin-bottom: 8px;
            color: #777777;
            font-size: 13px;
        }

        .question-step {
            min-height: 260px;
        }

        .question-title {
            margin-top: 0;
            margin-bottom: 20px;
            font-size: 20px;
        }

        .form-check {
            margin-bottom: 10px;
        }

        .form-check-input {
            margin-right: 7px;
        }

        .navigation-buttons {
            margin-top: 25px;
        }

        @media (max-width: 600px) {
            .survey-container {
                padding-top: 20px;
            }

            .survey-card {
                padding: 20px;
            }
        }
    </style>
</head>
        <body class="bg-light">

    <form id="form2" runat="server">

        <div class="container py-5">

            <div class="row justify-content-center">
                <div class="col-md-8 col-lg-6">

                    <div class="card shadow-sm">
                        <div class="card-body p-4">

                            <div class="mb-3">
                                <asp:Label
                                    ID="lblProgress"
                                    runat="server"
                                    CssClass="text-muted">
                                </asp:Label>
                            </div>

                            <asp:Label
                                ID="lblQuestion"
                                runat="server"
                                CssClass="form-label fw-bold fs-5">
                            </asp:Label>

                            <asp:HiddenField
                                ID="hiddenQuestionID"
                                runat="server" />

                            <asp:HiddenField
                                ID="hiddenAnswerType"
                                runat="server" />

                            <asp:HiddenField 
                                ID="hiddenFieldKey" 
                                runat="server" 
                                Value='<%# Eval("field_key") %>'/>


                            <!-- Radio answer -->

                            <asp:RadioButtonList
                                ID="rblAnswer"
                                runat="server"
                                CssClass="mt-3">
                            </asp:RadioButtonList>

                            <!-- Checkbox answer -->

                            <asp:CheckBoxList
                                ID="cblAnswer"
                                runat="server"
                                CssClass="mt-3">
                            </asp:CheckBoxList>

                            <!-- Dropdown answer -->

                            <asp:DropDownList
                                ID="ddlAnswer"
                                runat="server"
                                CssClass="form-select mt-3">
                            </asp:DropDownList>

                            <!-- Text answer -->

                            <asp:TextBox
                                ID="txtAnswer"
                                runat="server"
                                CssClass="form-control mt-3">
                            </asp:TextBox>

                           

                            <!-- Dynamically displays error message -->
                            <asp:Label
                                ID="lblError"
                                runat="server"
                                CssClass="text-danger d-block mt-3">
                            </asp:Label>

                            <div class="d-flex justify-content-between mt-4">

                                <asp:Button
                                    ID="btnPrevious"
                                    runat="server"
                                    Text="Previous"
                                    CssClass="btn btn-outline-secondary"
                                    OnClick="btnPrevious_Click" />

                                <asp:Button
                                    ID="btnNext"
                                    runat="server"
                                    Text="Next"
                                    CssClass="btn btn-primary"
                                    OnClick="btnNext_Click" />

                            </div>

                        </div>
                    </div>

                </div>
            </div>

        </div>

    </form>

</body>
</html>
