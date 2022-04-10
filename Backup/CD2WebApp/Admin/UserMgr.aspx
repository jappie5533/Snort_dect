<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="UserMgr.aspx.cs" Inherits="CD2WebApp.Admin.UserMgr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="RightContent" runat="server">
    <asp:TextBox ID="TextBox1" runat="server" Width="200px"></asp:TextBox>
    &nbsp;<asp:Button ID="Button1" runat="server" Text="Search" 
    onclick="Button1_Click" />
    <hr />
    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
        AllowSorting="True" AutoGenerateColumns="False" AutoGenerateEditButton="True" 
        CellPadding="4" DataKeyNames="uid" ForeColor="#333333" GridLines="None" 
        ondatabound="GridView1_DataBound" 
        onpageindexchanged="GridView1_PageIndexChanged" 
        onpageindexchanging="GridView1_PageIndexChanging" 
        onrowcancelingedit="GridView1_RowCancelingEdit" 
        onrowediting="GridView1_RowEditing" onrowupdating="GridView1_RowUpdating" 
        onsorted="GridView1_Sorted" onsorting="GridView1_Sorting" Width="760px" 
        PageSize="15">
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <Columns>
            <asp:TemplateField HeaderText="User ID" SortExpression="uid">
                <EditItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("uid") %>'></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("uid") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Account" SortExpression="account">
                <EditItemTemplate>
                    <asp:TextBox ID="Account" runat="server" Text='<%# Bind("account") %>' 
                        Width="70px"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                        ControlToValidate="Account" CssClass="failureNotification" Display="Dynamic" 
                        ValidationExpression="\w{3,50}" ValidationGroup="ModifyUserValidationGroup">*</asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                        ControlToValidate="Account" CssClass="failureNotification" Display="Dynamic" 
                        ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("account") %>'></asp:Label>
                </ItemTemplate>
                <ControlStyle Width="70px" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="E-mail" SortExpression="email">
                <EditItemTemplate>
                    <asp:TextBox ID="Email" runat="server" Text='<%# Bind("email") %>'></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                        ControlToValidate="Email" CssClass="failureNotification" Display="Dynamic" 
                        ValidationExpression="^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$" 
                        ValidationGroup="ModifyUserValidationGroup">*</asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                        ControlToValidate="Email" CssClass="failureNotification" Display="Dynamic" 
                        ErrorMessage="RequiredFieldValidator">*</asp:RequiredFieldValidator>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("email") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Agent type">
                <EditItemTemplate>
                    <asp:DropDownList ID="DDL_AgentType" runat="server" 
                        SelectedValue='<%# Bind("type") %>'>
                        <asp:ListItem Value="0">SPA</asp:ListItem>
                        <asp:ListItem Value="1">RAA</asp:ListItem>
                        <asp:ListItem Value="2">PAA</asp:ListItem>
                    </asp:DropDownList>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="AgentType" runat="server" Text='<%# Bind("Type") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Agent ID">
                <EditItemTemplate>
                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("id") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Recently login IP">
                <EditItemTemplate>
                    <asp:Label ID="Label5" runat="server" Text='<%# Bind("ip") %>'></asp:Label>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("ip") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <EditRowStyle BackColor="#999999" />
        <EmptyDataTemplate>
            <asp:Label ID="Label6" runat="server" Text="No Data."></asp:Label>
        </EmptyDataTemplate>
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>
</asp:Content>
