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

            <asp:Label ID="lbl_gender" runat="server" Text="Gender:* "></asp:Label>
            <asp:DropDownList ID="DropDown_gender" runat="server">
                <asp:ListItem Text="-- Select an option --" Value="" />
                <asp:ListItem Text="Male" Value="Male" />
                <asp:ListItem Text="Female" Value="Female" />
                <asp:ListItem Text="Prefer not to say" Value="N/A" />
            </asp:DropDownList>

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

            <asp:Label ID="Label13" runat="server" Text="List the services you used during your stay (max. 4): "></asp:Label>
            <asp:CheckBoxList
                ID="CheckServices"
                runat="server">
            </asp:CheckBoxList>

            <br />
            <br />

            <asp:Label ID="lblInsuranceProviders" runat="server" Text="Insurance Providers (max. 2): "></asp:Label>
            <asp:CheckBoxList
                ID="cblInsuranceProviders"
                runat="server">
            </asp:CheckBoxList>

            <br />
            <br />

            <asp:Label ID="lblDischargePlan" runat="server" Text="Discharge Plan (all that apply):* "></asp:Label>
            <asp:CheckBoxList
                ID="cblDischargePlan"
                runat="server">
            </asp:CheckBoxList>

            <br />
            <br />

            <%--Membership Question --%>
            <asp:Label ID="Label6" runat="server" Text="Would like to become a member?*"></asp:Label>
            <asp:RadioButtonList ID="rblRespondent" runat="server"
                 AutoPostBack="true"
                 OnSelectedIndexChanged="rblRespondent_SelectedIndexChanged">

                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
            </asp:RadioButtonList>

            <asp:RequiredFieldValidator
             runat="server"
            ControlToValidate="rblRespondent"
            ErrorMessage="Field required"
             ForeColor="Red" />

            <br />

            <%-- Details dinamically appears --%>
            <asp:Panel ID="pnlRespondentDetails" runat="server" Visible="false">


                <%-- Title Field --%>
            <asp:Label ID="lbl_title" runat="server" Text="Title:*" />
            <asp:DropDownList ID="select_title" runat="server">
                <asp:ListItem Text="-- Select Title --" Value="" />
                <asp:ListItem Text="Mr" Value="Mr" />
                <asp:ListItem Text="Mrs" Value="Mrs" />
                <asp:ListItem Text="Miss" Value="Miss" />
                <asp:ListItem Text="Ms" Value="Ms" />
                <asp:ListItem Text="Dr" Value="Dr" />
                <asp:ListItem Text="Prof" Value="Prof" />
            </asp:DropDownList>
                <br />
                <br />

            <%--Required field--%>
            <asp:RequiredFieldValidator
            runat="server"
            ControlToValidate="select_title"
            ErrorMessage="Field required"
            ForeColor="Red" />

                 <%-- Given Name Field --%>
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

                 <%-- Last Name Field --%>
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

             <asp:Label ID="Label9" runat="server" Text="Date of birth (YYYY-MM-DD):* " />
             <asp:TextBox ID="TextBox8" runat="server" TextMode="Date"/>

           <%--Required field--%>
            <asp:RequiredFieldValidator
             runat="server"
            ControlToValidate="TextBox8"
             ErrorMessage="Field required"
             ForeColor="Red" />
            </asp:Panel>

            <br />
            <br />

            <asp:Label ID="lblAttendedRehabOP" runat="server" Text="Did you have to attend Rehabilitation or Operation/Surgery?"></asp:Label>
            <asp:RadioButtonList ID="RadioBtnAttendedRehabOP" 
                runat="server" 
                OnSelectedIndexChanged="radioAttendedRehabOP_SelectedIndexChanged" 
                AutoPostBack="true">

                <asp:ListItem Text="No" Value="No"/>
                <asp:ListItem Text="Yes" Value="Yes" />
            </asp:RadioButtonList>

            <br />
            <br />

            <asp:Panel ID="PanelAttendedRehab" runat="server" Visible="false">
                <asp:Label ID="lblRoomType" runat="server" Text="What was your room type like?"></asp:Label>

                <asp:RadioButtonList ID="radioRoomType" runat="server" 
                    OnSelectedIndexChanged="radioRoomTypeService_SelectedIndexChanged" AutoPostBack="true">
                </asp:RadioButtonList>

                <%--Required field--%>
                <asp:RequiredFieldValidator
                    runat="server"
                    ControlToValidate="radioRoomType"
                    ErrorMessage="Field required"
                    ForeColor="Red" />

            </asp:Panel>

            <br />
            <br />

            <asp:Panel ID="PanelInRoomService" runat="server" Visible="false">

            <asp:Label ID="Label12" runat="server" Text="For how lond did you stay?"></asp:Label>
            <br />
            <br />

            <asp:Label ID="Label10" runat="server" Text="Select all In-Room services you used during your stay"></asp:Label>

            <asp:CheckBoxList runat="server" ID="CheckInRoomServices" AutoPostBack="true" OnSelectedIndexChanged="radioWifiService_SelectedIndexChanged">
            </asp:CheckBoxList>

         
            </asp:Panel>

            <br />
            <br />

            <asp:Panel ID="PanelWifiService" runat="server" Visible="false">
            <asp:Label ID="Label11" runat="server" Text="Select the Wi-Fi services you used during your stay"></asp:Label>

            <asp:CheckBoxList runat="server" ID="CheckWiFiService">              
            </asp:CheckBoxList>   

            </asp:Panel>


            <asp:Button ID="Button_submit" runat="server" Text="Submit" onClick="submit_form" CssClass="btn btn-primary"/>

            <br />
            <br />
            <asp:Label ID="lbMessage" runat="server" Text=""></asp:Label>
        </div>
    </form>
</body>
</html>
