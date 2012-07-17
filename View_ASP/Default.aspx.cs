using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model;

namespace View_ASP
{
    public partial class _Default : System.Web.UI.Page
    {
        private TimetableFitter TimeFitter {get;set;}
        private List<Timetable> All;
        private List<Timetable> Unique;

        protected void Page_Init(object sender, EventArgs e)
        {
            TimeFitter = new TimetableFitter();
            All = new List<Timetable>();
            Unique = new List<Timetable>();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptManager1.RegisterAsyncPostBackControl(WhichTimetables);
            
            if (IsPostBack)
            {
                LoadSession();
                UpdatePanel1.Update();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            GeneratedTables result = TimeFitter.NormalFitCoursesWrapperWithReturn(TimeFitter.generateDefaultCourses(), TimeFitter.generateDefaultTimeTable());
            All = result.All;
            Unique = result.Unique;
            BindTimetables();
            SetCount();
            SaveSession();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            GeneratedTables result = TimeFitter.PushedFitCoursesWrapperWithReturn(TimeFitter.generateDefaultCourses(), TimeFitter.generateDefaultTimeTable(), 30);
            All = result.All;
            Unique = result.Unique;
            BindTimetables();
            SetCount();
            SaveSession();
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            GeneratedTables result = TimeFitter.FlatSqueezedFitCoursesWrapperWithReturn(TimeFitter.generateDefaultCourses(), TimeFitter.generateDefaultTimeTable(), 30);
            All = result.All;
            Unique = result.Unique;
            BindTimetables();
            SetCount();
            SaveSession();
        }

        protected void WhichTimetables_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindTimetables();
            SetCount();
            UpdatePanel1.Update();
        }

        protected void DataPager_PreRender(object sender, EventArgs e)
        {
            BindTimetables();
            SetCount();
        }

        private void SaveSession()
        {
            Session["All"] = All;
            Session["Unique"] = Unique;
        }

        private void LoadSession()
        {
            All = (List<Timetable>)Session["All"];
            Unique = (List<Timetable>)Session["Unique"];
        }

        private void SetCount()
        {
            LabelAllCount.Text = All.Count + " timetables total";
            LabelUniqueCount.Text = "and " + Unique.Count + " are unique when looking at booked only";
        }

        private void BindTimetables()
        {
            if (WhichTimetables.SelectedItem.Text == "All")
            {
                ListViewTables.DataSource = All;
            }
            else
            {
                ListViewTables.DataSource = Unique;
            }
            ListViewTables.DataBind();
        }
    }
}
