<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Admin.master" AutoEventWireup="true" CodeBehind="UsingLog.aspx.cs" Inherits="CD2WebApp.Admin.UsingLog" %>
<asp:Content ID="Content1" ContentPlaceHolderID="RightContent" runat="server">
    <p>
        <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
            <asp:View ID="View1" runat="server">
                <asp:Label ID="Label2" runat="server" Font-Size="Large" Text="Account: "></asp:Label>
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                &nbsp;<asp:Button ID="Button1" runat="server" Text="Search" 
                    onclick="Button1_Click" />
                <br />
                <asp:Label ID="Label1" runat="server" Font-Size="Large" Text="Date: "></asp:Label>
                <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                &nbsp;~
                <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                &nbsp;<asp:Button ID="Button2" runat="server" Text="Search" 
                    onclick="Button2_Click" />
                <asp:ScriptManager ID="ScriptManager1" runat="server" 
                    EnableScriptGlobalization="True">
                </asp:ScriptManager>
                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" 
                TargetControlID="TextBox2">
                </ajaxToolkit:CalendarExtender>
                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" 
                TargetControlID="TextBox3">
                </ajaxToolkit:CalendarExtender>
                <hr />
                <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                    GridLines="None" onpageindexchanged="GridView1_PageIndexChanged" 
                    onpageindexchanging="GridView1_PageIndexChanging" 
                    onrowcommand="GridView1_RowCommand" PageSize="20" Width="750px">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField HeaderText="Date">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton2" runat="server" 
                                    CommandArgument='<%# bind("dt") %>' CommandName="selectLogByDate" 
                                    Text='<%# bind("dt") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Account">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("account") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkButton3" runat="server" 
                                    CommandArgument='<%# bind("account") %>' CommandName="selectLogByName" 
                                    Text='<%# bind("account") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="src" HeaderText="Peer ID" />
                        <asp:BoundField DataField="datalength" DataFormatString="{0} Bytes" 
                            HeaderText="Data Size" />
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <EmptyDataTemplate>
                        No Data.
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
            </asp:View>
            <asp:View ID="View2" runat="server">
                <asp:GridView ID="GridView2" runat="server" AllowPaging="True" 
                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" 
                    GridLines="None" onpageindexchanged="GridView2_PageIndexChanged" 
                    onpageindexchanging="GridView2_PageIndexChanging" PageSize="20" 
                    Width="750px" ondatabound="GridView2_DataBound">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField HeaderText="Date">
                            <EditItemTemplate>
                                <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("dt") %>'></asp:TextBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Bind("ts") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="account" HeaderText="Account" />
                        <asp:BoundField DataField="src" HeaderText="Peer ID" />
                        <asp:BoundField DataField="datalength" DataFormatString="{0} Bytes" 
                            HeaderText="Data Size" />
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <EmptyDataTemplate>
                        No Data.
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
                <asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click">&lt; Back</asp:LinkButton>
            </asp:View>
        </asp:MultiView>
    </p>
</asp:Content>
