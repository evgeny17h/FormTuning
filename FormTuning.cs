using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace AcademicLoad
{
    public partial class FormTuning : Form
    {
        public List<Subject> selectedSubjects = new List<Subject>();
        public List<Teacher> selectedTeachers = new List<Teacher>();
        public List<Loadtype> selectedLoadtypes = new List<Loadtype>();
        
        FormGrid formgrid;
        
        public FormTuning()
        {
            InitializeComponent();
        }

        private void FormTuning_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "academicLoadDataSet.loadtype". При необходимости она может быть перемещена или удалена.
            this.loadtypeTableAdapter.Connection.ConnectionString = Utility.GetConnectionString();
            this.loadtypeTableAdapter.Fill(this.academicLoadDataSet.loadtype);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "academicLoadDataSet1.view_semester_subject". При необходимости она может быть перемещена или удалена.
            //this.view_semester_subjectTableAdapter.Fill(this.academicLoadDataSet1.view_semester_subject);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "academicLoadDataSet.view_fio". При необходимости она может быть перемещена или удалена.
            //this.view_fioTableAdapter.Fill(this.academicLoadDataSet.view_fio);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "academicLoadDataSet.subject". При необходимости она может быть перемещена или удалена.
            //this.subjectTableAdapter.Fill(this.academicLoadDataSet.subject);            
            // TODO: данная строка кода позволяет загрузить данные в таблицу "academicLoadDataSet.view_fio". При необходимости она может быть перемещена или удалена.
            this.view_fioTableAdapter.Connection.ConnectionString = Utility.GetConnectionString();
            this.view_fioTableAdapter.Fill(this.academicLoadDataSet.view_fio);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "academicLoadDataSet.subject". При необходимости она может быть перемещена или удалена.
            //this.subjectTableAdapter.Fill(this.academicLoadDataSet.subject);
            this.view_semester_subjectTableAdapter.Connection.ConnectionString = Utility.GetConnectionString();
            this.view_semester_subjectTableAdapter.Fill(this.academicLoadDataSet.view_semester_subject);

            formgrid = (FormGrid)Owner;
            radioButtonSelectSubjects.Checked = true;

            string strCon = Utility.GetConnectionString();
            SqlConnection con = new SqlConnection(strCon);

            SqlCommand cmdGetAllLoadtype = new SqlCommand("SelectAllLoadtype", con);
            cmdGetAllLoadtype.CommandType = CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader reader = cmdGetAllLoadtype.ExecuteReader();
                while (reader.Read())
                {
                    selectedLoadtypes.Add(new Loadtype(reader.GetInt16(0), reader.GetString(1)));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            SetLoadtypesListDataSource();
        }

        private void buttonAddSubject_Click(object sender, EventArgs e)
        {
            if (listBoxSubjects.SelectedIndex == -1)
                return;
            long subjectID = Convert.ToInt64(listBoxSubjects.SelectedValue);
            if (selectedSubjects.Exists(x =>x.subjectID == subjectID))
            {
                MessageBox.Show("Данный предмет уже выбран");
                return;
            }
            string subjectName = listBoxSubjects.Text;
            selectedSubjects.Add(new Subject(subjectID, subjectName));
            SetSubjectsListDataSource();
        }

        private void buttonRemoveSubject_Click(object sender, EventArgs e)
        {
            if (listBoxSelectedSubjects.SelectedIndex == -1)
                return;
            selectedSubjects.RemoveAt(listBoxSelectedSubjects.SelectedIndex);
            SetSubjectsListDataSource();
        }

        private void SetSubjectsListDataSource()
        {
            listBoxSelectedSubjects.DataSource = null;
            listBoxSelectedSubjects.DataSource = selectedSubjects;
            listBoxSelectedSubjects.DisplayMember = "subjectName";
            listBoxSelectedSubjects.ValueMember = "subjectID";
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void buttonAddTeacher_Click(object sender, EventArgs e)
        {
            if (listBoxTeachers.SelectedIndex == -1)
                return;
            long teacherID = Convert.ToInt64(listBoxTeachers.SelectedValue);
            string teacherFIO = listBoxTeachers.Text;
            if (selectedTeachers.Exists(x => x.teacherID == teacherID))
            {
                MessageBox.Show("Данный преподаватель уже выбран");
                return;
            }
            selectedTeachers.Add(new Teacher(teacherID, teacherFIO));
            SetTeachersListDataSource();
        }

        private void SetTeachersListDataSource()
        {
            listBoxSelectedTeachers.DataSource = null;
            listBoxSelectedTeachers.DataSource = selectedTeachers;
            listBoxSelectedTeachers.DisplayMember = "teacherFIO";
            listBoxSelectedTeachers.ValueMember = "teacherID";
        }

        private void buttonRemoveTeacher_Click(object sender, EventArgs e)
        {
            if (listBoxSelectedTeachers.SelectedIndex == -1)
                return;
            selectedTeachers.RemoveAt(listBoxSelectedTeachers.SelectedIndex);
            SetTeachersListDataSource();
        }

        private void buttonShow_Click(object sender, EventArgs e)
        {
            if (radioButtonSelectSubjects.Checked)
                formgrid.showForSubjectsMode = true;
            if (radioButtonSelectStaff.Checked)
                formgrid.showForStaffMode = true;
            this.DialogResult = DialogResult.OK;
        }

        private void FormTuning_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void checkBoxAllSem_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAllSem.Checked)
            {
                checkBoxSem1.Checked = true;
                checkBoxSem2.Checked = true;
                checkBoxSem3.Checked = true;
                checkBoxSem4.Checked = true;
                checkBoxSem5.Checked = true;
                checkBoxSem6.Checked = true;
                checkBoxSem7.Checked = true;
                checkBoxSem8.Checked = true;
            }
            else
            {
                checkBoxSem1.Checked = false;
                checkBoxSem2.Checked = false;
                checkBoxSem3.Checked = false;
                checkBoxSem4.Checked = false;
                checkBoxSem5.Checked = false;
                checkBoxSem6.Checked = false;
                checkBoxSem7.Checked = false;
                checkBoxSem8.Checked = false;
            }
        }        

        private void AddSubjectsBySemester(short sem)
        {            
            string strCon = Utility.GetConnectionString();
            SqlConnection con = new SqlConnection(strCon);

            SqlCommand cmdGetSemesterBySubjectID = new SqlCommand("SelectSemesterBySubjectID", con);
            cmdGetSemesterBySubjectID.CommandType = CommandType.StoredProcedure;
            cmdGetSemesterBySubjectID.Parameters.Add("@subject_id", SqlDbType.BigInt);

            SqlCommand cmdGetViewSemesterSubjectBySemester = new SqlCommand("SelectViewSemesterSubjectBySemester", con);
            cmdGetViewSemesterSubjectBySemester.CommandType = CommandType.StoredProcedure;
            cmdGetViewSemesterSubjectBySemester.Parameters.Add("@semester", SqlDbType.SmallInt);
            
            try
            {
                con.Open();
                //очистить список выбранных предметов от предметов данного семестра
                for (int i = 0; i < selectedSubjects.Count; i++)
                {
                    cmdGetSemesterBySubjectID.Parameters["@subject_id"].Value = selectedSubjects[i].subjectID;
                    short semester = Convert.ToInt16(cmdGetSemesterBySubjectID.ExecuteScalar());
                    if (semester == sem)
                    {
                        selectedSubjects.RemoveAt(i);
                        i--;
                    }
                }

                cmdGetViewSemesterSubjectBySemester.Parameters["@semester"].Value = sem;
                SqlDataReader reader = cmdGetViewSemesterSubjectBySemester.ExecuteReader();
                while (reader.Read())
                {
                    selectedSubjects.Add(new Subject(reader.GetInt64(0), reader.GetString(2)));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            SetSubjectsListDataSource();
        }

        private void RemoveSubjectsBySemester(short sem)
        {            
            string strCon = Utility.GetConnectionString();
            SqlConnection con = new SqlConnection(strCon);

            SqlCommand cmdGetSemesterBySubjectID = new SqlCommand("SelectSemesterBySubjectID", con);
            cmdGetSemesterBySubjectID.CommandType = CommandType.StoredProcedure;
            cmdGetSemesterBySubjectID.Parameters.Add("@subject_id", SqlDbType.BigInt);

            try
            {
                con.Open();
                for (int i = 0; i < selectedSubjects.Count; i++)
                {
                    cmdGetSemesterBySubjectID.Parameters["@subject_id"].Value = selectedSubjects[i].subjectID;
                    short semester = Convert.ToInt16(cmdGetSemesterBySubjectID.ExecuteScalar());
                    if (semester == sem)
                    {
                        selectedSubjects.RemoveAt(i);
                        i--;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            SetSubjectsListDataSource();
        }

        private void checkBoxSem1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSem1.Checked)
                AddSubjectsBySemester(1);
            else
                RemoveSubjectsBySemester(1);
        }

        private void checkBoxSem2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSem2.Checked)
                AddSubjectsBySemester(2);
            else
                RemoveSubjectsBySemester(2);
        }

        private void checkBoxSem3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSem3.Checked)
                AddSubjectsBySemester(3);
            else
                RemoveSubjectsBySemester(3);
        }

        private void checkBoxSem4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSem4.Checked)
                AddSubjectsBySemester(4);
            else
                RemoveSubjectsBySemester(4);
        }

        private void checkBoxSem5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSem5.Checked)
                AddSubjectsBySemester(5);
            else
                RemoveSubjectsBySemester(5);
        }

        private void checkBoxSem6_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSem6.Checked)
                AddSubjectsBySemester(6);
            else
                RemoveSubjectsBySemester(6);
        }

        private void checkBoxSem7_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSem7.Checked)
                AddSubjectsBySemester(7);
            else
                RemoveSubjectsBySemester(7);
        }

        private void checkBoxSem8_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSem8.Checked)
                AddSubjectsBySemester(8);
            else
                RemoveSubjectsBySemester(8);
        }

        private void buttonAddAllTeachers_Click(object sender, EventArgs e)
        {
            selectedTeachers.Clear();
            string strCon = Utility.GetConnectionString();
            SqlConnection con = new SqlConnection(strCon);

            SqlCommand cmdGetAllFIO = new SqlCommand("SelectAllFIO", con);
            cmdGetAllFIO.CommandType = CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader reader = cmdGetAllFIO.ExecuteReader();
                while (reader.Read())
                {
                    selectedTeachers.Add(new Teacher(reader.GetInt64(0), reader.GetString(1)));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            SetTeachersListDataSource();
        }

        private void buttonRemoveAllTeachers_Click(object sender, EventArgs e)
        {
            selectedTeachers.Clear();
            SetTeachersListDataSource();
        }

        private void radioButtonSelectSubjects_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSelectSubjects.Checked)
            {
                listBoxSubjects.Enabled = true;
                listBoxSelectedSubjects.Enabled = true;
                buttonAddSubject.Enabled = true;
                buttonRemoveSubject.Enabled = true;
                checkBoxSem1.Enabled = true;
                checkBoxSem2.Enabled = true;
                checkBoxSem3.Enabled = true;
                checkBoxSem4.Enabled = true;
                checkBoxSem5.Enabled = true;
                checkBoxSem6.Enabled = true;
                checkBoxSem7.Enabled = true;
                checkBoxSem8.Enabled = true;
                checkBoxAllSem.Enabled = true;
                listBoxLoadtypes.Enabled = true;
                listBoxSelectedLoadtypes.Enabled = true;
                buttonAddLoadtype.Enabled = true;
                buttonRemoveLoadtype.Enabled = true;                
                buttonAddAllLoadtypes.Enabled = true;
                buttonRemoveAllLoadtypes.Enabled = true;

                listBoxTeachers.Enabled = false;
                listBoxSelectedTeachers.Enabled = false;
                buttonAddTeacher.Enabled = false;
                buttonRemoveTeacher.Enabled = false;
                buttonAddAllTeachers.Enabled = false;
                buttonRemoveAllTeachers.Enabled = false;
            }
        }

        private void radioButtonSelectStaff_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonSelectStaff.Checked)
            {
                listBoxTeachers.Enabled = true;
                listBoxSelectedTeachers.Enabled = true;
                buttonAddTeacher.Enabled = true;
                buttonRemoveTeacher.Enabled = true;
                buttonAddAllTeachers.Enabled = true;
                buttonRemoveAllTeachers.Enabled = true;

                listBoxSubjects.Enabled = false;
                listBoxSelectedSubjects.Enabled = false;
                buttonAddSubject.Enabled = false;
                buttonRemoveSubject.Enabled = false;
                checkBoxSem1.Enabled = false;
                checkBoxSem2.Enabled = false;
                checkBoxSem3.Enabled = false;
                checkBoxSem4.Enabled = false;
                checkBoxSem5.Enabled = false;
                checkBoxSem6.Enabled = false;
                checkBoxSem7.Enabled = false;
                checkBoxSem8.Enabled = false;
                checkBoxAllSem.Enabled = false;
                listBoxLoadtypes.Enabled = false;
                listBoxSelectedLoadtypes.Enabled = false;
                buttonAddLoadtype.Enabled = false;
                buttonRemoveLoadtype.Enabled = false;
                buttonAddAllLoadtypes.Enabled = false;
                buttonRemoveAllLoadtypes.Enabled = false;
            }
        }

        private void SetLoadtypesListDataSource()
        {
            listBoxSelectedLoadtypes.DataSource = null;
            listBoxSelectedLoadtypes.DataSource = selectedLoadtypes;
            listBoxSelectedLoadtypes.DisplayMember = "loadtypeAbbr";
            listBoxSelectedLoadtypes.ValueMember = "loadtypeID";
        }

        private void buttonAddLoadtype_Click(object sender, EventArgs e)
        {
            if (listBoxLoadtypes.SelectedIndex == -1)
                return;
            short loadtypeID = Convert.ToInt16(listBoxLoadtypes.SelectedValue);
            string loadtypeAbbr = listBoxLoadtypes.Text;
            if (selectedLoadtypes.Exists(x => x.loadtypeID == loadtypeID))
            {
                MessageBox.Show("Данный вид нагрузки уже выбран");
                return;
            }
            selectedLoadtypes.Add(new Loadtype(loadtypeID, loadtypeAbbr));
            SetLoadtypesListDataSource();
        }

        private void buttonRemoveLoadtype_Click(object sender, EventArgs e)
        {
            if (listBoxSelectedLoadtypes.SelectedIndex == -1)
                return;
            selectedLoadtypes.RemoveAt(listBoxSelectedLoadtypes.SelectedIndex);
            SetLoadtypesListDataSource();
        }

        private void buttonAddAllLoadtypes_Click(object sender, EventArgs e)
        {
            selectedLoadtypes.Clear();
            string strCon = Utility.GetConnectionString();
            SqlConnection con = new SqlConnection(strCon);

            SqlCommand cmdGetAllLoadtype = new SqlCommand("SelectAllLoadtype", con);
            cmdGetAllLoadtype.CommandType = CommandType.StoredProcedure;
            try
            {
                con.Open();
                SqlDataReader reader = cmdGetAllLoadtype.ExecuteReader();
                while (reader.Read())
                {
                    selectedLoadtypes.Add(new Loadtype(reader.GetInt16(0), reader.GetString(1)));
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
            SetLoadtypesListDataSource();
        }

        private void buttonRemoveAllLoadtypes_Click(object sender, EventArgs e)
        {
            selectedLoadtypes.Clear();
            SetLoadtypesListDataSource();
        }
    }

    public class Subject
    {
        long id;
        string name;

        public Subject(long new_id, string new_name)
        {
            id = new_id;
            name = new_name;
        }

        public long subjectID
        {
            get { return id; }
        }

        public string subjectName
        {
            get { return name; }
        }
    }

    public class Teacher
    {
        long id;
        string fio;

        public Teacher(long myid, string myfio)
        {
            id = myid;
            fio = myfio;
        }

        public long teacherID
        {
            get { return id; }
        }

        public string teacherFIO
        {
            get { return fio; }
        }
    }

    public class Loadtype
    {
        short id;
        string abbr;

        public Loadtype(short new_id, string new_abbr)
        {
            id = new_id;
            abbr = new_abbr;
        }

        public short loadtypeID
        {
            get { return id; }
        }

        public string loadtypeAbbr
        {
            get { return abbr; }
        }
    }
}
