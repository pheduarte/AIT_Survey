<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="survey.aspx.cs" Inherits="WebApplication.survey" MaintainScrollPositionOnPostback="true" %>


<script runat="server">
    protected override void OnInit(EventArgs e)
    {
        UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
        base.OnInit(e);
    }
</script>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Survey</title>

    <link href="Content/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">

        <div class="">
            <span class="h2">iHospital Sydney - Survey</span>
        </div>
        <div>

            <br />
            <br />
            <br />

            <div>
                <asp:Repeater 
                    ID="questions_rep"
                    runat="server"
                    OnItemDataBound="rptQuestions_ItemDataBound"
                    >

                    <ItemTemplate>

                        <div class="mb-4">

                            <h5><%# Eval("question_text") %></h5>

                            <asp:PlaceHolder ID="answerPlaceholder" runat="server"></asp:PlaceHolder>


                            <asp:Panel ID="Panel1" runat="server">

                                <asp:PlaceHolder ID="derivated_answerPlaceholder" runat="server"></asp:PlaceHolder>

                            </asp:Panel>

                        </div>

                        

                    </ItemTemplate>

                </asp:Repeater>
            </div>

            <br />
            <br />
       

            <asp:Button ID="Button_submit" runat="server" Text="Submit" onClick="submit_form" CssClass="btn btn-primary"/>

            <br />
            <br />
            <asp:Label ID="lbMessage" runat="server" Text=""></asp:Label>
        </div>
    </form>
</body>
</html>
