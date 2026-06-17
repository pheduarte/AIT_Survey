<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="survey.aspx.cs" Inherits="WebApplication.survey" %>


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
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="Label1" runat="server" Text="Email:* "></asp:Label>
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>

            <%--Required field--%>
            <asp:RequiredFieldValidator
                 runat="server"
                 ControlToValidate="TextBox1"
                 ErrorMessage="Field required"
                 ForeColor="Red" />

            <br />
            <br />
            <asp:Label ID="Label2" runat="server" Text="Age:* "></asp:Label>
            <asp:RadioButtonList ID="AgeRange" runat="server"
                AutoPostBack="true">

                <asp:ListItem Text="18-24 years old" Value="18-24"></asp:ListItem>
                <asp:ListItem Text="25-35 years old" Value="25-35"></asp:ListItem>
                <asp:ListItem Text="36-50 years old" Value="36-50"></asp:ListItem>
                <asp:ListItem Text="51-65 years old" Value="51-65"></asp:ListItem>
                <asp:ListItem Text="66 years old or older" Value="66+"></asp:ListItem>
            </asp:RadioButtonList>

            <%--Required field--%>
            <asp:RequiredFieldValidator
             runat="server"
              ControlToValidate="AgeRange"
              ErrorMessage="Field required"
              ForeColor="Red" />
            <br />
            <br />
      
            <asp:Label ID="Label3" runat="server" Text="State / Territory:* "></asp:Label>
            <asp:RadioButtonList ID="StateTerritoryRadio" runat="server"
            AutoPostBack="true">

              <asp:ListItem Text="NSW" Value="NSW"></asp:ListItem>
                 <asp:ListItem Text="VIC" Value="VIC"></asp:ListItem>
                <asp:ListItem Text="QLD" Value="QLD"></asp:ListItem>
                <asp:ListItem Text="WA" Value="WA"></asp:ListItem>
                <asp:ListItem Text="SA" Value="SA"></asp:ListItem>
                <asp:ListItem Text="TAS" Value="TAS"></asp:ListItem>
                <asp:ListItem Text="ACT" Value="ACT"></asp:ListItem>
                <asp:ListItem Text="NT" Value="NT"></asp:ListItem>
            </asp:RadioButtonList>

           <%--Required field--%>
            <asp:RequiredFieldValidator
            runat="server"
            ControlToValidate="StateTerritoryRadio"
             ErrorMessage="Field required"
             ForeColor="Red" />
            <br />
            <br />
            <asp:Label ID="Label4" runat="server" Text="Suburb:* "></asp:Label>
            <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>

           <%--Required field--%>
            <asp:RequiredFieldValidator
            runat="server"
             ControlToValidate="TextBox4"
             ErrorMessage="Field required"
             ForeColor="Red" />
            <br />
            <br />
            <asp:Label ID="Label5" runat="server" Text="Postcode:* "></asp:Label>
            <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>

                        <%--Required field--%>
            <asp:RequiredFieldValidator
            runat="server"
            ControlToValidate="TextBox5"
             ErrorMessage="Field required"
             ForeColor="Red" />
            <br />
            <br />

            <%--Membership Question --%>
            <asp:Label ID="Label6" runat="server" Text="Would like to become a member?*"></asp:Label>
            <asp:RadioButtonList ID="rblRespondent" runat="server"
                 AutoPostBack="true"
                 OnSelectedIndexChanged="rblRespondent_SelectedIndexChanged">

                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                <asp:ListItem Text="No" Value="No"></asp:ListItem>
            </asp:RadioButtonList>

            <asp:RequiredFieldValidator
             runat="server"
            ControlToValidate="rblRespondent"
            ErrorMessage="Field required"
             ForeColor="Red" />

            <br />

            <%-- Details dinamically appears --%>
            <asp:Panel ID="pnlRespondentDetails" runat="server" Visible="false">
                <asp:Label ID="lblRespondent_given_name" runat="server" Text="Given Name:*" />
                <asp:TextBox ID="txtRespondentID_given_name" runat="server" />

                            <%--Required field--%>
            <asp:RequiredFieldValidator
              runat="server"
              ControlToValidate="txtRespondentID_given_name"
              ErrorMessage="Field required"
              ForeColor="Red" />
                <br />
                <br />
                <asp:Label ID="Label7" runat="server" Text="Last Name:* " />
                <asp:TextBox ID="TextBox6" runat="server" />

                            <%--Required field--%>
<asp:RequiredFieldValidator
     runat="server"
     ControlToValidate="TextBox6"
     ErrorMessage="Field required"
     ForeColor="Red" />
                <br />
                <br />
                <asp:Label ID="Label8" runat="server" Text="Phone number:* " />
                <asp:TextBox ID="TextBox7" runat="server" />

                            <%--Required field--%>
<asp:RequiredFieldValidator
     runat="server"
     ControlToValidate="TextBox7"
     ErrorMessage="Field required"
     ForeColor="Red" />
                <br />
                <br />
                <asp:Label ID="Label9" runat="server" Text="Date of birthday (YYYY-MM-DD):* " />
                <asp:TextBox ID="TextBox8" runat="server" />

                            <%--Required field--%>
<asp:RequiredFieldValidator
     runat="server"
     ControlToValidate="TextBox8"
     ErrorMessage="Field required"
     ForeColor="Red" />
            </asp:Panel>

            <br />
            <br />
            <asp:Button ID="Button_submit" runat="server" Text="Submit" onClick="submit_form"/>

            <br />
            <br />
            <asp:Label ID="lbMessage" runat="server" Text=""></asp:Label>
        </div>
    </form>
</body>
</html>
