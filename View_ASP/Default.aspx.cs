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
        TimetableFitter TimeFitter {get;set;}

        protected void Page_Load(object sender, EventArgs e)
        {
            TimeFitter = new TimetableFitter();

            DataListTables.DataSource = TimeFitter.GeneratedTables;
            DataListTables.DataBind();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            TimeFitter.NormalFitCourses(TimeFitter.generateDefaultCourses(), TimeFitter.generateDefaultTimeTable());
            TimetableSourceChanged();
            SetCount();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            TimeFitter.PushedFitCourses(TimeFitter.generateDefaultCourses(), TimeFitter.generateDefaultTimeTable(), 30);
            TimetableSourceChanged();
            SetCount();
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            TimeFitter.FlatSqueezedFitCourses(TimeFitter.generateDefaultCourses(), TimeFitter.generateDefaultTimeTable(), 30);
            TimetableSourceChanged();
            SetCount();
        }

        protected void WhichTimetables_SelectedIndexChanged(object sender, EventArgs e)
        {
            TimetableSourceChanged();
        }

        private void SetCount()
        {
            LabelAllCount.Text = TimeFitter.GeneratedTables.Count + " timetables total";
            LabelUniqueCount.Text = "and " + TimeFitter.UniquelyGeneratedTables.Count + " are unique when looking at booked only";
        }

        private void TimetableSourceChanged()
        {
            if (WhichTimetables.SelectedItem.Text == "All")
            {
                DataListTables.DataSource = TimeFitter.GeneratedTables;
            }
            else
            {
                DataListTables.DataSource = TimeFitter.UniquelyGeneratedTables;
            }
            DataListTables.DataBind();
        }
    }
}
