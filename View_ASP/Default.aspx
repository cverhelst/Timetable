<%@ Page Title="Timetables" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="View_ASP._Default" %>
<%@ Assembly Name="Model" %>
<%@ Import Namespace="Model" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    
    <asp:UpdateProgress ID="UpdateProgress1" runat="server">
    <ProgressTemplate>
        Processing request...
    </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        
            <asp:RadioButtonList id="WhichTimetables" runat="server" 
                onselectedindexchanged="WhichTimetables_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Selected="True">All</asp:ListItem>
                <asp:ListItem>Unique</asp:ListItem>
             </asp:RadioButtonList>

            <asp:Button ID="Button1" runat="server" Text="Normal" onclick="Button1_Click" />
            <asp:Button ID="Button2" runat="server" Text="Pushed" onclick="Button2_Click" />
            <asp:Button ID="Button3" runat="server" Text="Squeezed" onclick="Button3_Click" />

            <hr />

            <asp:Label ID="LabelAllCount" runat="server" Text=""></asp:Label>
            <asp:Label ID="LabelUniqueCount" runat="server" Text=""></asp:Label>

            

            <asp:ListView ID="ListViewTables" runat="server" 
                ItemPlaceholderID="TimetablePlaceholder">
                <%--Timetable --%>
                <LayoutTemplate>
                    <asp:DataPager ID="DataPager1" runat="server" OnPreRender="DataPager_PreRender" PagedControlID="ListViewTables" PageSize="3">
                <Fields>
                    <asp:NextPreviousPagerField ShowFirstPageButton="True" ShowNextPageButton="False" />
                    <asp:NumericPagerField />
                    <asp:NextPreviousPagerField ShowLastPageButton="True" ShowPreviousPageButton="False" />
                </Fields>
            </asp:DataPager>
                    <asp:PlaceHolder ID="TimetablePlaceholder" runat="server"></asp:PlaceHolder>
                    
                </LayoutTemplate>
                <ItemTemplate>
                    <div class="Timetable">
                    <h1>Timetable <%# Container.DataItemIndex %></h1>
                    <asp:DataList ID="DataListDays" runat="server" DataSource='<%# DataBinder.Eval(Container.DataItem,"Days") %>'>
                        <%--Day --%>
                        <ItemTemplate>
                            <h2>Day <%# DataBinder.Eval(Container.DataItem,"Number")  %></h2>
                            <asp:DataList ID="DataListRooms" runat="server" DataSource='<%# DataBinder.Eval(Container.DataItem,"Rooms") %>'>
                                <%--BookableRoom --%>
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Room.Name") %>' ToolTip='<%# DataBinder.Eval(Container.DataItem,"Room").ToString() %>'></asp:Label>
                                    <asp:Repeater ID="DataListTime" runat="server" DataSource='<%# DataBinder.Eval(Container.DataItem,"Time")%>'>
                                        <%--TimeUnit --%>
                                        <ItemTemplate>
                                            <asp:Label ID="LabelTime" runat="server" 
                                            Text='<%# DataBinder.Eval(Container.DataItem,"AssignedCourse") != null ? DataBinder.Eval(Container.DataItem,"AssignedCourse") : "Unbooked" %>' 
                                            Width="<%# ((TimeUnit)Container.DataItem as TimeUnit).Duration() %>" 
                                            ToolTip="<%# Container.DataItem.ToString() %>"
                                            CssClass='<%# DataBinder.Eval(Container.DataItem,"AssignedCourse") != null ? "BookedTime" : "FreeTime" %>'
                                            ></asp:Label>
                                         </ItemTemplate>
                                    </asp:Repeater>
                                </ItemTemplate>
                            </asp:DataList>
                        </ItemTemplate>
                    </asp:DataList>
                    </div>
                </ItemTemplate>
            </asp:ListView>

            <asp:DataPager ID="DataPager2" runat="server" OnPreRender="DataPager_PreRender" PagedControlID="ListViewTables" PageSize="3">
                <Fields>
                    <asp:NextPreviousPagerField ShowFirstPageButton="True" ShowNextPageButton="False" />
                    <asp:NumericPagerField />
                    <asp:NextPreviousPagerField ShowLastPageButton="True" ShowPreviousPageButton="False" />
                </Fields>
            </asp:DataPager>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
