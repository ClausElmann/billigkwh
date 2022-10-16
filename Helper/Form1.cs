using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Helper
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

        public List<PropertyInfo> GetListOfProperties()
        {
            var assembly = Assembly.GetAssembly(typeof( BilligKwhWebApp.Core.Dto.DokumentDto));

            var dtoClasses = new List<Type>();

            var listOfProperties = new List<PropertyInfo>();

            dtoClasses.AddRange(assembly.GetTypes().Where(w => w.Name.Contains("Dto")));
            var foundDTOClass = (Type)null;

            if ((foundDTOClass = dtoClasses.Where(w => w.Name.Equals(textBoxDto.Text, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault()) == null)
            {
            }
            else
            {
                listOfProperties = new List<PropertyInfo>();

                foreach (var property in foundDTOClass.GetProperties())
                {
                    var attribute = property.GetCustomAttributes(false);//.Where(w => w is MetaUIFilterAttribute).FirstOrDefault() as MetaUIFilterAttribute;

                    if (attribute != null)
                    {
                        listOfProperties.Add(property);
                    }
                }
            }

            //foreach (var item in listOfProperties)
            //{
            //    var x = FormGroupLine(item);

            //}

            List<string> list = new() { "slettet", "id", "sidstrettet", "sidstrettetafbrugerid", "objektguid", "kundeid" };

            return listOfProperties.Where(w => !list.Contains(w.Name.ToLower())).ToList();
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


        private void buttonAssembly_Click(object sender, EventArgs e)
        {
            var assembly = Assembly.GetAssembly(typeof(BilligKwhWebApp.Core.Dto.DokumentDto));

            var dtoClasses = new List<Type>();

            var listOfProperties = new List<PropertyInfo>();

            dtoClasses.AddRange(assembly.GetTypes().Where(w => w.Name.Contains("Dto")));
            var foundDTOClass = (Type)null;

            if ((foundDTOClass = dtoClasses.Where(w => w.Name.Equals(textBoxDto.Text, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault()) == null)
            {



            }
            else
            {
                //foundDTOClass = dtoClasses.Where(w =>
                //   w.GetCustomAttributes(false).Length > 0 &&
                //   w.GetCustomAttributes(false).FirstOrDefault() != null).FirstOrDefault();


                listOfProperties = new List<PropertyInfo>();

                foreach (var property in foundDTOClass.GetProperties())
                {
                    var attribute = property.GetCustomAttributes(false);//.Where(w => w is MetaUIFilterAttribute).FirstOrDefault() as MetaUIFilterAttribute;

                    if (attribute != null)
                    {
                        listOfProperties.Add(property);
                    }
                }
            }
            sourceTextBox.Text = string.Join(Environment.NewLine, listOfProperties);
            //foreach (var item in listOfProperties)
            //{
            //    sourceTextBox.Text+= Environment.NewLine + item.Name;
            //}

            //foreach (var type in assembly.GetTypes().Where(w => w.IsPublic && !w.IsAbstract))
            //{
            //}

            //    Type assemblyType = billigKwhWebApp.GetType(item.ToolTip.ToString());
            //if (assemblyType != null)
            //{

            //    foreach (var prop in assemblyType.GetProperties())
            //    {

            //        PropertyInfo property = prop;
            //        TreeViewItem childItem = new TreeViewItem();
            //        childItem.Header = property.Name;
            //        /*Following line gives IOFileNotFound exception, if property  is declared in some other assembly.*/
            //        childItem.ToolTip = property.PropertyType.FullName;
            //        item.Items.Add(childItem);
            //    }
            //}








            //            var columns = GetList();

            //            destinationTextBox.Text = "";

            //            for (int i = 0; i < columns.Count(); i++)
            //            {
            //                var name = columns[i].Trim().ToLowerFirstChar();

            //                destinationTextBox.Text += Environment.NewLine + $@"<div class=""field"">
            //      <label for= ""{name}"" class=""block"">{name}</label>
            //      <input formControlName=""{name}"" type= ""number"" maxlength= ""200"" pInputText id=""{name}"" aria-describedby= ""{name}-help"" />
            //      <div *ngIf=""{name}.errors && ({name}.dirty || {name}.touched)"" class=""p-error block"">
            //        <div *ngIf=""{name}.errors?.required"">Påkrævet</div >
            //        <div *ngIf=""{name}.errors?.digitsOnly"">Kun hel tal</div>
            //      </div>
            //</div>";
            //            }
            //            Clipboard.SetText(destinationTextBox.Text);
        }

        private static string Accessors(List<string> columns)
        {
            var result = "";
            for (int i = 0; i < columns.Count(); i++)
            {
                result += Environment.NewLine + $@"
  public get {columns[i].Trim().ToLowerFirstChar()}() {{
      return this.mainForm.get(""{columns[i].Trim().ToLowerFirstChar()}"");
  }}";
            }

            return result;
        }

        private void buttonAccessorsReflection_Click(object sender, EventArgs e)
        {
            var list = GetListOfProperties();

            destinationTextBox.Text = Accessors(list.Select(s => s.Name).ToList());

            Clipboard.SetText(destinationTextBox.Text);
        }

        private void buttonFormReflection_Click(object sender, EventArgs e)
        {
            var list = GetListOfProperties();

            //var columns = list.Select(s => s.Name).ToList();

            string lines = "";

            for (int i = 0; i < list.Count(); i++)
            {
                lines += Environment.NewLine + $@"      " + FormGroupLine(list[i]);

                //    Environment.NewLine + $@"      {columns[i].Trim().ToLowerFirstChar()}: new FormControl(this.selectedItem.{columns[i].Trim().ToLowerFirstChar()}, [Validators.required]),";
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


        private static string FormGroupLine(PropertyInfo propertyInfo)
        {
            TypeCode typeCode = Type.GetTypeCode(propertyInfo.PropertyType);

            if (Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null)
            {
                //typeCode = Type.GetTypeCode(System.Nullable.GetUnderlyingType(propertyInfo.PropertyType));
                return $@"{propertyInfo.Name.ToLowerFirstChar()}: new FormControl(this.selectedItem.{propertyInfo.Name.ToLowerFirstChar()}),";
            }
            else
            {
                // It's not a nullable type
                switch (typeCode)
                {
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                        return $@"{propertyInfo.Name.ToLowerFirstChar()}: new FormControl(this.selectedItem.{propertyInfo.Name.ToLowerFirstChar()}, [Validators.required, CustomValidators.digitsOnly()]),";
                    case TypeCode.String:
                        return $@"{propertyInfo.Name.ToLowerFirstChar()}: new FormControl(this.selectedItem.{propertyInfo.Name.ToLowerFirstChar()}),";
                    case TypeCode.DateTime:
                    case TypeCode.Boolean:
                    case TypeCode.Decimal:
                    case TypeCode.Double:
                        return $@"{propertyInfo.Name.ToLowerFirstChar()}: new FormControl(this.selectedItem.{propertyInfo.Name.ToLowerFirstChar()}, [Validators.required]),";

                    default: break;
                }
            }
            return propertyInfo.Name + " " + typeCode;
        }


        private void buttonHTMLReflection_Click(object sender, EventArgs e)
        {
            var list = GetListOfProperties();

            //var columns = list.Select(s => s.Name).ToList();

            //string lines = "";

            for (int i = 0; i < list.Count(); i++)
            {
                destinationTextBox.Text += Environment.NewLine + $@"      " + HtmlFromPropertyInfo(list[i]);

                //    Environment.NewLine + $@"      {columns[i].Trim().ToLowerFirstChar()}: new FormControl(this.selectedItem.{columns[i].Trim().ToLowerFirstChar()}, [Validators.required]),";
            }

            //destinationTextBox.Text = "";

            //for (int i = 0; i < list.Count(); i++)
            //{
            //    var name = list[i].Trim().ToLowerFirstChar();

            //    destinationTextBox.Text += Environment.NewLine + HtmlFromPropertyInfo()
            //}
            Clipboard.SetText(destinationTextBox.Text);
        }


        private static string HtmlFromPropertyInfo(PropertyInfo propertyInfo)
        {
            string name = propertyInfo.Name.ToLowerFirstChar();

            string type = "text";
            string boundaries = "";


            TypeCode typeCode = Type.GetTypeCode(propertyInfo.PropertyType);

            if (Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null)
            {
                switch (typeCode)
                {
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                        type = "number";
                        break;
                    case TypeCode.String:
                        boundaries = @"maxlength=""50""";
                        break;
                    case TypeCode.DateTime:
                    case TypeCode.Boolean:
                    case TypeCode.Decimal:
                    case TypeCode.Double:
                        break;
                    default: break;
                }

            }
            else
            {
                // It's not a nullable type
                switch (typeCode)
                {
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                        type = "number";
                        break;
                    case TypeCode.String:
                        boundaries = @"maxlength=""50""";
                        break;
                    case TypeCode.DateTime:
                    case TypeCode.Boolean:
                    case TypeCode.Decimal:
                    case TypeCode.Double:
                        break;

                    default: break;
                }
            }

            string html =Environment.NewLine + $@"<div class=""field col-12 md:col-6"">
      <label for=""{name}"" class=""block"">{propertyInfo.Name}</label>
      <input formControlName=""{name}"" type=""{type}"" {boundaries} pInputText id=""{name}"" aria-describedby=""{name}-help"" />
      <div *ngIf=""{name}.errors && ({name}.dirty || {name}.touched)"" class=""p-error block"">
        <div *ngIf=""{name}.errors?.required"">Påkrævet</div >
        <div *ngIf=""{name}.errors?.digitsOnly"">Kun hel tal</div>
      </div>
</div>";
            return html;
        }

    }
}
