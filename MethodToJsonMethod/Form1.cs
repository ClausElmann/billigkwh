using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using billigKwhWebApp

namespace WindowsFormsApplication2
{



    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string[] GetList()
        {
            return sourceTextBox.Text.Replace(Environment.NewLine, ",").Split(',');
        }



        private void button1_Click(object sender, EventArgs e)
        {
            string[] lines = sourceTextBox.Text.Replace(Environment.NewLine, ",").Split(',');

            destinationTextBox.Text += "var httpResponseText = await JSON.SendJsonCallAndWaitForResponse(\"XXX\",";
            destinationTextBox.Text += Environment.NewLine;
            destinationTextBox.Text += "$@\"";

            for (int i = 0; i < lines.Count(); i++)
            {
                var x = lines[i].Trim().Split(' ');
                destinationTextBox.Text += Environment.NewLine + string.Format("'{0}':{1},", x[1], "{ToJson(o." + x[1] + ")}");
            }

            destinationTextBox.Text += Environment.NewLine + "\");";

            destinationTextBox.Text += Environment.NewLine + " return (httpResponseText == \"##\" ? 0 : JsonConvert.DeserializeObject<int>(httpResponseText));";

            Clipboard.SetText(destinationTextBox.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sourceTextBox.Text = "";
            destinationTextBox.Text = "";
        }

        private void buttonAccessors_Click(object sender, EventArgs e)
        {
            var columns = GetList();

            destinationTextBox.Text = "";

            for (int i = 0; i < columns.Count(); i++)
            {
                //var x = lines[i].Trim().Split(' ');

                destinationTextBox.Text += Environment.NewLine + $@"
  public get {columns[i].Trim().ToLowerFirstChar()}() {{
      return this.mainForm.get(""{columns[i].Trim().ToLowerFirstChar()}"");
  }}";
            }
            Clipboard.SetText(destinationTextBox.Text);
        }

        private void buttonForm_Click(object sender, EventArgs e)
        {
            var columns = GetList();

            string lines = "";

            for (int i = 0; i < columns.Count(); i++)
            {
                lines += Environment.NewLine + $@"      {columns[i].Trim().ToLowerFirstChar()}: new FormControl(this.selectedItem.{columns[i].Trim().ToLowerFirstChar()}, [Validators.required]),";
            }

            destinationTextBox.Text = $@"
  private initFormGroup() {{
      this.mainForm = new FormGroup({{
  {lines}
      }});
    }}
  ";

            Clipboard.SetText(destinationTextBox.Text);
        }

        private void buttonHtml_Click(object sender, EventArgs e)
        {
            var columns = GetList();

            destinationTextBox.Text = "";

            for (int i = 0; i < columns.Count(); i++)
            {
                var name = columns[i].Trim().ToLowerFirstChar();

                destinationTextBox.Text += Environment.NewLine + $@"<div class=""field"">
      <label for= ""{name}"" class=""block"">{name}</label>
      <input formControlName=""{name}"" type= ""number"" maxlength= ""200"" pInputText id=""{name}"" aria-describedby= ""{name}-help"" />
      <div *ngIf=""{name}.errors && ({name}.dirty || {name}.touched)"" class=""p-error block"">
        <div *ngIf=""{name}.errors?.required"">Påkrævet</div >
        <div *ngIf=""{name}.errors?.digitsOnly"">Kun hel tal</div>
      </div>
</div>";
            }
            Clipboard.SetText(destinationTextBox.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var assembly = Assembly.GetAssembly(billigKwhWebApp.).GetExecutingAssembly();

            Type assemblyType = billigKwhWebApp.GetType(item.ToolTip.ToString());
            if (assemblyType != null)
            {

                foreach (var prop in assemblyType.GetProperties())
                {

                    PropertyInfo property = prop;
                    TreeViewItem childItem = new TreeViewItem();
                    childItem.Header = property.Name;
                    /*Following line gives IOFileNotFound exception, if property  is declared in some other assembly.*/
                    childItem.ToolTip = property.PropertyType.FullName;
                    item.Items.Add(childItem);
                }
            }
        }
    }
}
